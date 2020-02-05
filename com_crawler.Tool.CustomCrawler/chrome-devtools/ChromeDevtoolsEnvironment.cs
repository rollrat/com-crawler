/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Community Crawler Developer

***/

using CefSharp.Wpf;
using com_crawler.Tool.CustomCrawler.chrome_devtools.Event;
using com_crawler.Tool.CustomCrawler.chrome_devtools.Event.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

    public class ChromeDevtoolsOptions
    {
        public const string Network = "{\"id\":1,\"method\":\"Network.enable\",\"params\":{\"maxPostDataSize\":65536}}";
    }

    /// <summary>
    /// This class is chrome devtools protocol wrapper.
    /// </summary>
    public class ChromeDevtoolsEnvironment : IDisposable
    {
        public static int Port = 8087;
        Timer timer;

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
        int id_count = 2;

        public ChromeDevtoolsEnvironment(ChromeDevtoolsListElement target_info)
        {
            target = target_info;

            wss = new ClientWebSocket();
            timer = new Timer(timer_callback, null, 0, 500);
        }

        private void timer_callback(object obj)
        {
            Task.Run(async () => await send($"{{\"id\":{id_count++}}}"));
        }

        public async Task Connect()
        {
            await wss.ConnectAsync(new Uri(target.WebSocketDebuggerUrl), CancellationToken.None);
        }


        Dictionary<string, List<object>> events = new Dictionary<string, List<object>>();

        public async Task Start()
        {
            await Task.WhenAll(Task.Run(async () =>
            {
                await send(ChromeDevtoolsOptions.Network);
            }),

            Task.Run(async () =>
            {
                var construct = new StringBuilder();
                byte[] buffer = new byte[65535];
                while (wss.State == WebSocketState.Open)
                {
                    var result = await wss.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await wss.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    }
                    else
                    {
                        var content = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        construct.Append(content);
                        try
                        {
                            var response = JsonConvert.DeserializeObject<ChromeDevtoolsResponse>(construct.ToString());
                            response.RawMessage = content;
                            construct.Clear();

                            if (response.Method != null && response.Method.ToString() == "Network.requestWillBeSent")
                            {
                                var xx = JsonConvert.DeserializeObject<RequestWillBeSent>(response.Params.ToString());
                                raise_event("RequestWillBeSent", xx);
                            }
                            else if (response.Method != null && response.Method.ToString() == "Network.requestWillBeSentExtraInfo")
                            {
                                var xx = JsonConvert.DeserializeObject<RequestWillBeSentExtraInfo>(response.Params.ToString());
                                raise_event("RequestWillBeSentExtraInfo", xx);
                            }
                            else if (response.Method != null && response.Method.ToString() == "Network.responseReceived")
                            {
                                var xx = JsonConvert.DeserializeObject<ResponseReceived>(response.Params.ToString());
                                raise_event("ResponseReceived", xx);
                            }
                            else if (response.Method != null && response.Method.ToString() == "Network.responseReceivedExtraInfo")
                            {
                                var xx = JsonConvert.DeserializeObject<RequestWillBeSent>(response.Params.ToString());
                                raise_event("ResponseReceivedExtraInfo", xx);
                            }

                            // ignore other events
                        }
                        catch { }
                    }
                }
            }));
        }

        private async Task send(string content)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            await wss.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private void raise_event<T>(string what, T obj)
        {
            if (events.ContainsKey(what))
            {
                var ll = events[what];
                ll.ForEach(x => (x as Action<T>).Invoke(obj));
            }
        }

        string[] events_list = new[] { "RequestWillBeSent", "RequestWillBeSentExtraInfo", "ResponseReceived", "ResponseReceivedExtraInfo" };

        public void Subscribe<T>(Action<T> callback)
        {
            foreach (var event_name in events_list)
            {
                if (typeof(T).Name == event_name)
                {
                    if (!events.ContainsKey(event_name))
                        events.Add(event_name, new List<object>());
                    events[event_name].Add(callback);
                }
            }
        }

        public void Send(ChromeDevtoolsResponse what)
        {
        }

        public void Dispose()
        {
            wss.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None).Wait();
            timer.Dispose();
        }
    }
}
