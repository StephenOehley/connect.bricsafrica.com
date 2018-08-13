using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BricsWeb.Models;
using BricsWeb.RepositoryHelper;

namespace BricsWeb.LocalModels
{
    public class CategoryViewModel
    {
        public CategoryModel Parent { get; set; }
        public List<CategoryViewModel> CategoryCollection { get; set; }

        public int ProductCount { get; set; }
        public int CompanyCount { get; set; }

        public CategoryViewModel()
        {
            CategoryCollection = new List<CategoryViewModel>();
        }

        public void PopulateProductCount(ProductModel[] allProducts,CategoryModel[] allCategories,CompanyModel[] allCompanies)
        {
            int childCategoryCount = 0;
            int childCompanyCount = 0;

            if (CategoryCollection != null)
            {
                Parallel.ForEach(CategoryCollection, category =>
                {
                    category.PopulateProductCount(allProducts, allCategories,allCompanies);
                });

                childCategoryCount = CategoryCollection.Sum(c => c.ProductCount);
                childCompanyCount = CategoryCollection.Sum(c => c.CompanyCount);
            }

            var currentCategoryCount = allProducts.Where(p => p.CategoryID == Parent.RowKey).Count();
            var currentCompanyCount = allProducts.Where(p => p.CategoryID == Parent.RowKey).Select(p => new { CompanyID = p.CompanyID }).Distinct().Where(pc => (allCompanies.Where(c => c.RowKey == pc.CompanyID).SingleOrDefault() != null)).Count();          

            ProductCount = currentCategoryCount + childCategoryCount;
            CompanyCount = currentCompanyCount + childCompanyCount;
        }
    }
}