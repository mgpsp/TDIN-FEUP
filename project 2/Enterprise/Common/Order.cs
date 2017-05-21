using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Common
{
    public class Order
    {
        public int id;
        public string name;
        public string status;
        public int quantity;

        public Order() { }

        public Order(string name, int quantity)
        {
            this.name = name;
            this.quantity = quantity;
        }

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
                case "status":
                    this.status = (string)p.Value;
                    break;
                case "quantity":
                    this.quantity = (int)p.Value;
                    break;
            }
        }

        public JObject toJSON()
        {
            JObject json = new JObject();
            json.Add("id", id);
            json.Add("name", name);
            json.Add("status", status);
            json.Add("quantity", quantity);
            return json;
        }
    }
}
