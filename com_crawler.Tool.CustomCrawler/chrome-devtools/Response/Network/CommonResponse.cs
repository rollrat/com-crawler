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

namespace com_crawler.Tool.CustomCrawler.chrome_devtools.Response.Network
{
    public class ChromeDevtoolsResponse
    {
        public string RawMessage { get; set; }

        [JsonProperty(PropertyName = "id")]
        public object Id { get; set; }
        [JsonProperty(PropertyName = "result")]
        public object Result { get; set; }
        [JsonProperty(PropertyName = "method")]
        public object Method { get; set; }
        [JsonProperty(PropertyName = "params")]
        public object Params { get; set; }
        [JsonProperty(PropertyName = "error")]
        public object Error { get; set; }
    }
}
