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
        private GameInfo ParseGameInfo(string url)
        {

            WebPage page = new WebPage(url);
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
            info.Description = RemoveTags(LineBreak(FindString(descriptionPattern, page)));
            info.Review = FindString(reviewPattern, page);
            info.WhatsNew = LineBreak(FindString(whatsNewPattern, page));
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

        private string FindString(string regexPattern, WebPage page)
        {
            Regex regex = new Regex(regexPattern);
            string answer = "";
            for (int i = 0; i < page.HTML.Length; i++)
            {
                MatchCollection matches = regex.Matches(page.HTML[i]);
                if (matches.Count > 0)
                {
                    for (int j = 1; j < matches[0].Groups.Count; j++)
                    {
                        answer += matches[0].Groups[j].ToString();
                        answer += (j + 1 < matches[0].Groups.Count ? " " : "");
                    }
                    break;
                }
            }

            return System.Net.WebUtility.HtmlDecode(answer);
        }

        private string LineBreak(string str)
        {
            return str.Replace("<br>", "\n");
        }
        private string RemoveTags(string str)
        {
            return str = Regex.Replace(str, @"<[^>]*>", "");
        }
    }
}
