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
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Extensions.MediaRss;
using Google.GData.YouTube;
using Google.YouTube;


namespace You_Audio
{
    /// <summary>
    /// Interaction logic for ResultsPage.xaml
    /// </summary>
    public partial class ResultsPage : UserControl
    {
        private Feed<Video> _searchResults;
        private string _downloadDir;

        public ResultsPage()
        {
            InitializeComponent();
            _downloadDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        public void Update(string keyword)
        {
            _searchResults = YouTube.SearchForVideo(keyword);
            lblDirectory.Content = "Download to: " + _downloadDir;
            foreach (Video entry in _searchResults.Entries)
            {
                listboxResults.Items.Add(entry.Title + "\n" + "Views: " + entry.ViewCount + "   Rating: " + entry.RatingAverage + "\n");
                /*
                if (entry.Thumbnails.Count > 0)
                {
                    MediaThumbnail thumbnail = entry.Thumbnails[0];
                }
                 */
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            listboxResults.Items.Clear();
            Pages.MainWindow.SetPage(Pages.SearchPage);
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            if (listboxResults.SelectedItem != null)
            {
                int idx = listboxResults.SelectedIndex;
                string videoID = _searchResults.Entries.ElementAt(idx).VideoId;
                listboxResults.Items.Clear();
                LinkBuilder linkBuilder = new LinkBuilder(videoID);
            }
        }

        private void btnDirectory_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                _downloadDir = dialog.SelectedPath;
                lblDirectory.Content = "Download to: " + _downloadDir;
            }
        }

        public string DownloadDir()
        {
            return _downloadDir;
        }
    }
}
