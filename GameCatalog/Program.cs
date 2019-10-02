using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GameCatalog
{
    class Program
    {
        const string url = @"https://play.google.com/store/apps/collection/cluster?clp=CiMKIQobdG9wc2VsbGluZ19mcmVlX0dBTUVfQ0FTSU5PEAcYAw%3D%3D:S:ANO1ljKaOLc&gsr=CiUKIwohCht0b3BzZWxsaW5nX2ZyZWVfR0FNRV9DQVNJTk8QBxgD:S:ANO1ljLq6J8";
        const string appUrlPattern = @"wXUyZd.*?(\/store\/apps\/details\?id=[^""]*)";

        static void Main(string[] args)
        {
            for (int i = 0; i < 20; i++)
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                Page page = new Page(url, appUrlPattern);
                timer.Stop();

                Console.WriteLine(timer.Elapsed);
            }
            
            Console.ReadKey();

            Crawler crawler = new Crawler();
            ObjectToXML<List<DevInfo>> data = new ObjectToXML<List<DevInfo>>(crawler.devs);
            data.SaveData();
            data.LoadData();
            foreach (var dev in data.Data)
            {
                crawler.PrintDev(dev);
            }
            Console.WriteLine(crawler.CountGames());
            Console.ReadKey();
        }
    }
}
