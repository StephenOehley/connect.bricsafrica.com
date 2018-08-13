using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BricsWeb.Models;

namespace BricsWeb.LocalModels
{
    public class BuyerRequestViewModel
    {
        public BuyerRequestModel RequestData { get; set; }
        public HttpPostedFileWrapper photo { get; set; }
    }
}