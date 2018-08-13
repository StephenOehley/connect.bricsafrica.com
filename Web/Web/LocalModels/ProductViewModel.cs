using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BricsWeb.Models;

namespace BricsWeb.LocalModels
{
    public class ProductViewModel
    {
        public ProductModel ProductData { get; set; }
        public string CategoryName { get; set; }
        public string CompanyName { get; set; }

        public bool ShowFeaturedProductOption { get; set; }
        public int MaxFeaturedProductCount { get; set; }

        public bool IsFeaturedProduct { get; set; } 

        public HttpPostedFileWrapper photo { get; set; }
    }
}