using System.Net;
using System.Web.Script.Serialization;

namespace TheNoonlife.Models
{
    public class YelpApiRequest
    {
        private int? _radius;
        
        private readonly LocationModel _locationModel;
        private readonly Restaurant _restaurant;
        private readonly CategoryModel _categoryModel;
        bool categoryProvided = false;
      
        //This constructor is used to make general search api calls, based on location
        public YelpApiRequest(LocationModel location)
        {
            _locationModel = location;
                        
        }

        public YelpApiRequest(LocationModel location, int? radius)
        {
            _locationModel = location;
            _radius = radius;
        }

        public YelpApiRequest(Restaurant place)
        {
            _restaurant = place;
        }  
         
        public YelpApiRequest( CategoryModel category)
        {
      
            _categoryModel = category;
            categoryProvided = true;

        }        

        public YelpApiRequest()
        {

        }

        public string AccessToken
        {
            get
            {
                var requestAccesTokenUrl = "https://api.yelp.com/oauth2/token";
                var urlPostData = $"grant_type=client_credentials&client_id={YelpClientInfo.ClientId}&client_secret={YelpClientInfo.ClientSecret}";
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
                if (categoryProvided == true)
                {
                    var requestUrl =
                        $"https://api.yelp.com/v3/businesses/search?category_filter=breakfast_brunch&term={_categoryModel.Category}&latitude={_categoryModel.Latitude}&longitude={_categoryModel.Longitude}";
                    return requestUrl;

                }

                if (_radius != null)
                {
                    var requestUrl =
                    $"https://api.yelp.com/v3/businesses/search?term=brunch&latitude={_locationModel.Latitude}&longitude={_locationModel.Longitude}&radius={_radius}";
                    return requestUrl;
                }
                else
                {
                    var  requestUrl =
                        $"https://api.yelp.com/v3/businesses/search?term=brunch&latitude={_locationModel.Latitude}&longitude={_locationModel.Longitude}";
                    return requestUrl;
                }              
            }
        }      

        public string RequestBusiness(string id)
        {       
                var requestUrl =
                    $"https://api.yelp.com/v3/businesses/{id}";
                return requestUrl;            
        }

        public YelpClientInfo YelpClientInfo => new YelpClientInfo();
    }
}