using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BricsWeb.Models;

namespace BricsWeb.LocalModels
{
    public class BackupModel
    {
        public IList<BuyerRequestModel> BuyerRequestList { get; set; }
        public IList<CategoryModel> CategoryList { get; set; }
        public IList<CompanyModel> CompanyList { get; set; }
        public IList<CompanySubscriptionModel> CompanySubscriptionList { get; set; }
        public IList<ProductModel> ProductList { get; set; }
        public IList<TransactionModel> TransactionList { get; set; }
    }
}