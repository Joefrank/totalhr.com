using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.Shared.Models;
using totalhr.data.Repositories.Infrastructure;
using totalhr.data.Repositories.Implementation;
using totalhr.services.messaging.Infrastructure;
using totalhr.services.Infrastructure;
using totalhr.services.Implementation;
using log4net;
using Authentication.Infrastructure;

namespace totalhr.web.Areas.Admin.Controllers
{
    public class ContractController : AdminBaseController
    {
        private readonly IAccountService _accountService;
        private readonly ICompanyService _companyService;
        private readonly IContractService _contractService;

        private static readonly ILog log = LogManager.GetLogger(typeof(AccountService));

        public ContractController(IContractService contractService, IAccountService accountService, 
            ICompanyService companyService, IOAuthService authService) :         base(authService)
        {
            _accountService = accountService;
            _companyService = companyService;
            _contractService = contractService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult Template()
        {
            ViewBag.SortInfo = new SortingInfo
            {
                SortColumn = "TemplateId",
                SortOrder = "asc"
            };
            return View(_contractService.ListContractTemplates());
        }

        public ActionResult AddTemplate()
        {

            return View("CreateTemplate", new totalhr.Shared.Models.TemplateInfo());
        }
    }
}
