using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.YouTube;
using Google.YouTube;

namespace You_Audio
{
    public static class YouTube
    {
        // Settings for youtube authentication
        private static YouTubeRequestSettings settings = new YouTubeRequestSettings("YouAudio", "AI39si5APbO_T2qBCo64UL2L1gOdxJG8-Ed-_P-WskOOtsCQmBFlR8ryP97l6zLUYWRy2RUr1SWOXccdN6bK6w7f1TFIG4ZBdQ");
        private static YouTubeRequest request = new YouTubeRequest(settings);

        // Search for a video given a keyword
        // @return feed of retrieved videos
        public static Feed<Video> SearchForVideo(string keyword)
        {
            YouTubeQuery query = new YouTubeQuery(YouTubeQuery.DefaultVideoUri);
            query.OrderBy = "relevance";
            query.Query = keyword;
            query.SafeSearch = YouTubeQuery.SafeSearchValues.None;
            query.NumberToRetrieve = 10;
            return request.Get<Video>(query);
        }
    }
}
