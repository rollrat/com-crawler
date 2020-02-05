using com_crawler.Tool.CustomCrawler.chrome_devtools;
using com_crawler.Tool.CustomCrawler.chrome_devtools.Response.Network;
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
    /// <summary>
    /// CustomCrawlerDynamicsRequest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomCrawlerDynamicsRequest : Window
    {
        int index_count = 0;

        public CustomCrawlerDynamicsRequest(ChromeDevtoolsEnvironment env)
        {
            InitializeComponent();

            RequestList.DataContext = new CustomCrawlerDynamicsRequestDataGridViewModel();
            RequestList.Sorting += new DataGridSortingEventHandler(new DataGridSorter<CustomCrawlerDynamicsRequestDataGridItemViewModel>(RequestList).SortHandler);

            env.Subscribe<RequestWillBeSent>(x =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(
                delegate
                {
                    (RequestList.DataContext as CustomCrawlerDynamicsRequestDataGridViewModel).Items.Add(new CustomCrawlerDynamicsRequestDataGridItemViewModel
                    {
                        Id = (++index_count).ToString(),
                        Url = x.Request.Url,
                        Type = x.ResourceType,
                        Request = x
                    });
                }));
            });

            Closed += (s, e) =>
            {
                env.Dispose();
            };
        }

        private void RequestList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }
    }
}
