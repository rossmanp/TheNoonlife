using System;
using System.Web.Mvc;
using TheNoonlife.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Microsoft.AspNet.Identity;


namespace TheNoonlife.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = _db.Users.Find(User.Identity.GetUserId());
                return View("UserHomePage", currentUser);
            }

            return View();
        }




        public ActionResult FindBrunchWithSearch(LocationModel location)
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

            //Build a list of results to pass to the view
            List<Restaurant> places = new List<Restaurant>();
            for (int i = 0; i < yelpJtoken["businesses"].Count(); i++)
            {
                places.Add(new Restaurant(yelpJtoken["businesses"][i]["name"].ToString(),
                 yelpJtoken["businesses"][i]["id"].ToString()));
            }

            return View("FindBrunch", places);
        }

        public ActionResult FindBrunchWithCurrentLocation()
        {
            var locater = new GoogleGeolocationApi();
            var currentLocation = locater.GetGeolocation();

            //Pass location to the YelpApiRequest constructor 
            //we will use this to build the request url
            //from the latitude and longitude properties of location
            var yelp = new YelpApiRequest(currentLocation);

            //Get the jtoken from the yelp api
            var jTokenFetcher = new JtokenFetcher();
            var yelpJtoken = jTokenFetcher.GetJTokenWithToken(yelp);

            //Build a list of results to pass to the view
            List<Restaurant> places = new List<Restaurant>();
            for (int i = 0; i < yelpJtoken["businesses"].Count(); i++)
            {
                places.Add(new Restaurant(yelpJtoken["businesses"][i]["name"].ToString(),
                    yelpJtoken["businesses"][i]["id"].ToString()));
            }

            return View("FindBrunch", places);

        }

        public ActionResult Result(string Id)
        {
            //Get json data as a jtoken
            var jTokenFetcher = new JtokenFetcher();
            var yelpJtoken = jTokenFetcher.GetBusinessJTokenWithToken(Id);

            //Instantiate a Restaurant to be passed to the view
            //setting the values equal to the relevant json data.
            //(This can be changed to use a deserialize method at some point)
            var restaurant = new Restaurant(
                yelpJtoken["name"].ToString(),
                yelpJtoken["image_url"].ToString(),
                yelpJtoken["price"].ToString());

            return View(restaurant);
        }

        [Authorize]
        public ActionResult AddFavoriteRestaurant(string selection)
        {
            if (ModelState.IsValid)
            {
                var currentUser = _db.Users.Find(User.Identity.GetUserId());
                currentUser.FavoriteRestaurant = selection;
                _db.SaveChanges();
            }
            return View("Index");
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

        public static string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }
    }
}