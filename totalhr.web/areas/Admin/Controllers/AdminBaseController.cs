using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Authentication.Infrastructure;
using Authentication.Models;
using totalhr.Shared;
using totalhr.web.Controllers;

namespace totalhr.web.Areas.Admin.Controllers
{
    [CustomAuth(Variables.Roles.CompanyAdmin)]
    public class AdminBaseController : BaseController
    {

        public AdminBaseController(IOAuthService authService): base(authService)
        {
                  
        }

    }
}
