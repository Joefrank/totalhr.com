using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;
using totalhr.services.Infrastructure;

namespace totalhr.services.Implementation
{
    public class ContractService : IContractService
    {
        private IContractRepository _contractRepos;
        private IContractTemplateRepository _templateRepos;

        public ContractService(IContractRepository contractRepos, IContractTemplateRepository templateRepos)
        {
            this._contractRepos = contractRepos;
            this._templateRepos = templateRepos;
        }

        public IEnumerable<UserContract> ListContracts()
        {
            return _contractRepos.GetAll();
        }

        public IEnumerable<ContractTemplate> ListContractTemplates()
        {
            return _templateRepos.GetAll();
        }
    }
}
