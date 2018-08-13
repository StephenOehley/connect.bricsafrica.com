using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.WindowsAzure.StorageClient;
using MvcHelper.UI.Helpers;

namespace BricsWeb.Models
{
    public class CompanyModel : TableServiceEntity
    {
        public CompanyModel()
        { }

        public CompanyModel(string userRowKey,string name,string salesEmailAddress,string accountsEmailAddress,string vatnumber)
        {
            RowKey = userRowKey;

            Name = name;
            SalesEmail = salesEmailAddress;
            AccountsEmail = accountsEmailAddress;
            VatNumber = vatnumber;

            IsGreenCertified = false;
            IsChamberCertified = false;

            PartitionKey = RowKey.Substring(0, 1);
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Company Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Business Type")]
        public string BusinessType { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [ValidateEmailAttribute]
        [Display(Name = "Sales Email")]
        public string SalesEmail { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [ValidateEmailAttribute]
        [Display(Name = "Accounts Email")]
        public string AccountsEmail { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "VAT Number")]
        public string VatNumber { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Fax")]
        public string Fax { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Province Or State")]
        public string ProvinceState { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Zip")]
        public string Zip { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Website URL")]
        public string WebsiteUrl { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Logo URL")]
        public string LogoUrl { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Contact Position")]
        public string ContactPosition { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Contact Mobile")]
        public string ContactMobile { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Contact Phone")]
        public string ContactPhone { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Products\\Services Offered")]
        public string ProductsOrServices { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Street Address (Registered)")]
        public string StreetAddressRegistered { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Country (Registered)")]
        public string CountryRegistered { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Province Or State (Registered)")]
        public string ProvinceStateRegistered { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Postal Code\\Zip (Registered)")]
        public string ZipRegistered { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Brands")]
        public string Brands { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Number Of Employees")]
        public string Employees { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Ownership & Capital")]
        public string OwnershipAndCapital { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Year Established")]
        public int YearEstablished { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Legal Representitive \\ Business Owner")]
        public string LegalRepresentitiveOrOwnerName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Main Market 0")]
        public string MainMarket0 { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Main Market 1")]
        public string MainMarket1 { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Main Market 2")]
        public string MainMarket2 { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Total Sales Volume")]
        public string TotalSalesVolume { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Sales Export Percentage")]
        public string SalesExportPercentage { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Annual Purchase Volume (USD)")]
        public int AnnualPurchaseVolume { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Trade Shows")]
        public string TradeShows { get; set; }

        [Display(Name = "Green Certified")]
        public bool IsGreenCertified { get; set; }

        [Display(Name = "Chamber Certified)")]
        public bool IsChamberCertified { get; set; }

        [Display(Name = "Verified)")]
        public bool IsVerified { get; set; }

        public string ChamberID { get; set; }
        public string Tier { get; set; }
    }
}