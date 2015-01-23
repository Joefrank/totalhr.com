using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using totalhr.Shared.Models;
using System.Web.Mvc;
using totalhr.Resources;

namespace totalhr.web.Areas.Admin.Helpers
{
    public class GenericHelper
    {
        public static List<SelectListItem> GetListFromNumerable(IEnumerable<ListItemStruct> lst)
        {
            if (lst == null)
                return null;

            var newList = new List<SelectListItem> { new SelectListItem { Value = "0", Text = AdminGeneric.V_Select } };
            newList.AddRange(lst.Select(item => new SelectListItem {Value = item.Id.ToString(CultureInfo.InvariantCulture), Text = item.Name}));

            return newList;
        }

        public static List<SelectListItem> GetListFromNumerable(IEnumerable<ListItemStruct> lst, string selectedValue)
        {
            if (lst == null)
                return null;

            var newList = new List<SelectListItem> { new SelectListItem { Value = "0", Text = AdminGeneric.V_Select } };

            newList.AddRange(lst.Select(item => item.Id.ToString(CultureInfo.InvariantCulture).Equals(selectedValue) ? new SelectListItem
                {
                    Value = item.Id.ToString(CultureInfo.InvariantCulture), Text = item.Name, Selected = true
                } : new SelectListItem
                    {
                        Value = item.Id.ToString(CultureInfo.InvariantCulture), Text = item.Name
                    }));

            return newList;
        }
    }
}