using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;
using totalhr.Shared.Models;

namespace totalhr.data.Repositories.Implementation
{
    public class RoleRepository : GenericRepository<TotalHREntities, Role>, IRoleRepository
    {
        public IEnumerable<User> ListUsersByRole(int roleId, int currentUserId)
        {
            return from user in Context.Users
                        join uRole in Context.UserRoles
                            on user.id equals uRole.UserId
                        where uRole.RoleId == roleId
                        select user;
        }

        public IEnumerable<ListItemStruct> ListRolesForSimple()
        {
            return from role in Context.Roles
                   select new ListItemStruct() { Id = role.id, Name = role.Name };
        }

        public IEnumerable<ListItemStruct> ListRolesForSimple(int userId)
        {
            return from r in
                       Context.Roles.Where(x =>
                           !(
                               from ur in Context.UserRoles
                               where ur.UserId == userId
                               select ur.RoleId
                               ).Contains(x.id)
                           )
                   select new ListItemStruct { Id = r.id, Name = r.Name };
        }
    }
}
