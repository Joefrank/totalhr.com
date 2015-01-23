using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using totalhr.data.EF;

namespace totalhr.web.Areas.Admin.Models
{
    public class UserContractFormDetails
    {
        public UserContract Contract { get; set; }

        public User UserDetails { get; set; }

        public Form Form { get; set; }
    }
}