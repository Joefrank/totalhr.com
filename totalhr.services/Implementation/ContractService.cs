using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Resources;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;
using totalhr.services.Infrastructure;
using totalhr.Shared.Models;

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

        public int CreateContractTemplate(TemplateInfo info)
        {
            var contract = new ContractTemplate
                {
                   FormId =  info.FormId,
                   Name = info.Name,
                   Description = info.Description,
                   Created = DateTime.Now,
                   CreatedBy = info.CreatedBy
                };
            return _contractRepos.AddTemplate(contract);
        }

        public FormInfo GetDefaultTemplate(int languageId)
        {
            return new FormInfo
            {
                Schema = @" {
                  ""fields"": [
                    {
                      ""type"": ""text"",
                      ""name"": ""startDate"",
                      ""displayName"": ""Start Date"",
                      ""validation"": {
                        ""messages"": {},
                        ""required"": true
                      },
                      ""placeholder"": ""Enter employee start date here"",
                      ""tooltip"": ""Enter employee start date here""
                    },
                    {
                      ""type"": ""text"",
                      ""name"": ""jobType"",
                      ""displayName"": ""Job Type"",
                      ""validation"": {
                        ""messages"": {},
                        ""required"": true
                      },
                      ""placeholder"": ""Enter employee job type here"",
                      ""tooltip"": ""Enter employee job type here""
                    }
                    ]
                }"
            };

        }
    }
}
