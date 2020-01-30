/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Community Crawler Developer

***/

using CefSharp;
using CefSharp.Wpf;
using com_crawler.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// CustomCrawlerCluster.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomCrawlerCluster : Window
    {
        ChromiumWebBrowser browser;
        string url;
        HtmlTree tree;

        public CustomCrawlerCluster(string url, HtmlTree tree)
        {
            InitializeComponent();

            //Cef.Initialize(new CefSettings());
            browser = new ChromiumWebBrowser(string.Empty);
            browserContainer.Content = browser;
            browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;

            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            browser.JavascriptObjectRepository.Register("ccw", new CallbackCCW(this), isAsync: true);

            this.url = url;
            this.tree = tree;

            ResultList.DataContext = new CustomCrawlerClusterDataGridViewModel();
            ResultList.Sorting += new DataGridSortingEventHandler(new DataGridSorter<CustomCrawlerClusterDataGridItemViewModel>(ResultList).SortHandler);

            for (int i = 0; i <= tree.Height; i++)
            {
                for (int j = 0; j < tree[i].Count; j++)
                {
                    if (tree[i][j].Name != "#comment" && tree[i][j].Name != "#text")
                    {
                        tree[i][j].SetAttributeValue("ccw_tag", $"ccw_{i}_{j}");
                        tree[i][j].SetAttributeValue("onmouseenter", $"ccw.hoverelem('ccw_{i}_{j}')");
                        tree[i][j].SetAttributeValue("onmouseleave", $"ccw.hoverelem('ccw_{i}_{j}')");
                    }
                }
            }
        }

        private void Browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                browser.LoadHtml(tree[0][0].OuterHtml, url);
            }
            catch { }
        }

        public class CallbackCCW
        {
            CustomCrawlerCluster instance;
            string before = "";
            string before_border = "";
            public CallbackCCW(CustomCrawlerCluster instance)
            {
                this.instance = instance;
            }
            public void hoverelem(string elem)
            {
                var i = Convert.ToInt32(elem.Split('_')[1]);
                var j = Convert.ToInt32(elem.Split('_')[2]);
                Application.Current.Dispatcher.BeginInvoke(new Action(
                delegate
                {
                    instance.hover_item.Text = instance.tree[i][j].XPath;
                    instance.browser.EvaluateScriptAsync($"document.querySelector('[{before}]').style.border = '{before_border}';").Wait();
                    before = $"ccw_tag=ccw_{i}_{j}";
                    //before_border = instance.browser.EvaluateScriptAsync($"document.querySelector('[{before}]').style.border").Result.Result.ToString();
                    instance.browser.EvaluateScriptAsync($"document.querySelector('[{before}]').style.border = '0.2em solid red';").Wait();
                }));
            }
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            if ((Functions.SelectedItem as ComboBoxItem).Content.ToString() == "LinearClustering")
            {
                var rr = tree.LinearClustering();
                var list = new List<CustomCrawlerClusterDataGridItemViewModel>();

                for (int i = 0; i < rr.Count; i++)
                {
                    list.Add(new CustomCrawlerClusterDataGridItemViewModel
                    {
                        Index = (i + 1).ToString(),
                        Count = rr[i].Item1.ToString("#,#"),
                        Accuracy = rr[i].Item2.ToString(),
                        Header = rr[i].Item3.Name + "+" + string.Join("/", rr[i].Item4.Select(x => x.Name)),
                        Node = rr[i].Item3
                    });
                }

                ResultList.DataContext = new CustomCrawlerClusterDataGridViewModel(list);
            }
        }

        string before = "";
        bool section = false;

        private void ResultList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (ResultList.SelectedItems.Count > 0)
            {
                var node = (ResultList.SelectedItems[0] as CustomCrawlerClusterDataGridItemViewModel).Node;

                if (section)
                {
                    browser.LoadHtml(tree[0][0].OuterHtml, url);
                    Thread.Sleep(100);
                    section = false;
                }

                browser.EvaluateScriptAsync($"document.querySelector('[{before}]').style.border = '0em';").Wait();
                before = $"ccw_tag={node.GetAttributeValue("ccw_tag", "")}";
                browser.EvaluateScriptAsync($"document.querySelector('[{before}]').style.border = '1em solid #FDFF47';").Wait();
                browser.EvaluateScriptAsync($"document.querySelector('[{before}]').scrollIntoView(true);").Wait();
            }
        }

        private void ResultList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ResultList.SelectedItems.Count > 0)
            {
                var node = (ResultList.SelectedItems[0] as CustomCrawlerClusterDataGridItemViewModel).Node;

                if (node.Name == "tbody")
                    browser.LoadHtml($"<table>{node.OuterHtml}</table>", url);
                else
                    browser.LoadHtml(node.OuterHtml, url);

                section = true;
            }
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            //browser.
        }

    }
}
