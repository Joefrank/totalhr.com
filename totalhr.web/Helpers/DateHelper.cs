using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace totalhr.web.Helpers
{
    public class DateHelper
    {
        public static string CurrentYearCalendarLink(string text, int calendarid = 0)
        {
            return string.Format(@"<button class=""btn tooltips"" data-placement=""top"" data-original-title=""tooltips in top"" 
                onclick=""/Calendar/GetCalendarYear/{0}/{1}"">{2}</button>",
                DateTime.Now.Year, calendarid, text);
        }
        
        //*** pass current date as links will depend on dates being viewed
        public static string CurrentMonthCalendarLink(string text, int calendarid = 0)
        {
            return string.Format(@"<a class=""btn tooltips"" href=""/Calendar/GenerateDefault/{0}"">{1}</a>",
                 calendarid, text);
        }

        public static string CurrentWeekCalendarLink(string text, int calendarid =0)
        {
            return string.Format(@"<a class=""btn tooltips"" href=""/calendar/GetWeekView/{0}/{1}/{2}/{3}"">{4}</a>",
                DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, calendarid, text);
        }

        public static string CurrentDayCalendarLink(string text, int calendarid = 0)
        {
            return string.Format(@"<a class=""btn tooltips"" href=""/calendar/GetDayView/{0}/{1}/{2}/{3}"">{4}</a>",
                DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, calendarid, text);
        }

        public static List<string> GetHourList()
        {
            return new List<string> { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
        } 

        public static List<string> GetMinuteList()
        {
            return new List<string> { "00", "05", "10", "15", "20", "25", "30", "35", "40", "45", "50", "55" };
        } 

        public static SelectList GetHourDDL()
        {
            return new SelectList(GetHourList());
        }

        public static SelectList GetMinuteDDL()
        {
            return new SelectList(GetMinuteList());
        }

        public static Dictionary<string, object> GetCalendarDictionary(string dicid, string cssclass, string dateformat, string defaultvalue)
        {
            return
                new Dictionary<string, object>
                {
                    {"class", cssclass},
                    {"data-beatpicker", "true"},
                    {"data-beatpicker-position", "['*','*']"},
                    {"data-beatpicker-format", dateformat},
                    {"data-beatpicker-module", "gotoDate,clear"},
                    {"data-beatpicker-id", dicid},
                    {"value", defaultvalue}
                };
        }

        public static Dictionary<string, object> GetCalendarDictionary(string dicid, string cssclass, string dateformat)
        {
            return
                new Dictionary<string, object>
                {
                    {"class", cssclass},
                    {"data-beatpicker", "true"},
                    {"data-beatpicker-position", "['*','*']"},
                    {"data-beatpicker-format", dateformat},
                    {"data-beatpicker-module", "gotoDate,clear"},
                    {"data-beatpicker-id", dicid}
                };
        }
    }
}