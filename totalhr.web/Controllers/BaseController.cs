using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.Shared;
using System.Configuration;
using Authentication.Infrastructure;
using Authentication.Models;
using System.IO;

namespace totalhr.web.Controllers
{
    [CustomAuth(new[] { Variables.Roles.Employee, Variables.Roles.CompanyAdmin })]
    public class BaseController : Controller
    {
        public int ViewingLanguageId { get; set; }
        public AdminStruct WebsiteKernel {get; set;}

        public bool UserIsAdmin { get { return CurrentUser.HasProfile((int) Variables.Roles.CompanyAdmin); } }

        protected IOAuthService AuthService;

        protected readonly ClientUser _currentUser;

        public string CompanyDocumentPath
        {
            get { return Path.Combine(
                    Server.MapPath(ConfigurationManager.AppSettings["CompanyDocumentPath"]), 
                    CurrentUser.CompanyId.ToString()); 
            }
        }

        public string ProfilePicturePath
        {
            get
            {
                return Path.Combine(
                  Server.MapPath(ConfigurationManager.AppSettings["ProfilePicturePath"]));
            }
        }

        public SMTPSettings SiteMailSettings
        {
            get
            {
                return new SMTPSettings
                {
                    SMTPServer = WebsiteKernel.SMTPServer,
                    UserName = WebsiteKernel.SMTPUser,
                    Password = WebsiteKernel.SMTPPassword
                };
            }
        }
       
        public BaseController(IOAuthService authService)
        {
            AuthService = authService;

            _currentUser = AuthService.GetClientUser();

            //keep user logged in if already logged.
            if (_currentUser != null && _currentUser.IsLogged())
            {
                _currentUser.CookieDuration = new TimeSpan(0, 0, LoginDuration, 0);
                AuthService.PersistClientUser(_currentUser);
            }
            //else
            //{
                
            //    System.Web.HttpContext.Current.Response.Redirect("/Home/Index");
            //}

            //this must be after initialization of authservice, find user regional settings and give local language as default
            ViewingLanguageId = (CurrentUser != null) ? CurrentUser.LanguageId : (int)Variables.Languages.English ;

            WebsiteKernel = new AdminStruct {
                AdminEmail = ConfigurationManager.AppSettings["AdminEmail"],
                AdminName = ConfigurationManager.AppSettings["AdminName"],
                WebsiteName = ConfigurationManager.AppSettings["WebsiteName"],
                SiteRootURL = ConfigurationManager.AppSettings["RootURL"],                
                SMTPServer = ConfigurationManager.AppSettings["SMTPServerName"],
                SMTPUser = ConfigurationManager.AppSettings["SMTPUsername"],
                SMTPPassword = CM.Security.Decrypt(ConfigurationManager.AppSettings["SMTPPass"])
            
            };


            ViewBag.UserIsAdmin = (CurrentUser != null) && UserIsAdmin;
            ViewBag.IsUserLoggedIn = (CurrentUser != null);
            ViewBag.UserName = (CurrentUser != null) ? CurrentUser.FullName : "";
        }

        //Duration is in minutes
        public int LoginDuration
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["LoginDuration"]);
            }
        }

        public ClientUser CurrentUser { 
            get{
                return _currentUser;
            }
        }
           
    }
}
