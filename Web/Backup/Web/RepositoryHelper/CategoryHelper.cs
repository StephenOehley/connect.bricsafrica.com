using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BricsWeb.Models;
using System.Runtime.Caching;
using BricsWeb.Repository;
using BricsWeb.LocalModels;
using System.Web.Mvc;
//using Microsoft.ApplicationServer.Caching;

namespace BricsWeb.RepositoryHelper
{
    public class CategoryHelper
    {
        #region Public Methods
        public static readonly CategoryModel AllCategories = new CategoryModel(Guid.Empty.ToString(), "All Categories", string.Empty, 0, string.Empty);

        public static IEnumerable<CategoryModel> GetAllCategoriesFromCache()
        {
            //DataCacheFactory cacheFactory = new DataCacheFactory();
            //DataCache cache = cacheFactory.GetCache("Category");

            //var categoriesFromCache = cache.GetObjectsInRegion("all");

            //if (categoriesFromCache.Count() == 0)
            //{
            //    cache.CreateRegion("all");

            //    var categoriesFromTable = new CategoryRepository().GetAll().ToList();

            //    categoriesFromTable.ForEach(category =>
            //    {
            //        try
            //        {
            //            cache.Add(category.RowKey, category, "all");
            //        }
            //        catch (Exception)
            //        {
            //        }
            //    });

            //    return categoriesFromTable;
            //}
            //else
            //{
            //    var categories = (from p in categoriesFromCache.ToList()
            //                    select (p.Value as CategoryModel)).ToList();

            //    return categories;
            //}            

            return new CategoryRepository().GetAll().ToList();

        }
            
        public static CategoryModel[] GetAllAncestors(string childID, CategoryModel[] categoryArray, CategoryModel[] ancestors)
        {
            List<CategoryModel> result = new List<CategoryModel>();
            result.AddRange(ancestors.ToList());

            CategoryModel ancestor = null;

            if (childID != "0")
            {
                ancestor = (from c in categoryArray
                                where c.RowKey == childID
                                select c).SingleOrDefault();
            }
            else
            {
                ancestor = CategoryHelper.AllCategories;
            }

            var ancestorCount = (from c in categoryArray
                                 where c.RowKey == ancestor.ParentID
                                 select c).Count();

            result.Add(ancestor);

            if (ancestorCount == 0)
            {
                return result.ToArray();
            }
            else
            {
                return GetAllAncestors(ancestor.ParentID, categoryArray, result.ToArray());
            }
        }

        public static CategoryViewModel[] GetAllDescendants(string parentID, CategoryModel[] categoryArray, CategoryModel[] children)
        {
            List<CategoryViewModel> categoryList = new List<CategoryViewModel>();

            children.ToList().ForEach(node =>
            {
                var childCategoryArray = (from child in categoryArray
                                          where child.ParentID == node.RowKey
                                          select child).ToArray();

                if (childCategoryArray.Count() == 0)
                {
                    categoryList.Add(new CategoryViewModel { Parent = node, CategoryCollection = null });
                }
                else
                {
                    categoryList.Add(new CategoryViewModel { Parent = node, CategoryCollection = GetAllDescendants(node.RowKey, categoryArray, childCategoryArray).OrderBy(c => c.Parent.Name).ToList() });
                }
            });

            return categoryList.ToArray();
        }

        public static CategoryModel[] GetAllDescendantsFlattened(string parentID, CategoryModel[] categoryArray, CategoryModel[] children)
        {
            List<CategoryModel> categoryList = new List<CategoryModel>();

            children.ToList().ForEach(node =>
            {
                var childCategoryArray = (from child in categoryArray
                                          where child.ParentID == node.RowKey
                                          select child).ToArray();

                if (childCategoryArray.Count() == 0)
                {
                    categoryList.Add(node);
                }
                else
                {
                    categoryList.Add(node);
                    var nodeChildren = GetAllDescendantsFlattened(node.RowKey, categoryArray, childCategoryArray).OrderBy(c => c.Name).ToList();
                    categoryList.AddRange(nodeChildren);
                }
            });

            return categoryList.ToArray();
        }
        #endregion
    }
}