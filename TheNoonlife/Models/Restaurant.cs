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

        public string Name { get; set; }

        public string Id { get; set; }
    }
}