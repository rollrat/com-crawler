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

namespace com_crawler.Tool.CustomCrawler.chrome_devtools.Method.DOM
{
    public class GetDocument
    {
        public static string Method = "DOM.getDocument";

        [JsonProperty(PropertyName = "depth")]
        public int Depth;
        [JsonProperty(PropertyName = "pierce")]
        public bool Pierce;
    }
}
