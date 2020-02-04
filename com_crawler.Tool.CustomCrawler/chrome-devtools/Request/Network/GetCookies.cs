/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Community Crawler Developer

***/

using com_crawler.Tool.CustomCrawler.chrome_devtools.Types.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com_crawler.Tool.CustomCrawler.chrome_devtools.Request.Network
{
    public class GetCookies
    {
        public static string Method = "Network.getCookies";

        [JsonProperty(PropertyName = "urls")]
        public string[] Urls;
        [JsonProperty(PropertyName = "cookies")]
        public Cookie[] Cookies;
    }
}
