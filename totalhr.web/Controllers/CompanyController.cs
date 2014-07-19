using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace totalhr.web.Controllers
{
    public class CompanyController : Controller
    {
        //
        // GET: /Company/
        //companyh details, departments, structure

        public ActionResult Index()
        {
            return View();
        }

    }
}
