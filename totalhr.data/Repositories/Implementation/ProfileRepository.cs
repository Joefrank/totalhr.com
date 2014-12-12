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
    public class ProfileRepository : GenericRepository<TotalHREntities, Profile>, IProfileRepository
    {
        public IEnumerable<User> ListUsersByProfile(int profileId, int currentUserId)
        {
            var users = from user in Context.Users
                        join uprofile in Context.UserProfiles  
                            on user.id equals uprofile.UserId
                        where uprofile.ProfileId == profileId
                        select user;

            return users;
            
        }

        public IEnumerable<ListItemStruct> ListProfilesForSimple()
        {
            return from profile in Context.Profiles
                        select new ListItemStruct() {Id = profile.id, Name = profile.Name };

        }

        public IEnumerable<ListItemStruct> ListProfilesForSimple(int userId)
        {
            return from profile in Context.Profiles
                   join uprofile in Context.UserProfiles on profile.id equals uprofile.ProfileId into mylist
                   from lst in mylist.DefaultIfEmpty()
                   where lst.UserId != userId && lst.UserId == null
                   select new ListItemStruct{ Id = profile.id, Name = profile.Name};
                  
        }
    }
}
