using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Shared.Models;
using totalhr.data.Repositories.Infrastructure;
using totalhr.data.EF;
using totalhr.Shared;
using FormService.Infrastructure;
using Newtonsoft.Json;


namespace FormService.Implementation
{
    public class FormEditorService : IFormEditorService
    {
        private IFormRepository _formRepository;

        public FormEditorService(IFormRepository formRepository)
        {
            _formRepository = formRepository;
        }

        public int CreateForm(string schema, int formTypeId, int userId)
        {
            var form = new Form();
            form.CreatedBy = userId;
            form.Created = DateTime.Now;
            form.StatusId = (int)Variables.FormStatus.Draft;
            form.FormSchema = schema;
            form.FormTypeId = formTypeId;
            _formRepository.Add(form);
            _formRepository.Save();

            return form.Id;
        }

        public IEnumerable<Form> ListFormsOfType(int formTypeId)
        {
            return _formRepository.FindBy(x => x.FormTypeId == formTypeId);
        }

        public IEnumerable<ListItemStruct> ListFormsOfTypeSimple(int formTypeId)
        {
            return _formRepository.FindBy(x => x.FormTypeId == formTypeId).Select(y => new ListItemStruct{Id = y.Id, Name = y.Name});
        }

        public Form GetForm(int id)
        {
            return _formRepository.FindBy(x => x.Id == id).FirstOrDefault();
        }

        public ResultInfo SaveFormFields(FormInfo info)
        {
            var lstFormFields = new List<FormFieldJSon>();
            

            var dicFields = new Dictionary<int, FormFieldJSon>();
            var dicValidations = new Dictionary<int, List<FormFieldValidationRule>>();

            int identifier = 0;


            dynamic dynJson = JsonConvert.DeserializeObject(info.Schema);
            
            foreach (var item in dynJson.fields)
            {
                identifier++;

                var field =new FormFieldJSon
                {
                    FormId = info.Id,
                    Created = DateTime.Now,
                    CreatedBy = info.UserId,
                    Name = item.name.Value,
                    TypeName = item.type.Value,
                    DisplayName = (item.displayName != null)? item.displayName.Value : "",
                    PlaceHolderText = (item.placeholder != null)? item.placeholder.Value : "",
                    InitialValue = (item.value != null)? item.value.Value : "",
                    ToolTip = (item.tooltip != null)? item.tooltip.Value : ""
                };

                dicFields.Add(identifier, field);

                var validationMessages = new Dictionary<string, string>();
                var lstRules = new List<FormFieldValidationRule>();

                foreach (var val in item.validation)
                {
                    if (validationMessages.Count == 0)
                    {
                        foreach (var message in val.Value)
                        {
                            var temp = message.ToString().Split(':');
                            validationMessages.Add(temp[0], temp[1]);
                        }
                    }
                    else if (validationMessages.Count > 0)
                    {
                        var temp = val.ToString().Split(':');
                        var tempMessage = string.Empty;

                        if (validationMessages[temp[0]] != null)
                        {
                            tempMessage = validationMessages[temp[0]];
                        }

                        lstRules.Add(new FormFieldValidationRule
                        {
                            Created = DateTime.Now,
                            CreatedBy = info.UserId,
                            ErrorMessage = tempMessage,
                            SetValue = temp[1],
                            ValidationRuleId = GetValidationRule(temp[0].Trim('"')),//remove double quotes
                            FormId = info.Id
                        });
                    }
                }

                if (lstRules.Count > 0)
                {
                    dicValidations.Add(identifier, lstRules);
                    lstRules = new List<FormFieldValidationRule>();
                }                              
            }

           _formRepository.DeleteFormFields(info.Id);
           return _formRepository.SaveFields(dicFields, dicValidations);
        }

        private int GetValidationRule(string rulename)
        {
            switch (rulename)
            {
                case "required": return (int)Variables.FormValidationRules.Required;
                 case "minlength": return (int)Variables.FormValidationRules.TxtMinLen;
                 case "maxlength": return (int)Variables.FormValidationRules.TxtMaxLen; 
                 case "pattern": return (int)Variables.FormValidationRules.MatchPattern;
                 default: return -1;
            }
        }


        private List<FormFieldValidationRule> GetValidationRule(string validation, int userId)
        {
            var lstValidations = new List<FormFieldValidationRule>();
            lstValidations.Add(new FormFieldValidationRule{ Created = DateTime.Now, CreatedBy = userId, ValidationRuleId = 1});
            return lstValidations;
        }

        public ResultInfo SaveData(ContractFillViewInfo model)
        {
            dynamic dynJson = JsonConvert.DeserializeObject(model.Data);
            var lstFieldData = new List<UserContractFieldData>();

            //grab hold of all form fields for validation
            var lstFormFields = _formRepository.GetFormFields(model.FormId);

            foreach (var item in dynJson)
            {
                var tempArr = item.ToString().Split(':');

                if (tempArr != null && tempArr.length == 2)
                {
                    //find related field
                    var field = lstFormFields.FirstOrDefault(x => x.Name == tempArr[0].Trim());

                    //validate input agains field
                    var result = ValidateInputForField(field, tempArr[1]);

                    if (string.IsNullOrEmpty(result))
                    {
                        lstFieldData.Add(new UserContractFieldData
                        {
                            Contractid = model.ContractId,
                            Created = DateTime.Now,
                            CreatedBy = model.CreatedBy,
                            Data = tempArr[1],
                            FormId = model.FormId,
                            FieldId = field.id
                        });
                    }
                    else
                    {
                        return new ResultInfo { Itemid = -1, ErrorMessage = "" };
                    }
                }               
            }


            return new ResultInfo { Itemid =1, ErrorMessage="" };
        }

        private string ValidateInputForField(FormFieldJSon field, string input)
        {
            //define validation rules
            return "";
        }
    }
}
