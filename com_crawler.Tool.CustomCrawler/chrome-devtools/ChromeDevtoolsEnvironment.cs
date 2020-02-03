/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Community Crawler Developer

***/

using CefSharp.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using com_crawler.Tool.CustomCrawler.chrome_devtools.Response;

namespace com_crawler.Tool.CustomCrawler.chrome_devtools
{
    public class ChromeDevtoolsListElement
    {
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "devtoolsFrontendUrl")]
        public string DevtoolsFrontendUrl { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "webSocketDebuggerUrl")]
        public string WebSocketDebuggerUrl { get; set; }
    }

    /// <summary>
    /// This class is chrome devtools protocol wrapper.
    /// </summary>
    public class ChromeDevtoolsEnvironment : IDisposable
    {
        public static int Port = 8087;

        public static void Settings(ref CefSettings settings)
        {
            settings.RemoteDebuggingPort = Port;
        }

        private static long GetTime()
        {
            var st = new DateTime(1970, 1, 1);
            TimeSpan t = (DateTime.Now.ToUniversalTime() - st);
            return (long)(t.TotalMilliseconds + 0.5);
        }

        public static List<ChromeDevtoolsListElement> GetDebuggeeList()
        {
            var list = Network.NetCommon.DownloadString($"http://localhost:{Port}/json/list?t=" + GetTime());
            return JsonConvert.DeserializeObject<List<ChromeDevtoolsListElement>>(list);
        }

        public static ChromeDevtoolsEnvironment CreateInstance(ChromeDevtoolsListElement element)
        {
            return new ChromeDevtoolsEnvironment(element);
        }

        ChromeDevtoolsListElement target;
        ClientWebSocket wss;

        public ChromeDevtoolsEnvironment(ChromeDevtoolsListElement target_info)
        {
            target = target_info;

            wss = new ClientWebSocket();
        }

        public async Task Connect()
        {
            await wss.ConnectAsync(new Uri(target.WebSocketDebuggerUrl), CancellationToken.None);
        }

        List<ChromeDevtoolsResponse> response = new List<ChromeDevtoolsResponse>();

        public async Task Start()
        {
            await Task.WhenAll(Task.Run(async () =>
            {
                await send("{\"id\":1,\"method\":\"Network.enable\",\"params\":{\"maxPostDataSize\":65536}}");
            }),

            Task.Run(async () =>
            {
                var construct = "";
                while (wss.State == WebSocketState.Open)
                {
                    byte[] buffer = new byte[65535];
                    var result = await wss.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await wss.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    }
                    else
                    {
                        var content = Encoding.UTF8.GetString(buffer);
                        construct += content;
                        try
                        {
                            var response = JsonConvert.DeserializeObject<ChromeDevtoolsResponse>(construct);
                            response.RawMessage = content;
                            this.response.Add(response);
                            construct = "";

                            if (response.Method != null && response.Method.ToString() == "Network.requestWillBeSent")
                            {
                                var xx = JsonConvert.DeserializeObject<RequestWillBeSent>(response.Params.ToString());

                            }
                        }
                        catch (Exception e)
                        {
                            ;
                        }
                    }
                }
            }));
        }

        private async Task send(string content)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            await wss.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public void Dispose()
        {
            wss.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None).Wait();
        }
    }
}
