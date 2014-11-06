using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Authentication.Infrastructure;
using Authentication.Models;
using totalhr.Shared;

namespace totalhr.web.Controllers.Admin
{
    [CustomAuth(Variables.Roles.CompanyAdmin)]
    public class AdminBaseController : BaseController
    {
       public AdminBaseController(IOAuthService authService) :base(authService)
       {
           
       }
    }
}
