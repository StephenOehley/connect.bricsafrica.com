using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AzureHelper.Storage;
using BricsWeb.Models;
using BricsWeb.Properties;

namespace BricsWeb.Repository
{
    public class BuyerRequestRepository : CloudRepositoryBase<BuyerRequestModel>
    {
        public BuyerRequestRepository()
            : base(Settings.Default.BuyerRequestTable, Settings.Default.ConnectionString)
        { }

        public BuyerRequestModel[] GetByCompanyID(string companyID)
        {
            try
            {
                var request = (from r in serviceContext.CreateQuery<BuyerRequestModel>(tableName)
                               where r.CompanyID == companyID
                               select r).ToArray();
                return request;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}