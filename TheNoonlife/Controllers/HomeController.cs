using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net;
using Newtonsoft.Json.Linq;
using TheNoonlife.Models;

namespace TheNoonlife.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult FindBrunchiness(LocationModel location)
        {
            //Fetch latitude and longitude using the geocode api
            var geocodeApiRequest = new GoogleGeocodeApiRequest(location);
            var jTokenFetcher = new JtokenFetcher();
            var geocodeJToken = jTokenFetcher.GetJToken(geocodeApiRequest.RequestUrl);

            //Set the location lat and lng equal to the results returned from the geocode api request
            location.Latitude = geocodeJToken["results"][0]["geometry"]["location"]["lat"].ToString();
            location.Longitude = geocodeJToken["results"][0]["geometry"]["location"]["lng"].ToString();
            var webClient = new WebClient();

            //Pass the LocationModel to the YelpApiRequest to build the request url
            //from the latitude and longitude properties of the object
            var yelp = new YelpApiRequest(location);
            webClient.Headers.Add("Authorization", "Bearer " + yelp.AccessToken);
            var strSearchResponse = webClient.DownloadString(yelp.RequestUrl);
            var yelpJtoken = JObject.Parse(strSearchResponse);
            ViewBag.Result = yelpJtoken;
            return View();
        }

        public static T JavascriptDeserialize<T>(string json)
        {
            var jsSerializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            return jsSerializer.Deserialize<T>(json);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}