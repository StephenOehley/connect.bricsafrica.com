using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AzureHelper.Storage;
using BricsWeb.Models;
using BricsWeb.Properties;


namespace BricsWeb.Repository
{
    public class ProductRepository : CloudRepositoryBase<ProductModel>
    {
        public ProductRepository()
            : base(Settings.Default.ProductTable, Settings.Default.ConnectionString)
        { }

        public ProductModel GetByProductName(string name)
        {
            try
            {
                var product = (from p in serviceContext.CreateQuery<ProductModel>(tableName)
                               where p.ProductName == name
                               select p).Take(1).Single();
                return product;
            }
            catch (InvalidOperationException)
            {
                return null;
            }

        }

        public IList<ProductModel> GetByCategoryID(string categoryID)
        {
            try
            {
                var product = (from p in serviceContext.CreateQuery<ProductModel>(tableName)
                               where p.CategoryID == categoryID
                               select p).Take(1).ToList();

                return product;
            }
            catch (InvalidOperationException)
            {
                return null;
            }

        }

        public IList<ProductModel> GetByCompanyRowKey(string companyRowKey)
        {
            try
            {
                var product = (from p in serviceContext.CreateQuery<ProductModel>(tableName)
                               where p.CompanyID == companyRowKey
                               select p).ToList();

                return product;
            }
            catch (InvalidOperationException)
            {
                return null;
            }

        }

        public IList<ProductModel> GetAll(int count)
        {
            var entityCollection = (from entity in serviceContext.CreateQuery<ProductModel>(tableName)
                                    select entity).Take(count).ToList();

            return entityCollection;
        }
    }
}