using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.IdentityModel.Web.Configuration;
using Microsoft.IdentityModel.Web;
using Microsoft.IdentityModel.Tokens;
using MvcHelper.ElmahExtensions;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace BricsWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class Web : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ElmahHandledErrorLoggerFilter());
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home",action = "Index", id = UrlParameter.Optional } // Parameter defaults
                //new { controller = "Home", action = "Search", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            FederatedAuthentication.ServiceConfigurationCreated += OnServiceConfigurationCreated;

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //no idea why the hell doing the following prevents RoleEnviroment exceptions later on
            //var isAzure = RoleEnvironment.IsAvailable;
            //var isEmulated = RoleEnvironment.IsEmulated;

        }

        private void OnServiceConfigurationCreated(object sender, ServiceConfigurationCreatedEventArgs e)
        {

            List<CookieTransform> sessionTransforms =

                new List<CookieTransform>(

                    new CookieTransform[]

            {

                new DeflateCookieTransform()

            });

            SessionSecurityTokenHandler sessionHandler = new SessionSecurityTokenHandler(sessionTransforms.AsReadOnly());

            e.ServiceConfiguration.SecurityTokenHandlers.AddOrReplace(sessionHandler);

        }
    }
}