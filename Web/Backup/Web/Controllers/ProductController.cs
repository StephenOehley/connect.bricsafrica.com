using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcHelper.Mvc;
using BricsWeb.Models;
using BricsWeb.Repository;
using System.Web.Security;
using BricsWeb.LocalModels;
using System.IO;
using BricsWeb.RepositoryHelper;
using ImageResizer;
using System.Diagnostics;
using BricsWeb.Properties;

namespace BricsWeb.Controllers
{
    public class ProductController : CrystalController
    {
        #region View Result Actions
        public ActionResult Search()
        {
            return MenuView("Find A Product", "SubMenuFindAProduct", "Product Search");
        }

        //
        // GET: /Product/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Product/Details/5
        [OutputCache(Duration = 10800, VaryByParam = "*")]
        public ActionResult Details(string id)
        {
            var product = new ProductRepository().GetByRowKey(id);

            var company = new CompanyRepository().GetByRowKey(product.CompanyID);

            if (company != null)
            {
                var productViewModel = new ProductSearchResultModel
                {
                    BusinessType = company.BusinessType,
                    CompanyName = company.Name,
                    IsChamberCertified = company.IsChamberCertified,
                    IsGreenCertified = company.IsGreenCertified,
                    CompanyID = company.RowKey,
                    Product = product
                };

                return MenuView(productViewModel, "Find A Product", "SubMenuFindAProduct", "Product Search");
            }
            else
            {
                return Content("Product Company Does Not Exit - This Listing Is Hidden");
            }
        }

        //
        // GET: /Product/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Product/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Product/Edit/5
        [Authorize]
        public ActionResult Edit()
        {
            var userRowKey = (Membership.GetUser().ProviderUserKey as string);
            var firstProduct = new ProductRepository().GetAll().Where(p => p.CompanyID.Equals(userRowKey, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (firstProduct != null)
            {
                var productViewModel = new ProductViewModel { ProductData = firstProduct, CategoryName = string.Empty };//pass in the first product found to the edit view 
                return MenuView(productViewModel, "MY PROFILE", "SubMenuMyProfile", "Product Profile");
            }
            else
            {//TODO: Set a default product image and then check for this url on save to avoid duplicates
                var blankProduct = new ProductModel("My New Product", "Please enter a description", string.Empty, userRowKey);//CompanyRowKey <==> UserRowkey

                var productViewModel = new ProductViewModel { ProductData = blankProduct, CategoryName = string.Empty };//pass in the first product found to the edit view 
                return MenuView(productViewModel, "MY PROFILE", "SubMenuMyProfile", "Product Profile");
            }
        }

        [Authorize(Roles = "Webmaster")]
        public ActionResult MasterEdit()
        {
            List<ProductViewModel> productList = new List<ProductViewModel>(); 

            var companyArray = CompanyHelper.GetAllCompaniesFromCache().OrderBy(c => c.Name).ToArray();
            var productArray = ProductHelper.GetAllProductsFromCache().ToArray();

            companyArray.ToList().ForEach(c =>
            {
                var companyProducts = productArray.Where(p => p.CompanyID == c.RowKey).OrderBy(p => p.ProductName).ToArray();

                companyProducts.ToList().ForEach(pa =>
                 {
                     productList.Add(new ProductViewModel { ProductData = pa, CategoryName = string.Empty,CompanyName = c.Name });
                 });
            });

            return MenuView(productList.ToArray(), "WEBMASTER", "SubMenuWebmaster", "Product Master Edit");
        }

        //
        // GET: /Product/Delete/5

        public ActionResult Delete(string id)
        {
            var userRowKey = (Membership.GetUser().ProviderUserKey as string);
            var product = new ProductRepository().GetByRowKey(id);

            if (userRowKey == product.CompanyID)
            {
                new ProductRepository().Delete(product);
                return Json(new { status = "success", message = "Product deleted successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = "error", message = "Product Delete Failed - Security Violation Occurred. Your IP Address has been logged" }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Webmaster")]
        public ActionResult MasterDelete(string id)
        {
            var product = new ProductRepository().GetByRowKey(id);
            new ProductRepository().Delete(product);//TODO: delete product photo

            return RedirectToAction("MasterEdit");
        }

        //
        // POST: /Product/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region PartialView Result Actions

        public PartialViewResult GetEditFormAsPartialView(string id)
        {
            var userRowKey = (Membership.GetUser().ProviderUserKey as string);
            var companySubscription = new CompanySubscriptionRepository().GetAll().Where(c => c.CompanyRowKey == userRowKey).SingleOrDefault();

            bool isSilverGoldPlatinum = false;
            int maxFeaturedProductCount = 0;

            if (companySubscription != null)
            {
                isSilverGoldPlatinum = true;
                var subscription = new SubscriptionProductRepository().GetAll().Where(p => p.ID == companySubscription.ProductID).SingleOrDefault();
                maxFeaturedProductCount = subscription.MaxFeatured;
            }

            HttpContext.Response.Expires = -1;
            HttpContext.Response.Cache.SetNoServerCaching();
            Response.Cache.SetAllowResponseInBrowserHistory(false);
            Response.CacheControl = "no-cache";
            Response.Cache.SetNoStore();

            var product = new ProductModel();

            if (id == null)
            {
                product = new ProductModel("My New Product", "Please enter a description...", @"http://matchmaker.blob.core.windows.net/image/NoImage240.jpg", (userRowKey as string));//CompanyRowKey <==> UserRowkey
            }
            else
            {
                var cleanid = id.Replace("#", string.Empty);//cleanup id string
                product = new ProductRepository().GetByRowKey(cleanid);
            }

            if (product != null)
            {
                if (product.FeaturedProductWeight == null)
                {
                    product.FeaturedProductWeight = 0;
                }

                string categoryName = string.Empty;

                if (product.CategoryID != null)
                {
                    categoryName = new CategoryRepository().GetByRowKey(product.CategoryID).Name;
                }

                var productViewModel = new ProductViewModel { ProductData = product, CategoryName = categoryName, ShowFeaturedProductOption = isSilverGoldPlatinum, MaxFeaturedProductCount = maxFeaturedProductCount };

                if (productViewModel.ProductData.FeaturedProductWeight >= 1)
                {
                    productViewModel.IsFeaturedProduct = true;
                }
                else
                {
                    productViewModel.IsFeaturedProduct = false;
                }

                return PartialView(productViewModel);
            }
            else
            {
                var blankProduct = new ProductModel("My New Product", "Please enter a description",@"http://matchmaker.blob.core.windows.net/image/NoImage240.jpg", userRowKey);//CompanyRowKey <==> UserRowkey
                var productViewModel = new ProductViewModel { ProductData = blankProduct, CategoryName = string.Empty,ShowFeaturedProductOption = isSilverGoldPlatinum,MaxFeaturedProductCount = maxFeaturedProductCount };
                return PartialView(productViewModel);
            }
        }

        [OutputCache(Duration = 10800, VaryByParam = "*")]
        public PartialViewResult GetSearchResultsAsPartialView(string country, int page = 1, int productsPerPage = 25, string categoryID = "#00000000-0000-0000-0000-000000000000",string searchText="")
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var allCategories = CategoryHelper.GetAllCategoriesFromCache().ToArray();
            var allCompanies = CompanyHelper.GetAllCompaniesFromCache().ToArray();
            var allSubscriptions = CompanySubscriptionHelper.GetAllCompanySubscriptionsFromCache().ToArray();
            var allCountries = new CountryRepository().GetAll().ToArray();
            var allSubscriptionProducts = new SubscriptionProductRepository().GetAll().ToArray();

            CategoryModel parentCategory = null;
            var cleanCategoryID = categoryID.Replace("#", string.Empty);

            //get parent category of sub categories
            if ((categoryID != null))
            {
                parentCategory = (from category in allCategories
                                  where category.RowKey.Equals(cleanCategoryID)
                                  select category).SingleOrDefault();//will return null for all cateogries
            }

            //check if root category was selected
            if (parentCategory == null)
            {
                parentCategory = CategoryHelper.AllCategories;
            }

            //var childCategoryArray = new CategoryModel[0];
            var childCategoryArray = new List<CategoryModel>().ToArray();
            if (parentCategory != CategoryHelper.AllCategories)
            {
                 childCategoryArray = (from child in allCategories
                                          where child.ParentID == cleanCategoryID
                                          select child).ToArray();
            }
            else
            {
                 childCategoryArray = (from child in allCategories
                                          where child.ParentID == "0"
                                          select child).ToArray();
            }

            var categoryAndDescendantsList = CategoryHelper.GetAllDescendantsFlattened(cleanCategoryID, allCategories, childCategoryArray).ToList();
            categoryAndDescendantsList.Add(parentCategory);
            var categoryAndDescendantsArray = categoryAndDescendantsList.ToArray();

            int resultsTotal;
            int productsDatabaseTotal;
            var results = ProductHelper.ProductSearch(categoryAndDescendantsArray, searchText, country, page, productsPerPage, allCompanies.ToArray(),null,out resultsTotal,out productsDatabaseTotal);
            List<ProductSearchResultModel> productSearchResults = new List<ProductSearchResultModel>();

            results.ToList().ForEach(productModel =>//TODO: Search on country criteria TODO: Cache company details for this loop and avoid getting all at start
            {
                var company = allCompanies.Where(f => f.RowKey == (productModel.CompanyID)).SingleOrDefault();

                if (company != null)//if company details have not been supplied - exclude products from the search results
                {
                    //get countryname and flagurl
                    string countryName = "Unknown";
                    string countryCode = "00";
                    
                    if (company.Country != null)
                    {
                        var countryModel = allCountries.Where(c=> c.Name.ToLower().Trim() == company.Country.ToLower().Trim()).SingleOrDefault();
                        if (countryModel != null) {countryCode = countryModel.Iso2DigitCode;countryName = countryModel.Name;}//country found so set name and code                      
                    }                

                    //get subscription status
                    int companyLevel = 0;
                    var companySubscription = allSubscriptions.Where(s => s.CompanyRowKey == company.RowKey).SingleOrDefault();
                    if (companySubscription != null)
                    {
                        if (!(companySubscription.StartDateTime.AddYears(1) >= DateTime.UtcNow))
                        {
                            companySubscription = null;//subscription is expired so make null
                        }
                        else
                        {
                            var companySubscriptionProduct = allSubscriptionProducts.Where(sp => sp.ID == companySubscription.ProductID).SingleOrDefault();
                            if (companySubscriptionProduct != null)
                            {
                                companyLevel = companySubscriptionProduct.Level;
                            }
                        }
                    }

                    var isVerified = company.IsVerified;
                    var isChamberCertified = company.IsChamberCertified;
                    var isGreenCertified = company.IsGreenCertified;

                    productSearchResults.Add(new ProductSearchResultModel 
                    { 
                        CompanyName = company.Name,
                        Product = productModel, 
                        BusinessType = company.BusinessType, 
                        IsChamberCertified = isChamberCertified, 
                        IsGreenCertified = isGreenCertified, 
                        IsVerified = isVerified, 
                        Country = countryName, 
                        FlagUrl = Settings.Default.FlagUrlPath.ToLower().Trim().Replace("%countrycode%",countryCode).ToLower(), 
                        CompanySubscriptionLevel = companyLevel, 
                        CompanyID = company.RowKey
                    });
                }
            });

            if (productSearchResults.Count() == 0)//set current page to zero if there are no results
            {
                page = 0;
            }

            var totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(resultsTotal) / Convert.ToDouble(productsPerPage)));
            var searchResultViewModel = new ProductSearchResultViewModel(productSearchResults.ToArray(), productsPerPage, page, parentCategory, totalPages, resultsTotal);

            stopWatch.Stop();

            if (Settings.Default.IsDiagnosticsModeEnabled.ToLower() == "true")
            {
                searchResultViewModel.SearchDiagnosticInformation = "[" + DateTime.UtcNow.ToLongTimeString() + " UTC Searched " + productsDatabaseTotal.ToString() + " Products in " + stopWatch.ElapsedMilliseconds + "ms]";
            }

            return PartialView(searchResultViewModel);
        }

        public PartialViewResult GetMoreCompanyProductsAsPartialView(string id)
        {
            var productArray = ProductHelper.GetAllProductsFromCache().Where(p => p.CompanyID == id.Trim()).ToArray();
            if (productArray.Count() <= 9)
            {
                return PartialView(productArray);
            }
            else
            {
                return PartialView(productArray.Take(10).ToArray());
            }
        }

        public PartialViewResult GetProductsAsPartialView()
        {
            HttpContext.Response.Expires = -1;
            HttpContext.Response.Cache.SetNoServerCaching();
            Response.Cache.SetAllowResponseInBrowserHistory(false);
            Response.CacheControl = "no-cache";
            Response.Cache.SetNoStore();

            var userRowKey = (Membership.GetUser().ProviderUserKey as string);
            var products = new ProductRepository().GetByCompanyRowKey(userRowKey).OrderBy(f => f.ProductName).ToArray();

            return PartialView(products);
        }

        #endregion

        #region JsonResult Actions
        [HttpPost]
        public JsonResult Edit(ProductViewModel product)
        {
            try
            {
                var userRowKey = (Membership.GetUser().ProviderUserKey as string);

                //ensure that user is only able to save products under there own company if not a webmaster 
                if (!User.IsInRole("Webmaster"))
                {
                    if (product.ProductData.CompanyID != userRowKey)//CompanyRowKey <==> UserRowkey
                    {
                        return Json(new { status = "error", message = "Product Save Failed - Security Violation Occurred. Your IP Address has been logged" });
                    }
                }

                if ((product.photo != null) || (product.ProductData.PhotoUrl != null))//save only if photo allready exists or has been supplied
                {
                    if (product.photo != null)
                    {
                        BinaryReader b = new BinaryReader(product.photo.InputStream);
                        byte[] binData_IN = b.ReadBytes(product.photo.ContentLength);

                        MemoryStream mstream_input = new MemoryStream(binData_IN);

                        MemoryStream mstream_output_120 = new MemoryStream();
                        MemoryStream mstream_output_160 = new MemoryStream();
                        MemoryStream mstream_output_240 = new MemoryStream();

                        var settings120 = new ResizeSettings("maxwidth=120&maxheight=120&format=png");
                        ImageBuilder.Current.Build(mstream_input, mstream_output_120, settings120,false);
                        byte[] binData_OUT_120 = mstream_output_120.ToArray();

                        var settings160 = new ResizeSettings("maxwidth=160&maxheight=160&format=png");
                        mstream_input.Seek(0, SeekOrigin.Begin);
                        ImageBuilder.Current.Build(mstream_input, mstream_output_160, settings160,false);
                        byte[] binData_OUT_160 = mstream_output_160.ToArray();

                        var settings240 = new ResizeSettings("maxwidth=240&maxheight=240&format=png");
                        mstream_input.Seek(0, SeekOrigin.Begin);
                        ImageBuilder.Current.Build(mstream_input, mstream_output_240, settings240);
                        byte[] binData_OUT_240 = mstream_output_240.ToArray();

                        var fileName = Guid.NewGuid().ToString();
                        PhotoRepository photoRepo = new PhotoRepository();

                        product.ProductData.PhotoUrl = photoRepo.SavePhoto(binData_OUT_120, fileName + "120.png");
                        photoRepo.SavePhoto(binData_OUT_160, fileName + "160.png");
                        photoRepo.SavePhoto(binData_OUT_240, fileName + "240.png");
                    }

                    if (product.ProductData.CategoryID == null)
                    {
                        product.ProductData.CategoryID = Settings.Default.DefaultCategoryRowkey;
                    }

                    string featuredProductMessage = string.Empty;
                    if (product.IsFeaturedProduct)
                    {
                        var companySubscription = new CompanySubscriptionRepository().GetAll().Where(c => c.CompanyRowKey == userRowKey).SingleOrDefault();

                        if (companySubscription != null)
                        {
                            var subscription = new SubscriptionProductRepository().GetAll().Where(s => s.ID == companySubscription.ProductID).SingleOrDefault();

                            if (subscription != null)
                            {   
                                //get featured product count for this company
                                var count = new ProductRepository().GetByCompanyRowKey(userRowKey).Where(p => p.FeaturedProductWeight >= 1).Count();

                                if (count >= subscription.MaxFeatured)
                                {
                                    product.ProductData.FeaturedProductWeight = 0;//max featured products limit exceeded
                                    featuredProductMessage = "Featured Products Limit Has Been Reached - Please Upgrade To Add More. Maximum allowed for your subscription is " + subscription.MaxFeatured;
                                }
                                else
                                {
                                    product.ProductData.FeaturedProductWeight = subscription.Level;
                                }
                            }
                        }
                    }
                    else
                    {
                        product.ProductData.FeaturedProductWeight = 0;
                    }

                    //swap out correct placeholder image
                    product.ProductData.PhotoUrl.ToLower().Trim().Replace(Settings.Default.NoImage240Url.ToLower().Trim(), Settings.Default.NoImage120Url.ToLower().Trim());


                    new ProductRepository().Save(product.ProductData);
                    //TODO: Delete old photo on save

                    return Json(new { status = "success", message = "Product Saved Successfully", photourl = product.ProductData.PhotoUrl }, "text/html");
                }
                else
                {
                    return Json(new { status = "error", message = "Product Save Failed, Please Include A Photo." }, "text/html");
                }
            }
            catch (ImageCorruptedException)
            {
                return Json(new { status = "error", message = "Product Save Failed, error reading image file. Please try another image." }, "text/html");
            }
            catch
            {
                return Json(new { status = "error", message = "Product Save Failed, Please try again later." }, "text/html");
            }
        }
        #endregion
    }
}
