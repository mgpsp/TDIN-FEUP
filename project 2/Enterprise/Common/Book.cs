using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Common
{
    public class Book
    {
        public int id;
        public string name;
        public string author;
        public string cover;
        public int year;
        public int stock;
        public double price;

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
                case "price":
                    this.price = (double)p.Value;
                    break;
                case "cover":
                    this.cover = (string)p.Value;
                    break;
            }
        }
    }
}
