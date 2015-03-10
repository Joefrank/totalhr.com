using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.Shared.Models;

namespace totalhr.services.Infrastructure
{
    public interface IProfileService
    {
        IEnumerable<Profile> GetProfileList();

        IEnumerable<ListItemStruct> GetProfileListForListing();

        IEnumerable<ListItemStruct> GetProfileListAgainstUserForListing(int userId);

        IEnumerable<User> ListUsers(int profileId, int currentUserId);

        Profile GetProfile(int id);

        ResultInfo CreateProfile(ProfileInfo info);

        IEnumerable<ListItemStruct> GetUserProfiles(int userId);
    }
}
