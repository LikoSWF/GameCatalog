using System.Text;
using System.IO;
using System.Net;

namespace GameCatalog
{
    class Page
    {
        public string HTML { get; private set; }
        public Page(string url)
        {
            this.HTML = WebPage(url);
        }
        private string WebPage(string pageUrl)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(pageUrl);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

            string page = reader.ReadToEnd();
            page = page.Substring(page.IndexOf("<bo")); // remove everything before <body> tag
            return page;
        }
    }
}
