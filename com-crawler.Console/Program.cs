// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using com_crawler.Log;
using System;
using System.Globalization;
using System.Text;

namespace com_crawler.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            AppProvider.Initialize();

            Logs.Instance.AddLogErrorNotify((s, e) => {
                var tuple = s as Tuple<DateTime, string, bool>;
                CultureInfo en = new CultureInfo("en-US");
                System.Console.Error.WriteLine($"[{tuple.Item1.ToString(en)}] [Error] {tuple.Item2}");
            });

            try
            {
                Runnable.Start(args);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("An error occured! " + e.Message);
                System.Console.WriteLine(e.StackTrace);
                System.Console.WriteLine("Please, check log.txt file.");
            }

            AppProvider.Deinitialize();

            Environment.Exit(0);
        }
    }
}
