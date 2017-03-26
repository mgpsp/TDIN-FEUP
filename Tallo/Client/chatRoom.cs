using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        delegate void ChCommDelegate(String username);
        AlterEventRepeater evRepeater;
        public ChatRoom(ISingleServer server, String username)
        {
            InitializeComponent();
            this.server = server;
            this.username = username;
            UpdateOnlineUsers();
            evRepeater = new AlterEventRepeater();
            evRepeater.alterEvent += new AlterDelegate(DoAlterations);
            server.alterEvent += new AlterDelegate(evRepeater.Repeater);
        }

        public void UpdateOnlineUsers()
        {
            Hashtable users = server.getUsers();
            foreach (var key in users.Keys)
            {
                if(key.ToString() != username)
                    onlineUsers.Items.Add(key.ToString());
            }
        }

        public void DoAlterations(Operation op, String username)
        {
            LVAddDelegate lvAdd;

            switch (op)
            {
                case Operation.New:
                    lvAdd = new LVAddDelegate(onlineUsers.Items.Add);
                    ListViewItem lvItem = new ListViewItem(new string[] { username });
                    BeginInvoke(lvAdd, new object[] { lvItem });
                    break;
            }
        }

        private void ChatRoom_Load(object sender, EventArgs e)
        {

        }
    }
}
