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
using FormService.Infrastructure;
using totalhr.Shared;
using TemplateInfo = totalhr.Shared.Models.TemplateInfo;

namespace totalhr.web.Areas.Admin.Controllers
{
    public class ContractController : AdminBaseController
    {
        private readonly IFormEditorService _formService;
        private readonly ICompanyService _companyService;
        private readonly IContractService _contractService;

        private static readonly ILog log = LogManager.GetLogger(typeof(AccountService));

        public ContractController(IContractService contractService, IFormEditorService formService, 
            ICompanyService companyService, IOAuthService authService) :         base(authService)
        {
            _formService = formService;
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
            ViewBag.FormList = _formService.ListFormsOfTypeSimple((int)Variables.FormType.ContractTemplate);
            return View("CreateTemplate", new totalhr.Shared.Models.TemplateInfo());
        }

        public ActionResult CreateTemplate(TemplateInfo info)
        {
            int templateId = _contractService.CreateContractTemplate(info);
            return RedirectToAction("Template");
        }
    }
}
