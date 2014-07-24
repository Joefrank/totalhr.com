﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using totalhr.Shared;

namespace totalhr.web.Helpers
{
    public class CalendarHelper
    {

        public static string GenerateRepeatByType(Variables.RepeatType myenum, string groupname)
        {
            return string.Format(@"<span class=""row"" id=""spRepeatType_{0}"" title=""{1}"" onmouseover=""ShowDetails(this);"" 
                onmouseout=""HideDetails(this);"" onclick=""ApplyRepeatSelection(this)""><i class=""info"" title=""{1}"">&nbsp;</i>
                <input type=""radio"" name=""{3}"" value=""{0}"" /><span id=""spRepeatDesc_{0}"">{2}</span></span>
                <span class=""repeatcount"" id=""spRepeatTypeCount_{0}"" onclick=""ShowRepeats('spRepeatTypeAdded_{0}')""></span>
                <span id=""spRepeatTypeAdded_{0}"" style=""display:none""></span> ", (int)myenum, EnumExtensions.FurtherInfo(myenum), 
                           EnumExtensions.Description(myenum), groupname);
        }

        public static string GenerateAttendee(Variables.CalendarEventTarget myenum, string groupname)
        {

            var temp = string.Empty;
            var targetId = (int)myenum;
            var selector = string.Empty;

            if (targetId == (int)Variables.CalendarEventTarget.User)
            {
                temp = @" onclick=""OpenSelector('USERS');"" ";
                selector = "USERS";
            }
            else if (targetId == (int)Variables.CalendarEventTarget.Department)
            {
                temp = @" onclick=""OpenSelector('DEPARTMENT');"" ";
                selector = "DEPARTMENT";
            }
            
            return string.Format(@"<span class=""row"" id=""spAttendeeTarget_{0}"" title=""{1}"" onmouseover=""ShowDetails(this);"" 
                onmouseout=""HideDetails(this);"" onclick=""ApplyAttendeeTargetSelection(this, {0}, '{5}')""><i class=""info"" title=""{1}"">&nbsp;</i>
                <input type=""radio"" name=""{4}"" value=""{0}"" {3} />
                <span id=""spAttendeeDesc_{0}"">{2}</span></span>", (int)myenum, EnumExtensions.FurtherInfo(myenum),
                                              EnumExtensions.Description(myenum), temp, groupname, selector);
        }
    }


}