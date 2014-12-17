using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.Shared.Models
{
    public class AdminRoleStruct
    {
        public int UserId { get; set; }

        public IEnumerable<ListItemStruct> UserRoles { get; set; }

        public IEnumerable<ListItemStruct> AllRoles { get; set; }

    }
}
