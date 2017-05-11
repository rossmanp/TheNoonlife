using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheNoonlife.Models
{
    public class Restaurant
    {
        public Restaurant (string Name, string Id)
        {
            this.Name = Name;
            this.Id = Id;
        }

        public Restaurant (string Name, string picture, string price)
        {
            this.Name = Name;
            this.Picture = picture;
            this.Price = price;
        }
       
        public string Name { get; set; }

        public string Id { get; set; }

        public string Picture { get; set; }

        public string Price { get; set; }
    }
}