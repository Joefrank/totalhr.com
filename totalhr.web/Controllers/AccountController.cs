using System;
using System.Collections.Generic;
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
        private const string UserDataFormat = "UserId:{0}|roles:{1}|profiles:{2}|fullname:{3}|culture:{4}|languageid:{5}";

        private static readonly ILog Log = LogManager.GetLogger(typeof(AccountController));

        public AccountController(IGlossaryService glossaryService, IAccountService accountService, IMessagingService messageService, IOAuthService authService) : base(authService)
        {
            _glossaryService = glossaryService;
            _accountService = accountService;
            _messagingService = messageService;           
            _messagingService.ReadSMTPSettings(new SMTPSettings {SMTPServer = WebsiteKernel.SMTPServer, UserName = WebsiteKernel.SMTPUser, Password = WebsiteKernel.SMTPPassword });
        }

        [CustomAuthorize(Roles = "3")]
        public ActionResult Index()
        {
            var user = AuthService.GetClientUser();

            if (user == null)
            {
                return View("Login");
            }
            return View(user);
        }

        [CustomAuthorize(Roles = "3")]
        public ActionResult MyDetails()
        {
            totalhr.data.EF.User user = _accountService.GetUserByEmail(CurrentUser.UserName.Trim());
            if (user == null)
            {
                return View("Login");
            }
            return View(user);
        }

        public ActionResult Login()
        {
            ViewBag.Currentuser = CurrentUser;
            return View();
        }

        private void LoadGlossaries()
        {
            ViewBag.LanguageList = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.Language);
            ViewBag.CountryList = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.Country);
            ViewBag.GenderList = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.Gender);
            ViewBag.TitleList = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.Title);
        }

        public ActionResult Register()
        {
            LoadGlossaries();
            return View(new NewUserInfo { Password = "", PasswordConfirm = "", UserName = "" });
        }

        [HttpPost]
        public ActionResult RegisterUser(NewUserInfo userinfo)
        {
            if (ModelState.IsValid)
            {
                Log.Debug("New User Registration Started");

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

        public ActionResult Welcome()
        {
            UserRegStruct userstruct = TempData["UserReg"] as UserRegStruct;
            return View(userstruct);
        }

        [HttpPost]
        public ActionResult LoginUser(LoginStruct userdetails)
        {
            if (!ModelState.IsValid)
                return null;

            totalhr.data.Models.UserDetailsStruct userstruct = _accountService.GetUserDetailsForLogin(userdetails.UserName, userdetails.Password);

            if (userstruct != null && userstruct.IsValid())
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

                AuthService.PersistClientUser(clientUser);

                return View("Index", clientUser);
            }
            else
            {
                Log.Debug("Login failed for user  " + userdetails.UserName);

                ModelState.AddModelError("LoginFailed", Resources.FormMessages.Error_Login_Failed);
                return View("Login");
            }
        }

        public ActionResult Logout()
        {
            AuthService.LogUserOut();
            return View("Login");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PasswordRemind(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                ModelState.AddModelError("MissingEmail", Resources.FormMessages.Error_Invalid_Email);
                return View("ForgotPassword");
            }

            totalhr.data.EF.User user = _accountService.GetUserByEmail(Email.Trim());

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
            return Json(lstUsers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompanyDepartmentsJson()
        {
            List<Department> lstDepartments = _accountService.GetCompanyDepartments(CurrentUser.CompanyId);

            var qry = from dept in lstDepartments
                      orderby dept.Name ascending
                      select new { dept.id, dept.Name };

            return Json(qry, JsonRequestBehavior.AllowGet);
        }
    }
}
