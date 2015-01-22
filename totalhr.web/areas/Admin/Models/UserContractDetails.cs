using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using totalhr.data.EF;
using totalhr.Shared.Models;

namespace totalhr.web.Areas.Admin.Models
{
    public class UserContractDetails
    {
        public User UserDetails { get; set; }

        public UserContract Contract { get; set; }

        public IEnumerable<ListItemStruct> TemplateList { get; set; }
    }
}