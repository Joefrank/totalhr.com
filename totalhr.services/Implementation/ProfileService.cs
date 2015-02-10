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
    public class ProfileService : IProfileService
    {
        private IProfileRepository _profileRepos;

        public ProfileService(IProfileRepository profileRepos)
        {
            _profileRepos = profileRepos;
        }

        public IEnumerable<Profile> GetProfileList()
        {
            return _profileRepos.GetAll();
        }

        public IEnumerable<ListItemStruct> GetProfileListForListing()
        {
            return _profileRepos.ListProfilesForSimple();
        }

        public IEnumerable<ListItemStruct> GetProfileListAgainstUserForListing(int userId)
        {
            return _profileRepos.ListProfilesForSimple(userId);
        }

        public IEnumerable<User> ListUsers(int profileId, int currentUserId)
        {
            return _profileRepos.ListUsersByProfile(profileId, currentUserId);
        }

        public Profile GetProfile(int id)
        {
            return _profileRepos.FindBy(x => x.id == id).FirstOrDefault();
        }

        public ResultInfo CreateProfile(ProfileInfo info)
        {
            var profile = new Profile();
            profile.Name = info.Name;
            profile.Description = info.Description;
            profile.Created = DateTime.Now;
            profile.CreatedBy = info.CreatedBy;
            profile.Identifier = info.Identifier;

            _profileRepos.Add(profile);
            _profileRepos.Save();

            return new ResultInfo { Itemid = profile.id, ErrorMessage = "" };
        }
    }
}
