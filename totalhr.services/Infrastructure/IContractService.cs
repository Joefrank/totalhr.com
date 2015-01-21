using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using totalhr.data.EF;
using totalhr.Shared.Models;

namespace totalhr.services.Infrastructure
{
    public interface IContractService
    {
        IEnumerable<UserContract> ListContracts();

        IEnumerable<ContractTemplate> ListContractTemplates(SortingInfo info);

        FormInfo GetDefaultTemplate(int languageId);

        int CreateContractTemplate(TemplateInfo info);

        ContractTemplate GetTemplate(int id);

        void UpdateContractTemplate(TemplateInfo info);
    }
}
