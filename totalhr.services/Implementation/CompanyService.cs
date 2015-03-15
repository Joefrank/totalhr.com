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

        /*** Please implement properly */
        public CompanyOrganigram GetOrganigram(int companyId)
        {
            var organigram = new CompanyOrganigram();

            organigram.EmployeeHierarchy = @"{ ""class"": ""go.TreeModel"",
                  ""nodeDataArray"": [
                {""key"":""1"", ""name"":""Stella Payne Diaz"", ""title"":""CEO""},
                {""key"":""2"", ""name"":""Luke Warm"", ""title"":""VP Marketing/Sales"", ""parent"":""1""},
                {""key"":""3"", ""name"":""Meg Meehan Hoffa"", ""title"":""Sales"", ""parent"":""2""},
                {""key"":""4"", ""name"":""Peggy Flaming"", ""title"":""VP Engineering"", ""parent"":""1""},
                {""key"":""5"", ""name"":""Saul Wellingood"", ""title"":""Manufacturing"", ""parent"":""4""},
                {""key"":""6"", ""name"":""Al Ligori"", ""title"":""Marketing"", ""parent"":""2""},
                {""key"":""7"", ""name"":""Dot Stubadd"", ""title"":""Sales Rep"", ""parent"":""3""},
                {""key"":""8"", ""name"":""Les Ismore"", ""title"":""Project Mgr"", ""parent"":""5""},
                {""key"":""9"", ""name"":""April Lynn Parris"", ""title"":""Events Mgr"", ""parent"":""6""},
                {""key"":""10"", ""name"":""Xavier Breath"", ""title"":""Engineering"", ""parent"":""4""},
                {""key"":""11"", ""name"":""Anita Hammer"", ""title"":""Process"", ""parent"":""5""},
                {""key"":""12"", ""name"":""Billy Aiken"", ""title"":""Software"", ""parent"":""10""},
                {""key"":""13"", ""name"":""Stan Wellback"", ""title"":""Testing"", ""parent"":""10""},
                {""key"":""14"", ""name"":""Marge Innovera"", ""title"":""Hardware"", ""parent"":""10""},
                {""key"":""15"", ""name"":""Evan Elpus"", ""title"":""Quality"", ""parent"":""5""},
                {""key"":""16"", ""name"":""Lotta B. Essen"", ""title"":""Sales Rep"", ""parent"":""3""}
                 ]
                }";
            return organigram;
        }

        public IEnumerable<User> ListEmployees(int companyId)
        {
            return _companyRepos.ListEmployees(companyId);
        }
    }
}
