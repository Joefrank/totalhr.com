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
using totalhr.data.EF;
using totalhr.web.Areas.Admin.Models;

namespace totalhr.web.Areas.Admin.Controllers
{
    public class ContractController : AdminBaseController
    {
        private readonly IFormEditorService _formService;
        private readonly ICompanyService _companyService;
        private readonly IContractService _contractService;
        private readonly IAccountService _accountService;

        private static readonly ILog log = LogManager.GetLogger(typeof(AccountService));

        public ContractController(IContractService contractService, IFormEditorService formService, 
            ICompanyService companyService, IAccountService accountService, IOAuthService authService) :        
            base(authService)
        {
            _formService = formService;
            _companyService = companyService;
            _contractService = contractService;
            _accountService = accountService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Manage()
        {
            return View(_accountService.GetCompanyUsers(CurrentUser.CompanyId));
        }

        public ActionResult ManageUserContract(int slUserList)
        {
            var userContractDetails = new UserContractDetails
            {
                UserDetails = _accountService.GetUser(slUserList),
                Contract =_contractService.GetUserContract(slUserList),
                TemplateList = _contractService.ListContractTemplates()
            };

            return View(userContractDetails);
        }

        public ActionResult SaveUserContract()
        {
            return View();
        }

        public ActionResult Template(SortingInfo info)
        {
            ViewBag.SortInfo = (info != null)? info :
                new SortingInfo
                {
                    SortColumn = "TemplateId",
                    SortOrder = "asc"
                };
            return View(_contractService.ListContractTemplates(info));
        }

        public ActionResult AddTemplate()
        {
            ViewBag.FormList = _formService.ListFormsOfTypeSimple((int)Variables.FormType.ContractTemplate);
            return View("CreateTemplate", new totalhr.Shared.Models.TemplateInfo());
        }

        public ActionResult CreateTemplate(TemplateInfo info)
        {
            info.CreatedBy = CurrentUser.UserId;
            int templateId = _contractService.CreateContractTemplate(info);
            return RedirectToAction("Template");
        }

        public ActionResult EditTemplate(int id)
        {
            ViewBag.FormList = _formService.ListFormsOfTypeSimple((int)Variables.FormType.ContractTemplate);            
            return View(_contractService.GetTemplate(id));
        }

        [HttpPost]
        public ActionResult EditTemplate(TemplateInfo info)
        {
            info.LastUpdatedBy = CurrentUser.UserId;
            _contractService.UpdateContractTemplate(info);
            return RedirectToAction("Template");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">this is template id</param>
        /// <returns></returns>
        public ActionResult FillContract(int id)
        {
            var template = _contractService.GetTemplate(id);
            var form = _formService.GetForm(template.FormId);

            ViewBag.TemplateName = template.Name;
            return View(form);
        }
    }
}
