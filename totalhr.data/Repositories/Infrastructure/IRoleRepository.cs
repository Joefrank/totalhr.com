using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.Shared.Models;

namespace totalhr.data.Repositories.Infrastructure
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        IEnumerable<User> ListUsersByRole(int roleId, int currentUserId);

        IEnumerable<ListItemStruct> ListRolesForSimple();

        IEnumerable<ListItemStruct> ListRolesForSimple(int userId);
    }
}
