using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Authentication.Infrastructure;
using ChatService.Infrastructure;

namespace totalhr.web.Controllers
{
    public class AbsenceController : BaseController
    {
        private IOAuthService _authService;

        public AbsenceController(IOAuthService authservice)
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
