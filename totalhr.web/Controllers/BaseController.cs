using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.Shared;
using System.Configuration;
using Authentication.Infrastructure;
using Authentication.Models;

namespace totalhr.web.Controllers
{
    [CustomAuth(new[] { Variables.Roles.Employee, Variables.Roles.CompanyAdmin })]
    public class BaseController : Controller
    {
        public int ViewingLanguageId { get; set; }
        public AdminStruct WebsiteKernel {get; set;}

        protected IOAuthService AuthService;

        private readonly ClientUser _currentUser;

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
