namespace TheNoonlife.Models
{
    public class GoogleGeocodeApiRequest
    {
        private LocationModel LocationModel;

        public GoogleGeocodeApiRequest(LocationModel location)
        {
            LocationModel = location;
        }

        private const string ApiKey = "AIzaSyCDZy_ZOyuQCTsLdmMCbTzbb3uaSxtpfKI";

        public string BaseUrl
        {
            get
            {
                var result =
                    "https://maps.googleapis.com/maps/api/geocode/json?address=";

                return result;
            }
        }

        public string RequestUrl
        {
            get
            {
                var result =
                    BaseUrl +
                    $"{LocationModel.LocationName}&key={ApiKey}";

                return result;
            }
        }
    }
}