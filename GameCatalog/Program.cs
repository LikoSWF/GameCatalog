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
        const string url = @"https://play.google.com/store/apps?hl=en_us";

        static void Main(string[] args)
        {
            for (int i = 0; i < 20; i++)
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                Links page = new Links(url);
                timer.Stop();

                Console.WriteLine(timer.Elapsed);
            }
            
            Console.ReadKey();

            Crawler crawler = new Crawler();
            XMLConverter<List<DevInfo>> data = new XMLConverter<List<DevInfo>>(crawler.devs);
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
