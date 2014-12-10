using Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace totalhr.web.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        public HomeController(IOAuthService authService)
            : base(authService)
        {
           
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
