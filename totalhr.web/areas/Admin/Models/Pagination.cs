using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace totalhr.web.Areas.Admin.Models
{
    public class Pagination
    {
        public Pagination()
        {
            AllPages = new List<Page>();
        }
        
        public int CurrentPage { get; set; }
        public int TotalNoOfItems { get; set; }
        public int NumberOfPages { get; set; }
        public int PageSize { get; set; }

        public string MainLink { get; set; }
        public string PrevLink { get; set; }
        public string NextLink { get; set; }

        public List<Page> AllPages { get; set; }

        public class Page
        {
            public int PageNumber { get; set; }
            public string Link { get; set; }
            public bool Selected { get; set; }
        }
    }
}