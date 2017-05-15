using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheNoonlife.Models
{
    public class CategoryModel
    {
        public CategoryModel()
        {

        }

        public CategoryModel(string latitude, string longitude, string category)
        {
            Latitude = latitude;
            Longitude = longitude;
            Category = category;
        }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string LocationName { get; set; }
        public string Category { get; set; }
    }
}