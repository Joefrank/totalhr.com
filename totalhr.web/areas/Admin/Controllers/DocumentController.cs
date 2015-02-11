using Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace totalhr.web.Areas.Admin.Controllers
{
    public class DocumentController : AdminBaseController
    {
        IOAuthService _authService;

        public DocumentController(IOAuthService authService) :
            base(authService)
        {
            _authService = authService;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
