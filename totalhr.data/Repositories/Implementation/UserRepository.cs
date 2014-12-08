using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;
using totalhr.Shared.Models;

namespace totalhr.data.Repositories.Implementation
{
    public class UserRepository : GenericRepository<TotalHREntities, User>, IUserRepository
    {
        public List<Profile> GetUserProfiles(int UserId)
        {
            return this.Context.Profiles.SqlQuery("dbo.GetUserProfiles @p0", UserId).ToList(); 
        }

        public List<Role> GetUserRoles(int UserId)
        {
            return this.Context.Roles.SqlQuery("dbo.GetUserRoles @p0", UserId).ToList(); 
        }

        public List<User> GetCompanyUsers(int companyid)
        {
            return this.FindBy(x => x.CompanyId == companyid).ToList();
        }

        public IEnumerable<SimpleUser> GetCompanyUsers(int companyid, int excludedUserId)
        {
            var qry = from user in this.Context.Users
                      where user.CompanyId == companyid && user.id != excludedUserId
                      orderby user.firstname
                      select new SimpleUser { Id = user.id, FullName = user.firstname + " " + user.surname };
            return qry;
        }

        public List<string> GetUserNamesByIds(List<int> ids)
        {
            return this.Context.Users.Where(x => ids.Contains(x.id)).
                Select(y => y.firstname + " " + y.surname).ToList();
        }
    }
}
