﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.Shared;
using System.Configuration;
using Authentication.Infrastructure;
using Authentication.Models;
using System.IO;
using System.Threading;
using System.Globalization;
using totalhr.Shared.Models;
using Authentication.Models.Enums;

namespace totalhr.web.Controllers
{
    [CustomAuth(new[] { Roles.Employee, Roles.CompanyAdmin })]
    public class BaseController : Controller
    {
        public int ViewingLanguageId { get; set; }
        public AdminStruct WebsiteKernel {get; set;}

        public bool UserIsAdmin { get { return CurrentUser.HasProfile((int)Roles.CompanyAdmin); } }

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

            //make sure culture cookie gets update when language changed.
            if (CurrentUser != null && !string.IsNullOrEmpty(CurrentUser.Culture))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(CurrentUser.Culture);
            }

            //keep user logged in if already logged.
            if (_currentUser != null && _currentUser.IsLogged())
            {
                _currentUser.CookieDuration = new TimeSpan(0, 0, LoginDuration, 0);
                AuthService.PersistClientUser(_currentUser);
            }           

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
            ViewBag.Currentuser = CurrentUser;
            
            
            if (CurrentUser != null)
            {
                
                ViewBag.DummyLanguageList =  CurrentUser.LanguageId == 2 ? 
                 new List<ListItemStruct>
                {
                    new ListItemStruct{Id = 1, Name = "Anglais"},
                    new ListItemStruct{Id= 2, Name= "Francais"}
                }
                :
                new List<ListItemStruct>
                {
                    new ListItemStruct{Id = 1, Name = "English"},
                    new ListItemStruct{Id= 2, Name= "French"}
                };
            }
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
