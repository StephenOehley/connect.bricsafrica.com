using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BricsWeb.Models;
using BricsWeb.Repository;

namespace BricsWeb.RepositoryHelper
{
    public class ResellerHelper
    {
        public static CompanyModel[] GetChildChambers(CompanyModel company, CompanyModel[] companyArray)
        {
            var result = new List<CompanyModel>();
            var children = companyArray.Where(c => c.ChamberID == company.RowKey).Where(c => c.BusinessType.ToUpper() == "CHAMBER").ToArray();

            result.AddRange(children);

            children.ToList().ForEach(c =>
                {
                    result.AddRange(GetChildChambers(c, companyArray));
                });

            return result.ToArray();
        }
    }
}