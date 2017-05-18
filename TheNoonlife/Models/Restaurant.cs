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

        public Restaurant (string Name, string picture, string price, string id, string phonenumber)
        {
            this.Name = Name;
            this.Picture = picture;
            this.Price = price;
            this.Id = id;
            PhoneNumber = phonenumber;
        }
       
        public string Name { get; set; }

        public string Id { get; set; }

        public string Picture { get; set; }

        public string Price { get; set; }

        public string PhoneNumber { get; set; }
    }
}