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

        public IEnumerable<ContractTemplate> ListContractTemplates(SortingInfo info)
        {
            return _templateRepos.ListTemplateWithSorting(info);
        }

        public IEnumerable<ListItemStruct> ListContractTemplates()
        {
            return _templateRepos.ListTemplatesSimple();
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

        public ContractTemplate GetTemplate(int id)
        {
            return _templateRepos.FindBy(x => x.id == id).FirstOrDefault();
        }

        public void UpdateContractTemplate(TemplateInfo info)
        {
            ContractTemplate template = _templateRepos.FindBy(x => x.id == info.Id).FirstOrDefault();
            if (template != null)
            {
                template.Name = info.Name;
                template.Description = info.Description;
                template.FormId = info.FormId;
                template.Lastupdated = DateTime.Now;
                template.LastUpdatedBy = info.LastUpdatedBy;

                _templateRepos.Save();
            }
        }

        public UserContract GetUserContract(int userId)
        {
           return _contractRepos.FindBy(x => x.Userid == userId).FirstOrDefault();
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
