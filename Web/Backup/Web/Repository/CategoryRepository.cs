using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BricsWeb.Models;
using AzureHelper.Storage;
using BricsWeb.Properties;

namespace BricsWeb.Repository
{
    public class CategoryRepository : CloudRepositoryBase<CategoryModel>
    {
        public CategoryRepository()
            : base(Settings.Default.CategoryTable, Settings.Default.ConnectionString)
        { }

        public IEnumerable<CategoryModel> GetRootItems()
        {
            var result = (from c in serviceContext.CreateQuery<CategoryModel>(tableName)
                          where c.Depth == 0
                          select c).ToList();

            return result;
        }

        public IEnumerable<CategoryModel> GetChildItems(string id)
        {
            var result = (from c in serviceContext.CreateQuery<CategoryModel>(tableName)
                          where c.ParentID == id
                          select c).ToList();

            return result;
        }

        public bool CheckForChildren(string id)
        {
            var result = (from c in serviceContext.CreateQuery<CategoryModel>(tableName)
                          where c.ParentID == id
                          select c).FirstOrDefault();

            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}