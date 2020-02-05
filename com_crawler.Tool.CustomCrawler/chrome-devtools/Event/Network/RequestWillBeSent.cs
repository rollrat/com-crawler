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
    public class RequestWillBeSent
    {
        [JsonProperty(PropertyName = "requestId")]
        public string RequestId { get; set; }
        [JsonProperty(PropertyName = "loaderId")]
        public string LoaderId { get; set; }
        [JsonProperty(PropertyName = "documentURL")]
        public string DocumentURL { get; set; }
        [JsonProperty(PropertyName = "request")]
        public Types.Network.Request Request { get; set; }
        [JsonProperty(PropertyName = "timestamp")]
        public double TimeStamp { get; set; }
        [JsonProperty(PropertyName = "wallTime")]
        public double WallTime { get; set; }
        [JsonProperty(PropertyName = "initiator")]
        public Initiator Initiator { get; set; }
        [JsonProperty(PropertyName = "redirectResponse")]
        public Types.Network.Response Response { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string ResourceType { get; set; }
        [JsonProperty(PropertyName = "frameId")]
        public string FrameId { get; set; }
        [JsonProperty(PropertyName = "hasUserGesture")]
        public bool HasUserGesture { get; set; }
    }
}
