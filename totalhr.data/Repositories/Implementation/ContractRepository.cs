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
    public class ContractRepository : GenericRepository<TotalHREntities, UserContract>, IContractRepository
    {
        public int AddTemplate(ContractTemplate template)
        {
            Context.ContractTemplates.Add(template);
            return Context.SaveChanges();
        }

        
    }
}
