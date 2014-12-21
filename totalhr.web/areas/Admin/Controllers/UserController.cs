using Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.data.EF;
using totalhr.services.Infrastructure;
using totalhr.Shared;
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

        public ActionResult Create()
        {
            return View();
        }
    }
}
