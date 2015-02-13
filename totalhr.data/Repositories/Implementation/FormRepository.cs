using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;
using totalhr.Shared.Models;

namespace totalhr.data.Repositories.Implementation
{
    public class FormRepository : GenericRepository<TotalHREntities, Form>, IFormRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FormRepository));

        public void AddFields(List<EF.FormFieldJSon> lstFormFields)
        {
            lstFormFields.ForEach(x => Context.FormFieldJSons.Add(x));                      
            Context.SaveChanges();
        }

        public ResultInfo SaveFields(Dictionary<int, EF.FormFieldJSon> dicFields, Dictionary<int, List<EF.FormFieldValidationRule>> dicValidations)
        {
            try
            {
                foreach (var key in dicFields.Keys)
                {
                    var field = dicFields[key];
                    Context.FormFieldJSons.Add(field);
                    Context.SaveChanges();

                    if (field.id > 0 && dicValidations.Keys.Contains(key))
                    {
                        var lstValidations = dicValidations[key];
                        foreach (var rule in lstValidations)
                        {
                            rule.FormFieldId = field.id;
                            Context.FormFieldValidationRules.Add(rule);
                        }

                        Context.SaveChanges();
                    }
                }

                return new ResultInfo { ErrorMessage = "", Itemid = 1 };
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
                return new ResultInfo { ErrorMessage = "Error saving fields", Itemid = -1 };
            }
        }

        public void DeleteFormFields(int formId)
        {
            this.Context.FormFieldJSons.RemoveRange(Context.FormFieldJSons.Where(x => x.FormId == formId));
            this.Context.FormFieldValidationRules.RemoveRange(Context.FormFieldValidationRules.Where(x => x.FormId == formId));
        }

        public List<EF.FormFieldJSon> GetFormFields(int formId)
        {
            return Context.FormFieldJSons.Where(x => x.FormId == formId).ToList();
        }
    }
}
