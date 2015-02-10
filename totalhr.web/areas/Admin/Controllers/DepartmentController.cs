using Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace totalhr.web.Areas.Admin.Controllers
{
    public class DepartmentController : AdminBaseController
    {
        IOAuthService _authService;

        public DepartmentController(IOAuthService authService) :
            base(authService)
        {
            _authService = authService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateDepartment()
        {
            return View();
        }

    }
}
