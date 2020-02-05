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
            browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged; ;
        }

        private void Browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        ChromeDevtoolsEnvironment env;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            browser.Load(URLText.Text);

            if (env == null)
            {
                var target = ChromeDevtoolsEnvironment.GetDebuggeeList().Where(x => x.Url == URLText.Text);

                if (target.Count() == 0)
                    return;

                env = ChromeDevtoolsEnvironment.CreateInstance(target.First());

                new CustomCrawlerDynamicsRequest(env).Show();

                await env.Connect();
                await env.Start();
            }
        }
    }
}
