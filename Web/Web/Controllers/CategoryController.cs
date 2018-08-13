using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcHelper.Mvc;
using BricsWeb.Repository;
using BricsWeb.LocalModels;
using BricsWeb.Models;
using System.Threading.Tasks;
using System.Runtime.Caching;
using BricsWeb.RepositoryHelper;

namespace BricsWeb.Controllers
{
    public class CategoryController : CrystalController
    {
        #region ViewResult Actions
        //
        // GET: /Category/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Category/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Category/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Category/Create

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
        // GET: /Category/Edit/5
        [HttpGet]
        public ActionResult Edit()
        {
            return MenuView("WEBMASTER", "SubMenuWebmaster", "Category Editor");
        }

        //
        // GET: /Category/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Category/Delete/5

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

        #region JsonResult Actions
        [HttpPost]
        [Authorize(Roles = "Webmaster")]
        public JsonResult Edit(string operation, string id, string position, string title, string type)
        {
            var newPosition = Convert.ToInt16(position) + 1;
            HttpResponse.RemoveOutputCacheItem(Url.Action("GetCategoryChildrenAsJson", "Category"));
            HttpResponse.RemoveOutputCacheItem(Url.Action("GetAllAsJson", "Category"));
            HttpResponse.RemoveOutputCacheItem(Url.Action("CheckForChildrenAsJson", "Category"));

            HttpResponse.RemoveOutputCacheItem(Url.Action("GetCategoryFilterAsPartialView", "Category"));
            HttpResponse.RemoveOutputCacheItem(Url.Action("GetCategoryDropdownAsPartialView", "Category"));
            HttpResponse.RemoveOutputCacheItem(Url.Action("GetNavigationBreadcrumbAsPartialView", "Category"));

            try
            {
                switch (operation)
                {
                    case "create_node":
                        new CategoryRepository().Save(new CategoryModel(Guid.NewGuid().ToString(),title,string.Empty,newPosition,id));
                        return Json(true);
                    case "remove_node":
                        var hasChildren = new CategoryRepository().CheckForChildren(id);

                        var productCount = new ProductRepository().GetByCategoryID(id).Count();

                        if (!hasChildren && productCount == 0)
                        {
                            var repoRemove = new CategoryRepository();
                            var categoryRemove = repoRemove.GetByRowKey(id);

                            if (categoryRemove != null)
                            {
                                repoRemove.Delete(categoryRemove);
                                return Json(true);
                            }
                            else
                            {
                                return Json(false);
                            }
                        }
                        else
                        {
                            return Json(false);
                        }
                    case "rename_node":
                        var repoRename = new CategoryRepository();
                        var categoryRename = repoRename.GetByRowKey(id);

                        if (categoryRename != null)
                        {
                            categoryRename.Name = title;
                            repoRename.Save(categoryRename);
                            return Json(true);
                        }
                        else
                        {
                           return Json(false);
                        }
                    default:
                        return Json(false);
                }

               // return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

        [OutputCache(Duration = 60000)]
        public JsonResult GetCategoryChildrenAsJson(string id)
        {
            var categoryRepo = new CategoryRepository();
            var treeItemArray = new List<TreeItemModel>();
            IEnumerable<CategoryModel> resultList;


            if (id == "1")//root node
            {
                resultList = categoryRepo.GetRootItems();
            }
            else
            {
                resultList = categoryRepo.GetChildItems(id);
            }

            foreach (CategoryModel cm in resultList)
            {
                var childExist = categoryRepo.CheckForChildren(cm.RowKey);
                string itemState = string.Empty;

                if (!childExist)
                {
                    itemState = "";
                }
                else
                {
                    itemState = "closed";
                }

                treeItemArray.Add(new TreeItemModel { attr = new BricsWeb.LocalModels.TreeItemModel.TreeItemAttribute { id = "node_" + cm.RowKey, rel = "folder", displaytext = cm.Name }, data = cm.Name, state = itemState });
            }

            var resultArrayDB = treeItemArray.ToArray();//if we omit this step .net will serialize the JSON object with our array under attribute 'Data'
            return Json(resultArrayDB);
        }

        [OutputCache(Duration = 60000)]
        public JsonResult GetAllAsJson()
        {
            var allCategories = new CategoryRepository().GetAll().ToArray();
            return Json(allCategories);
        }

        [OutputCache(Duration = 60000)]
        public JsonResult CheckForChildrenAsJson(string id)
        {
            var result = new CategoryRepository().CheckForChildren(id);

            if (result)
            {
                return Json(new { status = "true" });
            }
            else
            {
                return Json(new { status = "false" });
            }
        }

        #endregion

        #region PartialResult Actions
        [OutputCache(Duration = 60000)]
        public PartialViewResult GetCategoryFilterAsPartialView(string parentID, bool showProductCount = true)
        {
            string id = parentID;

            if (id == null)
            {
                id = "0";
            }

            var viewModel = new CategoryViewModel();
            var categoryArray = CategoryHelper.GetAllCategoriesFromCache().ToArray();
            
            var parentCategory = (from parent in categoryArray
                                  where parent.RowKey == id
                                  select parent).SingleOrDefault();//null if 'All Categories'

            if (parentID != null)
            {
                viewModel.Parent = parentCategory;
            }
            else
            {
                viewModel.Parent = CategoryHelper.AllCategories;
                parentCategory = viewModel.Parent;
            }

            var childCategoryArray = (from child in categoryArray
                                      where child.ParentID == id
                                      select child).ToArray();

            //populate viewmodel Category list with all decendants of parent
            viewModel.CategoryCollection = CategoryHelper.GetAllDescendants(parentCategory.RowKey, categoryArray, childCategoryArray).OrderBy(c => c.Parent.Name).ToList();

            //force viewmodel to populate product count (this action is recursive for all sub cateogries)
            viewModel.PopulateProductCount(
                ProductHelper.GetAllProductsFromCache().ToArray(),
                CategoryHelper.GetAllCategoriesFromCache().ToArray(),
                CompanyHelper.GetAllCompaniesFromCache().ToArray()
                );

            ViewBag.ShowproductCount = showProductCount;

            return PartialView(viewModel);
        }

        [OutputCache(Duration = 60000)]
        public PartialViewResult GetCategoryDropdownAsPartialView()
        {
            var rootNodes = new CategoryRepository().GetRootItems().OrderBy(c => c.Name).ToArray();
            return PartialView(rootNodes);
        }

        [OutputCache(Duration = 60000)]
        public PartialViewResult GetNavigationBreadcrumbAsPartialView(string categoryid,string leadstring)
        {
            var cleancategoryid = categoryid.Replace("#", string.Empty);
            var categoryArray = CategoryHelper.GetAllCategoriesFromCache().ToArray();

            var ancestors = CategoryHelper.GetAllAncestors(cleancategoryid, categoryArray, new List<CategoryModel>().ToArray());

            ViewBag.LeadString = leadstring;
            var ancestorsReversed = ancestors.Reverse().ToArray(); 
       
            return PartialView(ancestorsReversed);
        }
        #endregion

        #region Private Methods
       
        #endregion

    }
}
