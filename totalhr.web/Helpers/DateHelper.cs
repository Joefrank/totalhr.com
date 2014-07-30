using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace totalhr.web.Helpers
{
    public class DateHelper
    {
        //*** pass current date as links will depend on dates being viewed
        public static string CurrentMonthCalendarLink(string text, int calendarid = 0)
        {
            return string.Format(@"<a href=""/Calendar/GenerateDefault/{0}"">{1}</a>",
                 calendarid, text);
        }

        public static string CurrentWeekCalendarLink(string text, int calendarid =0)
        {
            return string.Format(@"<a href=""/calendar/GetWeekView/{0}/{1}/{2}/{3}"">{4}</a>",
                DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, calendarid, text);
        }

        public static string CurrentDayCalendarLink(string text, int calendarid = 0)
        {
            return string.Format(@"<a href=""/calendar/GetDayView/{0}/{1}/{2}/{3}"">{4}</a>",
                DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, calendarid, text);
        }
    }
}