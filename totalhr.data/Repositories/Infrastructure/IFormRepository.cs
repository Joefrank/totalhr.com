using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Shared.Models;
using totalhr.data.EF;

namespace totalhr.data.Repositories.Infrastructure
{
    public interface IFormRepository : IGenericRepository<EF.Form>
    {
        void AddFields(List<EF.FormFieldJSon> lstFormFields);

        ResultInfo SaveFields(Dictionary<int, EF.FormFieldJSon> dicFields, Dictionary<int, List<EF.FormFieldValidationRule>> dicValidations);

        void DeleteFormFields(int formId);

        List<EF.FormFieldJSon> GetFormFields(int formId);

        int SaveUserContractFieldData(List<UserContractFieldData> lstFieldData);
    }
}
