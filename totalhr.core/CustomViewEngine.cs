using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace totalhr.core
{
    public class CustomViewEngine : RazorViewEngine 
    {       

        public CustomViewEngine()
        {
            AreaViewLocationFormats = new[]
            {
                "~/Areas/Admin/Views/{1}/{0}.cshtml"
            };

            ViewLocationFormats = new[] 
            {
                "~/Areas/Admin/Views/{1}/{0}.cshtml"
            };

            MasterLocationFormats = new[] 
            {
                "~/Areas/Admin/Views/{1}/{0}.cshtml"
            };

            PartialViewLocationFormats = new[] 
            {
                "~/Areas/Admin/Views/{1}/{0}.cshtml"
            };
        }
       
    }

    /// <summary>
    /// Not used at the moment but if need be, we will use it.
    /// </summary>
    public class MyView : IView
    {
        private string _viewPhysicalPath;

        public MyView(string ViewPhysicalPath)
        {
            _viewPhysicalPath = ViewPhysicalPath;
        }

        #region IView Members

        public void Render(ViewContext viewContext, System.IO.TextWriter writer)
        {
            //Load File
            string rawcontents = File.ReadAllText(_viewPhysicalPath);

            //Perform Replacements
            string parsedcontents = Parse(rawcontents, viewContext.ViewData);

            writer.Write(parsedcontents);
        }

        #endregion

        public string Parse(string contents, ViewDataDictionary viewdata)
        {
            return Regex.Replace(contents, "\\{(.+)\\}", m => GetMatch(m, viewdata));
        }

        public virtual string GetMatch(Match m, ViewDataDictionary viewdata)
        {
            if (m.Success)
            {
                string key = m.Result("$1");
                if (viewdata.ContainsKey(key))
                {
                    return viewdata[key].ToString();
                }
            }
            return string.Empty;
        }
    }
}
