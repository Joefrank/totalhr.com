using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;

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
    }
}
