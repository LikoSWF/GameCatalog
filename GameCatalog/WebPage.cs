using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace GameCatalog
{
    class WebPage
    {
        public string[] HTML { get; private set; }

        public WebPage(string url)
        {
            Stopwatch sw = new Stopwatch();
            Stopwatch sw2 = new Stopwatch();
            sw.Start();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);

            sw.Stop();
            sw2.Start();

            List<string> lines = new List<string>();
            while (!sr.EndOfStream)
            {
                lines.Add(sr.ReadLine());
            }
            sr.Close();
            myResponse.Close();

            this.HTML = lines.ToArray();

            sw2.Stop();
            
            Console.WriteLine("SW1: " + sw.Elapsed);
            Console.WriteLine("SW2: " + sw2.Elapsed);
        }
    }
}
