using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.Shared.Models;

namespace totalhr.data.Repositories.Infrastructure
{
    public interface IProfileRepository : IGenericRepository<Profile>
    {
        IEnumerable<User> ListUsersByProfile(int profileId, int currentUserId);

        IEnumerable<ListItemStruct> ListProfilesForSimple();

        IEnumerable<ListItemStruct> ListProfilesForSimple(int userId);

        IEnumerable<ListItemStruct> GetUserPermissions(int userId);
    }
}
