using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheNoonlife.Models
{
    public class LocationModel
    {
        public LocationModel()
        {
            
        }

        public LocationModel(string latitude, string longitude)
        {
            Latitude = latitude;
            Longitude = longitude;           
        }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string LocationName { get; set; }
        public int Radius { get; set; }
    }
}