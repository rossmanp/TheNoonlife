using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net;
using Newtonsoft.Json.Linq;
using TheNoonlife.Models;
using System.Collections.Generic;
using System.Linq;

namespace TheNoonlife.Controllers
{
    public class HomeController : Controller
    {
  
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FindBrunchiness(LocationModel location)
        {
            //Fetch latitude and longitude using the geocode api
            var geocodeApiRequest = new GoogleGeocodeApiRequest(location);
            var jTokenFetcher = new JtokenFetcher();
            var geocodeJToken = jTokenFetcher.GetJTokenWithKey(geocodeApiRequest.RequestUrl);

            //Set the location's lat and lng equal to the results returned from the geocode api request
            location.Latitude = geocodeJToken["results"][0]["geometry"]["location"]["lat"].ToString();
            location.Longitude = geocodeJToken["results"][0]["geometry"]["location"]["lng"].ToString();

            //Pass location to the YelpApiRequest constructor 
            //we will use this to build the request url
            //from the latitude and longitude properties of location
            var yelp = new YelpApiRequest(location);
            
            //Get the jtoken from the yelp api
            var yelpJtoken = jTokenFetcher.GetJTokenWithToken(yelp);
            List<Restaurant> places = new List<Restaurant>();
            for (int i = 0; i < yelpJtoken["businesses"].Count(); i++)
            {
                
                places.Add(new Models.Restaurant(yelpJtoken["businesses"][i]["name"].ToString(),
                 yelpJtoken["businesses"][i]["id"].ToString()));            
            }
            ViewBag.Result = places;
            return View(places);

        }

        public ActionResult Result(string Id)
        {
            var jTokenFetcher = new JtokenFetcher();                      
            var yelpJtoken = jTokenFetcher.GetBusinessJTokenWithToken(Id);
            ViewBag.Result = yelpJtoken;

            ViewBag.Name = yelpJtoken["name"].ToString();
            ViewBag.Picture = yelpJtoken["image_url"].ToString();
            ViewBag.Price = yelpJtoken["price"].ToString();
            
            return View();
        }

        public ActionResult About()
        {
           var yelpAccess = new YelpApiRequest(new LocationModel());

            return View(yelpAccess);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}