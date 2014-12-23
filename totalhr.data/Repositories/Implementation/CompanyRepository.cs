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
    public class CompanyRepository : GenericRepository<TotalHREntities, Company>, ICompanyRepository
    {
        public List<Department> GetCompanyDepartments(int companyid)
        {
            return this.Context.Departments.Where(x => x.CompanyId == companyid).ToList();
        }

        public List<string> GetCompanyDepartmentsByIds(List<int> ids)
        {
            return this.Context.Departments.Where(x => ids.Contains(x.id)).
               Select(y => y.Name).ToList();
        }

        public IEnumerable<ListItemStruct> GetDeparmentSimple(int companyId)
        {
            return this.Context.Departments.Where(x => x.CompanyId == companyId).
                Select(y => new ListItemStruct{ Id = y.id, Name = y.Name});
        }

        public int CreateDepartment(int companyId, int userId, string departmentName, string description)
        {
            Department dept = new Department();
            dept.Name = departmentName;
            dept.Description = description;
            dept.CreatedBy = userId;
            dept.CompanyId = companyId;
            dept.Created = DateTime.Now;

            this.Context.Departments.Add(dept);
            this.Context.SaveChanges();
            return dept.id;
        }
    }
}
