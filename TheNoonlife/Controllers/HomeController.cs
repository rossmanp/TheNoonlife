using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace TheNoonlife.Controllers
{
    public class HomeController : Controller
    {
        const string clientID = "Z_gnHdaLf8031URZ0EkRzg";
        const string clientSecret = "lClwLGU6P5N2xrjm3dtKY7iA8pO697Qr5p6b7TGtc6J5QGSDecdUTndloC5jskGQ";
        public ActionResult SearchEx()
        {
            var strUrl = "https://api.yelp.com/oauth2/token";
            var strPostData = "grant_type=client_credentials";
            strPostData += "&client_id=" + clientID;
            strPostData += "&client_secret=" + clientSecret;
            System.Net.WebClient wc = new System.Net.WebClient();
            var strResponse = wc.UploadString(strUrl, strPostData);
            var login = JavascriptDeserialize<OAuthLogin>(strResponse);
            var strSearch = "https://api.yelp.com/v3/businesses/search?location=49523";
            wc.Headers.Add("Authorization", "Bearer " + login.access_token);
            var strSearchResponse = wc.DownloadString(strSearch);
            ViewBag.Result = strSearchResponse;
            return View();
        }

        public static T JavascriptDeserialize<T>(string json)
        {
            var jsSerializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            return jsSerializer.Deserialize<T>(json);
        }

        public class OAuthLogin
        {
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public string access_token { get; set; }
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