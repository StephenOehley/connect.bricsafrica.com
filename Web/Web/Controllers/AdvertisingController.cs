using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BricsWeb.Models;
using BricsWeb.Properties;
using BricsWeb.RepositoryHelper;
using BricsWeb.ExtensionMethods;
using System.Diagnostics;

namespace BricsWeb.Controllers
{
    public class FeaturedProductController : Controller
    {
        ProductModel[] _productArray = null;

        int _productHomeCount = 0;
        int _silverGoldCount = 0;
        int _platinumCount = 0;

        public FeaturedProductController()
        {
            _productArray = ProductHelper.GetAllProductsFromCache()
                .Where(p => p.PhotoUrl.ToLower().Trim().Replace("http://", string.Empty).Replace("https://", string.Empty) != 
                    Settings.Default.NoImage120Url.ToLower().Replace("http://", string.Empty).Replace("https://", string.Empty) 
                    .Trim()).Randomize()
                    .ToArray();

            _productHomeCount = Convert.ToInt32(Settings.Default.ProductHomeProductCount);
            _silverGoldCount = Convert.ToInt32(Settings.Default.SilverGoldBannerProductCount);
            _platinumCount = Convert.ToInt32(Settings.Default.PlatinumBannerProductCount);            
        }

        public FeaturedProductController(ProductModel[] products,int productHomeCount,int silverGoldCount,int platinumCount)
        {
            _productArray = products.ToList().Randomize().ToArray();
            
            _productHomeCount = productHomeCount;
            _silverGoldCount = silverGoldCount;
            _platinumCount = platinumCount;
        }

        //Obtain platinum featured products for Product Home Page - number determined by count - page determined by current minute of current hour
        public PartialViewResult GetProductHomeAsPartialView()
        {
            SetRequestNoCache();

            var platinumProductList = _productArray.Where(p => p.FeaturedProductWeight == 3).Take(_productHomeCount).ToList();

            var emptySlots = _productHomeCount - platinumProductList.Count();
            platinumProductList.AddRange(_productArray.Take(emptySlots));

            return PartialView(platinumProductList.ToArray());
        }

        public PartialViewResult GetSilverGoldBannerAsPartialView(string categoryID = "")
        {
            SetRequestNoCache();

            var cleanCategoryID = categoryID.Replace("#", string.Empty);

            ProductModel[] _productArraySortedByCategory;
            if (cleanCategoryID != string.Empty)
            {
                _productArraySortedByCategory = MoveCategoryProductsTop(_productArray, cleanCategoryID);
            }
            else
            {
                _productArraySortedByCategory = _productArray;
            }

            var silverGoldProductList = _productArraySortedByCategory.Where(p => (p.FeaturedProductWeight == 1 || p.FeaturedProductWeight == 2)).Take(_productHomeCount).ToList();

            //increase gold weighting
            var silverGoldProductWeightedList = silverGoldProductList;
            silverGoldProductList.ForEach(p =>
            {
                if (p.FeaturedProductWeight == 2)
                {
                    var weight = Convert.ToInt32(Settings.Default.GoldProductWeight);
                    for (int i=0; i <= weight; i++)
                    {
                        silverGoldProductWeightedList.Add(p);//add duplicate gold products    
                    }
                }
            });

            var emptySlots = _silverGoldCount - silverGoldProductWeightedList.Count();
            silverGoldProductWeightedList.AddRange(_productArraySortedByCategory.Where(p => (p.FeaturedProductWeight == 0)).Take(emptySlots));

            return PartialView(silverGoldProductWeightedList.ToArray());
        }

        public PartialViewResult GetPlatinumBannerAsPartialView(string categoryID = "")
        {
            SetRequestNoCache();

            var cleanCategoryID = categoryID.Replace("#", string.Empty);

            ProductModel[] _productArraySortedByCategory;
            if (cleanCategoryID != string.Empty)
            {
                _productArraySortedByCategory = MoveCategoryProductsTop(_productArray, cleanCategoryID);
            }
            else
            {
                _productArraySortedByCategory = _productArray;
            }

            var platinumProductList = _productArraySortedByCategory.Where(p => p.FeaturedProductWeight == 3).Take(_platinumCount).ToList();

            var emptySlots = _platinumCount - platinumProductList.Count();
            platinumProductList.AddRange(_productArraySortedByCategory.Where(p => p.FeaturedProductWeight == 0).Take(emptySlots));

            //platinumProductList.ForEach(p =>
            //{
            //    p.PhotoUrl.ToLower().Trim().Replace(Settings.Default.NoImage240Url.ToLower().Trim(), Settings.Default.NoImage120Url.ToLower().Trim());
            //});

            return PartialView(platinumProductList.ToArray());
        }

        private void SetRequestNoCache()
        {
            HttpContext.Response.Expires = -1;
            HttpContext.Response.Cache.SetNoServerCaching();
            Response.Cache.SetAllowResponseInBrowserHistory(false);
            Response.CacheControl = "no-cache";
            Response.Cache.SetNoStore();
        }

        private ProductModel[] MoveCategoryProductsTop(ProductModel[] products,string categoryID)
        {
            var allCategories = CategoryHelper.GetAllCategoriesFromCache().ToArray();

            List<ProductModel> result = new List<ProductModel>();
            List<ProductModel> not_in_category = new List<ProductModel>();

            var parentCategory = (from category in allCategories
                                  where category.RowKey.Equals(categoryID)
                                  select category).SingleOrDefault();//will return null for all cateogries

            var childCategoryArray = new List<CategoryModel>().ToArray();
            if (parentCategory != CategoryHelper.AllCategories)
            {
                 childCategoryArray = (from child in allCategories
                                          where child.ParentID == categoryID
                                          select child).ToArray();
            }
            else
            {
                 childCategoryArray = (from child in allCategories
                                          where child.ParentID == "0"
                                          select child).ToArray();
            }
            if (parentCategory == null)
            {
                parentCategory = CategoryHelper.AllCategories;
            }

            var categoryDescendantsArray = CategoryHelper.GetAllDescendantsFlattened(categoryID, allCategories, childCategoryArray);

            products.ToList().ForEach(p =>
                {
                    if (p.CategoryID != null)
                    {
                        var productCategory = allCategories.Where(c => c.RowKey == p.CategoryID).SingleOrDefault();

                        if (productCategory != null)
                        {
                            var count = categoryDescendantsArray.Where(c => c.RowKey == productCategory.RowKey).Count();

                            if (count == 1)
                            {
                                result.Add(p);
                            }
                            else
                            {
                                not_in_category.Add(p);
                            }
                        }
                        else
                        {
                            not_in_category.Add(p);
                        }
                    }
                    else
                    {
                        not_in_category.Add(p);
                    }
                });

            var randomResult = result.Randomize().ToList();
            randomResult.AddRange(not_in_category);

            //randomResult.Reverse();
            return randomResult.ToArray();
        }
    }
}
