using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class ChatRoom : Form
    {
        ISingleServer server;
        String username;
        String address;
        delegate ListViewItem LVAddDelegate(ListViewItem lvItem);
        delegate void LVRemDelegate(String lvItem);
        AlterEventRepeater evRepeater;
        RemMessage r;

        List<string> usersList;
        List<string> groupChatsList;
        Hashtable activeUsers;
        Hashtable chatTabs;
        String activeUser;
        private IClientRem activeUserRemObj;
        Boolean groupChatActive;

        String selectedUser;
        String selectedGroup;

        public ChatRoom(ISingleServer server, String username, String port)
        {
            InitializeComponent();
            this.server = server;
            this.username = username;
            this.address = "tcp://localhost:" + port.ToString() + "/Message";
            this.Text = " Chat - " + username;
            onlineUsers.View = View.List;
            groupChats.View = View.List;
            activeUsers = new Hashtable();
            chatTabs = new Hashtable();
            groupChatsList = new List<string>();
            UpdateOnlineUsers();
            UpdateGroupChats();
            evRepeater = new AlterEventRepeater();
            evRepeater.alterEvent += new AlterDelegate(DoAlterations);
            server.alterEvent += new AlterDelegate(evRepeater.Repeater);
            r = (RemMessage)RemotingServices.Connect(typeof(RemMessage), "tcp://localhost:" + port.ToString() + "/Message");    // connect to the registered my remote object here
            r.PutMyForm(this);
            groupChatActive = false;

            selectedGroup = null;
            selectedUser = null;
        }

        public void UpdateOnlineUsers()
        {
            usersList = server.getUsers();
            foreach (string key in usersList)
            {
                if(key != username)
                    onlineUsers.Items.Add(key.ToString());
            }
        }

        public void UpdateGroupChats()
        {
            groupChatsList = server.getGroupChats();
            foreach (string key in groupChatsList)
                groupChats.Items.Add(key.ToString());
        }

        public void DoAlterations(Operation op, String username)
        {
            LVAddDelegate lvAdd;
            LVRemDelegate lvRem;

            switch (op)
            {
                case Operation.Add:
                    usersList.Add(username);
                    lvAdd = new LVAddDelegate(onlineUsers.Items.Add);
                    ListViewItem lvItem = new ListViewItem(new string[] { username });
                    BeginInvoke(lvAdd, new object[] { lvItem });
                    break;
                case Operation.Remove:
                    lvRem = new LVRemDelegate(RemoveUser);
                    BeginInvoke(lvRem, new object[] { username });
                    break;
                case Operation.GroupChat:
                    groupChatsList.Add(username);
                    lvAdd = new LVAddDelegate(groupChats.Items.Add);
                    ListViewItem lvgcItem = new ListViewItem(new string[] { username });
                    BeginInvoke(lvAdd, new object[] { lvgcItem });
                    break;
            }
        }

        private void RemoveUser(String username)
        {
            usersList.Remove(username);
            if (chatTabs.Contains(username))
            {
                ChatTab tab = (ChatTab)chatTabs[username];
                tab.SetOfflineMsg(username);
                if (activeUser.Equals(username))
                    DisableSend();
            }
                
            foreach (ListViewItem lvI in onlineUsers.Items)
                if (lvI.SubItems[0].Text == username)
                {
                    lvI.Remove();
                    break;
                }
        }

        private void ChatRoom_FormClosed(object sender, FormClosedEventArgs e)
        {
            server.alterEvent -= new AlterDelegate(evRepeater.Repeater);
            evRepeater.alterEvent -= new AlterDelegate(DoAlterations);;
            server.Logout(username);
        }

        private void onlineUsers_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            String tabUsername = e.Item.Text;
            if (onlineUsers.SelectedItems.Count == 0)
            {
                startChat.Enabled = false;
                selectedUser = null;
            }
            else if (chatTabs.Contains(tabUsername))
            {
                selectedUser = tabUsername;
                groupChatActive = false;
                ChatTab tab = (ChatTab)chatTabs[tabUsername];
                int index = activeConversations.TabPages.IndexOf(tab.tabPage);
                activeConversations.SelectedIndex = index;
                startChat.Enabled = false;
                if (tab.offline)
                    DisableSend();
                else
                    msgToSend.Enabled = true;
            }
            else
                startChat.Enabled = true;

            if (selectedUser != null && selectedGroup != null)
                inviteToGroup.Enabled = true;

        }

        public void PutMessage(Message msg)
        {
            if (InvokeRequired)                                               // I'm not in UI thread
                BeginInvoke((MethodInvoker)delegate { PutMessage(msg); });  // Invoke using an anonymous delegate
            else
            {
                ChatTab tab;
                if (chatTabs.Contains(msg.tabName))
                {
                    tab = (ChatTab)chatTabs[msg.tabName];
                    tab.AddReceiverText(msg.text, msg.sender);
                }
                else
                {
                    tab = new ChatTab(msg.tabName);
                    tab.AddReceiverText(msg.text, msg.sender);
                    activeConversations.TabPages.Add(tab.tabPage);
                    chatTabs.Add(msg.sender, tab);
                    ChangeActiveUser(msg.tabName);
                    tab.offline = false;
                    msgToSend.Enabled = true;
                }
                if (activeConversations.SelectedTab != tab.tabPage)
                    tab.NewMessages();
            }
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            if (!groupChatActive)
                activeUserRemObj.ReceiveMessage(new Message(username, msgToSend.Text, username));
            else
                server.SendGroupChatMessage(activeUser, new Message(username, msgToSend.Text, activeUser));
            ChatTab tab = (ChatTab)chatTabs[activeUser];
            tab.AddSenderText(msgToSend.Text, username);
            msgToSend.Clear();
        }

        private void activeConversations_SelectedIndexChanged(object sender, EventArgs e)
        {
            String username = (sender as TabControl).SelectedTab.Text;
            username = username.Replace("*", string.Empty);
            if (groupChatsList.Contains(username))
                groupChatActive = true;
            else
                groupChatActive = false;
            ChatTab tab = (ChatTab)chatTabs[username];
            if (tab.offline)
                DisableSend();
            else
            {
                ChangeActiveUser(username);
                msgToSend.Enabled = true;
            }
            tab.MessagesRead();
        }

        private void ChangeActiveUser(String username)
        {
            if (!groupChatActive)
                activeUserRemObj = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)activeUsers[username]);
            activeUser = username;
        }

        private void msgToSend_TextChanged(object sender, EventArgs e)
        {
            sendBtn.Enabled = !string.IsNullOrWhiteSpace(msgToSend.Text);
        }

        private void DisableSend()
        {
            msgToSend.Enabled = false;
            sendBtn.Enabled = false;
        }

        public void ShowChatRequest(String requester)
        {
            DialogResult dialogResult = MessageBox.Show(requester + " wants to chat with you. Do you accept?", "Conversation request", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
                server.RequestAccepted(requester, username);
            else if (dialogResult == DialogResult.No)
                server.RequestRefused(requester, username);
        }

        public void RequestAccepted(String username, String address)
        {
            ChatTab tab = (ChatTab)chatTabs[username];
            tab.RequestAccepted(username);
            tab.offline = false;
            activeUsers.Add(username, address);

            BeginInvoke(new Action(() =>
            {
                if (activeConversations.SelectedTab == tab.tabPage)
                {
                    ChangeActiveUser(username);
                    msgToSend.Enabled = true;
                }
            }));
        }

        public void RequestRefused(String username)
        {
            ChatTab tab = (ChatTab)chatTabs[username];
            tab.RequestRefused(username);
        }

        public void AddActiveUser(String username, String address)
        {
            activeUsers.Add(username, address);
        }

        private void startChat_Click(object sender, EventArgs e)
        {
            String tabUsername = onlineUsers.SelectedItems[0].Text;
            ChatTab tab = tab = new ChatTab(tabUsername);
            activeConversations.TabPages.Add(tab.tabPage);
            chatTabs.Add(tabUsername, tab);
            tab.SendConversationRequest();
            Request request = new Request(server, username, tabUsername);
            Thread oThread = new Thread(new ThreadStart(request.Send));
            oThread.Start();
            int index = activeConversations.TabPages.IndexOf(tab.tabPage);
            activeConversations.SelectedIndex = index;
            if (tab.offline)
                DisableSend();
            else
                msgToSend.Enabled = true;
            startChat.Enabled = false;
        }

        private void newGroupChat_Click(object sender, EventArgs e)
        {
            GroupChatInput gc = new GroupChatInput();
            if (gc.ShowDialog(this) == DialogResult.OK)
                server.CreateGroupChat(gc.groupChatName.Text);
            gc.Dispose();
        }

        private void groupChats_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            String groupChatName = e.Item.Text;
            if (groupChats.SelectedItems.Count == 0)
            {
                joinGroupChat.Enabled = false;
                selectedGroup = null;
            }
            else if (chatTabs.Contains(groupChatName))
            {
                selectedGroup = groupChatName;
                groupChatActive = true;
                ChatTab tab = (ChatTab)chatTabs[groupChatName];
                int index = activeConversations.TabPages.IndexOf(tab.tabPage);
                activeConversations.SelectedIndex = index;
                joinGroupChat.Enabled = false;
                if (tab.offline)
                    DisableSend();
                else
                    msgToSend.Enabled = true;
            }
            else
                joinGroupChat.Enabled = true;
            if (selectedUser != null && selectedGroup != null)
                inviteToGroup.Enabled = true;

        }

        private void joinGroupChat_Click(object sender, EventArgs e)
        {
            String groupChatName = groupChats.SelectedItems[0].Text;
            ChatTab tab = tab = new ChatTab(groupChatName);
            activeConversations.TabPages.Add(tab.tabPage);
            chatTabs.Add(groupChatName, tab);
            tab.offline = false;
            int index = activeConversations.TabPages.IndexOf(tab.tabPage);
            activeConversations.SelectedIndex = index;
            List<String> groupUsers = server.GetGroupChatUsers(groupChatName);
            tab.JoinGroupChat(groupUsers);
            server.AddUserToGroupChat(groupChatName, username);
            if (tab.offline)
                DisableSend();
            else
                msgToSend.Enabled = true;
            if (activeConversations.SelectedTab == tab.tabPage)
            {
                groupChatActive = true;
                ChangeActiveUser(groupChatName);
            }
            joinGroupChat.Enabled = false;
        }

        public void AddUserToGroupChat(String username, String chatName)
        {
            BeginInvoke(new Action(() =>
            {
                ChatTab ct = (ChatTab)chatTabs[chatName];
                ct.AddGroupChatUser(username);
            }));
        }

        public void RemoveUserFromGroupChat(String username, String chatName)
        {
            BeginInvoke(new Action(() =>
            {
                ChatTab ct = (ChatTab)chatTabs[chatName];
                ct.RemoveGroupChatUser(username);
            }));
        }
    }

    public class RemMessage : MarshalByRefObject, IClientRem
    {
        private ChatRoom win;

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void PutMyForm(ChatRoom form)
        {
            win = form;
        }

        public void ReceiveRequest(String username)
        {
            win.ShowChatRequest(username);
        }

        public void ReceiveMessage(Message msg)
        {
            win.PutMessage(msg);
        }

        public void RequestAccepted(String username, String address)
        {
            win.RequestAccepted(username, address);
        }

        public void ReceiveAddress(String username, String address)
        {
            win.AddActiveUser(username, address);
        }

        public void AddUserToGroupChat(String username, String chatName)
        {
            win.AddUserToGroupChat(username, chatName);
        }

        public void RemoveUserFromGroupChat(String username, String chatName)
        {
            win.RemoveUserFromGroupChat(username, chatName);
        }

        public void RequestRefused(string username)
        {
            win.RequestRefused(username);
        }
    }

    public class Request
    {
        ISingleServer server;
        String username, tabUsername;

        public Request(ISingleServer server, String username, String tabUsername)
        {
            this.server = server;
            this.username = username;
            this.tabUsername = tabUsername;
        }

        public void Send()
        {
            server.RequestConversation(username, tabUsername);
        }
    };
}
