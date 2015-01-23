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
    public class ContractTemplateRepository : GenericRepository<TotalHREntities, ContractTemplate>, IContractTemplateRepository
    {

        public IQueryable<ContractTemplate> ListTemplateWithSorting(SortingInfo info)
        {
            return (info.SortOrder == "asc") ? Context.ContractTemplates.OrderBy(x => x.id) :
               Context.ContractTemplates.OrderByDescending(x => x.id);
        }

        public IEnumerable<ListItemStruct> ListTemplatesSimple()
        {
            return Context.ContractTemplates.Select(x => new ListItemStruct { Id = x.id, Name = x.Name });
        }

        public Form GetTemplateForm(int templateId)
        {
            var form = from f in Context.Forms
                       join t in Context.ContractTemplates on templateId equals t.id
                       where t.FormId == f.Id
                       select f;

            return form.FirstOrDefault();
        }
    }
}
