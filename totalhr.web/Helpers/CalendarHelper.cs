using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using totalhr.Shared;

namespace totalhr.web.Helpers
{
    public class CalendarHelper
    {

        public static string GenerateRepeatByType(Variables.RepeatType myenum)
        {
            return string.Format(@"<span class=""row"" id=""spRepeatType_{0}"" title=""{1}"" onmouseover=""ShowDetails(this);"" 
                onmouseout=""HideDetails(this);"" onclick=""ApplyRepeatSelection(this)""><i class=""info"" title=""{1}"">(i)</i>
                <input type=""radio"" name=""repeattype"" value=""{0}"" />{2}</span>
                <span id=""spRepeatTypeCount_{0}"" onclick=""ShowRepeats('spRepeatTypeAdded_{0}')""></span>
                <span id=""spRepeatTypeAdded_{0}""></span> ", (int)myenum, EnumExtensions.FurtherInfo(myenum), EnumExtensions.Description(myenum));
        }
    }
}