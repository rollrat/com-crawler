﻿// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using com_crawler.Utils;
using EmbedIO;
using EmbedIO.Actions;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Swan.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace com_crawler.Server
{
    /// <summary>
    /// Community Crawler HTTP Server for API
    /// </summary>
    public class Server : ILazy<Server>
    {
        public async void StartServer(int port)
        {
            Logger.UnregisterLogger<ConsoleLogger>();
            using (var server = CreateWebServer($"http://localhost:{port}/"))
            {
                await server.RunAsync();
            }
        }

        private static WebServer CreateWebServer(string url)
        {
            var server = new WebServer(o => o
                    .WithUrlPrefix(url)
                    .WithMode(HttpListenerMode.EmbedIO))
                .WithLocalSessionManager()
                .WithWebApi("/api", m => m
                    .WithController<ServerAPI>())
                .WithModule(new ActionModule("/", HttpVerbs.Any, ctx => ctx.SendStringAsync("Community Crawler API Server", "text/html", Encoding.UTF8)));

            server.StateChanged += (s, e) => $"WebServer New State - {e.NewState}".Info();

            return server;
        }
    }

    public class ServerAPI : WebApiController
    {
        [Route(HttpVerbs.Get, "/test")]
        public void GetData()
        {
            var dd = HttpContext.GetRequestBodyAsStringAsync().Result;
            HttpContext.SendStringAsync("Test data! " + dd, "text", Encoding.UTF8);
        }
    }

}
