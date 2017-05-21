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
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Warehouse
{
    public partial class Warehouse : Form
    {
        List<Order> orders;
        Boolean getOrders = true;
        string filter;
        public Warehouse()
        {
            InitializeComponent();
            orders = new List<Order>();
            filters.SelectedItem = "All";
        }

        private void Warehouse_Load(object sender, EventArgs e)
        {
            var socket = IO.Socket("http://localhost:3002/");
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Console.WriteLine("Connected to Warehouse server");
                if (getOrders)
                {
                    getOrders = false;
                    socket.Emit("getOrders");
                }
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
                filterOrders("All");
            });

            socket.On("order-error", (data) =>
            {
                Console.WriteLine("Error retrieving orders:" + data);
            });

            socket.On("error", (data) =>
            {
                Console.WriteLine("Error: " + data);
            });

            socket.On("newOrder", (data) =>
            {
                Console.WriteLine(data);
                JObject o = (JObject)data;
                Order order = new Order();
                foreach (JProperty p in o.Properties())
                {
                    order.addProperty(p);
                }
                orders.Add(order);
                addOrder(order, filter);
            });
        }

        private void filterOrders(string filter)
        {
            foreach (Order order in orders)
                addOrder(order, filter);
        }

        private void addOrder(Order order, string filter)
        {
            if (order.status == filter || filter == "All")
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

        private void filters_SelectedIndexChanged(object sender, EventArgs e)
        {
            filter = (string)filters.SelectedItem;
            ordersList.Items.Clear();
            if (orders.Count > 0)
                filterOrders(filter);
        }
    }
}
