using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BricsWeb.RepositoryHelper;

namespace BricsWeb.Controllers
{
    public class ConfigurationController : Controller
    {
        //
        // GET: /Configuration/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadSampleData()
        {
            new TestDataHelper().UploadTestData();
            return Json(true,JsonRequestBehavior.AllowGet);
        }

    }
}
