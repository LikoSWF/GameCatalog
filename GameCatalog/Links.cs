using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace GameCatalog
{
    class Links
    {
        public List<string> AppLinks { get; private set; }
        public List<string> DevLinks { get; private set; }
        public List<string> MoreLinks { get; private set; }

        public Links(string pageUrl)
        {
            Stopwatch timer = new Stopwatch();


            Page page = new Page(pageUrl);

            timer.Start();

            Regex findGame = new Regex(Pattern.AppUrl);
            Regex findDev = new Regex(Pattern.DevUrl);
            Regex findMore = new Regex(Pattern.MoreUrl);

            List<string> appList = new List<string>();
            List<string> devList = new List<string>();
            List<string> moreList = new List<string>();

            AddMatch(ref appList, findGame, page.HTML);
            AddMatch(ref devList, findDev, page.HTML);
            AddMatch(ref moreList, findMore, page.HTML);

            this.AppLinks = appList;
            this.DevLinks = devList;
            this.MoreLinks = moreList;

            timer.Stop();

            Console.WriteLine("GET LINKS:  " + timer.Elapsed);
            Console.WriteLine("App Count:  " + this.AppLinks.Count);
            Console.WriteLine("Dev Count:  " + this.DevLinks.Count);
            Console.WriteLine("More Count: " + this.MoreLinks.Count);
        }

        private void AddMatch(ref List<string> list, Regex regex, string line)
        {
            MatchCollection matches = regex.Matches(line);
            for (int i = 0; i < matches.Count; i++)
            {
                string match = FullURL(matches[i].Groups[1].ToString());
                if (!list.Contains(match))
                {
                    list.Add(match);
                }
            }
        }

        private string FullURL(string url)
        {
            return Pattern.GooglePlay + url + Pattern.Language;
        }

    }
}
