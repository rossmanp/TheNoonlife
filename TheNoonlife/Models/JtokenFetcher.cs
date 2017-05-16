using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace TheNoonlife.Models
{
    public class JtokenFetcher
    {
        //This method allows us to make an api request where only an api key is required
        public JToken GetJTokenWithKey(string webRequest)
        {
            var request =
                WebRequest.CreateHttp(webRequest);

            request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";

            var response = request.GetResponse();

            var reader = new StreamReader(response.GetResponseStream());

            var apiResult = reader.ReadToEnd();
            var json = JObject.Parse(apiResult);
            return json;
        }

        //This method makes a request to Yelp's Search api using an access token
        //We require a YelpApiRequest object to be passed as an argument
        public JToken GetYelpSearchJTokenWithAccessToken(YelpApiRequest yelp)
        {
            var webClient = new WebClient();
            webClient.Headers.Add("Authorization", "Bearer " + yelp.AccessToken);
            var requestResult = webClient.DownloadString(yelp.RequestUrl);
            var yelpJtoken = JObject.Parse(requestResult);
            return yelpJtoken;
        }

        //This method makes a request to Yelp's Business api using an access token
        public JToken GetYelpBusinessJTokenWithToken(string id)
        {
            var yelp = new YelpApiRequest();
            var webClient = new WebClient();
            webClient.Headers.Add("Authorization", "Bearer " + yelp.AccessToken);
            var requestResult = webClient.DownloadString(yelp.RequestBusiness(id));
            var yelpJtoken = JObject.Parse(requestResult);
            return yelpJtoken;
        }
    }
}