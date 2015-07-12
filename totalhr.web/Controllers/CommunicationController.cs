using Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace totalhr.web.Controllers
{
    public class CommunicationController : BaseController
    {
        private IOAuthService _authService;

        public CommunicationController(IOAuthService authService)
            : base(authService)
        {
            _authService = authService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Inbox()
        {
            return View();
        }

        public ActionResult Notifications()
        {
            return View();
        }
    }
}
