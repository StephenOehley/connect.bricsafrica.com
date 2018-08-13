using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace BricsWeb.Models
{
    public class SubscriptionProductModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Level { get; set; }
        public int MaxFeatured { get; set; }
        
        public string DisplayPrice
        {
            get
            {
                CultureInfo cultureInfo = new CultureInfo("en-US");
                cultureInfo.NumberFormat.CurrencySymbol = "ZAR ";

                Thread.CurrentThread.CurrentCulture = cultureInfo;

                double price = Price / 100;
                return String.Format("{0:C}", Convert.ToInt32(price));
            }
        }
    }
}