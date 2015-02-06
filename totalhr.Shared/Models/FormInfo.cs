using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.Shared.Models
{
    public class FormInfo
    {
        public int Id { get; set; }

        public int FormTypeId { get; set; }

        public string Schema { get; set; }

        public int UserId { get; set; }
    }
}
