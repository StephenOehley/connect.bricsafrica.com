using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BricsWeb.LocalModels
{
    public class ContactModel
    {
        public string ToCompanyAndContactName { get; set; }
        public string ToCompanyID { get; set; }

        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string Quantity { get; set; }
        public string Message { get; set; }
    }
}