using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BricsWeb.LocalModels;
using BricsWeb.Repository;

namespace BricsWeb.Controllers
{
    public class BackupController : Controller
    {
        //
        // GET: /Backup/

        public JsonResult Index()
        {
            var buyerRequestList = new BuyerRequestRepository().GetAll();
            var categoryList = new CategoryRepository().GetAll();
            var companyList = new CompanyRepository().GetAll();
            var companySubscriptionList = new CompanySubscriptionRepository().GetAll();
            var productList = new ProductRepository().GetAll();
            var transactionList = new TransactionRepository().GetAll();

            var backupResult = 
                new BackupModel 
                { 
                    BuyerRequestList = buyerRequestList, 
                    CategoryList = categoryList, 
                    CompanyList = companyList,
                    CompanySubscriptionList = companySubscriptionList, 
                    ProductList = productList, 
                    TransactionList = transactionList
                };

            return Json(backupResult,JsonRequestBehavior.AllowGet);
        }
    }
}
