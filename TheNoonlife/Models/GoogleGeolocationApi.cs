using Newtonsoft.Json.Linq;
using RestSharp;
using TheNoonlife.Controllers;

namespace TheNoonlife.Models
{
    public class GoogleGeolocationApi
    {
        public LocationModel GetGeolocation()
        {
            var client = new RestClient($"https://www.googleapis.com/geolocation/v1/geolocate?macAddress={HomeController.GetMACAddress()}&key=AIzaSyCDZy_ZOyuQCTsLdmMCbTzbb3uaSxtpfKI");
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "4364a843-d2ba-4c0a-6268-bf8003695bfa");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);
            var result = JObject.Parse(response.Content);

            var currentLocation = new LocationModel(result["location"]["lat"].ToString(), result["location"]["lng"].ToString());

            return currentLocation;
        }
    }
}