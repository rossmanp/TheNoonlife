namespace TheNoonlife.Models
{
    public class GoogleGeocodeApiRequest
    {
        private LocationModel LocationModel;
        private const string ApiKey = "AIzaSyCDZy_ZOyuQCTsLdmMCbTzbb3uaSxtpfKI";

        public GoogleGeocodeApiRequest(LocationModel location)
        {
            LocationModel = location;
        }
        
        public GoogleGeocodeApiRequest()
        {

        }

        //The RequestUrl is used to make calls to the GeoCode api.
        //This allows us to search for a location by city, state, zip, etc. 
        public string RequestUrl
        {
            get
            {
                var result =
                    //We will use the locationmodel's (passed to the constructor) latitude and longitude properties
                    // to dynamically build our request url
                    $"https://maps.googleapis.com/maps/api/geocode/json?address={LocationModel.LocationName}&key={ApiKey}";

                return result;
            }
        }
    }
}