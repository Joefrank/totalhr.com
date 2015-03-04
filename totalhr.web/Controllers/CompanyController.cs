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
            var organigram = _companyService.GetOrganigram(CurrentUser.CompanyId);
            return View(organigram);
        }
    }
}
