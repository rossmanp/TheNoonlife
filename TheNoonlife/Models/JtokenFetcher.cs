﻿using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace TheNoonlife.Models
{
    public class JtokenFetcher
    {
        public JToken GetJTokenWithKey(string webRequest)
        {
            HttpWebRequest request =
                WebRequest.CreateHttp(webRequest);

            request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream());

            string apiResult = reader.ReadToEnd();
            var json = JObject.Parse(apiResult);
            return json;
        }

        public JToken GetJTokenWithToken(YelpApiRequest yelp)
        {
            var webClient = new WebClient();
            webClient.Headers.Add("Authorization", "Bearer " + yelp.AccessToken);
            var requestResult = webClient.DownloadString(yelp.RequestUrl);
            var yelpJtoken = JObject.Parse(requestResult);
            return yelpJtoken;
        }
    }
}