using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalog
{
    static class Pattern
    {
        public const string MainPage = @"https://play.google.com/store/apps?hl=en_us";
        public const string GooglePlay = @"https://play.google.com";
        public const string Language = @"hl=en_us";

        // URLs ===========================================================================
        public const string AppUrl = @"wXUyZd.*?(\/store\/apps\/details\?id=[^""]*)";
        public const string DevUrl = @"""(\/store\/apps\/dev.*?)"".*?><.*?>(.*?)<";
        public const string MoreUrl = @"""(\S*)"".See\smore";
        public const string CatagoryUrl = @"=""(\/store\/apps\/(?:cat.*?|str.*?))""";
        public const string DevName = @"""\/store\/apps\/dev.*?><.*?>(.*?)<";

        // Content ========================================================================
        public const string Name = @"<h1[^>]*>.*<span[^>]*>([^<]*)<\/span>";
        public const string Price = @"price.*?content=""(.*?)""";
        public const string Description = @"description.*?""sngebd"">(.*?)<\/div>";
        public const string Review = @"BHMmbe[^>]*>([\d\.]*)<";
        public const string WhatsNew = @"New\W.*jsslot>(.*?)<\/span>";
        public const string LastUpdate = @">(\w+\s\d+,\s\d+)<";
        public const string Size = @">Size.*?>([^<]+)<";
        public const string Installs = @">Installs.*?>([^<]+)<";
        public const string Version = @"Version<.*?>([^<]+)<";
        public const string Requires = @">Requires\s(.*?)<.*?>([^<]+)<";
        public const string Rating = @"Rating<.*?>([^<]+)(?:<.*?>([^<]+))*.*?<a";
        public const string Interactive = @"Elements<.*?>([^<]+).*?(?:>([^<]+).*?)*?<\/div>";
        public const string Inapp = @"In-app.*?>([^<]+)<";

    }
}
