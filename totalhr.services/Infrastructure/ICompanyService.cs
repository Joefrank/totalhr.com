using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Models;
using totalhr.Shared;
using totalhr.Shared.Models;

namespace totalhr.services.Infrastructure
{
    public interface ICompanyService
    {

        List<Department> GetCompanyDepartments(int companyid);

        List<string> GetCompanyDepartmentsByIds(List<int> ids);
      
    }
}
