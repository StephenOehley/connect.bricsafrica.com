using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcHelper.Mvc;
using System.Web.Security;
using AzureHelper.Authentication;
using Microsoft.IdentityModel.Web;
using MvcHelper.Mail;
using System.Diagnostics;
using BricsWeb.Properties;
using BricsWeb.Repository;

namespace BricsWeb.Controllers
{
    public class AccountController : CrystalController
    {
        const string membershipProviderType = "[Federated]";

        public ActionResult SignUp()
        {
            if (User.Identity.IsAuthenticated)
            {
                var baseUrl = Settings.Default.BaseUrl;
                return Redirect(baseUrl);
            }

            return MenuView(string.Empty, "SubMenuFindAProduct", string.Empty);
        }

        public PartialViewResult GetSignupFormAsPartialView()
        {
            var baseUrl = Settings.Default.BaseUrl;
            var signupReturnUrl = baseUrl + "/account/addfederatedUser/emailplaceholder";


            ViewBag.HrdFeed = Settings.Default.HrdFeed.Replace("%returnurl%", signupReturnUrl).Replace("%context%", "returnurlplaceholder");
            ViewBag.BaseUrl = baseUrl;
            ViewBag.ValidateUrl = baseUrl + @"/Account/UserNameValid?callback=";

            return PartialView();
        }     

        /// <summary>
        /// Confirms that User.Identity is signed up and adds this user to the RegisteredUser Role
        /// Redirects to specified Url
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult AddFederatedUser(string returnUrl)//TODO: Add Captcha Support
        {
            //AzureClaimsAuthenticationManager intercepts this request and creates the user so all we need to do here is confirm this and add the roles
            AzureMembershipProvider membershipProvider = new AzureMembershipProvider();
            var user = membershipProvider.GetUser(User.Identity.Name, true);

            if (User != null)
            {
                Roles.AddUserToRole(user.UserName, "RegisteredUser");
                return Redirect(returnUrl);
            }
            else
            {
                throw new Exception("Failed: User does not exist");
            }
        }

        /// <summary>
        /// Creates a new Azure user 
        /// Redirects to specified Url
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult AddUser(string username, string email, string password, string returnUrl)//TODO: Add Captcha Support
        {//do not use username - replace username with email address to cater for dumb users that do not know what a username is
            if (!User.Identity.IsAuthenticated)
            {
                var userByEmail = Membership.GetUser(email, false);//confirm that email does not exist
                //var userByUsername = Membership.GetUser(username, false);//confirm that username does not exist

                if (userByEmail == null)
                {
                    AzureMembershipProvider membershipProvider = new AzureMembershipProvider();
                    MembershipCreateStatus createStatus;
                    membershipProvider.CreateUser(email.Trim(), password.Trim(), email.Trim(), string.Empty, string.Empty, true, null, out createStatus);

                    Roles.AddUserToRole(email, "RegisteredUser");

                    var createdUser = AzureMembershipProvider.GetAzureUser(email, true);
                    Request.RequestContext.HttpContext.CreateAzureAuthTicket(createdUser);//login user

                    return Redirect(returnUrl);
                }
                else
                {//user is allready signed up - redirect to login
                    return RedirectToAction("Login");
                }
            }
            else
            {//user is allready logged in - redirect to product search
                return RedirectToAction("Search","Product");
            }
        }

        public ActionResult UpdateUser()
        {
            return MenuView("MY PROFILE", "SubMenuMyProfile","Personal Profile");
        }

        public ActionResult ResetPassword(string email,string key=null)
        {
            if (key == null)
            {
                return MenuView("MY PROFILE", "SubMenuFindAProduct", "None");
            }
            else
            {
                if (email != null)
                {//TODO: Optimize query
                    var user = new AzureUserRepository().GetAll().Where(u => u.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();//confirm that username exists

                    if (user != null)
                    {
                        //Generate temporary password
                        string pwdnew = Guid.NewGuid().ToString().Substring(0, 8);
                        user.Password = pwdnew;//TODO: Hash password
                        new AzureUserRepository().Save(user);

                        //Send email
                        List<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>();
                        fields.Add(new KeyValuePair<string, string>("Your new BRICS Business Generator Password Is: ", pwdnew));//TODO: improve security - make key dynamic

                        INotificationService notificationService = new EmailNotificationService(
                            new EmailNotification(fields, email, "Password Reset Request ", "webmaster@bricsafricab2b.com"),//TODO: place webmaster email in config file
                            Properties.Settings.Default.SmtpHostName,
                            Properties.Settings.Default.SmtpUserName,
                            Properties.Settings.Default.SmtpPassword,
                            true,
                            Properties.Settings.Default.SmtpFromAddress);

                        notificationService.Notify();

                        return RedirectToAction("ResetPasswordComplete");
                    }
                    else
                    {
                        throw new Exception("Specified user does not exist");
                    }
                }
                else
                {
                    throw new Exception("Email address not specified");
                }
            }
        }

        [Authorize]
        public ActionResult ChangePassword(string passwordold,string passwordnew,string message)
        {
            if (message == null)
            {
                ViewBag.Message = string.Empty;
            }
            else
            {
                ViewBag.Message = message;
            }

            if ((passwordold != null) && (passwordnew != null))
            {
                var currentUser = AzureMembershipProvider.GetAzureUser(User.Identity.Name, true);

                if ((currentUser.Password == passwordold) && (passwordnew != null))
                {
                    currentUser.Password = passwordnew;//TODO: Validate new password
                    new AzureUserRepository().Save(currentUser);
                    return RedirectToAction("ChangePasswordComplete");
                }
                else
                {
                    return RedirectToAction("ChangePassword", new { message = "Invalid Password" });
                }
            }
            else
            {
                return MenuView("MY PROFILE", "SubMenuFindAProduct", "None");
            }
        }

        public ActionResult ChangePasswordComplete()
        {
            return MenuView("MY PROFILE", "SubMenuFindAProduct", "None"); 
        }
        
        /// <summary>
        /// Display pwd reset sucess page
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPasswordComplete()
        {
            return MenuView("MY PROFILE", "SubMenuFindAProduct", "None");
        }

        public ActionResult SendResetPasswordEmail(string email)
        {
            var baseUrl = Settings.Default.BaseUrl;

            if (email != null)
            {//TODO: Optimize query
                var user = new AzureUserRepository().GetAll().Where(u => u.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();//confirm that username exists

                if (user != null)
                {
                    //Send email
                    List<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>();
                    string resetUrl = baseUrl + "account/resetpassword?key=86BE6298F3D3&email=" + Url.Encode(email);

                    fields.Add(new KeyValuePair<string, string>("A Password Reset Request Was Received. If you requested this please click the following link: <a href=\"%url%\"/>".Replace("%url%", resetUrl), resetUrl));//TODO: improve security - make key dynamic

                    INotificationService notificationService = new EmailNotificationService(
                        new EmailNotification(fields, email, "Password Reset Request ", "webmaster@bricsafricab2b.com"),//TODO: place webmaster email in config file
                        Properties.Settings.Default.SmtpHostName,
                        Properties.Settings.Default.SmtpUserName,
                        Properties.Settings.Default.SmtpPassword,
                        true,
                        Properties.Settings.Default.SmtpFromAddress);

                    notificationService.Notify();
                }
                else
                {
                    throw new Exception("Specified user does not exist");
                }
            }
            else
            {
                throw new Exception("Email address not specified");
            }
            return MenuView("MY PROFILE", "SubMenuFindAProduct", "None");
        }

        /// <summary>
        /// Provides for updateing of basic information for the current user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateUser(string realname, string email, string returnurl)
        {
            var currentUser = AzureMembershipProvider.GetAzureUser(User.Identity.Name,true);
            AzureMembershipProvider.UpdateAzureUser(currentUser, realname, email);

            return Redirect(returnurl);
        }

        /// <summary>
        /// Check if username exists
        /// Returns Json (Result=boolean)
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public JsonResult UserNameValid(string username)
        {
            var user = Membership.GetUser(username);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json("Username Exists");
            }
        }

        public ActionResult Login(string username, string password, string message, string returnurl = "/")
        {
            var baseUrl = Settings.Default.BaseUrl;

            if (returnurl == "/")
            {
                ViewBag.ReturnUrl = Url.Encode(baseUrl);
                ViewBag.HrdFeed = Settings.Default.HrdFeed.Replace("%returnurl%", Url.Encode(baseUrl)).Replace("callback=?", "callback=ShowSigninPage").Replace("%context%", string.Empty); ;
            }
            else
            {
                ViewBag.ReturnUrl = Url.Encode(returnurl);
                ViewBag.HrdFeed = Settings.Default.HrdFeed.Replace("%returnurl%", Url.Encode(baseUrl + returnurl)).Replace("callback=?", "callback=ShowSigninPage").Replace("%context%", string.Empty); ; ;
            }

            ViewBag.ReturnUrl = (ViewBag.ReturnUrl as string).Replace(@"//", @"/");

            if (message == null)
            {
                ViewBag.Message = string.Empty;
            }
            else
            {
                ViewBag.Message = message;
            }

            if (username == null)
            {
                return MenuView(string.Empty, "SubMenuFindAProduct", string.Empty);
            }

            if (Membership.ValidateUser(username,password))
            {
                Trace.TraceInformation("Login Attempt [Success]: " + username);

                var authUser = AzureMembershipProvider.GetAzureUser(username, true);
                Request.RequestContext.HttpContext.CreateAzureAuthTicket(authUser);//login user                            
                               
                return Redirect(returnurl);
            }
            else
            {
                Trace.TraceInformation("Login Attempt [Fail]: " + username);

                return RedirectToAction("Login", new { message = "Login Failure" });
                //return MenuView(,string.Empty, "SubMenuFindAProduct", string.Empty);
            }
        }

        public ActionResult Logout(string returnUrl)
        {
            WSFederationAuthenticationModule fam = FederatedAuthentication.WSFederationAuthenticationModule;

            try
            {
                HttpContext.AzureLogout();
            }
            finally
            {
                fam.SignOut(true);
            }

            return Redirect(returnUrl);
        }

        public ActionResult SendConfirmationEmail()
        {
            return Json("SendConfirmationEmailOK", JsonRequestBehavior.AllowGet);
        }

    }
}
