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

namespace BricsWeb.Controllers
{
    public class BuyerRequestController : CrystalController
    {
        #region ViewResult Actions
        public ActionResult Search()
        {
            return MenuView("Find A Product", "SubMenuFindAProduct", "Buyer Requirements Search");
        }

        //
        // GET: /BuyerRequest/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /BuyerRequest/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /BuyerRequest/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BuyerRequest/Create

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
        // GET: /BuyerRequest/Edit/5

        public ActionResult Edit()
        {
            var userRowKey = (Membership.GetUser().ProviderUserKey as string);
            var buyerRequestModel = new BuyerRequestRepository().GetByCompanyID(userRowKey).FirstOrDefault();//CompanyRowKey <==> UserRowkey

            if (buyerRequestModel != null)
            {
                var buyerRequestViewModel = new BuyerRequestViewModel { RequestData = buyerRequestModel };
                return MenuView(buyerRequestViewModel, "MY PROFILE", "SubMenuMyProfile", "Buyer Profile");//pass in the first request found to the edit view 
            }
            else
            {
                var buyerRequestViewModel = new BuyerRequestViewModel { RequestData = new BuyerRequestModel() };
                return MenuView(buyerRequestViewModel, "MY PROFILE", "SubMenuMyProfile", "Buyer Profile");//use default blank edit form as no requests exist for current company
            }
        }

        //
        // POST: /BuyerRequest/Delete/5

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

        #region PartialViewResult Actions
        public PartialViewResult GetEditFormAsPartialView(string id)
        {
            HttpContext.Response.Expires = -1;
            HttpContext.Response.Cache.SetNoServerCaching();
            Response.Cache.SetAllowResponseInBrowserHistory(false);
            Response.CacheControl = "no-cache";
            Response.Cache.SetNoStore();

            var userRowKey = (Membership.GetUser().ProviderUserKey as string);

            if (id != null)
            {
                var cleanid = id.Replace("#", string.Empty);//cleanup id string

                var request = new BuyerRequestRepository().GetByRowKey(cleanid);

                if (request != null)
                {
                    return PartialView(new BuyerRequestViewModel { RequestData = request });
                }
                else
                {
                    var blankRequest = new BuyerRequestViewModel { RequestData = new BuyerRequestModel() };
                    return PartialView(blankRequest);
                }
            }
            else//return default blank request form
            {
                var request = new BuyerRequestModel(userRowKey, "My New Request", "None", "Please enter a description...", 0, DateTime.UtcNow.AddDays(30), string.Empty);//CompanyRowKey <==> UserRowkey
                return PartialView(new BuyerRequestViewModel { RequestData = request });
            }
        }

        public PartialViewResult GetRequestsAsPartialView()
        {
            HttpContext.Response.Expires = -1;
            HttpContext.Response.Cache.SetNoServerCaching();
            Response.Cache.SetAllowResponseInBrowserHistory(false);
            Response.CacheControl = "no-cache";
            Response.Cache.SetNoStore();

            var userRowKey = (Membership.GetUser().ProviderUserKey as string);
            var requests = new BuyerRequestRepository().GetByCompanyID(userRowKey);

            return PartialView(requests);
        }

        public PartialViewResult GetSearchResultsAsPartialView(string categoryID,string searchText,string country)
        {
            CategoryModel parentCategory = null;

            string cleanCategoryID = null;
            if (categoryID != null)
            {
                cleanCategoryID = categoryID.Replace("#", string.Empty);
            }

            //get array of sub categories
            var categoryArray = new CategoryRepository().GetAll().ToArray();

            if ((categoryID != null))
            {
                parentCategory = (from category in categoryArray
                                  where category.RowKey.Equals(cleanCategoryID)
                                  select category).SingleOrDefault();//will return null for all cateogries
            }

            if (parentCategory == null)
            {
                parentCategory = CategoryHelper.AllCategories;
            }

            var childCategoryArray = (from child in categoryArray
                                      where child.ParentID == cleanCategoryID
                                      select child).ToArray();

            var allCategories = new CategoryRepository().GetAll().ToArray();
            var categoryAndDescendantsList = CategoryHelper.GetAllDescendantsFlattened(cleanCategoryID, allCategories, childCategoryArray).ToList();
            categoryAndDescendantsList.Add(parentCategory);
            var categoryAndDescendantsArray = categoryAndDescendantsList.ToArray();

            List<BuyerRequestModel> categoryMatchedList = new List<BuyerRequestModel>();
            var requestArray = new BuyerRequestRepository().GetAll();

            categoryAndDescendantsArray.ToList().ForEach(category => {

                var categoryMatched = (from request in requestArray
                                           where request.Category == category.Name//TODO: compare on ID not name
                                           select request).SingleOrDefault();

                if (categoryMatched != null)
                {
                    categoryMatchedList.Add(categoryMatched);
                }
                
            });

            List<BuyerRequestModel> results = new List<BuyerRequestModel>();

            if ((searchText != null))
            {
                foreach (BuyerRequestModel request in categoryMatchedList)
                {
                    string searchCloud = string.Empty;

                    if (request.RequestDescription != null)
                    {
                        searchCloud = searchCloud + " " + request.RequestDescription;
                    }

                    if (request.RequestTitle != null)
                    {
                        searchCloud = searchCloud + " " + request.RequestTitle;
                    }

                    if (searchCloud.ToUpper().Contains(searchText.ToUpper()))
                    {
                        results.Add(request);
                    }
                }
            }
            else
            {
                var allRequests = (from request in new BuyerRequestRepository().GetAll()
                                           select request).ToList();

                results.AddRange(allRequests);
            }

            return PartialView(results.ToArray());
        }
        #endregion

        #region JsonResult Actions
        public JsonResult Delete(string id)
        {
            var userRowKey = (Membership.GetUser().ProviderUserKey as string);
            var request = new BuyerRequestRepository().GetByRowKey(id);

            if (userRowKey == request.CompanyID)//CompanyRowKey <==> UserRowkey
            {
                new BuyerRequestRepository().Delete(request);
                return Json(new { status = "success", message = "Request deleted successfully" },JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = "error", message = "Request Delete Failed - Security Violation Occurred. Your IP Address has been logged" },JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Edit(BuyerRequestViewModel buyerRequest)
        {
            try
            {
                var userRowKey = (Membership.GetUser().ProviderUserKey as string);

                //ensure that user is only able to save products under there own company if not a webmaster 
                if (!User.IsInRole("Webmaster"))
                {
                    if (buyerRequest.RequestData.CompanyID != userRowKey)//CompanyRowKey <==> UserRowkey
                    {
                        return Json(new { status = "error", message = "Buyer Request Save Failed - Security Violation Occurred. Your IP Address has been logged" });
                    }
                }

                if ((buyerRequest.photo != null) || (buyerRequest.RequestData.PhotoUrl != null))//save only if photo allready exists or has been supplied
                {
                    if (buyerRequest.photo != null)
                    {
                        BinaryReader b = new BinaryReader(buyerRequest.photo.InputStream);
                        byte[] binData_IN = b.ReadBytes(buyerRequest.photo.ContentLength);

                        MemoryStream mstream_input = new MemoryStream(binData_IN);
                        MemoryStream mstream_output = new MemoryStream();
                        var settings = new ResizeSettings("maxwidth=120&maxheight=120&format=png");
                        ImageBuilder.Current.Build(mstream_input, mstream_output, settings);
                        byte[] binData_OUT = mstream_output.ToArray();

                        PhotoRepository photoRepo = new PhotoRepository();
                        buyerRequest.RequestData.PhotoUrl = photoRepo.SavePhoto(binData_OUT, Guid.NewGuid().ToString() + ".png");
                    }

                    buyerRequest.RequestData.DatePosted = DateTime.UtcNow;
                    new BuyerRequestRepository().Save(buyerRequest.RequestData);
                    return Json(new { status = "success", message = "Buyer Request Saved Successfully", photourl = buyerRequest.RequestData.PhotoUrl }, "text/html");
                }
                else
                {
                    return Json(new { status = "error", message = "Request Save Failed, Please Include A Photo." },"text/html");
                }

            }
            catch
            {
                return Json(new { status = "error", message = "Buyer Request Save Failed, Please try again later." }, "text/html");
            }
        }
        #endregion

    }
}
