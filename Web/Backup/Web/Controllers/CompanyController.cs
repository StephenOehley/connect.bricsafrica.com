using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcHelper.Mvc;
using BricsWeb.Models;
using BricsWeb.Repository;
using System.Web.Security;
using BricsWeb.RepositoryHelper;
using BricsWeb.LocalModels;
using System.IO;
using ImageResizer;
using Elmah;
using AzureHelper.Authentication;

namespace BricsWeb.Controllers
{
    public class CompanyController : CrystalController
    {
        const string chamberHash = "38684EF0F222";
        const string greenHash = "B8A77A42ECA9";
    
        //
        // GET: /Company/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search()
        {
            return MenuView("Find A Product", "SubMenuFindAProduct", "Supplier Search");
        }

        public PartialViewResult GetSearchResultsAsPartialView(string country = "", int page = 1, int productsPerPage = 25, string categoryID = "#00000000-0000-0000-0000-000000000000", string searchText = "")
        {
            var companySubscriptionArray = CompanySubscriptionHelper.GetAllCompanySubscriptionsFromCache().ToArray();
            var countryArray  = new CountryRepository().GetAll().ToArray();
            var subscriptionProductArray = new SubscriptionProductRepository().GetAll().ToArray();
            var categoryArray = CategoryHelper.GetAllCategoriesFromCache().ToArray();
            var productArray = ProductHelper.GetAllProductsFromCache().ToArray();
            var companyArray = CompanyHelper.GetAllCompaniesFromCache().Where(c => c.Country != null).ToArray();
            
            CategoryModel parentCategory = null;
            var cleanCategoryID = categoryID.Replace("#", string.Empty);
           
            //get parent category of sub categories
            if ((categoryID != null))
            {
                parentCategory = (from category in categoryArray
                                  where category.RowKey.Equals(cleanCategoryID)
                                  select category).SingleOrDefault();//will return null for all cateogries
            }

            //check if root category was selected
            if (parentCategory == null)
            {
                parentCategory = CategoryHelper.AllCategories;
            }
            var childCategoryArray = new List<CategoryModel>().ToArray();
            if (parentCategory != CategoryHelper.AllCategories)
            {
                childCategoryArray = (from child in categoryArray
                                      where child.ParentID == cleanCategoryID
                                      select child).ToArray();
            }
            else
            {
                childCategoryArray = (from child in categoryArray
                                      where child.ParentID == "0"
                                      select child).ToArray();
            }
            var categoryAndDescendantsList = CategoryHelper.GetAllDescendantsFlattened(cleanCategoryID, categoryArray, childCategoryArray).ToList();
            
            var results = new List<CompanyModel>();

            //search by searchtext
            if (searchText != string.Empty)
            {
                var companyFiltered_SearchText =
                        companyArray.Where(c => (
                        c.Name.ToLower().Contains(searchText) || 
                        c.ProductsOrServices.ToLower().Contains(searchText))
                        ).Distinct().ToArray();

                if (country == string.Empty)
                {
                    results.AddRange(companyFiltered_SearchText.Distinct());
                }
                else
                {
                    results.AddRange(companyFiltered_SearchText.Where(c => c.Country.ToLower() == country.ToLower()).Distinct());
                }                
            }
            else
            {//ignore searchtext
                if (country == string.Empty || country.ToLower() == "all countries")
                {
                    results.AddRange(companyArray);
                }
                else
                {
                    results.AddRange(companyArray.Where(c => c.Country.ToLower() == country.ToLower()));
                }                
            }           

            var resultsFiltered = new List<CompanyModel>();
            //filter results by category if specified
            if (parentCategory != CategoryHelper.AllCategories)
            {
                categoryAndDescendantsList.Add(parentCategory);
                resultsFiltered = results.Where(r => 
                    ProductHelper.DoesHaveProductsInCategory(
                    productArray.Where(p => p.CompanyID == r.RowKey).ToArray(),
                    categoryAndDescendantsList.ToArray(),
                    companyArray,
                    r.RowKey
                    ))                    
                    .Distinct().ToList();                 
            }
            else
            {
                resultsFiltered = results;
            }

            var companyresultArray = resultsFiltered.Select(c => new CompanyViewModel(c, companySubscriptionArray, countryArray, subscriptionProductArray)).ToArray();
                                  

            int totalPages = 1;
            if (companyArray.Count() != 0)
            {
                totalPages = companyresultArray.Count() / productsPerPage;
            }

            var pagedResultArray = companyresultArray.OrderBy(c => c.CompanyData.Name).Skip(productsPerPage * (page - 1)).Take(productsPerPage).ToArray();
            return PartialView(new CompanySearchResultViewModel(pagedResultArray, productsPerPage, page, parentCategory, totalPages,companyresultArray.Count()));
        }

        //
        // GET: /Company/Details/5

        public ActionResult Details(string id)
        {
            var companyModel = new CompanyRepository().GetByRowKey(id);
            var companyViewModel = new CompanyViewModel(
                companyModel,
                CompanySubscriptionHelper.GetAllCompanySubscriptionsFromCache().ToArray(),
                new CountryRepository().GetAll().ToArray(),
                new SubscriptionProductRepository().GetAll().ToArray());

            return MenuView(companyViewModel, "Find A Product", "SubMenuFindAProduct", string.Empty);
        }

        //
        // GET: /Company/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Company/Create

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

        [Authorize(Roles = "Webmaster")]
        public ActionResult MasterEdit()
        {
            var companyArray = new CompanyRepository().GetAll();
            List<CompanyViewModel> companyViewModelList = new List<CompanyViewModel>();

            companyArray.ToList().ForEach(company =>
            {
                 var subscribeUrl = @"http://matchmakerapplication.cloudapp.net/Company/RegisterAsChamberCertified?key=" + chamberHash + "&id=" + Url.Encode(company.RowKey);
                 var greenSubscribeUrl = @"http://matchmakerapplication.cloudapp.net/Company/RegisterAsGreenCertified?key=" + greenHash;
                 
                 companyViewModelList.Add(new CompanyViewModel { CompanyData = company, IsChamber = true, ChamberSubscribeUrl = subscribeUrl, GreenSubscribeUrl = greenSubscribeUrl });        
            });
                
            var companyViewModelListOrdered = companyViewModelList.OrderBy(cvm => cvm.CompanyData.Name).ToArray();
            return MenuView(companyViewModelListOrdered, "WEBMASTER", "SubMenuWebmaster", "Company Master Edit"); 
        }
        
        //
        // GET: /Company/Edit/5
        [Authorize]
        public ActionResult Edit()
        {
            var user = Membership.GetUser();
            string userRowKey = (user.ProviderUserKey as string);
            var company = new CompanyRepository().GetByRowKey(userRowKey);

            var chamberArray = CompanyHelper.GetAllCompaniesFromCache().Where(c => c.BusinessType != null).Where(c => c.BusinessType.ToLower() == "chamber").ToArray();
            ViewBag.ChamberArray = chamberArray;

            if (company == null)
            {
                company = new CompanyModel(userRowKey, "New Company Name", user.Email, user.Email, string.Empty);
            }

            if (company.LogoUrl == null)
            {
                company.LogoUrl = "_blank";
            }

            if (company.BusinessType == null)
            {
                company.BusinessType = "Association";
            }

            if (company.BusinessType.ToLower() == "chamber")
            {
                var subscribeUrl = @"http://matchmakerapplication.cloudapp.net/Company/RegisterAsChamberCertified?key=" + chamberHash + "&id=" + Url.Encode(company.RowKey);
                var greenSubscribeUrl = @"http://matchmakerapplication.cloudapp.net/Company/RegisterAsGreenCertified?key=" + greenHash;
                
                ViewBag.ShowUpgradeLink = false;
                return MenuView(new CompanyViewModel { CompanyData = company, IsChamber = true, ChamberSubscribeUrl = subscribeUrl, GreenSubscribeUrl = greenSubscribeUrl }, "MY PROFILE", "SubMenuMyProfile", "Company Profile");
            }
            else
            {
                var subscription = new CompanySubscriptionRepository().GetByRowKey(company.RowKey);

                if (subscription == null)
                {
                    ViewBag.ShowUpgradeLink = true;
                }
                else
                {
                    ViewBag.ShowUpgradeLink = false;
                }

                return MenuView(new CompanyViewModel { CompanyData = company, IsChamber = false, ChamberSubscribeUrl = string.Empty }, "MY PROFILE", "SubMenuMyProfile", "Company Profile");
            }
        }
        
        //
        // POST: /Company/Edit/5

        [HttpPost]
        public JsonResult Edit(CompanyViewModel companyViewModel)
        {
            try
            {
                var user = Membership.GetUser();

                if (companyViewModel.logo != null)
                {
                    BinaryReader b = new BinaryReader(companyViewModel.logo.InputStream);
                    byte[] binData_IN = b.ReadBytes(companyViewModel.logo.ContentLength);

                    MemoryStream mstream_input = new MemoryStream(binData_IN);
                    MemoryStream mstream_output = new MemoryStream();
                    var settings = new ResizeSettings("maxwidth=240&maxheight=240&format=png");
                    ImageBuilder.Current.Build(mstream_input, mstream_output, settings);
                    byte[] binData_OUT = mstream_output.ToArray();

                    PhotoRepository photoRepo = new PhotoRepository();
                    companyViewModel.CompanyData.LogoUrl = photoRepo.SavePhoto(binData_OUT, Guid.NewGuid().ToString() + ".png");//TODO: save with correct MIME type
                }

                if (companyViewModel.CompanyData.ChamberID.ToLower() == "none")
                {
                    companyViewModel.CompanyData.ChamberID = null;
                    companyViewModel.CompanyData.IsChamberCertified = false;
                }
                else
                {
                    companyViewModel.CompanyData.IsChamberCertified = true;
                }

                companyViewModel.CompanyData.PartitionKey = companyViewModel.CompanyData.RowKey.Substring(0, 1);
                new CompanyRepository().Save(companyViewModel.CompanyData);

                //check reseller role
                if (companyViewModel.CompanyData.BusinessType.ToLower() == "chamber")
                {
                    if (!User.IsInRole("RegisteredReseller"))
                    {
                        Roles.AddUserToRole(user.UserName, "RegisteredReseller");
                    }
                }
                else
                {
                    if (User.IsInRole("RegisteredReseller"))
                    {
                        Roles.RemoveUserFromRole(user.UserName, "RegisteredReseller");
                    }
                }

                return Json(new { status = "success", message = "Company Saved Successfully", photourl = companyViewModel.CompanyData.LogoUrl },"text/html");
            }
            catch(Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
                return Json(new { status = "error", message = "Company Save Failed, Please try again later." },"text/html");
            }
        }

        //
        // GET: /Company/Delete/5
         [Authorize(Roles = "Webmaster")]
        public ActionResult MasterDelete(string id)
        {
            //delete company
            var company = new CompanyRepository().GetByRowKey(id);
            new CompanyRepository().Delete(company);

            //delete user
            var user = new AzureUserRepository().GetByRowKey(id);
            new AzureUserRepository().Delete(user);

            //delete roles
            var roleArray = new AzureRoleRepository().GetAll().Where(r => r.UserRowKey == id);
            roleArray.ToList().ForEach(r =>
            {
                new AzureRoleRepository().Delete(r);
            });
            
            //delete subscription
            var subscriptionArray = new CompanySubscriptionRepository().GetAll().Where(s => s.RowKey == id).ToArray();

            subscriptionArray.ToList().ForEach(s =>
            {
                new CompanySubscriptionRepository().Delete(s);
            });

            //delete transactions
            var transactionArray = new TransactionRepository().GetTransactionsByCompanyID(id).ToArray();

            transactionArray.ToList().ForEach(t =>
            {
                new TransactionRepository().Delete(t);
            });

            //delete products
            var productArray = new ProductRepository().GetAll().Where(p => p.CompanyID == id).ToArray();

            productArray.ToList().ForEach(p =>
            {
                new ProductRepository().Delete(p);
            });
            
            return RedirectToAction("MasterEdit");
        }

        //
        // POST: /Company/Delete/5

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

        [Authorize]
        public ActionResult RegisterAsGreenCertified(string key)
        {
            if (key == greenHash)
            {
                var user = Membership.GetUser();
                string userRowKey = (user.ProviderUserKey as string);
                var company = new CompanyRepository().GetByRowKey(userRowKey);

                company.IsGreenCertified = true;

                new CompanyRepository().Save(company);

                return MenuView("MY PROFILE", "SubMenuMyProfile", string.Empty);
            }
            else
            {
                throw new Exception("Invalid Key Specified");
            }
        }

        [Authorize]
        public ActionResult VerifyOn(string id, string returnUrl = "/company/masteredit")
        {
            var company = new CompanyRepository().GetByRowKey(id);
            company.IsVerified = true;
            new CompanyRepository().Save(company);

            return Redirect(returnUrl);
        }

        [Authorize]
        public ActionResult VerifyOff(string id, string returnUrl = "/company/masteredit")
        {
            var company = new CompanyRepository().GetByRowKey(id);
            company.IsVerified = false;
            new CompanyRepository().Save(company);

            return Redirect(returnUrl);
        }

        [Authorize]
        public ActionResult RegisterAsChamberCertified(string key,string id)
        {
            if (key == chamberHash)
            {
                var user = Membership.GetUser();
                string userRowKey = (user.ProviderUserKey as string);
                var company = new CompanyRepository().GetByRowKey(userRowKey);
                var chamber = new CompanyRepository().GetByRowKey(id);

                if (company == null)
                {
                    company = new CompanyModel(userRowKey, "Name Not Specified", string.Empty, string.Empty, string.Empty);
                }

                if (chamber != null)
                {
                    if (chamber.Tier != null)
                    {
                        company.Tier = (Convert.ToInt32(chamber.Tier) + 1).ToString();
                    }
                    else
                    {
                        company.Tier = "4";
                    }

                    company.IsChamberCertified = true;
                    company.ChamberID = id;
                }
                else
                {
                    throw new Exception("Invalid Chamber Specified");
                }

                new CompanyRepository().Save(company);

                return MenuView("MY PROFILE", "SubMenuMyProfile", string.Empty);
            }
            else
            {
                throw new Exception("Invalid Key Specified");
            }
        }
    }
}
