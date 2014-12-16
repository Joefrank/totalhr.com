using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Authentication.Infrastructure;
using totalhr.services.Infrastructure;
using totalhr.Shared.Models;
using totalhr.data.EF;

namespace totalhr.web.Areas.Admin.Controllers
{
    public class RoleController : AdminBaseController
    {
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;

        public RoleController(IAccountService accountService, IRoleService roleService, IOAuthService authService) :
            base(authService)
        {
            _accountService = accountService;
            _roleService = roleService;
        }

        public ActionResult Index()
        {
            return View(_roleService.GetRoleList());
        }

        public ActionResult ManageUserRoles(int userId = 0)
        {
            User user = (userId > 0) ? _accountService.GetUser(userId) :
                _accountService.GetUserByGuid(Request.QueryString["guid"]);
            if (user != null)
            {
                var profileStruct = new AdminRoleStruct
                {
                    UserId = CurrentUser.UserId,
                    UserRoles = _accountService.GetUserRoles(user.id),
                    AllRoles = _roleService.GetRoleListAgainstUserForListing(user.id)
                };
                ViewBag.UserName = user.firstname + " " + user.surname;
                ViewBag.UserId = user.id;

                return View(profileStruct);
            }
            else if (user.CompanyId != CurrentUser.CompanyId)
            {
                ViewData["ModelError"] = totalhr.Resources.Error.Error_NoAccess_ToUserRole;
                return RedirectToAction("AccessDenied", "Error");
            }

            return View();

        }

        [HttpPost]
        public ActionResult UpdateUserRole(string hdnSelectedRoleIds, int hdnUserId)
        {
            _accountService.UpdateUserRoles(hdnUserId, hdnSelectedRoleIds, CurrentUser.UserId);
            return RedirectToAction("ManageUserRoles", new { userId = hdnUserId });
        }

        public ActionResult ViewUsers(int id)
        {
            Role role = _roleService.GetRole(id);
            ViewBag.RoleName = role.Name;
            return View(_roleService.ListUsersByRole(id, CurrentUser.UserId));
        }

    }
}
