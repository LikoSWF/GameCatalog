using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalog
{
    public class GameInfo
    {
        public string Name { get; set;}
        public string Developer { get; set; }
        public string Price { get; set; }
        public string Catagory { get; set; }
        public string Description { get; set; }
        public string Rating { get; set; }
        public string WhatsNew { get; set; }
        public string LastUpdate { get; set; }
        public string Size { get; set; }
        public string Installs { get; set; }
        public string Version { get; set; }
        public string Requirements { get; set; }
        public string ContentRating { get; set; }
        public string InteractiveElements { get; set; }
        public string InappPurchases { get; set; }

        public GameInfo() { }

        public void PrintGame()
        {
            Console.WriteLine("NAME: " + this.Name);
            Console.WriteLine("DESCRIPTION: " + this.Description);
            Console.WriteLine("STARS: " + this.Rating);
            Console.WriteLine("WHAT\'S NEW: " + this.WhatsNew);
            Console.WriteLine("LAST UPDATED: " + this.LastUpdate);
            Console.WriteLine("SIZE: " + this.Size);
            Console.WriteLine("INSTALLS: " + this.Installs);
            Console.WriteLine("VERSION: " + this.Version);
            Console.WriteLine("REQUIRES: " + this.Requirements);
            Console.WriteLine("CONTENT: " + this.ContentRating);
            Console.WriteLine("INTERACTIVE ELEMENTS: " + this.InteractiveElements);
            Console.WriteLine("IN-APP PURCHASE: " + this.InappPurchases);
            Console.WriteLine();
        }
    }

    public class DevInfo
    {
        public string DeveloperName { get; set; }
        public List<GameInfo> GameLibrary { get; set; }

        public DevInfo()
        {
            this.GameLibrary = new List<GameInfo>();
        }

        public void PrintDev()
        {
            Console.WriteLine("DEVELOPER: " + this.DeveloperName);
            for (int i = 0; i < this.GameLibrary.Count; i++)
            {
                Console.WriteLine("GAME" + i);
                this.GameLibrary[i].PrintGame();
            }
        }
    }
}
