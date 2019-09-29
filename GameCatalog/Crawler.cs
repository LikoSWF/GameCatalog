using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GameCatalog
{
    class Crawler
    {
        const string url = @"https://play.google.com/store/apps/collection/cluster?clp=CiMKIQobdG9wc2VsbGluZ19mcmVlX0dBTUVfQ0FTSU5PEAcYAw%3D%3D:S:ANO1ljKaOLc&gsr=CiUKIwohCht0b3BzZWxsaW5nX2ZyZWVfR0FNRV9DQVNJTk8QBxgD:S:ANO1ljLq6J8";
        const string googlePlay = @"https://play.google.com";
        const string language = @"&hl=en_us";
        const string appUrlPattern = @"(\/store\/apps\/details\?id=[^""]*)";
        const string devUrlPattern = @"""(\/store\/apps\/dev.*?)"".*?><.*?>(.*?)<"; // @"(\/store\/apps\/dev.*?)""[^]]";
        const string devNamePattern = @"""\/store\/apps\/dev.*?><.*?>(.*?)<";
        List<string> devUrl = new List<string>();

        public List<WebPage> pages = new List<WebPage>();
        public List<GameInfo> games = new List<GameInfo>();
        public List<DevInfo> devs = new List<DevInfo>();
        public Crawler()
        {
            Crawl(url);
        }


        // =================================================================
        // CRAWLERS 
        // =================================================================

        private void Crawl(string url)
        {
            WebPage mainPage = new WebPage(url);
            Regex regex = new Regex(appUrlPattern);
            Regex devRegex = new Regex(devUrlPattern);

            foreach (var line in mainPage.HTML)
            {
                MatchCollection matches = regex.Matches(line);
                MatchCollection devMatches = devRegex.Matches(line);

                // Only goes through top 50 game // just use CrawlGames(); //

                //if (matches.Count > 0)
                //{
                //    WebPage page = new WebPage(googlePlay + matches[0].ToString() + language);
                //    // this.pages.Add(page);
                //    GameInfo game = ParseGameInfo(page);
                //    this.games.Add(game);
                //    PrintGame(game);
                //}

                // Goes through devs of top 50 games and finds all games from devs //

                if (devMatches.Count > 0)
                {
                    string devPageUrl = FullURL(devMatches[0].Groups[1].ToString());
                    if (!this.devUrl.Contains(devPageUrl))
                    {
                        this.devUrl.Add(devPageUrl);
                        this.devs.Add(CrawlDev(devPageUrl));
                    }
                    else continue;
                }
            }

            // temp print
            foreach (var dev in devs)
            {
                PrintDev(dev);
            }
            
        }
        private List<GameInfo> CrawlGames(string url)
        {
            WebPage page = new WebPage(url);
            Regex appUrl = new Regex(appUrlPattern);
            List<GameInfo> gameList = new List<GameInfo>();
            
            foreach (var line in page.HTML)
            {
                MatchCollection app = appUrl.Matches(line);
                if (appUrl.IsMatch(line))
                {
                    gameList.Add(ParseGameInfo(FullURL(app[0].Groups[1].ToString())));
                }
            }
            return gameList;
        }

        private DevInfo CrawlDev(string url)
        {
            const string featAppPattern = @"Featured.*?<a.*?""(.*?details.*?)""";
            const string seeMorePattern = @"""(\S*)"".See\smore";
            const string devNamePattern = @"<h\d[^>]*?>(.*?)<"; // or use global dev pattern...
            WebPage page = new WebPage(url);
            DevInfo devInfo = new DevInfo();

            Regex seeMore = new Regex(seeMorePattern);
            Regex featApp = new Regex(featAppPattern);
            foreach (var line in page.HTML)
            {
                MatchCollection feature = featApp.Matches(line);
                MatchCollection more = seeMore.Matches(line);
                if (feature.Count > 0)
                {
                    devInfo.GameLibrary.Add(ParseGameInfo(FullURL(feature[0].Groups[1].ToString())));
                }
                if (more.Count > 0)
                {
                    devInfo.GameLibrary.InsertRange(devInfo.GameLibrary.Count, CrawlGames(FullURL(more[0].Groups[1].ToString())));
                }
            }
            devInfo.DeveloperName = FindString(Crawler.devNamePattern, page);
            return devInfo;
        }

        // =====================================================================
        // PARSE GAME OBJECT
        // =====================================================================

        private GameInfo ParseGameInfo(string url)
        {
            WebPage page = new WebPage(url);
            GameInfo info = new GameInfo();
            const string namePattern = @"<h1[^>]*>.*<span[^>]*>([^<]*)<\/span>";
            const string descriptionPattern = @"sngebd.*?>(.+?)<\/div>"; // use only if on single line
            //const string descriptionStart = @"name=""description""[^>]*content=""(.*)"; // use for multiline
            //const string descriptionEnd = @"([^>]*)"">"; // use for multiline
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
                        answer += ( j+1 < matches[0].Groups.Count ? " " : "" );
                    }
                    break;
                }
            }

            return System.Net.WebUtility.HtmlDecode(answer);
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
            return System.Net.WebUtility.HtmlDecode(answer).Trim();
        }

        private string LineBreak(string str)
        {
            return str.Replace("<br>", "\n");
        }
        private string RemoveTags(string str)
        {
            return str = Regex.Replace(str, @"<[^>]*>", "");
        }

        private string FullURL(string url)
        {
            return googlePlay + url + language;
        }


        // PRINT =================================================================

        public void PrintDev(DevInfo dev)
        {
            Console.WriteLine("DEVELOPER: " + dev.DeveloperName);
            for (int i = 0; i < dev.GameLibrary.Count; i++)
            {
                Console.WriteLine("GAME" + 1);
                PrintGame(dev.GameLibrary[i]);
            }
        }

        public void PrintGame(GameInfo game)
        {
            Console.WriteLine("NAME: " + game.Name);
            Console.WriteLine("DESCRIPTION: " + game.Description);
            Console.WriteLine("STARS: " + game.Review);
            Console.WriteLine("WHAT\'S NEW: " + game.WhatsNew);
            Console.WriteLine("LAST UPDATED: " + game.LastUpdate);
            Console.WriteLine("SIZE: " + game.Size);
            Console.WriteLine("INSTALLS: " + game.Installs);
            Console.WriteLine("VERSION: " + game.Version);
            Console.WriteLine("REQUIRES: " + game.Requirements);
            Console.WriteLine("CONTENT: " + game.ContentRating);
            Console.WriteLine("INTERACTIVE ELEMENTS: " + game.InteractiveElements);
            Console.WriteLine("IN-APP PURCHASE: " + game.InappPurchases);
            Console.WriteLine();
        }
    }
}
