using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalog
{
    class PlayStore
    {
        public List<string> CatagoryLinks { get; private set; }
        public List<GameInfo> Catalog { get; private set; }

        private List<string> beenList;
        public PlayStore()
        {
            Page mainPage = new Page(Pattern.MainPage);
            this.CatagoryLinks = new FindLinks(mainPage, Pattern.CatagoryUrl).Links;
            this.Catalog = new List<GameInfo>();
            this.beenList = new List<string>();

            int i = 0;
            foreach (var link in this.CatagoryLinks)
            {
                Page page = new Page(link);
                FindLinks seeMore = new FindLinks(page, Pattern.MoreUrl);
                FindLinks dev = new FindLinks(page, Pattern.DevUrl);
                FindLinks app = new FindLinks(page, Pattern.AppUrl);
                int j = 0;
                foreach (var gameLink in app.Links)
                {
                    if (!this.beenList.Contains(gameLink))
                    {
                        CrawlGamePage game = new CrawlGamePage(new Page(gameLink));
                        this.Catalog.Add(game.Game);
                        ++j;
                        Console.CursorVisible = false;
                        string j1 = j.ToString();
                        Console.Write(j1);
                        Console.SetCursorPosition(Console.CursorLeft - j1.Length, Console.CursorTop);
                        Console.WriteLine();
                        game.Game.PrintGame();
                    }
                }
                Console.WriteLine("Catagory " + i.ToString().PadRight(5));
                Console.Write("SEE: " + seeMore.Links.Count.ToString().PadRight(5));
                Console.Write("DEV: " + dev.Links.Count.ToString().PadRight(5));
                Console.Write("APP: " + app.Links.Count.ToString().PadRight(5));
                Console.WriteLine();
                ++i;
            }

            XMLConverter<List<GameInfo>> file = new XMLConverter<List<GameInfo>>(Catalog);
            file.SaveData();
        }

        /*
        private bool Been(string url)
        {
            if (!this.beenList.Contains(url))
            {

            }
        }
        */
    }
}
