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

namespace Store
{
    public partial class Store : Form
    {
        List<Book> books;
        string selectedBook;
        int selectedOrder;
        public Store()
        {
            InitializeComponent();
        }

        private void Store_Load(object sender, EventArgs e)
        {
            books = new List<Book>();
            var socket = IO.Socket("http://localhost:3001/");
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
                    books.Add(book);
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
            Console.WriteLine(e.Item.Text);
            selectedBook = e.Item.Text;
        }
    }

    public class Book
    {
        public int id;
        public string name;
        public string author;
        public int year;
        public int stock;
        public Book() { }

        public void addProperty(JProperty p)
        {
            switch (p.Name)
            {
                case "id":
                    this.id = (int)p.Value;
                    break;
                case "name":
                    this.name = (string)p.Value;
                    break;
                case "author":
                    this.author = (string)p.Value;
                    break;
                case "year":
                    this.year = (int)p.Value;
                    break;
                case "stock":
                    this.stock = (int)p.Value;
                    break;
            }
        }
    }
}
