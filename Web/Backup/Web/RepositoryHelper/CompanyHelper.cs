using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BricsWeb.Models;
using System.Runtime.Caching;
using BricsWeb.Repository;
//using Microsoft.ApplicationServer.Caching;

namespace BricsWeb.RepositoryHelper
{
    public class CompanyHelper
    {
        public static IEnumerable<CompanyModel> GetAllCompaniesFromCache()
        {
            //DataCacheFactory cacheFactory = new DataCacheFactory();
            //DataCache cache = cacheFactory.GetCache("Company");

            //var companiesFromCache = cache.GetObjectsInRegion("all");

            //if (companiesFromCache.Count() == 0)
            //{
            //    cache.CreateRegion("all");

            //    var companiesFromTable = new CompanyRepository().GetAll().ToList();

            //    companiesFromTable.ForEach(company =>
            //    {
            //        try
            //        {
            //            cache.Add(company.RowKey, company, "all");
            //        }
            //        catch (Exception)
            //        {
            //        }
            //    });

            //    return companiesFromTable;
            //}
            //else
            //{
            //    var companies = (from p in companiesFromCache.ToList()
            //                    select (p.Value as CompanyModel)).ToList();

            //    return companies;
            //}

            return new CompanyRepository().GetAll().ToList();
        }
    }
}