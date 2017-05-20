using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json.Linq;
using Common;

namespace Warehouse
{
    public partial class Warehouse : Form
    {
        List<Order> orders;
        public Warehouse()
        {
            InitializeComponent();
        }

        private void Warehouse_Load(object sender, EventArgs e)
        {
            filters.SelectedItem = "All";
            orders = new List<Order>();

            var socket = IO.Socket("http://localhost:3002/");
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Console.WriteLine("Connected to Warehouse server");
                socket.Emit("getOrders");
            });

            socket.On("orders", (data) =>
            {
                JArray a = JArray.Parse(data.ToString());
                foreach (JObject o in a.Children<JObject>())
                {
                    Order order = new Order();
                    foreach (JProperty p in o.Properties())
                    {
                        order.addProperty(p);
                    }
                    orders.Add(order);
                }
                filterOrders("all");
            });

            socket.On("order-error", (data) =>
            {
                Console.WriteLine("Error retrieving orders:" + data);
            });

            socket.On("error", (data) =>
            {
                Console.WriteLine("Error: " + data);
            });
        }

        private void filterOrders(string filter)
        {
            foreach (Order order in orders)
            {
                if (order.status == filter || filter == "all")
                {
                    ordersList.Invoke((MethodInvoker)delegate ()
                    {
                        ListViewItem item = new ListViewItem(order.name);
                        item.SubItems.Add(order.quantity.ToString());
                        item.SubItems.Add(order.status);
                        ordersList.Items.Add(item);
                    });
                }
            }
        }
    }
}
