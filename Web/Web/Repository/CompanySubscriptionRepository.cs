using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AzureHelper.Storage;
using BricsWeb.Models;
using BricsWeb.Properties;

namespace BricsWeb.Repository
{
    public class CompanySubscriptionRepository : CloudRepositoryBase<CompanySubscriptionModel>
    {
        public CompanySubscriptionRepository()
            : base(Settings.Default.CompanySubscriptionTable, Settings.Default.ConnectionString)
        { }
    }
}