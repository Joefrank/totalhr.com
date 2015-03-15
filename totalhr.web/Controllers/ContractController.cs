using Authentication.Infrastructure;
using FormService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.services.Infrastructure;

namespace totalhr.web.Controllers
{
    public class ContractController : BaseController
    {
        private readonly ICompanyService _companyService;
        private readonly IContractService _contractService;

        public ContractController(IContractService contractService, ICompanyService companyService, 
            IOAuthService authService) :    base(authService)
        {
            _companyService = companyService;
            _contractService = contractService;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
