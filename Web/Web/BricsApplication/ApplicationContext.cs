using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BricsWeb.Properties;

namespace BricsWeb.BricsApplication
{
    public class ApplicationContext
    {
        public int ProductHomeProductCount { get; set; }
        public int PlatinumBannerProductCount { get; set; }
        public int SilverGoldBannerProductCount { get; set; }

        public ApplicationContext()
        {
            //get settings
            ProductHomeProductCount = Convert.ToInt32(Settings.Default.ProductHomeProductCount);
            PlatinumBannerProductCount = Convert.ToInt32(Settings.Default.PlatinumBannerProductCount);
            SilverGoldBannerProductCount = Convert.ToInt32(Settings.Default.SilverGoldBannerProductCount);
        }
    }
}