using System.Web.Mvc;
using TheNoonlife.Models;
using System.Collections.Generic;
using System.Linq;
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
                //Get json data as a jtoken
                var jTokenFetcher = new JtokenFetcher();
                if (currentUser.FavoriteRestaurant != null)
                {
                    var yelpJtoken = jTokenFetcher.GetYelpBusinessJTokenWithToken(currentUser.FavoriteRestaurant);

                    string priceInfo = "No price information available.";
                    if (yelpJtoken["price"] != null)
                    {
                        priceInfo = yelpJtoken["price"].ToString();
                    }

                    //Instantiate a Restaurant to be passed to the view
                    //setting the values equal to the relevant json data.
                    //(This can be changed to use a deserialize method at some point)
                    var restaurant = new Restaurant(
                        yelpJtoken["name"].ToString(),
                        yelpJtoken["image_url"].ToString(),

                        priceInfo,
                        yelpJtoken["id"].ToString(),
                        yelpJtoken["phone"].ToString());

                    return View("UserHomePage", restaurant);
                }
            }

            return View();
        }

        [Authorize]
        public ActionResult UserHomePage()
        {
            var user = _db.Users.Find(User.Identity.GetUserId());

            if (user.FavoriteRestaurant == null)
            {
                return View("Index");
            }
            //Get json data as a jtoken
            var jTokenFetcher = new JtokenFetcher();
            var yelpJtoken = jTokenFetcher.GetYelpBusinessJTokenWithToken(user.FavoriteRestaurant);

            string priceInfo = "No price information available.";
            if (yelpJtoken["price"] != null)
            {
                priceInfo = yelpJtoken["price"].ToString();
            }

            //Instantiate a Restaurant to be passed to the view
            //setting the values equal to the relevant json data.
            //(This can be changed to use a deserialize method at some point)
            var restaurant = new Restaurant(
                yelpJtoken["name"].ToString(),
                yelpJtoken["image_url"].ToString(),

                yelpJtoken["price"].ToString(),
                yelpJtoken["id"].ToString(),
                yelpJtoken["phone"].ToString());

            return View(restaurant);
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
            var yelpJtoken = jTokenFetcher.GetYelpSearchJTokenWithAccessToken(yelp);

            //Build a list of results to pass to the view
            List<Restaurant> places = new List<Restaurant>();
            for (int i = 0; i < yelpJtoken["businesses"].Count(); i++)
            {
                if (!yelpJtoken["businesses"][i]["is_closed"].ToObject<bool>())
                {
                    places.Add(new Restaurant(yelpJtoken["businesses"][i]["name"].ToString(),
                    yelpJtoken["businesses"][i]["id"].ToString()));
                }
            }
            return View("FindBrunch", places);
        }


        //This method gets the current location of the user and displays restaurants near
        //their location
        public ActionResult FindBrunchWithCurrentLocation(int? radius)
        {
            YelpApiRequest yelp;
            var locater = new GoogleGeolocationApi();
            var currentLocation = locater.GetGeolocation();

            if (radius != null)
            {
                //Pass location and radius to the YelpApiRequest constructor 
                //we will use this to build the request url
                //from the latitude and longitude properties of location
                yelp = new YelpApiRequest(currentLocation, radius);
            }
            else
            {
                //Pass location to the YelpApiRequest constructor 
                //we will use this to build the request url
                //from the latitude and longitude properties of location
                yelp = new YelpApiRequest(currentLocation);
            }

            //Get the jtoken (Json data as a C# object) from the yelp search api
            var jTokenFetcher = new JtokenFetcher();
            var yelpJtoken = jTokenFetcher.GetYelpSearchJTokenWithAccessToken(yelp);

            //Build a list of results to pass to the view
            List<Restaurant> places = new List<Restaurant>();
            for (int i = 0; i < yelpJtoken["businesses"].Count(); i++)
            {
                if (!yelpJtoken["businesses"][i]["is_closed"].ToObject<bool>())
                {
                    places.Add(new Restaurant(yelpJtoken["businesses"][i]["name"].ToString(),
                        yelpJtoken["businesses"][i]["id"].ToString()));
                }
            }

            return View("FindBrunch", places);
        }

        public ActionResult Result(string Id)
        {
            //Get json data as a jtoken
            var jTokenFetcher = new JtokenFetcher();
            var yelpJtoken = jTokenFetcher.GetYelpBusinessJTokenWithToken(Id);

            string priceInfo = "No price information available.";
            if (yelpJtoken["price"] != null)
            {
                priceInfo = yelpJtoken["price"].ToString();
            }

            //Instantiate a Restaurant to be passed to the view
            //setting the values equal to the relevant json data.
            //(This can be changed to use a deserialize method at some point)
            var restaurant = new Restaurant(
                yelpJtoken["name"].ToString(),
                yelpJtoken["image_url"].ToString(),

                yelpJtoken["price"].ToString(),
                yelpJtoken["id"].ToString(),
                yelpJtoken["phone"].ToString());

            return View(restaurant);
        }

        //This method saves a restaurant as a user's favorite
        [Authorize]
        public ActionResult AddFavoriteRestaurant(string selectedRestaurantId)
        {
            if (ModelState.IsValid)
            {
                //Find the current user based on the user id
                var currentUser = _db.Users.Find(User.Identity.GetUserId());
                //set the users favorite restaurant equal to the selected restaurant's id
                currentUser.FavoriteRestaurant = selectedRestaurantId;
                _db.SaveChanges();
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult ajaxLookup(CategoryModel indexViewModel)
        {
            var lat = indexViewModel.Latitude;
            var longi = indexViewModel.Longitude;
            var cat = indexViewModel.Category;
            var hellyea = "hellyea";
            // now you have lat, long and cat you need to get token for yelp like you do in the other functions

            var jTokenFetcher = new JtokenFetcher();
            var yelp = new YelpApiRequest(indexViewModel);
            var yelpJtoken = jTokenFetcher.GetYelpSearchJTokenWithAccessToken(yelp);
            List<Restaurant> places = new List<Restaurant>();
            for (int i = 0; i < yelpJtoken["businesses"].Count(); i++)
            {
                if (yelpJtoken["businesses"][i]["image_url"] == null)
                {
                    yelpJtoken["businesses"][i]["image_url"] = "na";
                }

                if (yelpJtoken["businesses"][i]["price"] == null)
                {
                    yelpJtoken["businesses"][i]["price"] = "na";
                }


                places.Add(new Restaurant(yelpJtoken["businesses"][i]["name"].ToString(), yelpJtoken["businesses"][i]["image_url"].ToString(), yelpJtoken["businesses"][i]["price"].ToString(),
                 yelpJtoken["businesses"][i]["id"].ToString(),
                yelpJtoken["phone"].ToString()));
            }

            return Json(new { success = true, latitude = lat, longitude = longi, thisworked = hellyea, restaurants = places, yelpInfo = yelp });
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