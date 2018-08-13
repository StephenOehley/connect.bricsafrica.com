using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BricsWeb.Models
{
    public class MenuItemModel
    {
        public string DisplayName { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public string[] AllowedRoles { get; set; }
    }
}