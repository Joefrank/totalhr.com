using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Authentication.Infrastructure;

namespace totalhr.web.Areas.Admin.Controllers
{
    public class CalendarController : AdminBaseController
    {
        public CalendarController(IOAuthService authService) :        
            base(authService)
        {
            
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateCalendar()
        {
            return View();
        }
    }
}
