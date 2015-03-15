using Authentication.Infrastructure;
using Authentication.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.services.Infrastructure;
using totalhr.Shared;
using totalhr.Shared.Models;

namespace totalhr.web.Areas.Admin.Controllers
{
    public class AccountController : AdminBaseController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AccountController));
        
        private readonly IOAuthService _authService;
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService, IOAuthService authService)
            : base(authService)
        {
            _authService = authService;
            _accountService = accountService;
        }

        public ActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.Currentuser = CurrentUser;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LoginAdminUser(LoginStruct userdetails)
        {
            if (ModelState.IsValid)
            {
                var userstruct = _accountService.GetUserDetailsForLogin(userdetails.UserName, userdetails.Password);

                if (userstruct != null && userstruct.IsValid() && 
                    userstruct.UserRoles.FirstOrDefault(x => x.id == (int)Variables.Roles.CompanyAdmin) != null)
                {
                    var clientUser = new ClientUser();

                    clientUser.LanguageId = userstruct.UserBasicDetails.preferedlanguageid;
                    clientUser.UserName = userstruct.UserBasicDetails.email;
                    clientUser.UserId = userstruct.UserBasicDetails.id;
                    clientUser.Profiles = userstruct.UserProfiles.ConvertAll(x => x.id.ToString());
                    clientUser.Roles = userstruct.UserRoles.ConvertAll(x => x.id.ToString());
                    clientUser.FullName = userstruct.UserBasicDetails.firstname + " " + userstruct.UserBasicDetails.surname;
                    clientUser.CookieDuration = new TimeSpan(0, 0, LoginDuration, 0);
                    clientUser.Culture = userstruct.UserBasicDetails.chosenculture;
                    clientUser.CompanyId = userstruct.UserBasicDetails.CompanyId;
                    clientUser.DepartmentId = userstruct.UserBasicDetails.departmentid;

                    AuthService.PersistClientUser(clientUser);
                    Response.Redirect("~/Admin/");
                    //return View("Index", clientUser);
                }
                
            }
           
            Log.Debug("Login failed for user  " + userdetails.UserName);
            ModelState.AddModelError("LoginFailed", Resources.FormMessages.Error_Login_Failed);
            return View("Login");
            
        }
    }
}
