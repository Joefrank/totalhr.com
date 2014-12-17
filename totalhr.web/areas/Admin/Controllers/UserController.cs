using Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.data.EF;
using totalhr.services.Infrastructure;
using totalhr.Shared.Models;
using totalhr.web.Areas.Admin.Models;


namespace totalhr.web.Areas.Admin.Controllers
{
    public class UserController : AdminBaseController
    {
        private readonly IAccountService _accountService;
        private readonly IProfileService _profileService;
        private readonly ICompanyService _companyService;

        public UserController(IAccountService accountService, IProfileService profileService, ICompanyService companyService, IOAuthService authService) :
            base(authService)
        {
            _accountService = accountService;
            _profileService = profileService;
            _companyService = companyService;
        }

        public ActionResult Index()
        {
            ViewBag.SearchInfo = new UserSearchInfo
            {
                UserList = _accountService.ListCompanyUsersSimple(CurrentUser.CompanyId),
                DepartmentList = _companyService.GetDepartmentSimple(CurrentUser.CompanyId)
            };

            var test = _accountService.GetUserListForAdmin(null, CurrentUser.LanguageId);
            return View(test);
        }

        public ActionResult UserDetails(string uniqueid)
        {
            return View(_accountService.GetUserDetailsForAdmin(uniqueid));
        }

        public ActionResult SearchForUser(UserSearchInfo info)
        {
            var result = new UserSearchResult
            {
                FoundUsers = _accountService.SearchUsers(info),
                SearchInfo = info
            };
            return View("SearchResult", result);
        }
    }
}
