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
using totalhr.core;


namespace FormService.Implementation
{
    public class FormEditorService : IFormEditorService
    {
        private readonly IFormRepository _formRepository;

        public FormEditorService(IFormRepository formRepository)
        {
            _formRepository = formRepository;
        }

        public int CreateForm(string schema, string name, int formTypeId, int userId)
        {
            var form = new Form
                {
                    CreatedBy = userId,
                    Created = DateTime.Now,
                    StatusId = (int) Variables.FormStatus.Draft,
                    FormSchema = schema,
                    FormTypeId = formTypeId,
                    Name = name
                };
            _formRepository.Add(form);
            _formRepository.Save();

            return form.Id;
        }

        public int CreateForm(FormInfo info)
        {
            var form = new Form
                {
                    CreatedBy = info.UserId,
                    Created = DateTime.Now,
                    StatusId = (int) Variables.FormStatus.Draft,
                    FormSchema = info.Schema,
                    FormTypeId = info.FormTypeId,
                    Name = info.FormName
                };
            _formRepository.Add(form);
            _formRepository.Save();

            if (form.Id > 0)
            {
                info.Id = form.Id;
                this.SaveFormFields(info);
            }

            return form.Id;
        }
        /// <summary>
        /// When updating a form, we need to check its status. Only update draft forms. 
        /// if form is published, then we need to create a new version of form instead
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int UpdateForm(FormInfo info)
        {
            var form = _formRepository.FindBy(x => x.Id == info.Id).FirstOrDefault();
            if (form != null)
            {
                form.LastUpdatedBy = info.UserId;
                form.LastUpdated = DateTime.Now;
                form.StatusId = (int)Variables.FormStatus.Draft;
                form.FormSchema = info.Schema;
                form.Name = info.FormName;

                _formRepository.Save();

                this.SaveFormFields(info);

                return form.Id;
            }

            return -1;
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

                foreach (var valItem in item.validation)
                {
                    //check first if it is a collection
                    string valItemName = valItem.Name.ToString().Trim('"');
                    var valItemValue = valItem.Value;

                    if (valItemName.Trim().Equals("messages"))
                    {
                        foreach (var message in valItemValue)
                        {
                            var temp = message.ToString().Split(':');
                            validationMessages.Add(temp[0].Trim('"'), temp[1]);
                        }
                    }
                    else
                    {
                        var tempMessage = "";

                        if (validationMessages.Keys.Any() && validationMessages.Keys.Contains(valItemName))
                        {
                            tempMessage = validationMessages[valItemName];
                        }

                        lstRules.Add(new FormFieldValidationRule
                        {
                            Created = DateTime.Now,
                            CreatedBy = info.UserId,
                            ErrorMessage = tempMessage,
                            SetValue = valItemValue,
                            ValidationRuleId = GetValidationRule(valItemName),//remove double quotes
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

        public static int GetValidationRule(string rulename)
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

        public ResultInfo SaveFieldData(ContractFillViewInfo model)
        {
            dynamic dynJson = JsonConvert.DeserializeObject(model.Data);
            var lstFieldData = new List<UserContractFieldData>();

            //grab hold of all form fields for validation
            var lstFormFields = _formRepository.GetFormFields(model.FormId);

            foreach (var item in dynJson)
            {
                string[] tempArr = item.ToString().Split(':');
               
               
                if (tempArr == null || tempArr.Count() != 2)
                {
                    throw new Exception("Failed to save data. Error occured!");
                }
                else
                {
                    
                    var key = tempArr[0].Replace("\"", string.Empty).Trim();
                    var value = tempArr[1].Replace("\"", string.Empty).Trim();

                    //find related field
                    var field = lstFormFields.FirstOrDefault(x => x.Name == key);

                    if (field != null)
                    {
                        //validate input agains field
                        var result = ValidateInputForField(field, value);

                        if (result.StateIsValid)
                        {
                            lstFieldData.Add(new UserContractFieldData
                                {
                                    Contractid = model.ContractId,
                                    Created = DateTime.Now,
                                    CreatedBy = model.CreatedBy,
                                    Data = value,
                                    FormId = model.FormId,
                                    FieldId = field.id
                                });
                        }
                        else //if there is any error, let's stop validation and return
                        {
                            return new ResultInfo {Itemid = -1, ErrorMessage = result.ErrorMessage};
                        }
                    }
                    else
                    {
                        return new ResultInfo
                            {
                                Itemid = -1,
                                ErrorMessage =
                                    string.Format("Field '{0}' is null in form {1}", key, model.FormId)
                            };
                    }
                }
            }

            //if we get here, then validation has passed
            //remove existing fields first before putting new ones.
            _formRepository.DeleteUserContractFieldData(model.ContractId);
            var saveResult = _formRepository.SaveUserContractFieldData(lstFieldData);

            return new ResultInfo { Itemid = saveResult, ErrorMessage = (saveResult > 0)? "" : "Failed to save Field data." };
        }

        private ValidationResult ValidateInputForField(FormFieldJSon field, string input)
        {
            //define validation rules
            var lstRules = field.FormFieldValidationRules;
            FormInputValidation validator = null;

            foreach (var rule in lstRules)
            {
                switch (rule.ValidationRuleId)
                {
                    case (int)Variables.FormValidationRules.Required:
                        validator = new RequiredFormInputValidator();
                        break;
                    case (int)Variables.FormValidationRules.TxtMinLen:
                        validator = new MinLengthFormInputValidator(Convert.ToInt32(rule.SetValue));
                        break;
                    case (int)Variables.FormValidationRules.TxtMaxLen:
                        validator = new MaxLengthFormInputValidator(Convert.ToInt32(rule.SetValue));
                        break;
                    case (int)Variables.FormValidationRules.MatchPattern:
                        validator = new PatternFormInputValidator(rule.SetValue);
                        break;
                    default: validator = null; break;
                }

                if (validator == null)
                {
                    return new ValidationResult { StateIsValid = false, ErrorMessage = "No validator found for rule {0}" };
                }
                else
                {
                    var result = validator.Validate(input, field.Name);
                    if (!result.StateIsValid)
                    {
                        return new ValidationResult
                        {
                            ErrorMessage = string.Format("Validation Failed for field {0} on rule {1}", field.Name, rule.ValidationRuleId),
                            StateIsValid = false
                        };
                    }
                }
            }

            return new ValidationResult { StateIsValid = true };
        }
    }
}
