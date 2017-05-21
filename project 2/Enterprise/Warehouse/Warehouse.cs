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
using System.Collections;

namespace Warehouse
{
    public partial class Warehouse : Form
    {
        Hashtable orders;
        Order selectedOrder;
        Boolean getOrders = true;
        string filter;
        Socket socket;
        public Warehouse()
        {
            InitializeComponent();
            orders = new Hashtable();
            ordersList.FullRowSelect = true;
            filters.SelectedItem = "All";
        }

        private void Warehouse_Load(object sender, EventArgs e)
        {
            socket = IO.Socket("http://localhost:3002/");
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
                    orders.Add(order.id, order);
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
                JObject o = (JObject)data;
                Order order = new Order();
                foreach (JProperty p in o.Properties())
                    order.addProperty(p);
                addOrder(order, filter);
            });
        }

        private void filterOrders(string filter)
        {
            foreach (DictionaryEntry order in orders)
                addOrder((Order)order.Value, filter);
        }

        private void addOrder(Order order, string filter)
        {
            orders.Add(order.id, order);
            if (order.status == filter || filter == "All")
            {
                ordersList.Invoke((MethodInvoker)delegate ()
                {
                    ListViewItem item = new ListViewItem(order.id.ToString());
                    item.SubItems.Add(order.name);
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

        private void ordersList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            selectedOrder = (Order)orders[Int32.Parse(e.Item.Text)];
            if (selectedOrder.status != "Dispatched")
                shipBtn.Enabled = true;
            else
                shipBtn.Enabled = false;
        }

        private void shipBtn_Click(object sender, EventArgs e)
        {
            ListViewItem item = ordersList.SelectedItems[0];
            item.SubItems[3].Text = "Dispatched";
            socket.Emit("orderShipped", selectedOrder.toJSON());
        }
    }
}
