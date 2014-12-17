using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;
using totalhr.services.Infrastructure;
using totalhr.Shared.Models;

namespace totalhr.services.Implementation
{
    public class RoleService : IRoleService
    {
        private IRoleRepository _roleRepos;

        public RoleService(IRoleRepository roleRepos)
        {
            _roleRepos = roleRepos;
        }
        
        public IEnumerable<Role> GetRoleList()
        {
            return _roleRepos.GetAll();
        }

        public IEnumerable<ListItemStruct> GetRoleListForListing()
        {
            return _roleRepos.ListRolesForSimple();
        }

        public IEnumerable<ListItemStruct> GetRoleListAgainstUserForListing(int userId)
        {
            return _roleRepos.ListRolesForSimple(userId);
        }

        public IEnumerable<User> ListUsersByRole(int roleId, int currentUserId)
        {
            return _roleRepos.ListUsersByRole(roleId, currentUserId);
        }

        public Role GetRole(int id)
        {
            return _roleRepos.FindBy(x => x.id == id).FirstOrDefault();
        }
    }
}
