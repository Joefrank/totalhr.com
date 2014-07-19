using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using totalhr.data.Repositories.Infrastructure;
using totalhr.data.EF;
using totalhr.Shared;
using totalhr.Resources;

namespace totalhr.web.Helpers
{
    public class GlossaryHelper
    {
        public static string EventTargetHTML(int targetId)
        {
            var temp = string.Empty;

            if (targetId == (int) Variables.CalendarEventTarget.User)
            {
                temp = string.Format(@"<span id=""sp_usercount_{0}""></span><span id=""sp_usersel_{0}""></span>
                    <input type=""button"" value=""{1}"" onclick=""OpenSelector('USERS');"" />", targetId, Common.btn_SelectUser);
            }
            else if (targetId == (int) Variables.CalendarEventTarget.Department)
            {
                temp = string.Format(@"<span id=""sp_departmentcount_{0}""></span><span id=""sp_departmentsel_{0}""></span>
                    <input type=""button"" value=""{1}"" onclick=""OpenSelector('DEPARTMENT');"" />", targetId, Common.btn_SelectDepartment);
            }

            return temp;
        }
    }
}