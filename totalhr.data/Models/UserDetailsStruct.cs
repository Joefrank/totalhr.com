using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;

namespace totalhr.data.Models
{
    public class UserDetailsStruct
    {
        public User UserBasicDetails { get; set; }

        public List<Profile> UserProfiles { get; set; }

        public List<Role> UserRoles { get; set; }

        public bool IsValid()
        {
            return (UserBasicDetails != null && UserProfiles != null && UserRoles != null);
        }
    }
}
