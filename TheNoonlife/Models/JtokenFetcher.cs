﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;

namespace TheNoonlife.Models
{
    public class JtokenFetcher
    {
        public JToken GetJToken(string webRequest)
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
    }
}