using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Windows.Forms;

namespace Client
{
    public partial class Window : Form
    {
        int port;
        ISingleServer server;

        public Window(int myPort)
        {
            port = myPort;
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
                if (server.RegisterAddress(username.Text, "tcp://localhost:" + port.ToString() + "/Message"))
                {
                    invalidLoginLabel.Visible = false;
                    this.Hide();
                    ChatRoom chatRoom = new ChatRoom(server, username.Text, port.ToString());
                    chatRoom.Show();
                }
                else
                {
                    invalidLoginLabel.Text = "You're already logged in.";
                    invalidLoginLabel.Visible = true;
                }
            }
            else
            {
                invalidLoginLabel.Text = "Wrong username or password.";
                invalidLoginLabel.Visible = true;
            }
        }

        private void registerBtn_Click(object sender, EventArgs e)
        {
            if (server.RegisterUser(usernameReg.Text, passwordReg.Text, firstName.Text, lastName.Text))
            {
                invalidRegisterLabel.Visible = false;
                server.RegisterAddress(usernameReg.Text, "tcp://localhost:" + port.ToString() + "/Message");
                this.Hide();
                ChatRoom chatRoom = new ChatRoom(server, usernameReg.Text, port.ToString());
                chatRoom.Show();
            }
            else
            {
                invalidRegisterLabel.Text = "This username already exists.";
                invalidRegisterLabel.Visible = true;
            }

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
}
