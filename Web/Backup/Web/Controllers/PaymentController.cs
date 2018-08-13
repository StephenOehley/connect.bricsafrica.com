using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcHelper.Mvc;
using BricsWeb.LocalModels;
using BricsWeb.Repository;
using System.Web.Security;
using BricsWeb.Properties;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using BricsWeb.Models;

namespace BricsWeb.Controllers
{
    public class PaymentController : CrystalController
    {
        const string Status_Submit = "VcsRequestAutoSubmit";
        const string Status_Ok = "PurchaseOk";
        const string Status_Fail = "PurchaseFail";
        const string Status_CallbackOk = "CallbackOk";
        const string Status_CallbackFail = "CallbackFail";

        [Authorize]
        public ActionResult Purchase(string ProductID, string UpgradeProductID)//TODO: add validation of upgradeproductid for current company - for security reasons
        {
            if (ProductID != null && UpgradeProductID != null)
            {
                var userRowKey = (Membership.GetUser().ProviderUserKey as string);
                var company = new CompanyRepository().GetByRowKey(userRowKey);
                if (company == null)
                {
                    return RedirectToAction("UpdateDetailsRequest");//company details do not exist
                }

                var selectedSubscriptionProduct = new SubscriptionProductRepository().GetAll().Where(product => product.ID == ProductID).SingleOrDefault();
                if (selectedSubscriptionProduct == null)
                {
                    return RedirectToAction("Select");//selected productid is invalid
                }

                CompanySubscriptionModel existingSubscription = null;
                if (UpgradeProductID != "none")
                {
                    existingSubscription = new CompanySubscriptionRepository().GetByRowKey(UpgradeProductID);
                }
               
                var purchaseViewModel = new PurchaseViewModel { AccountsEmail = company.AccountsEmail, Name = company.Name, VatNumber = company.VatNumber, SelectedProduct = selectedSubscriptionProduct };

                if (existingSubscription == null)
                {
                    ViewBag.IsProrata = false;
                    ViewBag.ProrataDiscount = "ZAR 0.00";
                    purchaseViewModel.AmountDue = selectedSubscriptionProduct.Price;
                }
                else
                {
                    //calculate prorata discount amount  

                    var subscriptionDaysRemaining = (existingSubscription.StartDateTime.AddYears(1).Subtract(DateTime.UtcNow)).Days;
                    double subscriptionCostPerDay = new SubscriptionProductRepository().GetAll().Where(s => s.ID == existingSubscription.ProductID).Single().Price / 366;
                    double amountRemaining = subscriptionDaysRemaining * subscriptionCostPerDay;

                    CultureInfo cultureInfo = new CultureInfo("en-US");
                    cultureInfo.NumberFormat.CurrencySymbol = string.Empty;
                    Thread.CurrentThread.CurrentCulture = cultureInfo;                                    
                    
                    string discountRounded = String.Format("{0:C}", Convert.ToInt32(amountRemaining / 100));

                    ViewBag.IsProrata = true;
                    ViewBag.ProrataDiscount = "ZAR " + discountRounded;
                    double amountDue = Convert.ToDouble(selectedSubscriptionProduct.Price)/100 - Convert.ToDouble(discountRounded);
                    purchaseViewModel.AmountDue = Convert.ToInt32(amountDue*100);                                
                }

                return MenuView(purchaseViewModel, "FIND A PRODUCT", "SubMenuFindAProduct", string.Empty);
            }
            else
            {//no product selected or upgrade product is null (should be 'none')
                return RedirectToAction("Select");
            }
        }

        public ActionResult UpdateDetailsRequest()
        {
            return MenuView("MY PROFILE", "SubMenuMyProfile", "None");
        }

        [Authorize]
        public ActionResult Select()
        {
            var userRowKey = (Membership.GetUser().ProviderUserKey as string);
            var companySubscription = new CompanySubscriptionRepository().GetAll().Where(c => c.CompanyRowKey == userRowKey).SingleOrDefault();
            var subscriptionProductList = new SubscriptionProductRepository().GetAll();

            if (companySubscription != null)
            {
                if (!(companySubscription.StartDateTime.AddYears(1) >= DateTime.UtcNow))
                {
                    companySubscription = null;//subscription is expired so make null
                }
            }

            if (companySubscription != null)
            {
                var CurrentSubscriptionDescription = string.Empty;
                var currentCompanySubscriptionProduct = subscriptionProductList.Where(s => s.ID == companySubscription.ProductID).Single();

                //subscription is valid
                ViewBag.CurrentSubscriptionDescription = currentCompanySubscriptionProduct.Name;
                ViewBag.UpgradeSubscription = companySubscription.RowKey;

                return MenuView(subscriptionProductList.Where(product => (product.Level > currentCompanySubscriptionProduct.Level)).ToArray(), "MY PROFILE", "SubMenuMyProfile", "None");              
            }
            else
            {
                //subscription is expired or does not exist
                ViewBag.CurrentSubscriptionDescription = "Bronze Subscription (Free)";
                ViewBag.UpgradeSubscription = "none";

                return MenuView(subscriptionProductList.ToArray(), "MY PROFILE", "SubMenuMyProfile", "None");
            }
        }

        public ActionResult PurchaseRequest(PurchaseViewModel purchaseModel)
        {
            var userRowKey = (Membership.GetUser().ProviderUserKey as string);

            var company = new CompanyRepository().GetByRowKey(userRowKey);
            var subscriptionInfo = new CompanySubscriptionRepository().GetByRowKey(userRowKey);

            company.Name = purchaseModel.Name;
            company.AccountsEmail = purchaseModel.AccountsEmail;
            company.VatNumber = purchaseModel.VatNumber;

            Trace.TraceInformation("PurchaseRequest() [Save] CompanyID=" + company.RowKey);
            new CompanyRepository().Save(company);

            return RedirectToAction("VcsRequestAutoSubmit", new { ProductID = purchaseModel.SelectedProduct.ID, Amount = purchaseModel.AmountDueRoundedNoCurrencySymbol });
        }

        public ActionResult PurchaseOk(string p2,string p3,string m_1,string pam,string CardHolderIpAddr)
        {
            var userRowKey = (Membership.GetUser().ProviderUserKey as string);

            var transactionID = p2;
            var companyID = m_1;
            var response = p3;

            var company = new CompanyRepository().GetByRowKey(userRowKey);

            if (company.RowKey != companyID)
            {
                throw new Exception("Security Exception - Your IP Address has been logged");
            }

            var transactionExists = new TransactionRepository().IsTransactionValid(transactionID);

            if (pam.Equals("D182986D-23A6-4AB5-8D61-F34AD967942C") && transactionExists)
            {
                //success
                new TransactionRepository().Save(new Models.TransactionModel(transactionID, company.RowKey, Status_Ok,null));
                ProcessUpgrade(transactionID);
            }

            return MenuView("FIND A PRODUCT", "SubMenuFindAProduct", string.Empty);
        }

        public ActionResult PurchaseDeclined(string p2 = "", string p3 = "", string m_1 = "", string pam = "", string CardHolderIpAddr = "")
        {
            //var info = "PurchaseDeclined() p2 = {0} , p3 = {1}, m_1 = {2}, pam = {3}, CardHolderIpAddr = {4}";
            //Trace.TraceInformation(String.Format(info), p2, p3, m_1, pam, CardHolderIpAddr);

            var userRowKey = (Membership.GetUser().ProviderUserKey as string);

            var transactionID = p2;
            var companyID = m_1;
            var response = p3;

            var company = new CompanyRepository().GetByRowKey(userRowKey);

            if (company.RowKey != companyID)
            {
                throw new Exception("Security Exception - Your IP Address has been logged");
            }

            new TransactionRepository().Save(new Models.TransactionModel(transactionID, company.RowKey,Status_CallbackFail,null,response));

            return MenuView("FIND A PRODUCT", "SubMenuFindAProduct", string.Empty);
        }

        public ActionResult PurchaseCallback(string p2 = "", string p3 = "", string m_1 = "", string pam = "", string CardHolderIpAddr = "")
        {
            try
            {
                var transactionID = p2;
                var companyID = m_1;
                var response = p3;

                var company = new CompanyRepository().GetByRowKey(companyID);

                if (company.RowKey != companyID)
                {
                    throw new Exception("PurchaseCallback() [Security Exception]");
                }

                var transactionExists = new TransactionRepository().IsTransactionValid(transactionID);
                bool fail = false;

                if (pam.Equals("D182986D-23A6-4AB5-8D61-F34AD967942C") && transactionExists)
                {
                    if (response.Count() >= 14)
                    {
                        if (response.Substring(6).Trim().Equals("Approved", StringComparison.InvariantCultureIgnoreCase))
                        {
                            //success
                            new TransactionRepository().Save(new Models.TransactionModel(transactionID, company.RowKey, Status_CallbackOk,null));
                            ProcessUpgrade(transactionID);
                        }
                        else
                        {
                            fail = true;
                        }
                    }
                    
                    if (fail)
                    {
                        new TransactionRepository().Save(new Models.TransactionModel(transactionID, company.RowKey,Status_CallbackFail, null,response));
                        return RedirectToAction("PurchaseDeclined");
                    }
                }

                Trace.TraceInformation("PurchaseCallback() [Accepted]");
                ViewBag.Response = "Accepted";
                return View();
            }
            catch (Exception e)
            {
                Trace.TraceInformation("PurchaseCallback() [Error] : " + e.Message);
                throw e;
            }
        }

        public ActionResult VcsRequestAutoSubmit (string ProductID,string Amount)
        {
            var userRowKey = (Membership.GetUser().ProviderUserKey as string);
            var company = new CompanyRepository().GetByRowKey(userRowKey);

            var transactionID = Convert.ToBase64String(Guid.NewGuid().ToByteArray());//returns a 24 character transactionID

            var transaction = new TransactionViewModel { 
                TerminalID_p1 = Settings.Default.TerminalID, 
                TransactionID_p2 = transactionID,
                Description_p3 = new SubscriptionProductRepository().GetAll().Where(s => s.ID == ProductID).Single().Name,
                Amount_p4 = Amount,//assume that amount is allready rounded to nearest cent
                CardholderEmail_CardHolderEmail = company.AccountsEmail,
                CompanyID_m_1 = company.RowKey,
                HashPassword_HashPassword = Settings.Default.Hash
            };

            transaction.Hash_Hash = transaction.GenerateHash();

            new TransactionRepository().Save(new Models.TransactionModel(transactionID,company.RowKey,Status_Submit,ProductID,string.Empty,Convert.ToDouble(Amount)));

            return View(transaction);
        }

        private void ProcessUpgrade(string transactionID)
        {
            var transaction = new TransactionRepository().GetTransactionsByTransactionID(transactionID).Where(t => t.Description.ToLower().Trim() == Status_Submit.ToLower().Trim()).SingleOrDefault();
            if (transaction == null)
            {
                throw new Exception("ProcessUpgrade() => Unable to find transaction: " + transactionID);
            }

            var product = new SubscriptionProductRepository().GetAll().Where(p => p.ID.ToLower().Trim() == transaction.ProductID.ToLower().Trim()).SingleOrDefault();
            if (product == null)
            {
                throw new Exception("ProcessUpgrade() => Unable to find product: " + transaction.ProductID);
            }

            var company = new CompanyRepository().GetByRowKey(transaction.CompanyID);                       

            string chamber1Rowkey = null;
            string chamber2Rowkey = null;
            string chamber3Rowkey = null;
            string chamber4Rowkey = null;

            if (company.ChamberID != null)
            {
                var chamber1 = new CompanyRepository().GetByRowKey(company.ChamberID);

                if (chamber1.ChamberID != null)
                {
                    var chamber2 = new CompanyRepository().GetByRowKey(chamber1.ChamberID);
                    chamber1Rowkey = chamber1.RowKey;

                    if (chamber2.ChamberID != null)
                    {
                        var chamber3 = new CompanyRepository().GetByRowKey(chamber2.ChamberID);
                        chamber2Rowkey = chamber2.RowKey;

                        if (chamber3 != null)
                        {
                            var chamber4 = new CompanyRepository().GetByRowKey(chamber3.ChamberID);
                            chamber3Rowkey = chamber3.RowKey;

                            if (chamber4 != null)
                            {
                                chamber4Rowkey = chamber4.RowKey;
                            }
                        }
                    }
                }
            }

            var companySubscription = new CompanySubscriptionRepository().GetAll().Where(c => c.CompanyRowKey == company.RowKey).SingleOrDefault();

            if (companySubscription != null)
            {
                new CompanySubscriptionRepository().Delete(companySubscription);
            }

            new CompanySubscriptionRepository().Save(new Models.CompanySubscriptionModel(product.ID, chamber1Rowkey, chamber2Rowkey, chamber3Rowkey, chamber4Rowkey,transactionID,company.RowKey));
        }
    }
}
