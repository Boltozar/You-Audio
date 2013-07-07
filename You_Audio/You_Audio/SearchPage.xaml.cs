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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace You_Audio
{
    /// <summary>
    /// Interaction logic for SearchPage.xaml
    /// </summary>
    public partial class SearchPage : UserControl
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text != "" && txtSearch.Text != "Enter keyword to search...")
            {            
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += worker_DoWork;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync();
            }
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text != "")
            {
                txtSearch.Text = "";
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtSearch.Text != "" && txtSearch.Text != "Enter keyword to search...")
                {
                    Dispatcher.Invoke((Action)(()=>
                    {
                        Pages.ResultsPage.Update(txtSearch.Text);
                        Pages.MainWindow.SetPage(Pages.ResultsPage);
                        txtSearch.Clear();
                    }));
                }
            }
        }
        
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //txtsearch.text on different thread, must use dispatcher
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Pages.ResultsPage.Update(txtSearch.Text);
            }));
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Pages.MainWindow.SetPage(Pages.ResultsPage);
            txtSearch.Clear();
        }
    }
}
