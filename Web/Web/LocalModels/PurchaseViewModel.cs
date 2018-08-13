using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MvcHelper.UI.Helpers;
using BricsWeb.Models;
using System.Globalization;
using System.Threading;

namespace BricsWeb.LocalModels
{
    public class PurchaseViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Company Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [ValidateEmailAttribute]
        [Display(Name = "Accounts Email")]
        public string AccountsEmail { get; set; }

        //[Required]
        [DataType(DataType.Text)]
        [Display(Name = "VAT Number")]
        public string VatNumber { get; set; }

        public SubscriptionProductModel SelectedProduct { get; set; }

        public int AmountDue { get; set; }

        public string DisplayAmount
        {
            get
            {
                CultureInfo cultureInfo = new CultureInfo("en-US");
                cultureInfo.NumberFormat.CurrencySymbol = "ZAR ";

                Thread.CurrentThread.CurrentCulture = cultureInfo;//TODO: find a better way without modifying current thread culture - in all places

                return String.Format("{0:C}", AmountDue/100);
            }
        }

        public string AmountDueRoundedNoCurrencySymbol
        {
            get
            {
                CultureInfo cultureInfo = new CultureInfo("en-US");
                cultureInfo.NumberFormat.CurrencySymbol = string.Empty;

                Thread.CurrentThread.CurrentCulture = cultureInfo;

                return String.Format("{0:C}", AmountDue/100);
            }
        }
    }
}