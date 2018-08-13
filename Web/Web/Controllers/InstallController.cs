using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureHelper.Authentication;

namespace BricsWeb.Controllers
{
    public class InstallController : Controller
    {
        public ActionResult Index()
        {
            new AzureHelper.Authentication.AzureRoleRepository().Initialise();
            new AzureHelper.Authentication.AzureUserRepository().Initialise();
            new BricsWeb.Repository.CompanyRepository().Initialise();
            new BricsWeb.Repository.ProductRepository().Initialise();
            new BricsWeb.Repository.BuyerRequestRepository().Initialise();
            new BricsWeb.Repository.CompanySubscriptionRepository().Initialise();
            new BricsWeb.Repository.TransactionRepository().Initialise();

            return View();
        }

        public ActionResult CreateDefaultRoles()
        {
            string rk = Guid.NewGuid().ToString();
            new AzureHelper.Authentication.AzureRoleRepository().Save(new AzureRole { RowKey = rk, UserRowKey = Guid.Empty.ToString(), PartitionKey = rk.Substring(0, 1), RoleName = "RegisteredUser" });

            rk = Guid.NewGuid().ToString();
            new AzureHelper.Authentication.AzureRoleRepository().Save(new AzureRole { RowKey = rk, UserRowKey = Guid.Empty.ToString(), PartitionKey = rk.Substring(0, 1), RoleName = "Webmaster" });

            rk = Guid.NewGuid().ToString();
            new AzureHelper.Authentication.AzureRoleRepository().Save(new AzureRole { RowKey = rk, UserRowKey = Guid.Empty.ToString(), PartitionKey = rk.Substring(0, 1), RoleName = "RegisteredReseller" });
            
            return View();
        }

    }
}
