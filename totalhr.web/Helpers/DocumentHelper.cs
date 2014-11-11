using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using totalhr.Shared;

namespace totalhr.web.Helpers
{
    public class DocumentHelper
    {
        public static string GetDocumentPermissionType()
        {
            var sbTemp = new StringBuilder();

            foreach (var num in (Variables.DocumentPermissionType[])Enum.GetValues(typeof(Variables.DocumentPermissionType)))
            {
                sbTemp.Append(string.Format(@"<span class=""actionlink"" id=""sp_perm_{0}"" onclick=""ShowDocPermissionOption('sp_perm_{0}')"">
                    <input type=""radio"" name=""PermissionSelection"" value=""{0}"" />&nbsp;{1} <span id=""sp_selections_{0}""></span></span><br/> ", 
                                             (int)num, EnumExtensions.Description(num)));
            }
            sbTemp.Append(@"<input type=""hidden"" name=""PermissionSelectionValue"" />");

            return sbTemp.ToString();
        }
    }
}