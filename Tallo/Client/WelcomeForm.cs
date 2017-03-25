using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Windows.Forms;

namespace Client
{
    public partial class Window : Form
    {
        int port;
        Guid guid;
        ISingleServer server;

        public Window(int myPort)
        {
            port = myPort;
            guid = Guid.NewGuid();
            InitializeComponent();
        }
  
        private void Window_Load_1(object sender, EventArgs e)
        {
            server = (ISingleServer)R.New(typeof(ISingleServer));  // get reference to the singleton remote object
        }

        private void signIn_Click(object sender, EventArgs e)
        {
            if (server.LoginUser(username.Text, password.Text))
            {
                invalidLoginLabel.Visible = false;
                server.RegisterAddress(guid, "tcp://localhost:" + port.ToString() + "/Message");
                Hashtable users = server.GetActiveUsers();
                new ChatRoom(users);
                this.Hide();
            }
            else
                invalidLoginLabel.Visible = true;
            
        }
    }
    class R
    {
        private static IDictionary wellKnownTypes;

        public static object New(Type type)
        {
            if (wellKnownTypes == null)
                InitTypeCache();
            WellKnownClientTypeEntry entry = (WellKnownClientTypeEntry)wellKnownTypes[type];
            if (entry == null)
                throw new RemotingException("Type not found!");
            return Activator.GetObject(type, entry.ObjectUrl);
        }

        public static void InitTypeCache()
        {
            Hashtable types = new Hashtable();
            foreach (WellKnownClientTypeEntry entry in RemotingConfiguration.GetRegisteredWellKnownClientTypes())
            {
                if (entry.ObjectType == null)
                    throw new RemotingException("A configured type could not be found!");
                types.Add(entry.ObjectType, entry);
            }
            wellKnownTypes = types;
        }
    }

    public class RemMessage : MarshalByRefObject, IClientRem
    {
        private Window win;
        private ChatRoom chat;

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void PutMyForm(Window form)
        {
            win = form;
        }

        public void PutChat(ChatRoom chat)
        {
            this.chat = chat;
        }

        public void SendMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void UpdateActiveUsersList(Hashtable users)
        {
            //chat.UpdateOnlineUsers(users);
            chat.Show();
        }

        internal void putChat(ChatRoom chatRoom)
        {
            throw new NotImplementedException();
        }
    }
}
