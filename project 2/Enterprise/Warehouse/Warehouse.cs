using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quobject.SocketIoClientDotNet.Client;

namespace WindowsFormsApplication1
{
    public partial class Warehouse : Form
    {
        public Warehouse()
        {
            InitializeComponent();
        }

        private void Warehouse_Load(object sender, EventArgs e)
        {
            var socket = IO.Socket("http://localhost:3002/");
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Console.WriteLine("Connect");
            });

            socket.On("hi", (data) =>
            {
                Console.WriteLine("hi");
                socket.Disconnect();
            });
            Console.ReadLine();
        }
    }
}
