using Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.services.Infrastructure;

namespace totalhr.web.Controllers
{
    public class CompanyController : BaseController
    {
        private ICompanyService _companyService;

        public CompanyController(ICompanyService companyService, IOAuthService authservice)
            : base(authservice)
        {
            _companyService = companyService;            
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Structure()
        {            
            return View();
        }

        public ActionResult Organigram()
        {
            var organigram = _companyService.GetOrganigram(CurrentUser.CompanyId);
            return View(organigram);
        }

        public ActionResult EmployeeDirectory()
        {
            return View(_companyService.ListEmployees(CurrentUser.CompanyId));
        }

        public ActionResult Departments()
        {
            return View(_companyService.GetCompanyDepartments(CurrentUser.CompanyId));
        }
       
    }
}
