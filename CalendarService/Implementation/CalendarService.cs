using Calendar.Infrastructure;
using Calendar.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;

namespace Calendar.Implementation
{
    public class CalendarService : ICalendarService
    {
        private const string HeaderHtml = "<tr><th>{0}</th><th>{1}</th><th>{2}</th><th>{3}</th><th>{4}</th><th>{5}</th><th>{6}</th></tr>";
        private const string TdHtml = "<td {0}>{1}</td>";
        private const string ThHtml = "<th {0}>{1}</th>";
        private const string ThHtmlNoAttrib = "<th>{0}</th>";
        private const string TdHtmlNoAttrib = "<td>{0}</td>";
        private const string TrHtmlNoAttrib = "<tr>{0}</tr>";
        private const string TableHtml = "<table {0}>{1}</table>";
        private const string CalendarMonthViewLink = @"/Calendar/GetCalendarMonth/{0}/{1}/{2}";
        private const string CalendarWeekViewLink = @"/Calendar/WeekView/{0}/{1}/{2}";
        private const string CalendarDayViewLink = @"/Calendar/DayView/{0}/{1}/{2}";

        private readonly CultureInfo _info;
        private const int NoWeekDays = 7;

        readonly int[] _dayHours = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        int[] _dayMinutes = { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };        

        public string LocalizedAm { get; set; }
        public string LocalizedPm { get; set; }

        public CalendarService()
        {
            LocalizedAm = "AM";
            LocalizedPm = "PM";
        }

        public CalendarService(CultureInfo info) : this()
        {
            _info = info;
        }

        private WeekBoundaries GetWeekBoundaries(DateTime now, CultureInfo cultureInfo)
        {
            if (now == null)
                throw new ArgumentNullException("Date not provided");

            if (cultureInfo == null)
                throw new ArgumentNullException("CultureInfo Culture info required");

            var weekBoundaries = new WeekBoundaries();
            var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            int offset = firstDayOfWeek - now.DayOfWeek;

            if (offset != 1)
            {
                DateTime weekStart = now.AddDays(offset);
                DateTime endOfWeek = weekStart.AddDays(6);

                weekBoundaries.FirstDay = weekStart;
                weekBoundaries.LastDay = endOfWeek;
            }
            else
            {
                weekBoundaries.FirstDay = now.AddDays(-6);
                weekBoundaries.LastDay = now;
            }

            return weekBoundaries;
        }

        public string[] GetWeekDaysByName(CultureInfo info)
        {
            var firstDay = info.DateTimeFormat.FirstDayOfWeek;
            string[] arrOfDays = new string[NoWeekDays];

            for (int dayIndex = 0; dayIndex < NoWeekDays; dayIndex++)
            {
                var currentDay = (DayOfWeek)(((int)firstDay + dayIndex) % NoWeekDays);
                // Output the day               
                arrOfDays[dayIndex] = currentDay.ToString();
            }
            return arrOfDays;
        }

        public CalendarHTML GenerateCalendarHTML(CalendarRequestStruct rqStruct)
        {   
            var sbAllHtml = new StringBuilder();
            var sbTemp = new StringBuilder();
            var dateTime = Convert.ToDateTime("01-" + rqStruct.Month + "-" + rqStruct.Year, rqStruct.Info);
            var info = rqStruct.Info ?? _info;
            var dateOfPrevMonth = dateTime.AddMonths(-1);
            var noDaysInprevMonth = DateTime.DaysInMonth(dateOfPrevMonth.Year, dateOfPrevMonth.Month);
            var startDay = dateTime.Day;
            var frontGap = (int)(((int)dateTime.DayOfWeek == 0 ? 7 : (int)dateTime.DayOfWeek) - info.DateTimeFormat.FirstDayOfWeek);
            var noDays = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            var noRows =0;
            var rearGap = 0;
            var ClientPageId = 0;
            var lstEvents = rqStruct.RelatedEvents;
            var sbEvents = new StringBuilder();
            var sbJavascript = new StringBuilder(" var " + rqStruct.ClientConfig.JsArrayEventName + " = new Array(); " + Environment.NewLine);
            var firstDay = info.DateTimeFormat.FirstDayOfWeek;
            var firstDayDate = new DateTime(rqStruct.Year, rqStruct.Month, 1);
            var monthName = firstDayDate.ToString("MMMM", rqStruct.Info);
            var currentTdId = string.Empty;

            ClientPageId = rqStruct.ClientConfig.PageClientId == 0 ? 1 : rqStruct.ClientConfig.PageClientId;

            for (int dayIndex = 0; dayIndex < NoWeekDays; dayIndex++)
            {
                var currentDay = (DayOfWeek)(((int)firstDay + dayIndex) % NoWeekDays);
                // Output the day               
                sbTemp.Append(string.Format(ThHtmlNoAttrib, currentDay)); 
            }

            //This is top row with Month's name
            sbAllHtml.Append(string.Format(ThHtml, string.Format(@" colspan=""{0}"" ", NoWeekDays), monthName + " - " + rqStruct.Year));

            //insert day header html of calendar
            sbAllHtml.Append(string.Format(TrHtmlNoAttrib, sbTemp.ToString()));

            sbTemp.Clear();

            //let's calculate how many days on the last week of calendar we need from next month
            rearGap = (NoWeekDays - (frontGap + noDays) % NoWeekDays);
            
            //let's generate html for all days of chosen month
            for (int i = 1; i <= noDays; i++)
            {
                if (i == 1)
                {
                    sbTemp.Append("<tr>");
                    var start = noDaysInprevMonth - frontGap;

                    //insert days of previous month starting from Sunday
                    for (int x = start + 1; x <= noDaysInprevMonth; x++)
                    {
                        sbTemp.Append(string.Format(TdHtml, @" class=""prev"" " + rqStruct.ClientConfig.InActiveTdClickCallBack, x));
                    }
                }
                //check if current day has some events
                var currentdate = new DateTime(rqStruct.Year, rqStruct.Month, i);
                currentTdId = "td_" + ClientPageId + "_" + i;

                //***consider putting events in chuncks of days at start. then just inject them when day matches.
                if (lstEvents != null)
                {
                    var foundEvents = lstEvents.FindAll(x => x.StartOfEvent.Date == currentdate.Date
                        || x.EndOfEvent.Date == currentdate.Date).ToList();

                    foreach (CalendarEvent ce in foundEvents)
                    {
                        sbEvents.Append(string.Format(@"<span class=""event"" id=""evt_{0}_{1}"" {2}>", ClientPageId, ce.id, rqStruct.ClientConfig.EventClickCallBack)
                            + ce.Title + "</span>");
                        sbJavascript.Append(string.Format(@" {3}['evt_{0}_{1}']=[{0},{1},'{2}'];" + Environment.NewLine, ClientPageId, ce.id, currentTdId, rqStruct.ClientConfig.JsArrayEventName));
                    }
                }

                sbTemp.Append(string.Format(TdHtml, @" id=""" + currentTdId + @""" " + rqStruct.ClientConfig.ActiveTdClickCallBack,  
                        @"<span class=""day"">" + i + "</span>" + sbEvents.ToString()));

                sbEvents.Clear();

                //if we have reached the end of the current week (7 days), create new table row
                if ((i + frontGap) % NoWeekDays == 0 && (i > 1 || (int)dateTime.DayOfWeek == 0))
                {
                    noRows++;
                    sbTemp.Append("</tr>");
                    if (i < noDays - 1)//if we haven't reached end of days
                    {
                        sbTemp.Append("<tr>");
                    }
                }

                //if we have reached the maximum no of days for this calendar month
                if (i == noDays)
                {//complete/borrow a few days from next month
                    for (int y = 0; y < rearGap; y++)
                    {
                        sbTemp.Append(string.Format(TdHtml, @" class=""next"" ", (y + 1)));
                    }
                    sbTemp.Append("</tr></table>");//close calendar
                }
            }

            //we need to know what calendar id we are dealing with on client side
            sbJavascript.AppendLine(string.Format(" CalendarId = {0};", rqStruct.CalendarId));

            sbAllHtml.Insert(0, string.Format("<table {0}>", rqStruct.TableTemplate))
                .Append(sbTemp.ToString());
            
            var nextMonthDate = firstDayDate.AddMonths(1);
            var prevMonthDate = firstDayDate.AddMonths(-1);

            return new CalendarHTML { 
                GridHTML = sbAllHtml.ToString(),
                NextRequest = string.Format(CalendarMonthViewLink, nextMonthDate.Year, nextMonthDate.Month, rqStruct.CalendarId ),
                PreviousRequest = string.Format(CalendarMonthViewLink, prevMonthDate.Year, prevMonthDate.Month,rqStruct.CalendarId),
                Javascript = sbJavascript.ToString()

            };
        }

        public CalendarHTML GenerateWeekHTML(CalendarWeekRequestStruct rqStruct)
        {
            var sbHtml = new StringBuilder();
            var sbTemp = new StringBuilder();
            var nextweek = rqStruct.DateRequested.AddDays(NoWeekDays);
            var prevweek = rqStruct.DateRequested.AddDays(-NoWeekDays);
            var info = rqStruct.Info ?? _info;
            
            //insert table left top edge
            sbTemp.Append(string.Format(ThHtmlNoAttrib, rqStruct.CrossEdgeContent));

            var weekBoundaries = GetWeekBoundaries(rqStruct.DateRequested, info);
            var tempDate = weekBoundaries.FirstDay;

            //add all days headers
            for (int i = 0; i < NoWeekDays; i++)
            {
                sbTemp.Append(string.Format(ThHtmlNoAttrib, tempDate.AddDays(i)
                    .ToString(rqStruct.DayHeaderFormat, info)));
            }

            //close header row
            sbHtml.Append(string.Format(TrHtmlNoAttrib, sbTemp.ToString()));

            //clean the temp SB
            sbTemp.Clear();

            //add row for each hour
            foreach (var hour in _dayHours)
            {
                //add hour cell
                sbTemp.Append(string.Format(TdHtml, "", + hour + " " + (hour < 12 ? LocalizedAm : LocalizedPm)));
                //add other cells
                for (int i = 0; i < NoWeekDays; i++)
                {
                    sbTemp.Append(string.Format(TdHtml, "", "&nbsp;"));
                }

                //add the whole row
                sbHtml.Append(string.Format(TrHtmlNoAttrib, sbTemp.ToString()));
                sbTemp.Clear();
            }

            return new CalendarHTML
                {
                    GridHTML = string.Format(TableHtml, rqStruct.TableTemplate, sbHtml.ToString()),
                    NextRequest = string.Format(CalendarWeekViewLink, nextweek.Year, nextweek.Month, nextweek.Day),
                    PreviousRequest = string.Format(CalendarWeekViewLink, prevweek.Year, prevweek.Month, prevweek.Day)
                };
        }

        public CalendarHTML GenerateDayHTML(CalendarWeekRequestStruct rqStruct)
        {
            var sbHtml = new StringBuilder();
            var sbTemp = new StringBuilder();
            var nextday = rqStruct.DateRequested.AddDays(1);
            var prevday = rqStruct.DateRequested.AddDays(-1);
            var info = rqStruct.Info ?? _info;

            //insert table left top edge
            sbTemp.Append(string.Format(ThHtml, @" width=""50px"" ", rqStruct.CrossEdgeContent));
                     
            //add requested day headers
            sbTemp.Append(string.Format(ThHtmlNoAttrib, rqStruct.DateRequested
                    .ToString(rqStruct.DayHeaderFormat, info)));
            //close header row
            sbHtml.Append(string.Format(TrHtmlNoAttrib, sbTemp.ToString()));

            //clean the temp SB
            sbTemp.Clear();

            //add row for each hour
            foreach (var hour in _dayHours)
            {
                //add hour cell
                sbTemp.Append(string.Format(TdHtml, "", + hour + " " + (hour < 12 ? LocalizedAm : LocalizedPm)));
                //add other cells              
                sbTemp.Append(string.Format(TdHtml, "", "&nbsp;"));             

                //add the whole row
                sbHtml.Append(string.Format(TrHtmlNoAttrib, sbTemp.ToString()));
                sbTemp.Clear();
            }

            return new CalendarHTML
                {
                    GridHTML = string.Format(TableHtml, rqStruct.TableTemplate, sbHtml.ToString()),
                    NextRequest = string.Format(CalendarWeekViewLink, nextday.Year, nextday.Month, nextday.Day),
                    PreviousRequest = string.Format(CalendarWeekViewLink, prevday.Year, prevday.Month, prevday.Day)
                };
        }       

    }
}
