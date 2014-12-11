using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using totalhr.Shared.Models;

namespace totalhr.web.Areas.Admin.Helpers
{
    public class ProfileHelper
    {
        public static string GenerateListBox(List<ListItemStruct> lst, string attrib)
        {

            StringBuilder sbTemp = new StringBuilder();

            using (var sequence = lst.GetEnumerator())
            {
                while (sequence.MoveNext())
                {
                    sbTemp.Append(string.Format(@"<option value=""{0}"">{1}</option>", sequence.Current.Id, sequence.Current.Name));
                }
            }

            if (sbTemp.Length > 10)
            {
                sbTemp.Insert(0, string.Format(@"<select {0}>", attrib)).Append("</select>");
            }

            return sbTemp.ToString();
        }
    }
}