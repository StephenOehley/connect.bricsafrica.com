using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BricsWeb.Repository;
using BricsWeb.Models;
using BricsWeb.LocalModels;
using ServiceStack.Text;
using System.Text;
using BricsWeb.RepositoryHelper;
using BricsWeb.Properties;
using MvcHelper.Mvc;

namespace BricsWeb.Controllers
{
    public class ResellerController : CrystalController
    {
        public ActionResult SalesReport()
        {
            if (User.IsInRole("Webmaster"))
            {
                 return RedirectToAction("SalesReportWebmaster");
            }
            else
            {
              return RedirectToAction("SalesReportReseller");
            }
        }

        [Authorize(Roles="Webmaster")]
        public ActionResult SalesReportWebmaster()
        {
            var chamberArray = CompanyHelper.GetAllCompaniesFromCache().Where(c => c.BusinessType != null).Where(c => c.BusinessType.ToLower() == "chamber").OrderBy(c => c.Name).ToArray();
            return MenuView(chamberArray, "Find A Product", "SubMenuResellerInformation", string.Empty);
        }

        //
        // GET: /Reseller/
        [Authorize(Roles = "RegisteredReseller,Webmaster")]
        public FileResult SalesReportReseller(string userid = "")
        {
            string userRowKey = string.Empty;
            if (userid == string.Empty)
            {
                var user = Membership.GetUser();
                userRowKey = (user.ProviderUserKey as string);
            }
            else
            {
                userRowKey = userid;
            }

            var chamber = new CompanyRepository().GetByRowKey(userRowKey);
            var historyList = new List<TransactionHistoryModel>();

            //get companies
            var allCompanies = CompanyHelper.GetAllCompaniesFromCache();
            //get subscriptions
            var allSubscriptions = CompanySubscriptionHelper.GetAllCompanySubscriptionsFromCache();    
            //get transactions
            var allTransactions = new TransactionRepository().GetAll().ToArray();
            //get all products
            var allProducts = new ProductRepository().GetAll().ToArray();
           
            //query for list  of subscriptions for members
            var allHistoryTier1 = allSubscriptions.Where(s => s.Chamber1 == chamber.RowKey).OrderBy(s => s.StartDateTime).ToArray();
            var allHistoryTier2 = allSubscriptions.Where(s => s.Chamber2 == chamber.RowKey).OrderBy(s => s.StartDateTime).ToArray();
            var allHistoryTier3 = allSubscriptions.Where(s => s.Chamber3 == chamber.RowKey).OrderBy(s => s.StartDateTime).ToArray();
            var allHistoryTier4 = allSubscriptions.Where(s => s.Chamber4 == chamber.RowKey).OrderBy(s => s.StartDateTime).ToArray();

            //add tier 1 history
            historyList.AddRange(
            allHistoryTier1.Select(h => new TransactionHistoryModel(
                h.RowKey,
                allCompanies.Where(c => c.RowKey == h.CompanyRowKey).SingleOrDefault().Name,
                allTransactions.Where(t => t.RowKey == h.TransactionID).SingleOrDefault().Amount,
                allProducts.Where(p => p.RowKey == h.ProductID).SingleOrDefault().ProductName,
                h.StartDateTime,
               GetTierCommission("1"),
                chamber.Name))      
                );

            //add tier 2 history
            historyList.AddRange(
            allHistoryTier2.Select(h => new TransactionHistoryModel(
                h.RowKey,
                allCompanies.Where(c => c.RowKey == h.CompanyRowKey).SingleOrDefault().Name,
                allTransactions.Where(t => t.RowKey == h.TransactionID).SingleOrDefault().Amount,
                allProducts.Where(p => p.RowKey == h.ProductID).SingleOrDefault().ProductName,
                h.StartDateTime,
               GetTierCommission("2"),
                chamber.Name))
                );

            //add tier 3 history
            historyList.AddRange(
            allHistoryTier3.Select(h => new TransactionHistoryModel(
                h.RowKey,
                allCompanies.Where(c => c.RowKey == h.CompanyRowKey).SingleOrDefault().Name,
                allTransactions.Where(t => t.RowKey == h.TransactionID).SingleOrDefault().Amount,
                allProducts.Where(p => p.RowKey == h.ProductID).SingleOrDefault().ProductName,
                h.StartDateTime,
               GetTierCommission("3"),
                chamber.Name))
                );

            //add tier 4 history
            historyList.AddRange(
            allHistoryTier4.Select(h => new TransactionHistoryModel(
                h.RowKey,
                allCompanies.Where(c => c.RowKey == h.CompanyRowKey).SingleOrDefault().Name,
                allTransactions.Where(t => t.RowKey == h.TransactionID).SingleOrDefault().Amount,
                allProducts.Where(p => p.RowKey == h.ProductID).SingleOrDefault().ProductName,
                h.StartDateTime,
               GetTierCommission("4") * allTransactions.Where(t => t.RowKey == h.TransactionID).SingleOrDefault().Amount,//todo: optimize
                chamber.Name))
                );

            var orderedHistoryNoTotalsList = historyList.OrderBy(h => h.TransactionTimeStamp).ToList();
            var orderedHistoryWithTotalsList = new List<TransactionHistoryModel>();

            double runningTotal = 0;
            orderedHistoryNoTotalsList.ForEach(h =>
                {
                    h.ChamberRunningTotal = runningTotal + h.Commission;
                    orderedHistoryWithTotalsList.Add(h);        
                });

            //generate csv
            StringBuilder csvStringBuilder = new StringBuilder();
            CsvConfig<TransactionHistoryModel>.OmitHeaders = false;
            
            orderedHistoryNoTotalsList.ForEach(oh =>
            {
                var csv = CsvSerializer.SerializeToString<TransactionHistoryModel>(oh);
                csvStringBuilder.Append(csv);
                CsvConfig<TransactionHistoryModel>.OmitHeaders = true;
            });
            
            return File(ASCIIEncoding.ASCII.GetBytes(csvStringBuilder.ToString()), "text/csv","sales.csv");
        }


        private double GetTierCommission(string tier)
        {          
            //setup tiers
            var Tier1 = Settings.Default.Tier1;
            var Tier2 = Settings.Default.Tier2;
            var Tier3 = Settings.Default.Tier3;
            var Tier4 = Settings.Default.Tier4;

            if (tier == null)
            {
                return Convert.ToDouble(Tier4);
            }
            else
            {

                double currentTierCommission = Convert.ToDouble(Tier4);

                switch (Convert.ToInt16(tier))
                {
                    case 1:
                        currentTierCommission = Convert.ToDouble(Tier1);
                        break;
                    case 2:
                        currentTierCommission = Convert.ToDouble(Tier2);
                        break;
                    case 3:
                        currentTierCommission = Convert.ToDouble(Tier3);
                        break;
                    case 4:
                        currentTierCommission = Convert.ToDouble(Tier4);
                        break;
                    default:
                        currentTierCommission = Convert.ToDouble(Tier4);
                        break;
                }

                return currentTierCommission/100.00;
            }
        }

    }

   



}
