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

        public ActionResult ManageUserProfiles(int userId =0)
        {
            User user = (userId > 0)? _accountService.GetUser(userId) : 
                _accountService.GetUserByGuid(Request.QueryString["guid"]);
            if (user != null)
            {
                var profileStruct = new AdminProfileStruct
                {
                    UserId = CurrentUser.UserId,
                    UserProfiles = _accountService.GetUserProfile(user.id),
                    Allprofiles = _profileService.GetProfileListAgainstUserForListing(user.id)
                };
                ViewBag.UserName = user.firstname + " " + user.surname;
                ViewBag.UserId = user.id;

                return View(profileStruct);
            }
            else if (user.CompanyId != CurrentUser.CompanyId)
            {
                ViewData["ModelError"] = totalhr.Resources.Error.Error_NoAccess_ToUserProfile;
                return RedirectToAction("AccessDenied", "Error");
            }
            
            return View();
           
        }

        public ActionResult UpdateUserProfile(string hdnSelectedProfileIds, int hdnUserId)
        {
            _accountService.UpdateUserProfiles(hdnUserId, hdnSelectedProfileIds, CurrentUser.UserId);
            return RedirectToAction("ManageUserProfiles", new { userId = hdnUserId });
        }

        public ActionResult ViewUsers(int id)
        {
            Profile profile = _profileService.GetProfile(id);
            ViewBag.ProfileName = profile.Name;
            ViewBag.ProfileId = id;
            return View(_profileService.ListUsers(id, CurrentUser.UserId));
        }

        public ActionResult SelectProfile()
        {
            return View(_profileService.GetProfileList());
        }

        public ActionResult CreateProfile()
        {
            return View(new ProfileInfo());
        }

        public ActionResult CreateProfile(ProfileInfo info)
        {
            ResultInfo result = _profileService.CreateProfile(info);

            if (result.Itemid > 0)
            {
                info.Id = result.Itemid;
                info.NewlyCreated = true;               
            }           
            
            return View(info);            
        }
    }
}
