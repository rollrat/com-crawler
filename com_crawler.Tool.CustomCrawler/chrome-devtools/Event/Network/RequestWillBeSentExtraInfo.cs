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

namespace com_crawler.Tool.CustomCrawler.chrome_devtools.Event.Network
{
    public class RequestWillBeSentExtraInfo
    {
        public const string Event = "Network.requestWillBeSentExtraInfo";

        [JsonProperty(PropertyName = "requestId")]
        public string RequestId { get; set; }
        [JsonProperty(PropertyName = "blockedCookies")]
        public BlockedCookieWithReason[] BlockedCookies { get; set; }
        [JsonProperty(PropertyName = "headers")]
        public Dictionary<string, string> Headers { get; set; }
    }
}
