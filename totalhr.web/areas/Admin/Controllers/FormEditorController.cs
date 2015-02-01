using Authentication.Infrastructure;
using FormService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.services.Infrastructure;
using totalhr.Shared;

namespace totalhr.web.Areas.Admin.Controllers
{
    public class FormEditorController : AdminBaseController
    {
        private readonly IOAuthService _authService;
        private readonly IContractService _contractService;
        private readonly IFormEditorService _formService;

        public FormEditorController(IContractService contractService, IFormEditorService formService, IOAuthService authService)
            : base(authService)
        {
            _authService = authService;
            _contractService = contractService;
            _formService = formService;
        }       

        //make this display a list of exising forms
        public ActionResult Index()
        {
            return View();
            //return View(new totalhr.Shared.Models.FormInfo { Schema = DummySchema });
        }
                
        public ActionResult EditForm(int id)
        {
            // make form write protected. only selected users should edit
            return View();
        }
       
        public ActionResult CreateForm([Bind(Prefix="id")] int formType)
        {
            var form = new totalhr.Shared.Models.FormInfo();

            switch (formType)
            {
                case (int)Variables.FormType.ContractTemplate :
                    form = _contractService.GetDefaultTemplate(CurrentUser.LanguageId);
                    break;
            }

            form.FormTypeId = formType;

            return View(form);
        }

        [HttpPost]
        public JsonResult CreateForm(FormViewModel data)
        {
            if (string.IsNullOrEmpty(data.Schema))
            {
                return Json(new { Id = -1, Message = "Empty form provided" });
            }

            int result = _formService.CreateForm(data.Schema, data.FormTypeId, CurrentUser.UserId);

            if (result > 0)
            {
                _formService.SaveFormFields(result, data.Schema);
                return Json(new { Id = 1, Message = "" });
            }
            else
            {
                return Json(new { Id = -1, Message = "Error" });
            }
                     
        }


        public ActionResult Preview([Bind(Prefix = "id")] int templateid)
        {
            var contract = _contractService.GetTemplate(templateid);
            var form = _formService.GetForm(contract.FormId);
            return View(form);
        }

        public class FormViewModel
        {
            public int Id { get; set; }
            public int FormTypeId { get; set; }
            public string Schema { get; set; }
        }

    }
}
