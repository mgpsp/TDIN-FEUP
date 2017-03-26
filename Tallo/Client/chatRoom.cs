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
        delegate ListViewItem LVAddDelegate(ListViewItem lvItem);
        delegate void LVRemDelegate(String lvItem);
        AlterEventRepeater evRepeater;
        RemMessage r;

        Hashtable users;
        private IClientRem activeUser;

        public ChatRoom(ISingleServer server, String username, String port)
        {
            InitializeComponent();
            this.server = server;
            this.username = username;
            onlineUsers.View = View.List;
            users = new Hashtable();
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

        public void DoAlterations(Operation op, String username)
        {
            LVAddDelegate lvAdd;
            LVRemDelegate lvRem;

            switch (op)
            {
                case Operation.Add:
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
            String username = e.Item.Text;
            /*if (!users.Contains(username))
                server.GetReference(username);
            else*/
            activeUser = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)users[username]);
        }

        public void PutMessage(String msg)
        {
            if (InvokeRequired)                                               // I'm not in UI thread
                BeginInvoke((MethodInvoker)delegate { PutMessage(msg); });  // Invoke using an anonymous delegate
            else
                conversation.Text += (msg + Environment.NewLine);
        }

        public void AddUser(String address, String username)
        {
            activeUser = (IClientRem)RemotingServices.Connect(typeof(IClientRem), address);
            users.Add(username, activeUser);
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            activeUser.SendMessage(msgToSend.Text);
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

        public void SendMessage(String msg)
        {
            win.PutMessage(msg);
        }
    }
}
