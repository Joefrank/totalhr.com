using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.Shared.Models;

namespace totalhr.data.Repositories.Infrastructure
{
    public interface IContractRepository : IGenericRepository<UserContract>
    {
        int AddTemplate(ContractTemplate template);

        UserContractData SaveContractData(ContractFillViewInfo model);

        IEnumerable<GetUserContractDetails_Result> GetUserContractDetails(int userId, int? contractId = null);

        void SaveContractFieldData(List<UserContractFieldData> lstFieldData);

        IEnumerable<EmployeeContractModel.FieldData> GetEmployeeContractDisplay(int employeeId);
    }
}
