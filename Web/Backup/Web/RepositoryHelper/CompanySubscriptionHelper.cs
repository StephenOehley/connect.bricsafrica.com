using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BricsWeb.Models;
using BricsWeb.Repository;
//using Microsoft.ApplicationServer.Caching;

namespace BricsWeb.RepositoryHelper
{
    public class CompanySubscriptionHelper
    {
        public static IEnumerable<CompanySubscriptionModel> GetAllCompanySubscriptionsFromCache()
        {
            //DataCacheFactory cacheFactory = new DataCacheFactory();
            //DataCache cache = cacheFactory.GetCache("CompanySubscription");

            //var companysubscriptionsFromCache = cache.GetObjectsInRegion("all");

            //if (companysubscriptionsFromCache.Count() == 0)
            //{
            //    cache.CreateRegion("all");

            //    var companysubscriptionsFromTable = new CompanySubscriptionRepository().GetAll().ToList();

            //    companysubscriptionsFromTable.ForEach(product =>
            //    {
            //        try
            //        {
            //            cache.Add(product.RowKey, product, "all");
            //        }
            //        catch (Exception)
            //        {
            //        }
            //    });

            //    return companysubscriptionsFromTable;
            //}
            //else
            //{
            //    var companySubscriptions = (from p in companysubscriptionsFromCache.ToList()
            //                                select (p.Value as CompanySubscriptionModel)).ToList();

            //    return companySubscriptions;
            //}

            return new CompanySubscriptionRepository().GetAll().ToList();
               
        }
    }
}