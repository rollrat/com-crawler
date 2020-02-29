// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using com_crawler;
using com_crawler.CL;
using com_crawler.Crypto;
using com_crawler.Extractor;
using com_crawler.Log;
using com_crawler.Network;
using com_crawler.Proxy;
using com_crawler.Setting;
using com_crawler.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Version = com_crawler.Version;

namespace com_crawler.Console
{
    public class Options : IConsoleOption
    {
        [CommandLine("--help", CommandType.OPTION)]
        public bool Help;
        [CommandLine("--version", CommandType.OPTION, ShortOption = "-v", Info = "Show version information.")]
        public bool Version;

        /// <summary>
        /// Atomic Options
        /// </summary>

        [CommandLine("--build-free-proxy", CommandType.OPTION, Info = "Build free proxy list.")]
        public bool BuildFreeProxy;

        /// <summary>
        /// Extractor Options
        /// </summary>

        [CommandLine("--list-extractor", CommandType.OPTION, Info = "Enumerate all implemented extractor.")]
        public bool ListExtractor;

        [CommandLine("--url", CommandType.ARGUMENTS, ArgumentsCount = 1,
            Info = "Set extracting target.", Help = "use --url <URL>")]
        public string[] Url;
        [CommandLine("--path-format", CommandType.ARGUMENTS, ShortOption = "-o", ArgumentsCount = 1,
            Info = "Set extracting file name format.", Help = "use -o <Output Format>")]
        public string[] PathFormat;

        [CommandLine("--extract-info", CommandType.OPTION, ShortOption = "-i", Info = "Extract information of url.", Help = "use -i")]
        public bool ExtractInformation;
        [CommandLine("--extract-link", CommandType.OPTION, ShortOption = "-l", Info = "Extract just links.", Help = "use -l")]
        public bool ExtractLinks;
        [CommandLine("--print-process", CommandType.OPTION, ShortOption = "-p", Info = "Print download processing.", Help = "use -p")]
        public bool PrintProcess;

        [CommandLine("--disable-download-progress", CommandType.OPTION, Info = "Disable download progress.", Help = "use --disable-download-progress")]
        public bool DisableDownloadProgress;

        [CommandLine("--download-path", CommandType.ARGUMENTS, ArgumentsCount = 1, Info = "Set download path manually.", Help = "use -p")]
        public string[] DownloadPath;

        /// <summary>
        /// Component Options
        /// </summary>

        [CommandLine("--build-component", CommandType.OPTION, Info = "Build component for fast querying.")]
        public bool BuildComponent;

        /// <summary>
        /// Bot Options
        /// </summary>

        [CommandLine("--start-bot", CommandType.OPTION, Info = "Start ChatBot server.", Help = "use --start-bot <port>")]
        public bool StartBot;

        /// <summary>
        /// Server
        /// </summary>

        [CommandLine("--start-server", CommandType.ARGUMENTS, ArgumentsCount = 1, Info = "Start API server.", Help = "use --start-server <port>")]
        public string[] StartServer;
    }

    public class Runnable
    {
        public static void Start(string[] arguments)
        {
            var origin = arguments;
            arguments = CommandLineUtil.SplitCombinedOptions(arguments);
            arguments = CommandLineUtil.InsertWeirdArguments<Options>(arguments, true, "--url");
            var option = CommandLineParser.Parse<Options>(arguments);

            //
            //  Single Commands
            //
            if (option.Help)
            {
                PrintHelp();
            }
            else if (option.Version)
            {
                PrintVersion();
            }
            else if (option.BuildFreeProxy)
            {
                FreeProxy.Instance.Build();
            }
            else if (option.ListExtractor)
            {
                foreach (var extractor in ExtractorManager.Extractors)
                {
                    System.Console.WriteLine($"[{extractor.GetType().Name}]");
                    System.Console.WriteLine($"[HostName] {extractor.HostName}");
                    System.Console.WriteLine($"[Checker] {extractor.ValidUrl}");
                    System.Console.WriteLine($"[Information] {extractor.ExtractorInfo}");
                    var builder = new StringBuilder();
                    CommandLineParser.GetFields(extractor.RecommendOption("").GetType()).ToList().ForEach(
                        x =>
                        {
                            var key = x.Key;
                            if (!key.StartsWith("--"))
                                return;
                            if (!string.IsNullOrEmpty(x.Value.Item2.ShortOption))
                                key = $"{x.Value.Item2.ShortOption}, " + key;
                            var help = "";
                            if (!string.IsNullOrEmpty(x.Value.Item2.Help))
                                help = $"[{x.Value.Item2.Help}]";
                            if (!string.IsNullOrEmpty(x.Value.Item2.Info))
                                builder.Append($"   {key}".PadRight(30) + $" {x.Value.Item2.Info} {help}\r\n");
                            else
                                builder.Append($"   {key}".PadRight(30) + $" {help}\r\n");
                        });
                    if (builder.ToString() != "")
                    {
                        System.Console.WriteLine($"[Options]");
                        System.Console.Write(builder.ToString());
                    }
                    System.Console.WriteLine($"-------------------------------------------------------------");
                }
            }
            else if (option.Url != null)
            {
                if (!(option.Url[0].StartsWith("https://") || option.Url[0].StartsWith("http://")))
                {
                    System.Console.WriteLine($"'{option.Url[0]}' is not correct url format or not supported scheme.");
                }

                var weird = CommandLineUtil.GetWeirdArguments<Options>(arguments);
                var n_args = new List<string>();

                weird.ForEach(x => n_args.Add(arguments[x]));

                ProcessExtract(option.Url[0], n_args.ToArray(), option.PathFormat, option.ExtractInformation, option.ExtractLinks, option.PrintProcess, option.DisableDownloadProgress, option.DownloadPath);
            }
            else if (option.StartBot)
            {
                ProcessStartChatBotServer();
            }
            else if (option.StartServer != null)
            {
                ProcessStartServer(option.StartServer);
            }
            else if (option.Error)
            {
                System.Console.WriteLine(option.ErrorMessage);
                if (option.HelpMessage != null)
                    System.Console.WriteLine(option.HelpMessage);
                return;
            }
            else
            {
                System.Console.WriteLine("Nothing to work on.");
                System.Console.WriteLine("Enter './com_crawler.Console --help' to get more information");
            }

            return;
        }

        static byte[] art_console = {
            0xCD, 0x92, 0x4D, 0x6E, 0x03, 0x21, 0x0C, 0x85, 0xF7, 0x91, 0x72, 0x07, 0x67, 0xD5, 0x4D, 0x70, 0xF6, 0x5C, 0x65, 0x3A, 0x38,
            0x27, 0xF1, 0xD9, 0xFB, 0x9E, 0x81, 0x86, 0xF9, 0xD5, 0x6C, 0x2A, 0xD5, 0x48, 0x83, 0x6D, 0x1E, 0x9F, 0x31, 0x83, 0xC8, 0x65,
            0x2B, 0xE5, 0x64, 0xF1, 0x7E, 0x5B, 0x8A, 0x54, 0x2F, 0x52, 0x37, 0xC2, 0x4E, 0xEA, 0x36, 0xCD, 0x17, 0x49, 0x1B, 0xE1, 0x92,
            0xA4, 0x79, 0x9A, 0xB3, 0x5D, 0x68, 0xE7, 0x49, 0xA1, 0x1E, 0x92, 0x34, 0x0B, 0x05, 0x92, 0xD3, 0x79, 0x8B, 0xCF, 0xD4, 0x85,
            0xBA, 0x4B, 0x22, 0xA7, 0x09, 0x24, 0xBF, 0xD5, 0x8E, 0x30, 0xFA, 0x35, 0x0A, 0x75, 0x43, 0xAA, 0x9C, 0x5F, 0x01, 0x47, 0xDA,
            0xD0, 0x2C, 0x4E, 0xB3, 0x12, 0xEA, 0x40, 0x32, 0xED, 0x9C, 0x05, 0xA9, 0xD1, 0xBA, 0xE9, 0x87, 0xB2, 0x5B, 0x32, 0x48, 0xF9,
            0x33, 0x78, 0xE3, 0x97, 0xC6, 0x52, 0xD8, 0x48, 0x8F, 0xC1, 0xA6, 0x79, 0x8C, 0x4E, 0x6C, 0x25, 0xAC, 0xA4, 0x32, 0xD8, 0x34,
            0x8F, 0xD1, 0x89, 0xAD, 0x84, 0x7F, 0xF7, 0x32, 0xFF, 0x11, 0xC9, 0xBD, 0x24, 0x2F, 0xE6, 0xC5, 0x5F, 0x22, 0xDF, 0x11, 0x38,
            0xA2, 0xE4, 0x56, 0x3C, 0xC1, 0x2D, 0x1E, 0x13, 0xB2, 0x48, 0xD5, 0x4C, 0x17, 0x86, 0xCC, 0x53, 0x23, 0xB9, 0x18, 0x03, 0xE4,
            0x62, 0x23, 0x5C, 0x6A, 0xE9, 0x99, 0xB0, 0x02, 0x3E, 0x06, 0x79, 0x49, 0xD4, 0x91, 0x64, 0x55, 0xEB, 0x09, 0x2A, 0x87, 0x3F,
            0x76, 0xE7, 0xD2, 0x74, 0xA0, 0x3B, 0xC8, 0x48, 0x00, 0x27, 0x02, 0x3E, 0x84, 0x60, 0x31, 0xE4, 0x1A, 0xFC, 0x48, 0x71, 0x85,
            0xAF, 0x88, 0x07, 0x30, 0x5F, 0xDC, 0x13, 0x75, 0x28, 0xC6, 0x0A, 0xB1, 0x2D, 0xB6, 0xA3, 0x1F, 0x6E, 0x95, 0x68, 0x0C, 0x53,
            0x40, 0x50, 0x12, 0xC4, 0xF8, 0x56, 0xF1, 0xF6, 0xC6, 0x99, 0xE7, 0xC1, 0xEB, 0x14, 0x28, 0x86, 0xC1, 0xC0, 0x56, 0x22, 0xE1,
            0x33, 0x55, 0x4F, 0x19, 0x8D, 0x1C, 0xFE, 0x3B, 0x9E, 0x82, 0x6A, 0x16, 0xF3, 0xE8, 0x33, 0xF6, 0xA6, 0x42, 0x34, 0x7A, 0xE7,
            0xD5, 0xB1, 0x75, 0xF4, 0x34, 0x50, 0xF6, 0x48, 0xF5, 0xD2, 0xE2, 0xD2, 0xA3, 0x6C, 0x1C, 0x1F, 0xC5, 0x19, 0x01, 0xDC, 0x9C,
            0xBD, 0x67, 0x71, 0xBF, 0xFD, 0x00,
        };

        static void PrintHelp()
        {
            PrintVersion();
            System.Console.WriteLine(Encoding.UTF8.GetString(CompressUtils.Decompress(art_console)));
            System.Console.WriteLine($"Copyright (C) 2020. Commnunity Crawler Developer");
            System.Console.WriteLine($"E-Mail: rollrat.cse@gmail.com");
            System.Console.WriteLine($"Source-code: https://github.com/rollrat/com_crawler");
            System.Console.WriteLine($"");
            System.Console.WriteLine("Usage: ./com_crawler.Console [OPTIONS...] <URL> [URL OPTIONS ...]");

            var builder = new StringBuilder();
            CommandLineParser.GetFields(typeof(Options)).ToList().ForEach(
                x =>
                {
                    var key = x.Key;
                    if (!key.StartsWith("--"))
                        return;
                    if (!string.IsNullOrEmpty(x.Value.Item2.ShortOption))
                        key = $"{x.Value.Item2.ShortOption}, " + key;
                    var help = "";
                    if (!string.IsNullOrEmpty(x.Value.Item2.Help))
                        help = $"[{x.Value.Item2.Help}]";
                    if (!string.IsNullOrEmpty(x.Value.Item2.Info))
                        builder.Append($"   {key}".PadRight(30) + $" {x.Value.Item2.Info} {help}\r\n");
                    else
                        builder.Append($"   {key}".PadRight(30) + $" {help}\r\n");
                });
            System.Console.Write(builder.ToString());

            System.Console.WriteLine($"");
            System.Console.WriteLine("Enter './com_crawler.Console --list-extractor' to get more url options.");
        }

        public static void PrintVersion()
        {
            System.Console.WriteLine($"{Version.Name} {Version.Text}");
            System.Console.WriteLine($"Build Date: " + Internals.GetBuildDate().ToLongDateString());
        }

        static void ProcessExtract(string url, string[] args, string[] PathFormat, bool ExtractInformation, bool ExtractLinks, bool PrintProcess, bool DisableDownloadProgress, string[] DownloadPath)
        {
            var extractor = ExtractorManager.Instance.GetExtractor(url);

            if (extractor == null)
            {
                extractor = ExtractorManager.Instance.GetExtractorFromHostName(url);

                if (extractor == null)
                {
                    System.Console.WriteLine($"[Error] Cannot find a suitable extractor for '{url}'.");
                    return;
                }
                else
                {
                    System.Console.WriteLine("[Warning] Found an extractor for that url, but the url is not in the proper format to continue.");
                    System.Console.WriteLine("[Warning] Please refer to the following for proper conversion.");
                    System.Console.WriteLine($"[Input URL] {url}");
                    System.Console.WriteLine($"[Extractor Name] {extractor.GetType().Name}");
                    System.Console.WriteLine(extractor.ExtractorInfo);
                    return;
                }
            }
            else if (extractor != null)
            {
                try
                {
                    System.Console.WriteLine("Extractor Selected: " + extractor.GetType().Name.Replace("Extractor", ""));

                    if (extractor.IsForbidden)
                    {
                        System.Console.WriteLine("Crawling is prohibited by subject of recommendation in robots.txt provided by that website.");
                        return;
                    }

                    System.Console.Write("Extracting urls... ");

                    WaitProgress wp = null;

                    if (PrintProcess)
                    {
                        Logs.Instance.AddLogNotify((s, e) => {
                            var tuple = s as Tuple<DateTime, string, bool>;
                            CultureInfo en = new CultureInfo("en-US");
                            System.Console.WriteLine($"[{tuple.Item1.ToString(en)}] {tuple.Item2}");
                        });
                    }
                    else
                    {
                        if (!DisableDownloadProgress)
                            wp = new WaitProgress();
                    }

                    var option = extractor.RecommendOption(url);
                    option.CLParse(ref option, args);

                    if (option.Error)
                    {
                        if (wp != null) wp.Dispose();
                        System.Console.WriteLine($"[Input URL] {url}");
                        System.Console.WriteLine($"[Extractor Name] {extractor.GetType().Name}");
                        System.Console.WriteLine(option.ErrorMessage);
                        if (option.HelpMessage != null)
                            System.Console.WriteLine(option.HelpMessage);
                        return;
                    }

                    long extracting_progress_max = 0;
                    ExtractingProgressBar epb = null;

                    option.ProgressMax = (count) =>
                    {
                        extracting_progress_max = count;
                        if (wp != null)
                        {
                            wp.Dispose();
                            wp = null;
                            epb = new ExtractingProgressBar();
                            epb.Report(extracting_progress_max, 0);
                        }
                    };

                    long extracting_cumulative_count = 0;

                    option.PostStatus = (count) =>
                    {
                        var val = Interlocked.Add(ref extracting_cumulative_count, count);
                        if (epb != null)
                            epb.Report(extracting_progress_max, extracting_cumulative_count);
                    };

                    var tasks = extractor.Extract(url, option);

                    if (epb != null)
                    {
                        epb.Dispose();
                        System.Console.WriteLine("Done.");
                    }

                    if (wp != null)
                    {
                        wp.Dispose();
                        System.Console.WriteLine("Done.");
                    }

                    if (ExtractLinks)
                    {
                        foreach (var uu in tasks.Item1)
                            System.Console.WriteLine(uu.Url);
                        return;
                    }

                    string format;

                    if (PathFormat != null)
                        format = PathFormat[0];
                    else
                        format = extractor.RecommendFormat(option);

                    if (ExtractInformation)
                    {
                        System.Console.WriteLine($"[Input URL] {url}");
                        System.Console.WriteLine($"[Extractor Name] {extractor.GetType().Name}");
                        System.Console.WriteLine($"[Information] {extractor.ExtractorInfo}");
                        System.Console.WriteLine($"[Format] {format}");
                        return;
                    }

                    if (tasks.Item1 == null)
                    {
                        if (tasks.Item2 == null)
                        {
                            System.Console.WriteLine($"[Input URL] {url}");
                            System.Console.WriteLine($"[Extractor Name] {extractor.GetType().Name}");
                            System.Console.WriteLine("Nothing to work on.");
                            return;
                        }

                        System.Console.WriteLine(Logs.SerializeObject(tasks.Item2));
                        return;
                    }

                    int download_count = 0;

                    ProgressBar pb = null;

                    if (!PrintProcess && !DisableDownloadProgress)
                    {
                        System.Console.Write("Download files... ");
                        pb = new ProgressBar();
                    }

                    var downloadpath = Settings.Instance.Model.SuperPath;
                    if (DownloadPath != null && DownloadPath.Length > 0)
                        downloadpath = DownloadPath[0];

                    tasks.Item1.ForEach(task => {
                        task.Filename = Path.Combine(downloadpath, task.Format.Formatting(format));
                        if (!Directory.Exists(Path.GetDirectoryName(task.Filename)))
                            Directory.CreateDirectory(Path.GetDirectoryName(task.Filename));
                        if (!PrintProcess && !DisableDownloadProgress)
                        {
                            task.DownloadCallback = (sz) =>
                                pb.Report(tasks.Item1.Count, download_count, sz);
                            task.CompleteCallback = () =>
                                Interlocked.Increment(ref download_count);
                        }
                        AppProvider.Scheduler.Add(task);
                    });

                    while (AppProvider.Scheduler.busy_thread != 0)
                    {
                        Thread.Sleep(500);
                    }

                    if (pb != null)
                    {
                        pb.Dispose();
                        System.Console.WriteLine("Done.");
                    }

                    WaitPostprocessor wpp = null;

                    if (AppProvider.PPScheduler.busy_thread != 0 && !PrintProcess && !DisableDownloadProgress)
                    {
                        System.Console.Write("Wait postprocessor... ");
                        wpp = new WaitPostprocessor();
                    }

                    while (AppProvider.PPScheduler.busy_thread != 0)
                    {
                        if (wpp != null) wpp.Report(AppProvider.PPScheduler.busy_thread + AppProvider.PPScheduler.queue.Count);
                        Thread.Sleep(500);
                    }

                    if (wpp != null)
                    {
                        wpp.Dispose();
                        System.Console.WriteLine("Done.");
                    }
                }
                catch (Exception e)
                {
                    Logs.Instance.PushError("[Extractor] Unhandled Exception - " + e.Message + "\r\n" + e.StackTrace);
                }
            }
        }

        static void ProcessStartServer(string[] args)
        {
            Server.Server.Instance.StartServer(Convert.ToInt32(args[0]));

            while (true)
            {
                Thread.Sleep(500);
            }
        }

        static void ProcessStartChatBotServer()
        {
            ChatBot.BotManager.Instance.StartBots();

            while (true)
            {
                Thread.Sleep(500);
            }
        }
    }

}
