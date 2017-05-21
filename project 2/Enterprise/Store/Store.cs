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
        Book selectedBook;
        int selectedOrder;
        public Store()
        {
            InitializeComponent();
            books = new Hashtable();
            socket = IO.Socket("http://localhost:3001/");
        }

        private void Store_Load(object sender, EventArgs e)
        {
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Console.WriteLine("Connected to Store server");
                socket.Emit("getBooks");
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

            socket.On("error", (data) =>
            {
                Console.WriteLine("Error: " + data);
            });
        }

        private void booksList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            sellBtn.Enabled = true;
            orderBtn.Enabled = true;
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
    }
}
