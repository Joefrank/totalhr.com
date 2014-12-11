using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.Shared.Models
{
    public class AdminProfileStruct
    {
        public int UserId { get; set; }

        public List<ListItemStruct> UserProfiles { get; set; }

        public List<ListItemStruct> Allprofiles { get; set; }

        public IEnumerable<ListItemStruct> UserRoles { get; set; }

        public List<ListItemStruct> AllUsers { get; set; }
    }

    
}
