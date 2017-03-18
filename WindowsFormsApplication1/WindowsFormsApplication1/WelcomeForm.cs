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
            server.ClientAddress(guid, "tcp://localhost:" + port.ToString() + "/Message");
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

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void PutMyForm(Window form)
        {
            win = form;
        }

        public void SendMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void UpdateActiveUsersList(Hashtable users)
        {
            foreach (DictionaryEntry pair in users)
            {
                Console.WriteLine("{0}={1}", pair.Key, pair.Value);
            }
        }

        /*public void SomeMessage(string message)
        {
            win.AddMessage(message);
        }*/
    }
}
