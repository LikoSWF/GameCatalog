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
                    // this.pages.Add(page);
                    GameInfo game = ParseGameInfo(page);
                    this.games.Add(game);
                    PrintGame(game);
                }
            }
        }






        private GameInfo ParseGameInfo(WebPage page)
        {
            GameInfo info = new GameInfo();
            const string namePattern = @"<h1[^>]*>.*<span[^>]*>([^<]*)<\/span>";
            // const string descriptionPattern = @"name=""description""[^>]*content=""([^""]*)"">"; // use only if on single line
            const string descriptionStart = @"name=""description""[^>]*content=""(.*)";
            const string descriptionEnd = @"([^>]*)"">";
            const string reviewPattern = @"BHMmbe[^>]*>([\d\.]*)<";
            const string whatsNewPattern = @"New\W.*jsslot>(.*?)<\/span>";

            info.Name = FindString(namePattern, page);
            info.Description = FindString(descriptionStart, descriptionEnd, page);
            info.Review = FindString(reviewPattern, page);
            info.WhatsNew = FindString(whatsNewPattern, page);
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
                    for (int j = 1; j < matches[0].Length; j++)
                    {
                        answer += matches[0].Groups[j].ToString();
                    }
                    break;
                }
            }
            return answer;
        }
        private string FindString(string startPattern, string endPattern, WebPage page)
        {
            Regex startRegex = new Regex(startPattern);
            Regex endRegex = new Regex(endPattern);
            string answer = "";
            for (int i = 0; i < page.HTML.Length; i++)
            {
                MatchCollection matches = startRegex.Matches(page.HTML[i]);
                if (matches.Count > 0)
                {
                    answer = matches[0].Groups[1].ToString();
                    while (i < page.HTML.Length)
                    {
                        i++;
                        MatchCollection endMatches = endRegex.Matches(page.HTML[i]);
                        if (endMatches.Count > 0)
                        {
                            answer += endMatches[0].Groups[1].ToString();
                            break;
                        }
                        else
                        {
                            answer += page.HTML[i];
                        }
                    } 
                    break;
                }
            }
            return answer;
        }

        public void Print()
        {
            foreach (var game in this.games)
            {
                Console.WriteLine(game.Name);
                Console.WriteLine(game.Description);
                Console.WriteLine(game.Review);
            }
        }

        public void PrintGame(GameInfo game)
        {
            Console.WriteLine("NAME: " + game.Name);
            Console.WriteLine("DESCRIPTION: " + game.Description);
            Console.WriteLine("STARS: " + game.Review);
            Console.WriteLine("WHAT\'S NEW: " + game.WhatsNew);
        }
    }
}
