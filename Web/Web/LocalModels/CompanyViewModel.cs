using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BricsWeb.Models;
using BricsWeb.Properties;

namespace BricsWeb.LocalModels
{
    public class CompanyViewModel
    {
        public CompanyViewModel()
        {
        }

        public CompanyViewModel(CompanyModel model,CompanySubscriptionModel[] allSubscriptions,CountryModel[] allCountries,SubscriptionProductModel[] allSubscriptionProducts)
        {
            CompanyData = model;

            //get countryname and flagurl
            string countryName = "Unknown";
            string countryCode = "00";

            if (model.Country != null)
            {
                var countryModel = allCountries.Where(c => c.Name.ToLower().Trim() == model.Country.ToLower().Trim()).SingleOrDefault();
                if (countryModel != null) { countryCode = countryModel.Iso2DigitCode; countryName = countryModel.Name; }//country found so set name and code                      
            }

            //get subscription status
            int companyLevel = 0;
            var companySubscription = allSubscriptions.Where(s => s.CompanyRowKey == model.RowKey).SingleOrDefault();
            if (companySubscription != null)
            {
                if (!(companySubscription.StartDateTime.AddYears(1) >= DateTime.UtcNow))
                {
                    companySubscription = null;//subscription is expired so make null
                }
                else
                {
                    var companySubscriptionProduct = allSubscriptionProducts.Where(sp => sp.ID == companySubscription.ProductID).SingleOrDefault();
                    if (companySubscriptionProduct != null)
                    {
                        companyLevel = companySubscriptionProduct.Level;
                    }
                }
            }

            Level = companyLevel;
            FlagUrl = Settings.Default.FlagUrlPath.ToLower().Trim().Replace("%countrycode%", countryCode).ToLower();
        }

        public string GetLogoUrlWithMissingProtection()
        {
            if (CompanyData.LogoUrl.ToLower().Trim() == "_blank")
            {
                return Settings.Default.NoImage120Url;
            }
            else
            {
                return CompanyData.LogoUrl.ToLower().Trim();
            }
        }

        public CompanyModel CompanyData { get; set; }
        public HttpPostedFileWrapper logo { get; set; }
        public string test { get; set; }

        public bool IsChamber { get; set; }

        public string FlagUrl { get; set; }
        public int Level { get; set; }

        public string ChamberSubscribeUrl { get; set; }
        public string GreenSubscribeUrl { get; set; }
    }
}