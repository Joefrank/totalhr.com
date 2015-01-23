using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;
using totalhr.Shared;
using totalhr.Shared.Models;

namespace totalhr.data.Repositories.Implementation
{
    public class ContractRepository : GenericRepository<TotalHREntities, UserContract>, IContractRepository
    {
        public int AddTemplate(ContractTemplate template)
        {
            Context.ContractTemplates.Add(template);
            return Context.SaveChanges();
        }

        public UserContractData SaveContractData(ContractFillViewInfo model)
        {
            var data = new UserContractData
                {
                   UserId = model.UserId,
                   ContractId = model.ContractId,
                   Created = DateTime.Now,
                   CreatedBy = model.CreatedBy,
                   Data = model.Data,
                   StatusId = (int)Variables.UserContractDataStatus.New
                };

            Context.UserContractDatas.Add(data);
            Context.SaveChanges();

            return data;
        }
    }
}
