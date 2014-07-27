﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using totalhr.Shared;
using totalhr.Shared.Models;

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

        public static string GenerateFrequencyDDl(string id, string name, string callback)
        {
            StringBuilder sbTemp = new StringBuilder();

            foreach(var num in (Variables.EventFrequency[])Enum.GetValues(typeof(Variables.EventFrequency)))
            {
                sbTemp.Append(string.Format(@"<option value=""{0}"">{1}</option>",(int)num, EnumExtensions.Description(num)));
            }

            return string.Format(@"<select id=""{0}"" name=""{1}"" {2}>
                                        {3}
                                    </select>",id, name, callback, sbTemp.ToString());
        }

        public static string GenerateReminderList(List<CalendarEventReminder> lstReminder)
        {
            StringBuilder sbTemp = new StringBuilder();

            //(Variables.ReminderType[])Enum.GetValues(typeof(Variables.ReminderType))

            foreach (var reminder in lstReminder)
            {
                if (reminder.ReminderType == (int)Variables.ReminderType.TimeBefore)
                {
                    sbTemp.Append(string.Format(@"<span>{0} {1} {2}</span>", reminder.Frequency,
                        EnumExtensions.Description(Variables.ReminderType[reminder.FrequencyType])));
                    //Convert.ChangeType(1, Variables.ReminderType.GetTypeCode())
                }
            }
            return sbTemp.ToString();
        }
    }


}