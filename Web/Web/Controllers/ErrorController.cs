using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace BricsWeb.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            Trace.TraceError("An error occurred. Custom Error Page Displayed");
            return View();
        }

        public ActionResult KnownError(string errortext)
        {
            ViewBag.ErrorText = errortext;
            return View();
        }

    }
}
