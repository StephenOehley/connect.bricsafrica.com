using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BricsWeb.Controllers
{
    public class RssController : Controller
    {
        //
        // GET: /Rss/
        // Changes MIME type of google feed so that IE treats it as RSS and displays it in rss feed preview
         [OutputCache(Duration = 10800)]
        public MvcHtmlString Index()
        {
            WebClient client = new WebClient();
            var result = client.DownloadString("https://news.google.com/news/feeds?um=1&ned=us&hl=en&q=brics&output=rss");

            HttpContext.Response.ContentType = "text/html";

            return new MvcHtmlString(result); ;
        }

    }
}
