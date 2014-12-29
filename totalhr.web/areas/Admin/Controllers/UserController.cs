using Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using totalhr.data.EF;
using totalhr.services.Infrastructure;
using totalhr.Shared;
using totalhr.Shared.Models;
using totalhr.web.Areas.Admin.Models;
using totalhr.Resources;


namespace totalhr.web.Areas.Admin.Controllers
{
    public class UserController : AdminBaseController
    {
        private readonly IAccountService _accountService;
        private readonly IProfileService _profileService;
        private readonly ICompanyService _companyService;
        private readonly IGlossaryService _glossaryService;

        private static readonly ILog Log = LogManager.GetLogger(typeof(AccountController));

        public UserController(IGlossaryService glossaryService, IAccountService accountService, IProfileService profileService, ICompanyService companyService, IOAuthService authService) :
            base(authService)
        {
            _accountService = accountService;
            _profileService = profileService;
            _companyService = companyService;
            _glossaryService = glossaryService;
        }

        public ActionResult Index()
        {
            return View(DoSearch());
        }

        public ActionResult Page(int id)
        {
            return View("Index", DoSearch(id));
        }

        public UserSearchResult DoSearch(int pageNumber = 1)
        {
            var searchInfo = new UserSearchInfo
            {
                PageNumber = pageNumber,
                PageSize = DefaultPageSize,
                UserList = _accountService.ListCompanyUsersSimple(CurrentUser.CompanyId),
                DepartmentList = _companyService.GetDepartmentSimple(CurrentUser.CompanyId),
                HrefLocation = "/Admin/User/",
                LanguageId = CurrentUser.LanguageId
            };

            var searchResult = new UserSearchResult{
                FoundUsers = _accountService.SearchUserWithPaging(searchInfo),
                SearchInfo = searchInfo
            };

            return searchResult;
        }

        

        public ActionResult UserDetails(string uniqueid)
        {
            return View(_accountService.GetUserDetailsForAdmin(uniqueid));
        }

        public ActionResult SearchForUser(UserSearchInfo info)
        {
            info.UserList = _accountService.ListCompanyUsersSimple(CurrentUser.CompanyId);
            info.DepartmentList = _companyService.GetDepartmentSimple(CurrentUser.CompanyId);
            info.HrefLocation = "/Admin/User/SearchForUser/";
            info.LanguageId = CurrentUser.LanguageId;

            var result = new UserSearchResult
            {
                FoundUsers = _accountService.SearchUserWithPaging(info),
                SearchInfo = info
            };
            return View("SearchResult", result);
        }

        private void LoadGlossaries()
        {
            ViewBag.LanguageList = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.Language);
            ViewBag.CountryList = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.Country);
            ViewBag.GenderList = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.Gender);
            ViewBag.TitleList = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.Title);
            ViewBag.DepartmentList = _companyService.GetDepartmentSimple(CurrentUser.CompanyId);
        }

        public ActionResult Create()
        {
            LoadGlossaries();
            return View(new NewEmployeeInfo { CompanyId = CurrentUser.CompanyId, Password = "", PasswordConfirm = "", UserName = "" });
        }

        [HttpPost]
        public ActionResult Create(NewEmployeeInfo info)
        {
            if (info.DepartmentId < 1)
            {
                ModelState.AddModelError("Missing_Department", Error.Error_Missing_Department); 
            }

            if (ModelState.IsValid)
            {
                //create user. *** set some admin tasks for profiles, roles and picture.
                //*** review profile, permissions and roles
                var user = _accountService.CreateUser(info);

                if (user != null && user.id > 0)
                {
                    return View("UserCreated", info);
                }
                else
                {
                    ModelState.AddModelError("RegistrationFailed", Error.Error_Could_Not_CreateUser);
                    Log.Debug("Employee Registration failed ");
                }
            }

            LoadGlossaries();
            return View();
        }
    }
}
