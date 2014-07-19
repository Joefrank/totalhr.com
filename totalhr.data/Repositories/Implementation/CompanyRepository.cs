using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;

namespace totalhr.data.Repositories.Implementation
{
    public class CompanyRepository : GenericRepository<TotalHREntities, Company>, ICompanyRepository
    {
        public List<Department> GetCompanyDepartments(int companyid)
        {
            return this.Context.Departments.Where(x => x.CompanyId == companyid).ToList();
        }
    }
}
