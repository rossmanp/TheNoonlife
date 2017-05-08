using System.Web.Mvc;
using TheNoonlife.Models;

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
            ViewBag.Result = yelpJtoken;
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