using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BricsWeb.Models;

namespace BricsWeb.LocalModels
{
    public class CompanySearchResultViewModel
    {
        public CompanyViewModel[] Results { get; private set; }
        public int ProductsPerPage { get; private set; }
        public int CurrentPage { get; private set; }
        public CategoryModel Category {get; private set;}

        public int TotalPages { get; set; }
        public int TotalResults { get; set; }

        public string SearchDiagnosticInformation { get; set; }

        public CompanySearchResultViewModel(CompanyViewModel[] results, int productsPerPage, int currentPage, CategoryModel category, int totalPages,int totalResults)
        {
            Results = results;
            ProductsPerPage = productsPerPage;
            CurrentPage = currentPage;
            Category = category;
            TotalPages = totalPages;
            TotalResults = totalResults;
        }
    }
}