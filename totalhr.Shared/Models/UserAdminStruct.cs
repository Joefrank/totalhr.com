using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.Shared.Models
{
    public class UserAdminStruct
    {
        public bool DetailsSaveSuccess { get; set; }
        
        public UserPersonalInfo PersonalInfo { get; set; }

        public IEnumerable<ListItemStruct> UserProfiles { get; set; }

        public IEnumerable<ListItemStruct> UserRoles { get; set; }

        public IEnumerable<ListItemStruct> UserCalendars { get; set; }

        public IEnumerable<ListItemStruct> UserDocuments { get; set; }
    }
}
