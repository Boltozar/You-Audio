using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.IO;
using System.Windows.Threading;
using System.ComponentModel;

namespace You_Audio
{
    class Download
    {
        private System.Diagnostics.Process _process;
        private string _tmpdirpath;
        private string _title;

        public Download(string url, string title)
        {
            _tmpdirpath = System.IO.Path.GetTempPath();
            _title = title;

            Pages.SearchPage.Dispatcher.BeginInvoke(new Action(delegate()
            {
                Pages.SearchPage.listboxDownloadStatus.Items.Add("Downloading " + _title);
            }));

            WebClient client = new WebClient();
            try
            {
                client.Proxy = null;
                client.DownloadFile(url, _tmpdirpath + _title); 
            }
            catch
            {
                MessageBox.Show("Error downloading " + _title);
                Pages.SearchPage.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    int idx = Pages.SearchPage.listboxDownloadStatus.Items.IndexOf("Downloading " + _title);
                    string dl = (string)Pages.SearchPage.listboxDownloadStatus.Items[idx];
                    dl = "Error downloading " + _title;
                    Pages.SearchPage.listboxDownloadStatus.Items[idx] = dl;
                }));
                return;
            }

            // run ffmpeg
            _process = new System.Diagnostics.Process();
            _process.StartInfo.FileName = _tmpdirpath + "ffmpeg";
            _process.StartInfo.Arguments = "-i " + '"' + _tmpdirpath + _title + '"' + " " + "-f mp3 " + "-y " + '"' + Pages.ResultsPage.DownloadDir() + "/" + _title + ".mp3" + '"';
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.CreateNoWindow = true;
            if (!_process.Start())
            {
                MessageBox.Show("Error starting ffmpeg.");
                System.IO.File.Delete(_tmpdirpath + _title);
                Pages.SearchPage.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    int idx = Pages.SearchPage.listboxDownloadStatus.Items.IndexOf("Downloading " + _title);
                    string dl = (string)Pages.SearchPage.listboxDownloadStatus.Items[idx];
                    dl = "Error downloading " + _title;
                    Pages.SearchPage.listboxDownloadStatus.Items[idx] = dl;
                }));
                return;
            }

            _process.WaitForExit();
            _process.Close();

            Pages.SearchPage.Dispatcher.BeginInvoke(new Action(delegate()
            {
                int idx = Pages.SearchPage.listboxDownloadStatus.Items.IndexOf("Downloading " + _title);
                string dl = (string)Pages.SearchPage.listboxDownloadStatus.Items[idx];
                dl = "Finished " + _title;
                Pages.SearchPage.listboxDownloadStatus.Items[idx] = dl;
            }));

            // clean up  
            System.IO.File.Delete(_tmpdirpath + _title);
        }


        public static void SetupFFMPEG()
        {
            string tmpdirpath = System.IO.Path.GetTempPath();

            // see if ffmpeg already exists
            if (System.IO.File.Exists(tmpdirpath + "ffmpeg.exe"))
            {
                return;
            }

            // extract ffmpeg from resources
            byte[] exeBytes = Properties.Resources.ffmpeg;
            string exeToRun = tmpdirpath + "ffmpeg.exe";

            // write data
            using (System.IO.FileStream exeFile = new System.IO.FileStream(exeToRun, System.IO.FileMode.CreateNew))
            {
                exeFile.Write(exeBytes, 0, exeBytes.Length);
            }
        }
    }
}
