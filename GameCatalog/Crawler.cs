using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameCatalog
{
    class Crawler
    {
        const string url = @"https://play.google.com/store/apps/collection/cluster?clp=CiMKIQobdG9wc2VsbGluZ19mcmVlX0dBTUVfQ0FTSU5PEAcYAw%3D%3D:S:ANO1ljKaOLc&gsr=CiUKIwohCht0b3BzZWxsaW5nX2ZyZWVfR0FNRV9DQVNJTk8QBxgD:S:ANO1ljLq6J8";
        const string googlePlay = @"https://play.google.com";
        const string appUrlPattern = @"(\/store\/apps\/details\?id=[^""]*)";
        public List<WebPage> pages = new List<WebPage>();
        public List<GameInfo> games = new List<GameInfo>();


        public Crawler()
        {
            Crawl(url);
        }

        private void Crawl(string url)
        {
            WebPage mainPage = new WebPage(url);
            Regex regex = new Regex(appUrlPattern);

            foreach (var line in mainPage.HTML)
            {
                MatchCollection matches = regex.Matches(line);
                if (matches.Count > 0)
                {
                    WebPage page = new WebPage(googlePlay + matches[0].ToString());
                    this.pages.Add(page);
                    ParseGameInfo(page);
                }
            }
        }








        private void Parse()
        {
            foreach (var page in this.pages)
            {
                this.games.Add(ParseGameInfo(page));
            }
        }

        private GameInfo ParseGameInfo(WebPage page)
        {
            GameInfo info = new GameInfo();
            const string namePattern = @"<h1[^>]*>.*<span[^>]*>([^<]*)<\/span>";
            Regex name = new Regex(namePattern);
            for (int i = 0; i < page.HTML.Length; i++)
            {
                MatchCollection matches = name.Matches(page.HTML[i]);
                if (matches.Count > 0)
                {
                    Console.WriteLine("NAME: " + matches[0].Groups[1].ToString());
                }
            }
            return info;
        }

    }
}
