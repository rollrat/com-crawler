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

namespace com_crawler.Tool.CustomCrawler.chrome_devtools.Method.Network
{
    public class Enable
    {
        public const string Method = "Network.enable";

        [JsonProperty(PropertyName = "maxTotalBufferSize")]
        public int MaxTotalBufferSize;
        [JsonProperty(PropertyName = "maxResourceBufferSize")]
        public int MaxResourceBufferSize;
        [JsonProperty(PropertyName = "maxPostDataSize")]
        public int MaxPostDataSize;
    }
}
