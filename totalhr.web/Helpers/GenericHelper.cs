using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using totalhr.Resources;
using totalhr.Shared;
using totalhr.Shared.Models;

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

        public static string ProfileUrlPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ProfilePicturePath"];
            }
        }

        public static string GetProfilePictureUrl(string imageSrc)
        {
            return System.Web.VirtualPathUtility.ToAbsolute(ProfileUrlPath + imageSrc);
        }

        public static List<SelectListItem> GetListFromNumerable(IEnumerable<ListItemStruct> lst)
        {
            if (lst == null)
                return null;

            var newList = new List<SelectListItem> { new SelectListItem { Value = "0", Text = AdminGeneric.V_Select } };

            foreach (ListItemStruct item in lst)
            {
                newList.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }
            return newList;
        }

        public static List<SelectListItem> GenerateSelectFromEnum<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var newList = new List<SelectListItem> { new SelectListItem { Value = "0", Text = AdminGeneric.V_Select } };                        

            foreach (var num in (T[])Enum.GetValues(typeof(T)))
            {
                newList.Add(new SelectListItem { Value = Convert.ToInt32(num).ToString(), Text = EnumExtensions.Description(num) });
            }

            return newList;
        }

        public static string GenerateSelectFromEnum<T>(string selectName, string callbackJS) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var sbTemp = new StringBuilder();

            foreach (var num in (T[])Enum.GetValues(typeof(T)))
            {
                sbTemp.Append(string.Format(@"<option value=""{0}"">{1}</option>", Convert.ToInt32(num), EnumExtensions.Description(num)));
            }

            return string.Format(@"<select id=""{0}"" name=""{1}"" {2}>
                                        {3}
                                    </select>", selectName, selectName, callbackJS, sbTemp.ToString());
        }

        public static string MakeSortableTH(string currentSortColumn, string idPrefix, string selectedSortColumn, 
            string selectedSortDirection, string label )
        {
            var dir = (currentSortColumn == selectedSortColumn && selectedSortDirection == "asc")? "desc" : "asc";

            return string.Format(@"<th data-sortcolumn=""{0}"" id=""{1}{0}"" onclick=""ToggleOrder(this)"" 
                class=""{2}"">{3}</th>", currentSortColumn, idPrefix, dir, label);
        }

        public static string MakeSortableTH(string currentSortColumn, SortingInfo info, string label, string idPrefix = "th")
        {
            var dir = (currentSortColumn == info.SortColumn && info.SortOrder == "asc") ? "desc" : "asc";

            return string.Format(@"<th data-sortcolumn=""{0}"" id=""{1}{0}"" onclick=""ToggleOrder(this)"" 
                class=""{2}"">{3}</th>", currentSortColumn, idPrefix, dir, label);
        }
    }
}