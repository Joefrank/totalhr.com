using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.Shared.Models;

namespace totalhr.services.Infrastructure
{
    public interface IRoleService
    {
        IEnumerable<Role> GetRoleList();

        IEnumerable<ListItemStruct> GetRoleListForListing();

        IEnumerable<ListItemStruct> GetRoleListAgainstUserForListing(int userId);

        IEnumerable<User> ListUsersByRole(int roleId, int currentUserId);

        Role GetRole(int id);
    }
}
