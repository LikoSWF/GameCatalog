using System.Text.RegularExpressions;
using System.Net;

namespace GameCatalog
{
    class CrawlGamePage
    {
        public GameInfo Game { get; set; }

        public CrawlGamePage(Page page)
        {
            this.Game = ParseGameInfo(page);
        }

        private GameInfo ParseGameInfo(Page page)
        {
            GameInfo info = new GameInfo();

            info.Name                = FindString(Pattern.Name, page);
            string price = FindString(Pattern.Price, page);
            if (price == "0") price = "Free";
            info.Price               = price;
            info.Developer           = FindString(Pattern.DevName, page);
            info.Description         = FindString(Pattern.Description, page);
            info.Rating              = FindString(Pattern.Review, page);
            info.WhatsNew            = FindString(Pattern.WhatsNew, page);
            info.LastUpdate          = FindString(Pattern.LastUpdate, page);
            info.Size                = FindString(Pattern.Size, page);
            info.Installs            = FindString(Pattern.Installs, page);
            info.Version             = FindString(Pattern.Version, page);
            info.Requirements        = FindString(Pattern.Requires, page);
            info.ContentRating       = FindString(Pattern.Rating, page);
            info.InteractiveElements = FindString(Pattern.Interactive, page);
            info.InappPurchases      = FindString(Pattern.Inapp, page);
            return info;
        }

        private string FindString(string regexPattern, Page page)
        {
            Regex regex = new Regex(regexPattern);
            string answer = "";
            MatchCollection matches = regex.Matches(page.HTML);
            for (int i = 0; i < matches.Count; i++)
            {
                for (int j = 1; j < matches[0].Groups.Count; j++)
                {
                    answer += matches[0].Groups[j].ToString();
                    answer += (j + 1 < matches[0].Groups.Count ? " " : "");
                }
            }
            return Format(answer);
        }

        private string Format(string str) => HtmlDecode(RemoveTags(LineBreak(str)));
        private string LineBreak(string str) => str.Replace("<br>", "\n");
        private string RemoveTags(string str) => Regex.Replace(str, @"<[^>]*>", "");
        private string HtmlDecode(string str) => WebUtility.HtmlDecode(str);
    }
}
