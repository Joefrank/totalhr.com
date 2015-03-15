using Authentication.Infrastructure;
using FormService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.services.Infrastructure;
using totalhr.Shared;
using totalhr.Shared.Models;
using totalhr.Resources;

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
            var formList = _formService.ListFormsOfType((int)Variables.FormType.ContractTemplate);
            //Test(@"{""fields"":[{""type"":""text"",""name"":""field1"",""displayName"":""Field 1"",""validation"":{""messages"":{""minlength"":""Minimum 3 characters"",""maxlength"":""Maximum 50 characters"",""required"":""This is a test message""},""required"":true,""minlength"":3,""maxlength"":50},""placeholder"":""Place holder field 1"",""tooltip"":""Tooltip for Field 1"",""$$hashKey"":""00F"",""$_invalid"":false,""$_displayProperties"":true,""value"":""Initial value"",""$_redraw"":false},{""type"":""text"",""name"":""Email"",""displayName"":""Email"",""validation"":{""messages"":{""required"":""You need to enter a valid email address"",""maxlength"":""Do not exceed 100 characters""},""required"":true,""maxlength"":100},""placeholder"":""Enter field1 details here"",""tooltip"":""Enter your email address"",""$$hashKey"":""00G"",""$_invalid"":false,""$_displayProperties"":true,""value"":""Your email address"",""$_redraw"":false}],""$_invalid"":true}");
           //Test(@"{""fields"":[{""type"":""text"",""name"":""field1"",""displayName"":""Field 1"",""validation"":{""messages"":{""required"":""dsfdfdf"",""minlength"":""min""},""required"":true,""minlength"":3},""placeholder"":""Enter field1 details here"",""tooltip"":""Enter field1 details here"",""$$hashKey"":""00F"",""$_invalid"":false,""$_displayProperties"":false,""$_redraw"":false},{""type"":""text"",""name"":""field2"",""displayName"":""Field 2"",""validation"":{""messages"":{""required"":""sdfdf""},""required"":true,""maxlength"":45},""placeholder"":""Enter field1 details here"",""tooltip"":""Enter field1 details here"",""$$hashKey"":""00G"",""$_invalid"":false,""$_displayProperties"":true,""$_redraw"":false}],""$_invalid"":false}");
            return View(formList);
        }

        //***testing only
        private void Test(string schema)
        {
            var info = new FormInfo
            {
             UserId = CurrentUser.UserId,
             FormTypeId = (int)Variables.FormType.ContractTemplate,
             Id = 3,
              Schema = schema
            };

            _formService.SaveFormFields(info);
        }


        public ActionResult EditForm(int id)
        {
            // make form write protected. only selected users should edit
            return View(_formService.GetForm(id));
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
        public JsonResult SaveForm(FormInfo info)
        {
            if (string.IsNullOrEmpty(info.Schema))
            {
                return Json(new { Id = -1, Message = FormGenerator.Error_Form_Editor_Empty });
            }
            else if (string.IsNullOrEmpty(info.FormName))
            {
                return Json(new { Id = -1, Message = FormGenerator.Error_Provide_Form_Name });
            }

            try
            {
                info.UserId = CurrentUser.UserId;
                var result = 0;

                result = info.Id > 0 ? _formService.UpdateForm(info) : _formService.CreateForm(info);

                return Json(result <= 0 ? 
                    new {Id = -1, Message = FormGenerator.Error_Couldn_Save_Form}
                    : 
                    new { Id = result, Message = FormGenerator.V_Form_Saved_Success }
                    );
            }
            catch (Exception ex)
            {
                return Json(new { Id = -1, Message = FormGenerator.Error_Couldn_Save_Form });
            }
                     
        }

       
        public ActionResult Preview([Bind(Prefix = "id")] int templateid)
        {
            var contract = _contractService.GetTemplate(templateid);
            var form = _formService.GetForm(contract.FormId);

            return View(form);
        }

    }
}
