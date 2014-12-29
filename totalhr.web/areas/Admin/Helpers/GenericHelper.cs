using System;
using System.Collections.Generic;
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

            foreach (ListItemStruct item in lst)
            {
                newList.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }
            return newList;
        }
    }
}