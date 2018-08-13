using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BricsWeb.Models;

namespace BricsWeb.LocalModels
{
    public class ProductSearchResultModel
    {
        public ProductModel Product { get; set; }

        public string CompanyName { get; set; }
        public string CompanyID { get; set; }

        public string BusinessType { get; set; }

        public bool IsGreenCertified { get; set; }
        public bool IsChamberCertified { get; set; }
        public bool IsVerified { get; set; }

        public int CompanySubscriptionLevel { get; set; }

        public string Country { get; set; }
        public string FlagUrl { get; set; }
    }
}