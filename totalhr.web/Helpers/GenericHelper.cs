using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace totalhr.web.Helpers
{
    public class GenericHelper
    {
        public static string WebsiteName
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["WebsiteName"]; }
        }

        public static string WebsiteRoot
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["RootURL"];
            }
        }

    }
}