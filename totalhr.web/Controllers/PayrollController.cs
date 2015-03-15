using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Authentication.Infrastructure;

namespace totalhr.web.Controllers
{
    public class PayrollController : BaseController
    {
        private IOAuthService _authService;

        public PayrollController(IOAuthService authservice)
            : base(authservice)
        {
            _authService = authservice;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
