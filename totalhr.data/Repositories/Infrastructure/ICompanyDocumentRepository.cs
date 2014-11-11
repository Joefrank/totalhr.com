using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;

namespace totalhr.data.Repositories.Infrastructure
{
    public interface ICompanyDocumentRepository : IGenericRepository<totalhr.data.EF.CompanyDocument>
    {
        List<CompanyFolder> ListFoldersByUser(int userId, int userDepartmentId);

        List<CompanyDocument> ListDocumentAndFoldersByUser(int userId, int departmentId);
    }
}
