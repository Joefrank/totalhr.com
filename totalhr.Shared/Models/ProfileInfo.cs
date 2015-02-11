using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.Shared.Models
{
    public class ProfileInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CreatedBy { get; set; }

        public int Created { get; set; }

        public string Identifier { get; set; }

        public bool NewlyCreated { get; set; }
    }
}
