using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;
using totalhr.services.Infrastructure;

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
    }
}
