using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace totalhr.web.Areas.Admin.Models
{
    public class BreadCrumbItem
    {
        public string Url { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }
    }
}