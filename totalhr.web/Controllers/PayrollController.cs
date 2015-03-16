using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Authentication.Infrastructure;
using totalhr.services.Infrastructure;

namespace totalhr.web.Controllers
{
    public class PayrollController : BaseController
    {
        private IOAuthService _authService;
        private IPayrollService _payrollService;

        public PayrollController(IPayrollService payrollService, IOAuthService authservice)
            : base(authservice)
        {
            _authService = authservice;
            _payrollService = payrollService;
        }

        public ActionResult Index()
        {
            return View(_payrollService.GetUserPayrollDetails(CurrentUser.UserId));
        }

    }
}
