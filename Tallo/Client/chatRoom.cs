using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
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

        Hashtable users;
        Hashtable chatTabs;
        String activeUser;
        private IClientRem activeUserRemObj;

        public ChatRoom(ISingleServer server, String username, String port)
        {
            InitializeComponent();
            this.server = server;
            this.username = username;
            this.address = "tcp://localhost:" + port.ToString() + "/Message";
            this.Text = " Chat - " + username;
            onlineUsers.View = View.List;
            users = new Hashtable();
            chatTabs = new Hashtable();
            UpdateOnlineUsers();
            evRepeater = new AlterEventRepeater();
            evRepeater.alterEvent += new AlterDelegate(DoAlterations);
            server.alterEvent += new AlterDelegate(evRepeater.Repeater);
            r = (RemMessage)RemotingServices.Connect(typeof(RemMessage), "tcp://localhost:" + port.ToString() + "/Message");    // connect to the registered my remote object here
            r.PutMyForm(this);
        }

        public void UpdateOnlineUsers()
        {
            users = server.getUsers();
            foreach (var key in users.Keys)
            {
                if(key.ToString() != username)
                    onlineUsers.Items.Add(key.ToString());
            }
        }

        public void DoAlterations(Operation op, String username, String address)
        {
            LVAddDelegate lvAdd;
            LVRemDelegate lvRem;

            switch (op)
            {
                case Operation.Add:
                    users.Add(username, address);
                    lvAdd = new LVAddDelegate(onlineUsers.Items.Add);
                    ListViewItem lvItem = new ListViewItem(new string[] { username });
                    BeginInvoke(lvAdd, new object[] { lvItem });
                    break;
                case Operation.Remove:
                    lvRem = new LVRemDelegate(RemoveUser);
                    BeginInvoke(lvRem, new object[] { username });
                    break;
            }
        }

        private void RemoveUser(String username)
        {
            users.Remove(username);
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
            server.Logout(username, address);
        }

        // Change/create tab when user in online users list is clicked
        private void onlineUsers_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            String username = e.Item.Text;
            ChatTab tab;
            if (chatTabs.Contains(username))
            {
                // Change tab event will change active user, so no need to do that here
                tab = (ChatTab)chatTabs[username];
                int index = activeConversations.TabPages.IndexOf(tab.tabPage);
                activeConversations.SelectedIndex = index;
            }
            else
            {
                ChangeActiveUser(username);
                tab = new ChatTab(username);
                activeConversations.TabPages.Add(tab.tabPage);
                chatTabs.Add(username, tab);
            }
            msgToSend.Enabled = true;
        }

        public void PutMessage(Message msg)
        {
            if (InvokeRequired)                                               // I'm not in UI thread
                BeginInvoke((MethodInvoker)delegate { PutMessage(msg); });  // Invoke using an anonymous delegate
            else
            {
                if (chatTabs.Contains(msg.sender))
                {
                    ChatTab tab = (ChatTab)chatTabs[msg.sender];
                    tab.AddReceiverText(msg.text);
                }
                else
                {
                    ChangeActiveUser(msg.sender);
                    ChatTab tab = new ChatTab(msg.sender);
                    tab.AddReceiverText(msg.text);
                    activeConversations.TabPages.Add(tab.tabPage);
                    chatTabs.Add(msg.sender, tab);
                }
                msgToSend.Enabled = true;
            }
        }

        public void AddUser(String address, String username)
        {
            activeUserRemObj = (IClientRem)RemotingServices.Connect(typeof(IClientRem), address);
            users.Add(username, activeUserRemObj);
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            activeUserRemObj.SendMessage(new Message(username, msgToSend.Text));
            ChatTab tab = (ChatTab)chatTabs[activeUser];
            tab.AddSenderText(msgToSend.Text);
            msgToSend.Clear();
        }

        private void activeConversations_SelectedIndexChanged(object sender, EventArgs e)
        {
            String username = (sender as TabControl).SelectedTab.Text;
            ChangeActiveUser(username);
        }

        private void ChangeActiveUser(String username)
        {
            activeUserRemObj = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)users[username]);
            activeUser = username;
        }

        private void msgToSend_TextChanged(object sender, EventArgs e)
        {
            sendBtn.Enabled = !string.IsNullOrWhiteSpace(msgToSend.Text);
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

        public void SendReference(String address, String username)
        {
            win.AddUser(address, username);
        }

        public void SendMessage(Message msg)
        {
            win.PutMessage(msg);
        }
    }
}
