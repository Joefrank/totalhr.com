using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.core
{
    public delegate string BuildPaginationUrl(string url, int pageNumber);

    public class UrlBuilder
    {
        public static string BuildPaginationUrlNoExtention(string url, int pageNumber)
        {
            if (string.IsNullOrEmpty(url))
                return "";

            return url.Trim() + (url.EndsWith("/") ? "Page/" : "/Page/") + pageNumber;
        } 
    }
}
