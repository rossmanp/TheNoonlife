using System.Net;
using System.Web.Script.Serialization;

namespace TheNoonlife.Models
{
    public class YelpApiRequest
    {
        private const string clientID = "Z_gnHdaLf8031URZ0EkRzg";
        private const string clientSecret = "lClwLGU6P5N2xrjm3dtKY7iA8pO697Qr5p6b7TGtc6J5QGSDecdUTndloC5jskGQ";
        private readonly LocationModel _locationModel;

        public YelpApiRequest(LocationModel location)
        {
            _locationModel = location;
        }

        public string AccessToken
        {
            get
            {
                var requestAccesTokenUrl = "https://api.yelp.com/oauth2/token";
                var urlPostData = $"grant_type=client_credentials&client_id={clientID}&client_secret={clientSecret}";
                var webClient = new WebClient();
                var uploadString = webClient.UploadString(requestAccesTokenUrl, urlPostData);
                var jsonData = new JavaScriptSerializer {MaxJsonLength = int.MaxValue};
                var oAuthLogin = jsonData.Deserialize<OAuthLogin>(uploadString);
                return oAuthLogin.access_token;
            }
        }

        public string RequestUrl
        {
            get
            {
                var requestUrl =
                    $"https://api.yelp.com/v3/businesses/search?term=brunch&latitude={_locationModel.Latitude}&longitude={_locationModel.Longitude}";
                return requestUrl;
            }
        }

        public string RequestBusiness(string id)
        {       
                var requestUrl =
                    $"https://api.yelp.com/v3/businesses/{id}";
                return requestUrl;            
        }
    }
}