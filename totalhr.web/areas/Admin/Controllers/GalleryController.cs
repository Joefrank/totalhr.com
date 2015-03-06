using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Authentication.Infrastructure;
using totalhr.services.Infrastructure;

namespace totalhr.web.Areas.Admin.Controllers
{
    public class GalleryController : AdminBaseController
    {
        private IOAuthService _authService;

        public  GalleryController(IAccountService accountService, IOAuthService authService)
            : base(authService)
        {
            _authService = authService;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
