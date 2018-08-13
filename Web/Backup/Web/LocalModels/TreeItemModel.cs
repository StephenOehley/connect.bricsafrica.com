using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BricsWeb.LocalModels
{
    public class TreeItemModel
    {
        public TreeItemAttribute attr { get; set; }

        public string data { get; set; }
        public string state { get; set; }

        public class TreeItemAttribute
        {
            public string id { get; set; }
            public string rel { get; set; }
            public string displaytext { get; set; }
        }
    }

    
}