using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using totalhr.data.EF;
using totalhr.Shared.Models;

namespace totalhr.web.Areas.Admin.Models
{
    public class UserSearchResult
    {
        public UserSearchInfo SearchInfo { get; set; }

        public IEnumerable<SearchUser_Result> FoundUsers { get; set; }
    }
}