using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BricsWeb.RepositoryHelper;
using MvcHelper.Mvc;
using MvcHelper.Mail;
using BricsWeb.LocalModels;
using System.Web.Security;
using BricsWeb.Repository;

namespace BricsWeb.Controllers
{
    public class ContactController : CrystalController
    {
        public ActionResult SendMessage(string id,string formTitle,string subject="")
        {
            var allCompanies = new CompanyRepository().GetAll();

            ViewBag.FormTitle = "Contact " + formTitle;
            ViewBag.Subject = subject;

            string fromEmail = string.Empty;

            try
            {
                var userRowKey = (Membership.GetUser().ProviderUserKey as string);

                var fromCompany = (from c in allCompanies
                                   where c.RowKey == userRowKey
                                   select c).Single();

                fromEmail = fromCompany.SalesEmail;
            }
            catch(Exception)
            {}

            var company = (from c in allCompanies
                           where c.RowKey == id
                           select c).SingleOrDefault();

            var contactViewModel = new ContactModel { ToCompanyAndContactName = company.ContactName + "(" + company.Name + ")", FromEmail = fromEmail,ToCompanyID = id };

            return MenuView(contactViewModel, "Find A Product", "SubMenuFindAProduct", "Product Search");
        }

        public ActionResult SubmitContactRequestBusinessGenerator(ContactModel info)
        {
            var allCompanies = new CompanyRepository().GetAll();

            var company = (from c in allCompanies
                          where c.RowKey == info.ToCompanyID
                          select c).Single();

            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                    return Json(new { status = "error", message = "All fields are required." });

                return View(info);
            }

            try
            {
                //Send email
                List<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>();
                fields.Add(new KeyValuePair<string, string>("From Email", info.FromEmail));
                fields.Add(new KeyValuePair<string, string>("Subject", info.Subject));
                fields.Add(new KeyValuePair<string, string>("Quantity", info.Quantity));
                fields.Add(new KeyValuePair<string, string>("Message", info.Message));

                INotificationService notificationService = new EmailNotificationService(
                    new EmailNotification(fields,company.SalesEmail, "Business Generator - Contact Request ", "webmaster@bricsafricab2b.com"),
                    Properties.Settings.Default.SmtpHostName,
                    Properties.Settings.Default.SmtpUserName,
                    Properties.Settings.Default.SmtpPassword,
                    true,
                    Properties.Settings.Default.SmtpFromAddress);

                notificationService.Notify();
            }
            catch (NotificationException)
            {
                ModelState.AddModelError("notifyerror", "Could not connect to mail server.");
            }

            if (Request.IsAjaxRequest())
                return ModelState.IsValid ? Json(new { status = "Success", message = "Message sent successfully, we will contact you shortly." }) : Json(new { status = "error", message = "Could not connect to mail server." });

            return ModelState.IsValid ? ContactRequestSuccess(info) : View(info);
        }

        protected ViewResult ContactRequestSuccess<TModel>(TModel viewModel)
        {
            ViewData["success"] = "";
            return View(viewModel);
        }


    }
}
