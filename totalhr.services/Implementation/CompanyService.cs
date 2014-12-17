using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.services.Infrastructure;
using totalhr.Shared;
using totalhr.Shared.Models;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;
using totalhr.data.Repositories.Implementation;
using Ninject;
using totalhr.services.messaging.Infrastructure;
using CM;
using log4net;
using totalhr.data.Models;

namespace totalhr.services.Implementation
{
    public class CompanyService : ICompanyService
    {       
        private ICompanyRepository _companyRepos;
        private static readonly ILog log = LogManager.GetLogger(typeof(AccountService));

        public CompanyService(ICompanyRepository companyRepos)
        {             
            _companyRepos = companyRepos;
        }      

        public List<Department> GetCompanyDepartments(int companyid)
        {
            return _companyRepos.GetCompanyDepartments(companyid);
        }

        public List<string> GetCompanyDepartmentsByIds(List<int> ids)
        {
            return _companyRepos.GetCompanyDepartmentsByIds(ids);
        }

        public IEnumerable<ListItemStruct> GetDepartmentSimple(int companyId)
        {
            return _companyRepos.GetDeparmentSimple(companyId);
        }
    }
}
