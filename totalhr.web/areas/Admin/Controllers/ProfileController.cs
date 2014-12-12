using Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.services.Infrastructure;
using totalhr.Shared.Models;
using totalhr.data.EF;

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
            return View(_profileService.GetProfileList());
        }

        public ActionResult ManageUserProfiles()
        {            
            User user = _accountService.GetUserByGuid(Request.QueryString["guid"]);
            if (user != null)
            {
                var profileStruct = new AdminProfileStruct
                {
                    UserId = CurrentUser.UserId,
                    UserProfiles = _accountService.GetUserProfile(user.id),
                    Allprofiles = _profileService.GetProfileListAgainstUserForListing(user.id)
                };
                ViewBag.UserName = user.firstname + " " + user.surname;
                return View(profileStruct);
            }
            else if (user.CompanyId != CurrentUser.CompanyId)
            {
                ViewData["ModelError"] = totalhr.Resources.Error.Error_NoAccess_ToUserProfile;
                return RedirectToAction("AccessDenied", "Error");
            }
            
            return View();
           
        }

        public ActionResult ViewUsers(int id)
        {
            Profile profile = _profileService.GetProfile(id);
            ViewBag.ProfileName = profile.Name;
            return View(_profileService.ListUsers(id, CurrentUser.UserId));
        }
    }
}
