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
        public ChatRoom()
        {
            InitializeComponent();
        }

        public void UpdateOnlineUsers(Hashtable users)
        {
            Console.WriteLine("sim");
            /*foreach (DictionaryEntry pair in users)
            {
                Console.WriteLine("{0}={1}", pair.Key, pair.Value);
            }*/
        }
    }
}
