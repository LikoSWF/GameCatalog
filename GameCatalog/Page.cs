using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace GameCatalog
{
    class Page
    {
        const string appUrlPattern = @"wXUyZd.*?(\/store\/apps\/details\?id=[^""]*)";
        const string devPattern = @"""(\/store\/apps\/dev.*?)"".*?><.*?>(.*?)<";
        public List<string> AppLinks { get; private set; }
        public List<string> DevLinks { get; private set; }

        public Page(string pageUrl, string linkRegex)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(pageUrl);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

            timer.Stop();
            Console.WriteLine("HTTP REQUEST: "+ timer.Elapsed);
            timer.Reset();

            Regex findGame = new Regex(appUrlPattern);
            Regex findDev = new Regex(devPattern);

            timer.Start();
            List<string> appList = new List<string>();
            List<string> devList = new List<string>();

            
            while (!reader.EndOfStream)
            {
                string line = reader.ReadToEnd();
                AddMatch(ref appList, findGame, line);
                AddMatch(ref devList, findDev, line);
            }
            this.AppLinks = appList;
            this.DevLinks = devList;

            timer.Stop();
            Console.WriteLine("GET LINKS: " + timer.Elapsed);
            Console.WriteLine("App Count: " + this.AppLinks.Count);
            Console.WriteLine("Dev Count: " + this.DevLinks.Count);


        }

        private void AddMatch(ref List<string> list ,Regex regex, string line)
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


        private const string googlePlay = @"https://play.google.com";
        private const string language = @"&hl=en_us";
        private string FullURL(string url)
        {
            return googlePlay + url + language;
        }

    }
}
