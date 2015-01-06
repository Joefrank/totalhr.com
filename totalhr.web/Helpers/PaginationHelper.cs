using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using totalhr.web.Areas.Admin.Models;
using totalhr.core;

namespace totalhr.web.Helpers
{
    public class PaginationHelper
    {
        public static Pagination BuildPagination(Pagination pagination, BuildPaginationUrl urlHandler)
        {
            pagination.NumberOfPages = (int)(pagination.TotalNoOfItems / pagination.PageSize) + (pagination.TotalNoOfItems % pagination.PageSize > 0 ? 1 : 0);

            for (int x = 1; x <= pagination.NumberOfPages; x++)
            {
                pagination.AllPages.Add(new Pagination.Page { 
                    PageNumber = x, 
                    Link = urlHandler(pagination.MainLink, x), 
                    Selected = x == (pagination.CurrentPage) 
                });
            }

            pagination.PrevLink = (pagination.CurrentPage > 1) ? urlHandler(pagination.MainLink, pagination.CurrentPage - 1) : "";
            pagination.NextLink = (pagination.CurrentPage < pagination.NumberOfPages) ? urlHandler(pagination.MainLink, pagination.CurrentPage + 1) : "";

            return pagination;
        }

        public static string BuildPageButton(string label, string classCSS, string callbackJS)
        {
            var sClass = string.Format(@" class=""{0}"" ", classCSS);

            return string.Format(@"<span {0} {1}>{2}</span>", (!string.IsNullOrEmpty(classCSS) ? sClass : ""),
                @" onclick=""" + callbackJS + @""" ", label);
        }
        
    }
    
}