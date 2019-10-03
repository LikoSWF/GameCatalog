using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace GameCatalog
{
    class FindLinks
    {
        public List<string> Links { get; private set; }

        public FindLinks(Page page, string linkTypeRegex)
        {
            Regex findLink = new Regex(linkTypeRegex);

            List<string> linkList = new List<string>();

            AddMatch(ref linkList, findLink, page.HTML);

            this.Links = linkList;
        }

        private void AddMatch(ref List<string> list, Regex regex, string line)
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

        private string FullURL(string url)
        {
            string lang = url.IndexOf('?') >= 0 ? "&" : "?";
            return Pattern.GooglePlay + url + lang + Pattern.Language;
        }

    }
}
