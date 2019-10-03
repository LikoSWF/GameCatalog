using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GameCatalog
{
    class CrawlGamePage
    {
        public GameInfo Game { get; set; }

        public CrawlGamePage(string url)
        {
            this.Game = ParseGameInfo(url);
        }

        private GameInfo ParseGameInfo(string url)
        {

            Page page = new Page(url);
            GameInfo info = new GameInfo();
            const string namePattern = @"<h1[^>]*>.*<span[^>]*>([^<]*)<\/span>";
            const string descriptionPattern = @"description.*?""sngebd"">(.*?)<\/div>";
            const string reviewPattern = @"BHMmbe[^>]*>([\d\.]*)<";
            const string whatsNewPattern = @"New\W.*jsslot>(.*?)<\/span>";
            const string lastUpdatePattern = @">(\w+\s\d+,\s\d+)<";
            const string sizePattern = @">Size.*?>([^<]+)<";
            const string installsPattern = @">Installs.*?>([^<]+)<";
            const string versionPattern = @"Version<.*?>([^<]+)<";
            const string requiresPattern = @">Requires\s(.*?)<.*?>([^<]+)<";
            const string ratingPattern = @"Rating<.*?>([^<]+)(?:<.*?>([^<]+))*.*?<a";
            const string interactivePattern = @"Elements<.*?>([^<]+).*?(?:>([^<]+).*?)*?<\/div>";
            const string inappPattern = @"In-app.*?>([^<]+)<";

            info.Name = FindString(namePattern, page);
            info.Description = FindString(descriptionPattern, page);
            info.Review = FindString(reviewPattern, page);
            info.WhatsNew = FindString(whatsNewPattern, page);
            info.LastUpdate = FindString(lastUpdatePattern, page);
            info.Size = FindString(sizePattern, page);
            info.Installs = FindString(installsPattern, page);
            info.Version = FindString(versionPattern, page);
            info.Requirements = FindString(requiresPattern, page);
            info.ContentRating = FindString(ratingPattern, page);
            info.InteractiveElements = FindString(interactivePattern, page);
            info.InappPurchases = FindString(inappPattern, page);
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
        private string HtmlDecode(string str) => System.Net.WebUtility.HtmlDecode(str);
    }
}
