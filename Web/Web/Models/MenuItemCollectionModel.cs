using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BricsWeb.Models
{
    public class MenuItemCollectionModel
    {
        public string MenuName { get; set; }
        public MenuItemModel[] Items;
    }
}