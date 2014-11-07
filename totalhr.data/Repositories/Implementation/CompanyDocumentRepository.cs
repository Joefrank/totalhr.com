using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.Shared;
using totalhr.data.Repositories.Infrastructure;

namespace totalhr.data.Repositories.Implementation
{
    public class CompanyDocumentRepository : GenericRepository<TotalHREntities, CompanyDocument>, ICompanyDocumentRepository
    {
        public List<CompanyFolder> ListFoldersByUser(int userId, int userDepartmentId)
        {

            var result =  Context.GetCompanyFoldersByUser(userId, userDepartmentId);
            return result.Select(x => x) as List<CompanyFolder>;
            
            
        }
    }
}
