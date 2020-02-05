/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Community Crawler Developer

***/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com_crawler.Tool.CustomCrawler.chrome_devtools.Types.Network
{
    public class BlockedCookieWithReason
    {
        [JsonProperty(PropertyName = "blockedReasons")]
        public string[] BlockedReasons { get; set; }
        [JsonProperty(PropertyName = "cookie")]
        public Cookie Cookie { get; set; }
    }
}
