using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.Shared.Models
{
    public class UserRegStruct
    {
        public int UserId { get; set; }

        public int CompanyId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string ActivationLink { get; set; }

        public string RegError { get; set; }
    }
}
