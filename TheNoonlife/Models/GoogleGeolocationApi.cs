using Newtonsoft.Json.Linq;
using RestSharp;

namespace TheNoonlife.Models
{
    public class GoogleGeolocationApi
    {
        //This method gets the users current location using google's geolocation api
        public LocationModel GetGeolocation()
        {
            //instantiate a new RestClient and set the base url to make a call to the geolocation api
            var client = new RestClient($"https://www.googleapis.com/geolocation/v1/geolocate?macAddress={SystemHardware.GetMACAddress()}&key=AIzaSyCDZy_ZOyuQCTsLdmMCbTzbb3uaSxtpfKI");
            //instantiate a RestRequest setting its method to POST
            var request = new RestRequest(Method.POST);          
            request.AddHeader("postman-token", "4364a843-d2ba-4c0a-6268-bf8003695bfa");
            request.AddHeader("cache-control", "no-cache");
            //Execute the request and get the response
            IRestResponse response = client.Execute(request);
            //Parse the resulting json into a JObject
            var result = JObject.Parse(response.Content);

            //Instatiate a new LocationModel, setting the latitude and longitude equal to the lat and lng returned
            //from the api request
            var currentLocation = new LocationModel(result["location"]["lat"].ToString(), result["location"]["lng"].ToString());

            return currentLocation;
        }
    }
}