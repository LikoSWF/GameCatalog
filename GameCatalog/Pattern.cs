using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalog
{
    static class Pattern
    {
        public const string GooglePlay = @"https://play.google.com";
        public const string Language = @"&hl=en_us";

        // URLs ===========================================================================
        public const string AppUrl = @"wXUyZd.*?(\/store\/apps\/details\?id=[^""]*)";
        public const string DevUrl = @"""(\/store\/apps\/dev.*?)"".*?><.*?>(.*?)<";
        public const string MoreUrl = @"""(\S*)"".See\smore";
        public const string CatagoryUrl = @"""(\/store\/apps\/(?:cat.*?|str.*?))"">";

        // Content ========================================================================
        public const string Price = @"price.*?content=""(.*?)""";

    }
}
