﻿using System;
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
            ObjectToXML<List<GameInfo>> data = new ObjectToXML<List<GameInfo>>(crawler.games);
            data.SaveData();
            Console.ReadKey();
        }
    }
}
