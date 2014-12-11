using Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.services.Infrastructure;
using totalhr.Shared.Models;

namespace totalhr.web.Areas.Admin.Controllers
{
    public class ProfileController : AdminBaseController
    {

        private readonly IAccountService _accountService;
        private readonly IProfileService _profileService;

        public ProfileController(IAccountService accountService, IProfileService profileService, IOAuthService authService):
            base(authService)
        {
            _accountService = accountService;
            _profileService = profileService;
        }

        public ActionResult Index()
        {
            var lst = _accountService.GetUserProfile(CurrentUser.UserId);
            
            var sList = lst.Select(x => new ListItemStruct { Id = x.id, Name = x.Name }).ToList();

            var profList = _profileService.GetProfileList();

            var profileStruct = new AdminProfileStruct
            {
                UserId = CurrentUser.UserId,
                UserProfiles = sList,
                Allprofiles = profList.Select(x => new ListItemStruct { Id = x.id, Name = x.Name }).ToList() // new List<ListItemStruct>{new ListItemStruct{Id=1,Name ="Test"}}
            };
            
            return View(profileStruct);
        }

    }
}
