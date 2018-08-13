using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcHelper.Mvc;

namespace BricsWeb.Controllers
{
    public class HomeController : CrystalController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Search", "Product");//show find-a-product
            }
            else//this may be a new user
            {
                return RedirectToAction("GettingStarted");
            }
        }

        public ActionResult GettingStarted()
        {
            return MenuView("None", "SubMenuFindAProduct", "None");//show getting started
        }

        public JsonResult ThrowException()
        {
            throw new Exception("test exception");
        }
    }
}
