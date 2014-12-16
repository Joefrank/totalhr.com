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

        public IEnumerable<ListItemStruct> UserProfiles { get; set; }

        public IEnumerable<ListItemStruct> Allprofiles { get; set; }

    }

    
}
