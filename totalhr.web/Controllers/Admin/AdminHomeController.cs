using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Authentication.Infrastructure;

namespace totalhr.web.Controllers.Admin
{
    public class AdminHomeController : AdminBaseController
    {

        public AdminHomeController(IOAuthService authService)
            : base(authService)
        {
            
        }
        public ActionResult Index()
        {
            return View();
        }

    }
}
