using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using totalhr.Shared.Models;
using System.Web.Mvc;

namespace totalhr.web.Areas.Admin.Helpers
{
    public class GenericHelper
    {
        public static List<SelectListItem> GetListFromNumerable(IEnumerable<ListItemStruct> lst)
        {
            var newList = new List<SelectListItem>();

            foreach (ListItemStruct item in lst)
            {
                newList.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }
            return newList;
        }
    }
}