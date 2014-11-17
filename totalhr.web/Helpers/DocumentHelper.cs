using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using totalhr.data.EF;
using totalhr.Shared;
using totalhr.Resources;

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
                    <input type=""radio"" name=""PermissionSelection"" value=""{0}"" />&nbsp;{1}</span><br/> ", 
                                             (int)num, EnumExtensions.Description(num)));
            }
            sbTemp.Append(@"<input type=""hidden"" name=""PermissionSelectionValue"" id=""PermissionSelectionValue"" />");

            return sbTemp.ToString();
        }

        public static string GetDocumentPermissionType(List<CompanyDocumentPermission> permissions, 
            int PermissionType, List<string> lstObjNames, string selectedIds = "")
        {
            var sbTemp = new StringBuilder("<table>");

            CompanyDocumentPermission docperm = (permissions != null && permissions.Count > 0) ? permissions.FirstOrDefault() : null;

            
            foreach (var num in (Variables.DocumentPermissionType[])Enum.GetValues(typeof(Variables.DocumentPermissionType)))
            {
                string schecked = string.Empty;               
                string selectedNames =string.Empty;

                if (PermissionType == (int)num)
                {
                    schecked = @" checked=""checked"" ";
                }

                if (lstObjNames != null && lstObjNames.Count > 0 && PermissionType == (int)num)
                {
                    selectedNames = "(" + string.Join(",", lstObjNames.ToArray()) + ")";
                }

                sbTemp.Append(string.Format(@"<tr><td><span class=""actionlink"" id=""sp_perm_{0}"" onclick=""ShowDocPermissionOption('sp_perm_{0}')"">
                    <input type=""radio"" {2} name=""PermissionSelection"" value=""{0}"" />&nbsp;{1}: <i>{3}</i></span></td></tr>",
                                             (int)num, EnumExtensions.Description(num), schecked, selectedNames));
            }
            sbTemp.Append(string.Format(@"</table><input type=""hidden"" name=""PermissionSelectionValue"" 
                id=""PermissionSelectionValue"" value=""{0}"" />", selectedIds));

            return sbTemp.ToString();
        }

        public static string SpitDocTableHeader()
        {
            return string.Format(@"<table class=""tab"">
                  <tr><th>{0}</th><th>{1}</th><th>{2}</th><th>{3}</th>
                    <th>{4}</th><th>{5}</th><th>{6}</th><th>{7}</th><th>{8}</th><th>{9}</th></tr>",
                     Document.TH_DisplayName, Document.TH_Created, Document.TH_CreatedBy,
                     Document.TH_Permissions, Document.TH_Views, Document.TH_Downloads, Document.TH_Details,
                     Document.V_Download, Document.V_Edit, Document.V_Archive);
        }

        public static string SpitDocTableFooter()
        {
            return "</table>";
        }

        public static string GetPermissionText(CompanyDocument doc)
        {           
            string permmessage = string.Empty;
           
            switch (doc.PermissionTypeId)
            {
                case (int)Variables.DocumentPermissionType.Private:
                    permmessage = @"<span class=""private"">" + EnumExtensions.Description(Variables.DocumentPermissionType.Private) + "</span>";
                    break;
                case (int)Variables.DocumentPermissionType.Department:
                    permmessage = @"<span class=""department"">" + EnumExtensions.Description(Variables.DocumentPermissionType.Department) + "</span>";
                    break;
                case (int)Variables.DocumentPermissionType.SelectedUsers:
                    permmessage = @"<span class=""user"">" + EnumExtensions.Description(Variables.DocumentPermissionType.SelectedUsers) + "</span>";
                    break;
                case (int)Variables.DocumentPermissionType.WholeCompany:
                    permmessage = @"<span class=""company"">" + EnumExtensions.Description(Variables.DocumentPermissionType.WholeCompany) + "</span>";
                    break;
            }
           
            return  permmessage;
        }
        


        public static string MakeTableRowHtml(CompanyDocument doc, int currentUserId)
        {
            string delete = (currentUserId == doc.CreatedBy) ?
                string.Format(@"<a href=""/Document/Delete/{0}"" title=""{1}"">{1}</a>", doc.Id, Document.V_Archive) :
                @"<span class=""noaccess"">&nbsp;</span>";

            string edit = (currentUserId == doc.CreatedBy) ?
                string.Format(@"<a href=""/Document/Edit/{0}"" title=""{1}"">{1}</a>", doc.Id, Document.V_Edit) :
                @"<span class=""noaccess"">&nbsp;</span>";

            return string.Format(
                @"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td>
                <td><a href=""/Document/Details/{6}"" title=""{7}"">{7}</a></td>
                <td><a href=""/Document/Download/{6}"" title=""{8}"">{8}</a></td>
                <td>{9}</td>
                <td>{10}</td>
                </tr>",
                doc.DisplayName, doc.Created.ToShortDateString(), (doc.User.firstname + " " + doc.User.surname),
                GetPermissionText(doc), doc.NoOfViews, doc.NoOfDownloads,
                @doc.Id, Document.V_View, Document.V_Download, edit, delete
                );
        }
    }
}