using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BricsWeb.Models;
using System.Runtime.Caching;
using BricsWeb.Repository;
using System.Threading.Tasks;
//using Microsoft.ApplicationServer.Caching;

namespace BricsWeb.RepositoryHelper
{
    public class ProductHelper
    {
        public static ProductModel[] ProductSearch(CategoryModel[] categoryFilterArray, string searchText, string country, int page, int productsPerPage, CompanyModel[] companyArray,IEnumerable<ProductModel> productArray, out int resultsTotal,out int productDatabaseTotal)//TODO: Optimize
        {
            IEnumerable<ProductModel> allProducts;

            if (productArray == null)
            {
                allProducts = GetAllProductsFromCache(); ;
            }
            else
            {
                allProducts = productArray;
            }

            List<ProductModel> results = new List<ProductModel>();
           
            string searchTextUpper = null;
            if (searchText != null)
            {
                searchTextUpper = searchText.ToUpper();
            }

            string countryUpper = null;
            if (country != null)
            {
                countryUpper = country.ToUpper();
            }
            else
            {
                countryUpper = "ALL COUNTRIES";
            }

            try
            {
                if (categoryFilterArray.Count() >= 1)
                {
                    if (categoryFilterArray[0] == CategoryHelper.AllCategories)
                    {
                        results.AddRange(allProducts);
                    }
                    else
                    {
                        Parallel.ForEach(categoryFilterArray, category =>
                        {
                            var productsInCategory = allProducts.Where(p => p.CategoryID == category.RowKey).ToArray();

                            Parallel.ForEach(productsInCategory, product =>
                            {
                                if (searchText != string.Empty)
                                {
                                    string productTextCloud = string.Empty;

                                    if (product.Description != null)//description
                                    {
                                        productTextCloud = productTextCloud + " " + product.Description;
                                    }

                                    if (product.BrandName != null)//BrandName
                                    {
                                        productTextCloud = productTextCloud + " " + product.BrandName;
                                    }

                                    if (product.ProductName != null)//ProductName
                                    {
                                        productTextCloud = productTextCloud + " " + product.ProductName;
                                    }

                                    if (productTextCloud.ToUpper().Contains(searchTextUpper))
                                    {
                                        results.Add(product);
                                    }
                                }
                                else
                                {
                                    results.Add(product);//do not filter on searchtext - return product if it is in specified category
                                }
                            });
                        });
                    }
                }

                var resultsFiltered = new List<ProductModel>();

                if (countryUpper == "ALL COUNTRIES")
                {
                    resultsFiltered.AddRange(results);
                }
                else
                {
                    results.ForEach(r =>
                    {
                        var productCompany = companyArray.Where(c => c.RowKey == r.CompanyID).Single();

                        if (productCompany.Country != null)
                        {
                            if (productCompany.Country.ToUpper() == countryUpper)
                            {
                                resultsFiltered.Add(r);
                            }
                        }
                    });
                    
                }

                resultsTotal = resultsFiltered.Count;
                productDatabaseTotal = allProducts.Count();
                return resultsFiltered.Skip((page - 1) * productsPerPage).Take(productsPerPage).ToArray();
            }
            catch (InvalidOperationException)
            {
                resultsTotal = 0;
                productDatabaseTotal = 0;
                return null;
            }

        }

        public static bool DoesHaveProductsInCategory(ProductModel[] productArray,CategoryModel[] categoryArray,CompanyModel[] companyArray,string companyID)
        {
            var resultsCount = 0;
            var totalCount = 0;

            ProductHelper.ProductSearch(categoryArray, "", null, 1, 100, companyArray,productArray,out resultsCount,out totalCount);

            if (resultsCount == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static IEnumerable<ProductModel> GetAllProductsFromCache()
        {
            //DataCacheFactory cacheFactory = new DataCacheFactory();
            //DataCache cache = cacheFactory.GetCache("Product");            

            //var productsFromCache = cache.GetObjectsInRegion("all");

            //if (productsFromCache.Count() == 0)
            //{
            //    cache.CreateRegion("all");

            //    var productsFromTable = new ProductRepository().GetAll().ToList();

            //    productsFromTable.ForEach(product =>
            //        {
            //            try
            //            {
            //                cache.Add(product.RowKey, product, "all");
            //            }
            //            catch (Exception)
            //            {
            //            }
            //        });

            //    return productsFromTable;
            //}
            //else
            //{
            //    var products = (from p in productsFromCache.ToList()
            //                    select (p.Value as ProductModel)).ToList();

            //    return products;
            //}

            

            return new ProductRepository().GetAll().ToList();

        }

    }
}