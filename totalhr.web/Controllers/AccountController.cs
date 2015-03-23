using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.data.Repositories.Infrastructure;
using totalhr.Shared;
using totalhr.Shared.Models;
using totalhr.services.Implementation;
using totalhr.services.Infrastructure;
using Authentication.Infrastructure;
using totalhr.services.messaging.Infrastructure;
using log4net;
using totalhr.web.Models;
using totalhr.data.Models;
using Authentication.Models;
using totalhr.data.EF;
using totalhr.Resources;

namespace totalhr.web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IGlossaryService _glossaryService;
        private readonly IAccountService _accountService;
        private readonly IMessagingService _messagingService;
        private readonly IProfileService _profileService;
        private readonly IContractService _contractService;
        
        private static readonly ILog Log = LogManager.GetLogger(typeof(AccountController));

        public AccountController(IGlossaryService glossaryService, IAccountService accountService, IMessagingService messageService, 
            IProfileService profileService, IOAuthService authService, IContractService contractService) : base(authService)
        {
            _glossaryService = glossaryService;
            _accountService = accountService;
            _messagingService = messageService;
            _messagingService.ReadSMTPSettings(SiteMailSettings);
            _profileService = profileService;
            _contractService = contractService;
        }
        
        public ActionResult Index()
        {            
            return View(CurrentUser);
        }
        
        public ActionResult MyDetails()
        {
            var userinfo = _accountService.GetUserProfileDetails(CurrentUser.UserId, CurrentUser.UserName.Trim());           
          
            return View(userinfo);
        }

        public ActionResult EditMyDetails()
        {
            LoadGlossaries();
            return View(_accountService.GetUserInfoByEmail(CurrentUser.UserName));
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(new LoginStruct { ReferringUrl = (Request.UrlReferrer != null)? Request.UrlReferrer.AbsoluteUri : "" });
        }

        private void LoadGlossaries()
        {
            ViewBag.LanguageList = _glossaryService.GetLanguageList(this.ViewingLanguageId);
            ViewBag.CountryList = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.Country);
            ViewBag.GenderList = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.Gender);
            ViewBag.TitleList = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.Title);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            LoadGlossaries();
            return View(new NewUserInfo { Password = "", PasswordConfirm = "", UserName = "" });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RegisterUser(NewUserInfo userinfo)
        {
            if (ModelState.IsValid)
            {
                Log.Debug("New User Registration Started");

                userinfo.UserTypeId = (int)Variables.UserType.CompanyCreator;
                //register user.
                UserRegStruct structresult = _accountService.RegisterUserCompany(userinfo, WebsiteKernel);

                if (structresult.UserId > 0 && structresult.CompanyId > 0)
                {
                    Log.Debug("Notification/Messaging started");

                    //send notifications to both admin and registrant.
                    _messagingService.AcknowledgeNewUserRegistration(WebsiteKernel, structresult);
                    //prepare and redirect to welcome screen.
                    TempData["UserReg"] = structresult;
                    return RedirectToAction("Welcome", "Account");
                }
                else
                {
                    ModelState.AddModelError("RegistrationFailed", structresult.RegError);
                    Log.Debug("New User Registration failed " + structresult.RegError);
                }
            }

            LoadGlossaries();
            return View("Register", userinfo);           
        }

        [AllowAnonymous]
        public ActionResult Welcome()
        {
            var userstruct = TempData["UserReg"] as UserRegStruct;
            return View(userstruct);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LoginUser(LoginStruct userdetails)
        {
            if (!ModelState.IsValid)
                return null;

            totalhr.data.Models.UserDetailsStruct userstruct = _accountService.GetUserDetailsForLogin(userdetails.UserName, userdetails.Password);

            if (userstruct != null && userstruct.IsValid())
            {
                var clientUser = new ClientUser
                    {
                        LanguageId = userstruct.UserBasicDetails.preferedlanguageid,
                        UserName = userstruct.UserBasicDetails.email,
                        UserId = userstruct.UserBasicDetails.id,
                        Profiles = userstruct.UserProfiles.ConvertAll(x => x.id.ToString(CultureInfo.InvariantCulture)),
                        Roles = userstruct.UserRoles.ConvertAll(x => x.id.ToString(CultureInfo.InvariantCulture)),
                        FullName = userstruct.UserBasicDetails.firstname + " " + userstruct.UserBasicDetails.surname,
                        CookieDuration = new TimeSpan(0, 0, LoginDuration, 0),
                        Culture = userstruct.UserBasicDetails.chosenculture,
                        CompanyId = userstruct.UserBasicDetails.CompanyId,
                        DepartmentId = userstruct.UserBasicDetails.departmentid
                    };

                AuthService.PersistClientUser(clientUser);

                ViewBag.UserIsAdmin = clientUser.HasRole((int)Variables.Roles.CompanyAdmin);
                ViewBag.IsUserLoggedIn = true;
                ViewBag.UserName = clientUser.FullName;

                if (!string.IsNullOrEmpty(userdetails.ReferringUrl))
                {
                    Response.Redirect(userdetails.ReferringUrl);
                }

                return View("Index", clientUser);
            }
            else
            {
                Log.Debug("Login failed for user  " + userdetails.UserName);

                ModelState.AddModelError("LoginFailed", Resources.FormMessages.Error_Login_Failed);
                return View("Login");
            }
        }


        /// <summary>
        /// this is for testing only do not go live with it.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ExpressLogAdmin()
        {
            var user = new ClientUser
            {
                FullName = "Joe Tester Temp",
                CompanyId = 30,
                DepartmentId = 2,
                CookieDuration = new TimeSpan(0, 5, 0, 0),
                Culture = "en-GB",
                UserName = "joe_bolla@yahoo.com",
                LanguageId = 1,
                Profiles = new List<string> { "1", "4" },
                Roles = new List<string> { "1", "2", "3" },
                UserId = 59
            };
           
            AuthService.PersistClientUser(user);
            ViewBag.UserIsAdmin = true;
            ViewBag.IsUserLoggedIn = true;
            ViewBag.UserName = "Admin-Joe Tester";
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [AllowAnonymous]
        public ActionResult ExpressLogNonAdmin()
        {
            var user = new ClientUser
            {
                FullName = "Joe Tester Temp",
                CompanyId = 30,
                DepartmentId = 2,
                CookieDuration = new TimeSpan(0, 5, 0, 0),
                Culture = "en-GB",
                UserName = "joseph.bolla@cyberminds.co.uk",
                LanguageId = 1,
                Profiles = new List<string> { "1", "4" },
                Roles = new List<string> { "3" },
                UserId = 59
            };

            AuthService.PersistClientUser(user);
            ViewBag.UserIsAdmin = false;
            ViewBag.IsUserLoggedIn = true;
            ViewBag.UserName = "Joe Tester";

            return View("Index", user);
        }

        public ActionResult Logout()
        {
            AuthService.LogUserOut();
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PasswordRemind(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("MissingEmail", Resources.FormMessages.Error_Invalid_Email);
                return View("ForgotPassword");
            }

            totalhr.data.EF.User user = _accountService.GetUserByEmail(email.Trim());

            if (user != null)
            {
                //send user email 
                _messagingService.SendPasswordReminderEmail(WebsiteKernel, user); 
              
                return View("Login");
            }
            else
            {
                ModelState.AddModelError("UserNotFound", Resources.FormMessages.Error_Email_Doesnt_Exist);
                return View("ForgotPassword");
            }
        }

        [AllowAnonymous]
        public ActionResult Activate(string id)
        {
            Log.Debug("Account Activation Token: " + id);//token is user guid 

            if (Functions.IsGuid(id))
            {

                totalhr.data.EF.User user = _accountService.ActivateUser(id);
                //send user email 
                _messagingService.AcknowledgeAccountActivation(WebsiteKernel, user);

                return View(user);
            }
            else
            {
                Log.Debug("Account Activation Token - Invalid Guid: " + id);
                return View();
            }
        }

        public ActionResult GetCompanyUsers()
        {
            List<User> lstUsers = _accountService.GetCompanyUsers(CurrentUser.CompanyId);

            return View("CompanyUsersSelector", lstUsers);
        }

        public ActionResult GetCompanyDepartments()
        {
            List<Department> lstDepartments = _accountService.GetCompanyDepartments(CurrentUser.CompanyId);

            return View("CompanyDepartmentSelector", lstDepartments);
        }

        public JsonResult GetCompanyUsersJson()
        {
            List<User> lstUsers = _accountService.GetCompanyUsers(CurrentUser.CompanyId);

            var qry = from user in lstUsers
                      orderby user.firstname
                      select new {UserId = user.id, FullName = user.firstname + " " + user.surname};

            return Json(qry, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCompanyUserSimple()
        {
            var lstUsers = _accountService.GetCompanyUsersSimple(CurrentUser.CompanyId, CurrentUser.UserId);
            return View("UserListSelector", lstUsers);
        }

        public JsonResult GetCompanyDepartmentsJson()
        {
            List<Department> lstDepartments = _accountService.GetCompanyDepartments(CurrentUser.CompanyId);

            var qry = from dept in lstDepartments
                      orderby dept.Name ascending
                      select new { dept.id, dept.Name };

            return Json(qry, JsonRequestBehavior.AllowGet);
        }

               
        [HttpPost]
        public ActionResult SaveUserDetails(UserPersonalInfo userinfo)
        {

            if (ModelState.IsValid)
            {
                int result = _accountService.UpdateUserDetails(userinfo);

                if (result < 0)
                {
                    ModelState.AddModelError("Error_Saving_User", FormMessages.Error_Sorry_CouldNot_Save_User);
                    userinfo.DetailsSaveSuccess = false;
                }
                else
                {
                    userinfo.DetailsSaveSuccess = true;
                }
            }
            else
            {
                userinfo.DetailsSaveSuccess = false;
            }

            LoadGlossaries();
            return View("EditMyDetails", userinfo);

        }

        public ActionResult ProfilePreview(string id)
        {
            UserPersonalInfo user = _accountService.GetUserInfoByGuid(id);

            if (user != null)
            {
                return View(user);
            }

            return RedirectToAction("AccessDenied", "Error");
        }

        public ActionResult Permissions()
        {
            return View(_profileService.GetUserProfiles(CurrentUser.UserId));
        }

        public ActionResult Contract()
        {
            return View(_contractService.GetEmployeeContractDisplay(CurrentUser.UserId));
        }

        public ActionResult PayRoll()
        {
            return View();
        }
    }
}
