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
    public interface IAccountService
    {
        Company CreateCompany(NewUserInfo info);

        User CreateUser(NewUserInfo info);

        string GenerateUserActivationLink(string email);

        bool UserExistByEmail(string email);

        UserRegStruct RegisterUserCompany(NewUserInfo info, AdminStruct adminstruct);

        void ClearUserDataByEmail(string email);

        User GetUser(string UserName, string Password);

        User GetUserByEmail(string email);

        User GetUserByUsername(string username);

        User GetActiveUser(string UserName, string Password);

        User GetUserByGuid(string guid);

        User ActivateUser(string email);

        UserDetailsStruct GetUserDetailsForLogin(string UserName, string Password);

        List<User> GetCompanyUsers(int companyid);

        List<Department> GetCompanyDepartments(int companyid);

        UserPersonalInfo GetUserInfoByEmail(string email);

        int UpdateUserDetails(UserPersonalInfo info);

        List<string> GetUserNamesByIds(List<int> ids);
    }
}
