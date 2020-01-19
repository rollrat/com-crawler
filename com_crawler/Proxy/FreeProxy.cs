// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using com_crawler.Log;
using com_crawler.Network;
using com_crawler.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace com_crawler.Proxy
{
    /// <summary>
    /// You may need to use a Proxy, VPN, or Tor to handle many requests at once.
    /// The default proxy configuration provided by the community crawrling engine
    /// allows you to use proxies, but most people take a long time to populate this
    /// list. As a result, community crawling engines automatically find high quality
    /// proxies and provide list of proxies.
    /// </summary>
    public class FreeProxy : ILazy<FreeProxy>
    {
        public List<ProxyInfo> Proxy;

        public bool IsBuildRequire()
        {
            return !File.Exists(Path.Combine(AppProvider.ApplicationPath, "freeproxy.txt"));
        }

        public void Load()
        {
            Proxy = Extends.ReadJson<List<ProxyInfo>>(Path.Combine(AppProvider.ApplicationPath, "freeproxy.txt")).Result;
        }

        /// <summary>
        /// This build process should be run atomically.
        /// </summary>
        /// <param name="output_filename"></param>
        public void Build()
        {
            NetTaskPass.RemoveFromPasses<FreeProxyPass>();

            var proxies = new List<ProxyInfo>();
            proxies.AddRange(BuildFromFreeProxyListsNet());

            Extends.WriteJson(Path.Combine(AppProvider.ApplicationPath, "freeproxy.txt"), proxies);
        }

        public class ProxyInfo
        {
            public string IP;
            public int Port;
            public string Country;
            public bool IsSecureSupport;
            public bool IsAnonymityGuaranteed;
            public double UploadTime;
            public double Latency;
        }

        /// <summary>
        /// Collect proxy lists from http://www.freeproxylists.net/.
        /// </summary>
        /// <returns></returns>
        private List<ProxyInfo> BuildFromFreeProxyListsNet()
        {
            var htmls = new List<string>();

            for (int i = 0; i < 23; i++)
            {
                htmls.Add(NetTools.DownloadString($"http://www.freeproxylists.net/?s=u&page={i + 1}"));
                File.WriteAllText($"{i}.html", htmls.Last());
                Thread.Sleep(1000);
            }

            var result = new List<ProxyInfo>();

            return result;
        }

        /// <summary>
        /// Collect porxy lists from 
        /// </summary>
        /// <returns></returns>
        private List<ProxyInfo> BuildFromFreeProxyCZ()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Collect proxy lists from http://spys.one/en/https-ssl-proxy/
        /// </summary>
        /// <returns></returns>
        private List<ProxyInfo> BuildFromSpysOne()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Collect proxy lists form https://openproxy.space/
        /// </summary>
        /// <returns></returns>
        private List<ProxyInfo> BuildFormOpenProxySpace()
        {
            throw new NotImplementedException();
        }
    }

    public class FreeProxyPass : NetTaskPass
    {
        public static void Init()
        {
            Passes.Add(new FreeProxyPass());
        }

        public override void RunOnPass(ref NetTask content)
        {
            if (FreeProxy.Instance.Proxy == null || FreeProxy.Instance.Proxy.Count == 0)
            {
                Logs.Instance.Push("[Free Proxy Pass] Free Proxy is not initialized!");

                //
                //  Raise Critical Error
                //

                throw new Exception("[Free Proxy Pass] Free Proxy is not initialized!");
            }

            var proxy = FreeProxy.Instance.Proxy[new Random().Next(FreeProxy.Instance.Proxy.Count)];
            content.Proxy = new WebProxy(proxy.IP, proxy.Port);
        }
    }
}
