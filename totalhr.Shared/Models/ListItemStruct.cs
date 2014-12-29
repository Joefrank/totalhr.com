using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.Shared.Models
{
    public class ListItemStruct
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ListItemStructExtended : ListItemStruct
    {
        public bool Assigned { get; set; }
    }
}
