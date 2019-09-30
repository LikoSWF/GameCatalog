using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameCatalog
{
    class Program
    {
        

        static void Main(string[] args)
        {

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
