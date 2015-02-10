using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace totalhr.web.Areas.Admin.Controllers
{
    public class CommunicationController : Controller
    {
        //
        // GET: /Admin/Communication/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyMessages()
        {
            return View();
        }

        public ActionResult OtherMessages()
        {
            return View();
        }

        public ActionResult Chat()
        {
            return View();
        }
    }
}
