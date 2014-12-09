using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.Shared.Models;

namespace totalhr.data.Repositories.Infrastructure
{
    public interface IUserRepository : IGenericRepository<User>
    {
        List<Profile> GetUserProfiles(int UserId);

        List<Role> GetUserRoles(int UserId);

        List<User> GetCompanyUsers(int companyid);

        IEnumerable<SimpleUser> GetCompanyUsers(int companyid, int excludedUserId);

        List<string> GetUserNamesByIds(List<int> ids);

        List<Profile> GetUserProfile(int userId);
    }
}
