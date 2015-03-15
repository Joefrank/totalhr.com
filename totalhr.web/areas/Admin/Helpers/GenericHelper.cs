using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using totalhr.Shared.Models;
using System.Web.Mvc;
using totalhr.Resources;
using totalhr.web.Areas.Admin.Models;
using System.Text;

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

        public static string MakeBreadCrumb(List<BreadCrumbItem> itemList)
        {
            var allHtml = new StringBuilder();
            int itemcount = itemList.Count;
            int index = 0;
            var url = string.Empty;
            var lastClass = string.Empty;
            var uri = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
            var homeUrl = uri.IndexOf("/Admin/", StringComparison.CurrentCultureIgnoreCase) >= 0? "/Admin/" : "/Account/";

            foreach(var item in itemList)
            {
                index++;
                url = string.IsNullOrEmpty(item.Url)? "#" : item.Url;
                lastClass = (index == itemcount? "-last": "");

                allHtml.Append(
                    string.Format(
                @"<li>
                    <a href=""{0}"" title=""{1}"">{2}</a> <span class=""divider{3}"">&nbsp;</span>
                  </li>", url, item.Title, item.Text, lastClass));
            }

            return string.Format(
            @"<ul class=""breadcrumb"">
                <li>
                    <a href=""{1}""><i class=""icon-home""></i></a><span class=""divider"">&nbsp;</span>
                </li>
                {0}           
             </ul>", allHtml.ToString(), homeUrl);
        }
    }
}