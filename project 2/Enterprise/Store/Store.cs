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
using System.Collections;
using Common;
using RabbitMQ.Client;

namespace Store
{
    public partial class Store : Form
    {
        Socket socket;
        Hashtable books;
        Hashtable orders;
        Book selectedBook;
        Order selectedOrder;
        Boolean getBooks = true;
        Boolean getWarehouseOrders = true;
        public Store()
        {
            InitializeComponent();
            books = new Hashtable();
            orders = new Hashtable();
            ordersList.FullRowSelect = true;
            booksList.FullRowSelect = true;
            socket = IO.Socket("http://localhost:3001/");
        }

        private void Store_Load(object sender, EventArgs e)
        {
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Console.WriteLine("Connected to Store server");
                if (getBooks)
                {
                    getBooks = false;
                    socket.Emit("getBooks");
                }

                if (getWarehouseOrders)
                {
                    getWarehouseOrders = false;
                    socket.Emit("getWarehouseOrders");
                }
            });

            socket.On("books", (data) =>
            {
                JArray a = JArray.Parse(data.ToString());
                foreach (JObject o in a.Children<JObject>())
                {
                    Book book = new Book();
                    foreach (JProperty p in o.Properties())
                    {
                        book.addProperty(p);
                    }
                    books.Add(book.name, book);
                    booksList.Invoke((MethodInvoker)delegate ()
                    {
                        booksList.Items.Add(book.name);
                    });
                }
            });

            socket.On("book-error", (data) =>
            {
                Console.WriteLine("Error retrieving books:" + data);
            });

            socket.On("warehouseOrders", (data) =>
            {
                JArray a = JArray.Parse(data.ToString());
                foreach (JObject o in a.Children<JObject>())
                {
                    Order order = new Order();
                    foreach (JProperty p in o.Properties())
                    {
                        order.addProperty(p);
                    }
                    addOrder(order);
                }
            });

            socket.On("warehouseOrder-error", (data) =>
            {
                Console.WriteLine("Error retrieving warehouse orders:" + data);
            });

            socket.On("error", (data) =>
            {
                Console.WriteLine("Error: " + data);
            });

            socket.On("warehouseOrder", (data) =>
            {
                JObject o = (JObject)data;
                Order order = new Order();
                foreach (JProperty p in o.Properties())
                    order.addProperty(p);
                addOrder(order);
            });

            socket.On("updateBookStock", (data) =>
            {
                JObject o = (JObject)data;
                Book book = new Book();
                foreach (JProperty p in o.Properties())
                    book.addProperty(p);
                Book b = (Book)books[book.name];
                b.stock = book.stock;
            });
        }

        private void addOrder(Order order)
        {
            orders.Add(order.id, order);
            ordersList.Invoke((MethodInvoker)delegate ()
            {
                ListViewItem item = new ListViewItem(order.id.ToString());
                item.SubItems.Add(order.name);
                item.SubItems.Add(order.quantity.ToString());
                ordersList.Items.Add(item);
            });
        }

        private void booksList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (booksList.SelectedItems.Count > 0)
            {
                selectedBookPanel.Visible = true;
                selectedBook = (Book)books[e.Item.Text];
                bookName.Text = selectedBook.name;
                authorName.Text = selectedBook.author;
                bookYear.Text = selectedBook.year.ToString();
                bookStock.Text = selectedBook.stock.ToString();
                bookPrice.Text = selectedBook.price.ToString() + "€";
                Console.WriteLine(System.IO.Path.GetDirectoryName(Application.ExecutablePath));
                Image cover = Image.FromFile("../../img/hp1.jpg");
                bookCover.Image = cover;
            }
            else
            {
                selectedBookPanel.Visible = false;
            }
        }

        private void sellBtn_Click(object sender, EventArgs e)
        {
            SellBook sb = new SellBook(selectedBook);
            if (sb.ShowDialog(this) == DialogResult.OK)
            {
                var sell = new JObject();
                sell.Add("id", selectedBook.id);
                sell.Add("name", selectedBook.name);
                sell.Add("quantity", sb.quantity);
                socket.Emit("sellBook", sell);
                socket.On("sold", () =>
                {
                    printReceipt(sb.client, sb.quantity);
                });
            }
        }

        private void printReceipt(string clientName, int quantity)
        {
            Console.WriteLine("** RECEIPT **");
            Console.WriteLine("Sold to " + clientName + " at " + DateTime.Now.ToString("0:MM/dd/yy H:mm:ss"));
            Console.WriteLine(quantity + "x " + selectedBook.name);
            Console.WriteLine("Total: " + quantity * selectedBook.price + "€");
        }

        private void orderBtn_Click(object sender, EventArgs e)
        {
            OrderBooks ob = new OrderBooks();
            if (ob.ShowDialog(this) == DialogResult.OK)
            {
                Order order = new Order(selectedBook.name, ob.quantity);
                socket.Emit("order", order.toJSON());
            }
        }

        private void ordersList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (ordersList.SelectedItems.Count > 0)
            {
                acceptBtn.Enabled = true;
                selectedOrder = (Order)orders[Int32.Parse(e.Item.Text)];
            }
            else
                acceptBtn.Enabled = false;
        }

        private void acceptBtn_Click(object sender, EventArgs e)
        {
            ordersList.SelectedItems[0].Remove();
            socket.Emit("acceptOrder", selectedOrder.toJSON());
        }
    }
}
