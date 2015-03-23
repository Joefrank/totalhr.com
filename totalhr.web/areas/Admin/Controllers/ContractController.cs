using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using totalhr.Resources;
using Newtonsoft.Json;

namespace totalhr.web.Areas.Admin.Controllers
{
    public class ContractController : AdminBaseController
    {
        private readonly IFormEditorService _formService;
        private readonly ICompanyService _companyService;
        private readonly IContractService _contractService;
        private readonly IAccountService _accountService;
        private StringBuilder _sbContractErrors = new StringBuilder();

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

        private UserContractDetails GetUserContractDetails(int userId)
        {
            var contract = _contractService.GetUserContract(userId);

            return new UserContractDetails
            {
                UserDetails = _accountService.GetUser(userId),
                Contract = contract,
                TemplateList = _contractService.ListContractTemplates(),
                TemplateId = (contract != null)? contract.TemplateId : 0
            };
        }

        public ActionResult ManageUserContract(int slUserList)
        {
            return View(GetUserContractDetails(slUserList));
        }

        [HttpPost]
        public ActionResult SaveUserContract(UserContractInfo info)
        {
            info.CreatedBy = CurrentUser.UserId;

            if (info.UserId < 1)
            {
                ModelState.AddModelError("Error_User_Req", Contract.Error_User_Req);
            }

            if (info.TemplateId < 1)
            {
                ModelState.AddModelError("Error_Template_Rq", Contract.Error_Template_Rq);
            }

            if (ModelState.IsValid)
            {
                var contract = info.ContractId > 0 ? _contractService.UpdateUserContract(info) :
                _contractService.CreateUserContract(info);
                ViewBag.ModelSaved = true;
            }

            return View("ManageUserContract", GetUserContractDetails(info.UserId));
        }

        public ActionResult Template(SortingInfo info)
        {
            ViewBag.SortInfo = info ?? new SortingInfo
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
            var contractDetails = _contractService.GetUserContractDetails(id);           
            return View(contractDetails);
        }

        [HttpPost]
        public JsonResult SaveUserContractData(ContractFillViewInfo model)
        {
            if (IsValid(model))
            {
                model.CreatedBy = CurrentUser.UserId;
                var data = _contractService.SaveUserContractData(model); //persist json data
                var result = _formService.SaveFieldData(model);//persist field data

                return Json(new { Id = data.ContractId, Message = HttpUtility.HtmlEncode(Contract.V_Contract_Saved) });
            }
            else 
            {
                return Json(new { Id = -1, Message = HttpUtility.HtmlEncode(_sbContractErrors .ToString())});
            }
        }

        public JsonResult UpdateFormFields(int id)
        {
            var form = _formService.GetForm(id);
            var info = new FormInfo
                {
                    Id = form.Id,
                    FormTypeId = form.FormTypeId,
                    Schema = form.FormSchema,
                    UserId = CurrentUser.UserId
                };
            return Json(_formService.SaveFormFields(info));
        }

        bool IsValid(ContractFillViewInfo model)
        {
            bool bValid = true;

            if (model.ContractId < 1){
                _sbContractErrors.Append("<br/>" + Contract.Error_Template_Name_Rq);
                bValid = false;
            }

            if (model.UserId < 1)
            {
                _sbContractErrors.Append("<br/>" + Contract.Error_User_Req);
                bValid = false;
            }

            if (string.IsNullOrEmpty(model.Data))
            {
                _sbContractErrors.Append("<br/>" + Contract.Error_Form_Data);
                bValid = false;
            }

            return bValid;

        }


    }
}
