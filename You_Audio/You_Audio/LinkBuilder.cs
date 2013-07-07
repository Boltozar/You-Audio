using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Web;
using System.Windows;
using System.Threading;

namespace You_Audio
{
    class LinkBuilder
    {
        //collection of URLs to vids of various formats
        //maps keys (itags) to values (urls) in _urlColln
        private NameValueCollection _urlColln;

        //name of video to be downloaded
        private string _vidTitle;

        public LinkBuilder(string videoID)
        {
            FetchUrls(videoID);
            string bestQualityUrl = BestQualityUrl();
            if (bestQualityUrl != "NO ITAG FOUND")
            {
                new Thread(new ThreadStart(() =>
                {
                    Download download = new Download(bestQualityUrl, _vidTitle);
                })).Start();
                if (Pages.MainWindow.Content != Pages.SearchPage)
                {
                    Pages.MainWindow.SetPage(Pages.SearchPage);
                }
            }
            else
            {
                MessageBox.Show("Error downloading video...");
                if (Pages.MainWindow.Content != Pages.SearchPage)
                {
                    Pages.MainWindow.SetPage(Pages.SearchPage);
                }
            }
        }

        //populates the _urlColln with valid urls
        private void FetchUrls(string videoID)
        {
            try
            {
                // Web request
                _urlColln = new NameValueCollection();
                string vidInfoUrl = "http://www.youtube.com/get_video_info?&video_id=" + videoID + "&el=detailpage&ps=default&eurl=&gl=US&hl=en";
                System.Net.WebRequest req = System.Net.WebRequest.Create(vidInfoUrl);
                req.Proxy = null;
                string src = "";
                using (System.Net.WebResponse resp = req.GetResponse())
                {
                    // Get the page's source code
                    src = new System.IO.StreamReader(resp.GetResponseStream(), System.Text.Encoding.UTF8).ReadToEnd();
                    resp.Close();
                }

                // Parse source into collection
                NameValueCollection vidInfoColln = HttpUtility.ParseQueryString(src);

                // Video's avail formats
                string availFmts = vidInfoColln["url_encoded_fmt_stream_map"];
                
                // Video title
                _vidTitle = vidInfoColln["title"];

                // Split avail formats into array
                string[] arrFmts = availFmts.Split(',');

                // Populate the collection
                AddFmtsToColln(arrFmts);
            }
            catch
            {
                //MessageBox.Show("Couldn't download YouTube video.");
                MessageBox.Show("Error downloading video...");
            }
        }
        
        //add key and value to colln
        private void AddFmtsToColln(string[] arrFmts)
        {
            for (int i = 0; i < arrFmts.Length; ++i)
            {
                NameValueCollection fmtInfoColln = HttpUtility.ParseQueryString(arrFmts[i]);
                string itag = fmtInfoColln["itag"];
                string urlDecoded = HttpUtility.UrlDecode(BuildUrl(fmtInfoColln));
                _urlColln.Add(itag, urlDecoded);
            }
        }

        private string BuildUrl(NameValueCollection fmtInfoColln)
        {
            // Get info
            string urlEncoded = fmtInfoColln["url"];
            string itag = fmtInfoColln["itag"];
            string signature = fmtInfoColln["sig"];
            string fallbackHost = fmtInfoColln["fallback_host"];

            // Build
            urlEncoded = string.Format("{0}&fallback_host={1}&signature={2}", urlEncoded, fallbackHost, signature);

            return urlEncoded;
        }

        private string BestQualityUrl()
        {
            //itags (keys) in descending order of quality
            string[] itag = { "13", "22", "37", "38", "45", "46", "101", "102", "84", "85", "34", "35", "43", "44", "100", "120[3]", "18", "82", "83", "5", "6", "36", "17" };
            for (int i = 0; i < itag.Length; ++i)
            {
                if (_urlColln[itag[i]] != null)
                {
                    return _urlColln[itag[i]];
                }
            }
            return "NO ITAG FOUND";
        }
    }
}
