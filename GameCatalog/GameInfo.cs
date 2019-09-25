using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalog
{
    class GameInfo
    {
        public string Name { get; set;}
        public string Description { get; set; }
        public float Review { get; set; }
        public DateTime LastUpdate { get; set; }
        public string Size { get; set; }
        public string Installs { get; set; }
        public string Version { get; set; }
        public string Requirements { get; set; }
        public string ContentRating { get; set; }
        public string InteractiveElements { get; set; }
        public string InappPurchases { get; set; }

        public GameInfo() { }
    }
}
