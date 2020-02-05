/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Community Crawler Developer

***/

using CefSharp;
using CefSharp.Wpf;
using com_crawler.Tool.CustomCrawler.chrome_devtools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace com_crawler.Tool.CustomCrawler
{
    /// <summary>
    /// CustomCrawlerDynamics.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomCrawlerDynamics : Window
    {
        ChromiumWebBrowser browser;

        public CustomCrawlerDynamics()
        {
            InitializeComponent();

            browser = new ChromiumWebBrowser(string.Empty);
            browserContainer.Content = browser;

            Closed += CustomCrawlerDynamics_Closed;
        }

        private void CustomCrawlerDynamics_Closed(object sender, EventArgs e)
        {
            if (env != null)
                env.Dispose();
        }

        ChromeDevtoolsEnvironment env;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (env == null)
            {
                var token = new Random().Next();
                browser.LoadHtml(token.ToString());

                var target = ChromeDevtoolsEnvironment.GetDebuggeeList().Where(x => x.Title == $"data:text/html,{token}");
                env = ChromeDevtoolsEnvironment.CreateInstance(target.First());
                new CustomCrawlerDynamicsRequest(env).Show();

                await env.Connect();
                await env.Option();

                _ = Task.Run(async () => { await env.Start(); });
            }

            browser.Load(URLText.Text);
        }
    }
}
