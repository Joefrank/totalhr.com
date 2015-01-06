using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.core
{
    public delegate string BuildPaginationItem(string url, string label, string classCSS, string callbackJS);

    public class PaginationItemBuilder
    {
        public static string BuildPageButton(string url, string label, string classCSS, string callbackJS)
        {
            var sClass = string.Format(@" class=""{0}"" ", classCSS);

            return string.Format(@"<span {0} {1}>{2}</span>", (!string.IsNullOrEmpty(classCSS) ? sClass : ""),
                @" onclick=""" + callbackJS + @""" ", label);
        }
    }
}
