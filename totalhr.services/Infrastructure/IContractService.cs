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

        IEnumerable<ContractTemplate> ListContractTemplates();

        FormInfo GetDefaultTemplate(int languageId);

        int CreateContractTemplate(TemplateInfo info);
    }
}
