using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Hosting;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using BricsWeb.Models;

namespace BricsWeb.Controllers
{
    public class DynamicMenuController : Controller
    {
        //[OutputCache(Duration = 3600)]
        [ChildActionOnly]
        public PartialViewResult RenderMenu(string menuName, string activeItemDisplayName, string inActiveItemClass, string activeItemClass)
        {
            ViewBag.InActiveItemClass = inActiveItemClass;
            ViewBag.ActiveItemClass = activeItemClass;

            string filePath = HostingEnvironment.ApplicationPhysicalPath + menuName + ".json";

            if (global:: System.IO.File.Exists(filePath))
            {
                DataContractJsonSerializer jsonSerialize = new DataContractJsonSerializer(typeof(MenuItemCollectionModel));

                string menuJsonString = global:: System.IO.File.ReadAllText(filePath);
                Stream menuJsonStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(menuJsonString));

                MenuItemCollectionModel menuCollection = (jsonSerialize.ReadObject(menuJsonStream) as MenuItemCollectionModel);

                //Filter collection based on  user role
                List<MenuItemModel> allowedItems = new List<MenuItemModel>();
                menuCollection.Items.ToList().ForEach(item =>
                {
                    var allowedRoleList = item.AllowedRoles.ToList();

                    if (item.AllowedRoles.Count() == 0)
                    {
                        if (item.DisplayName.Equals(activeItemDisplayName, StringComparison.CurrentCultureIgnoreCase))
                        { item.IsActive = true; };//if this is the selected menu item then set as active

                        allowedItems.Add(item);
                    }
                    else
                    {
                        if (User.Identity.IsAuthenticated)
                        {
                            string resultRole = allowedRoleList.FirstOrDefault(role => User.IsInRole(role));//null if user is not in an allowed role
                            if (resultRole != null)
                            {
                                if (item.DisplayName.Equals(activeItemDisplayName, StringComparison.CurrentCultureIgnoreCase))
                                { item.IsActive = true; };//if this is the selected menu item then set as active

                                allowedItems.Add(item);
                            }
                        }
                    }
                });

                return PartialView(allowedItems.ToArray());
            }
            else
            {
                throw new FileNotFoundException("Menu definition file not found: " + filePath);
            }
        }
    }
}
