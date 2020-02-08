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

namespace com_crawler.Tool.CustomCrawler.chrome_devtools.Method.Debugger
{
    public class Resume
    {
        public const string Method = "Debugger.resume";

        [JsonProperty(PropertyName = "terminateOnResume")]
        public bool TerminateOnResume { get; set; }
    }
}
