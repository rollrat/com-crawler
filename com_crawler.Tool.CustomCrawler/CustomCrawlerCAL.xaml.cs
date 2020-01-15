/***

   Copyright (C) 2018-2020. rollrat. All Rights Reserved.
   
   Author: Community Crawler Developer

***/

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public partial class CustomCrawlerCAL : Window
    {
        HtmlNode root_node;

        public CustomCrawlerCAL(HtmlNode root_node)
        {
            InitializeComponent();

            this.root_node = root_node;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //Result.Text = string.Join("\r\n", HtmlCAL.Calculate(Pattern.Text, root_node));
            }
            catch (Exception ex)
            {
                Result.Text = ex.Message + "\r\n" + ex.StackTrace;
            }
        }
    }
}
