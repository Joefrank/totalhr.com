﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace totalhr.web.Controllers
{
    public class TaskController : Controller
    {
        //
        // GET: /Task/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

    }
}
