using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace totalhr.web.Areas.Admin.Controllers
{
    public class CalendarController : Controller
    {
        //
        // GET: /Admin/Calendar/

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
