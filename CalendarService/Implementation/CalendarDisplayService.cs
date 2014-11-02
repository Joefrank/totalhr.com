using Calendar.Infrastructure;
using Calendar.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Shared;
using totalhr.data.EF;
using totalhr.Shared.Models;
using log4net;

namespace Calendar.Implementation
{
    public class CalendarDisplayService : ICalendarDisplayService
    {
        private const string HeaderHtml = "<tr><th>{0}</th><th>{1}</th><th>{2}</th><th>{3}</th><th>{4}</th><th>{5}</th><th>{6}</th></tr>";
        private const string TdHtml = "<td {0}>{1}</td>";
        private const string ThHtml = "<th {0}>{1}</th>";
        private const string ThHtmlNoAttrib = "<th>{0}</th>";
        private const string TdHtmlNoAttrib = "<td>{0}</td>";
        private const string TrHtmlNoAttrib = "<tr>{0}</tr>";
        private const string TableHtml = "<table {0}>{1}</table>";

        private const string CalendarYearViewLink = @"/Calendar/GetCalendarYear/{0}/{1}";
        private const string CalendarMonthViewLink = @"/Calendar/GetCalendarMonth/{0}/{1}/{2}";
        private const string CalendarWeekViewLink = @"/Calendar/GetWeekView/{0}/{1}/{2}/{3}";
        private const string CalendarDayViewLink = @"/Calendar/GetDayView/{0}/{1}/{2}/{3}";

        private const int maxDaysInMonth = 31;
        private const int noOfMonthsinYear = 12;

        private readonly CultureInfo _info;
        private const int NoWeekDays = 7;

        readonly int[] _dayHours = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        int[] _dayMinutes = { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };

        public string LocalizedAm { get; set; }
        public string LocalizedPm { get; set; }

        public CalendarDisplayService()
        {
            LocalizedAm = "AM";
            LocalizedPm = "PM";
        }

        public CalendarDisplayService(CultureInfo info)
            : this()
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
                arrOfDays[dayIndex] = info.DateTimeFormat.GetDayName(currentDay);
            }
            return arrOfDays;
        }


        private string BuildTableGrid(CalendarCellDetails[,] arrValues, int lenX, int lenY, string tableattrib)
        {
            StringBuilder sbTemp = new StringBuilder();
            StringBuilder sbFinal = new StringBuilder();

            for (int x = 0; x <= lenX; x++)
            {
                for (int y = 0; y <= lenY; y++)
                {
                    if(x == 0)
                        sbTemp.Append(string.Format(ThHtmlNoAttrib, arrValues[x, y].Content));
                    else{
                        var attrib = (y > 0) ? arrValues[x, y].CSSClass + arrValues[x, y].CallbackScript + @" id=""" + arrValues[x, y].Id + @""" " : "";
                        sbTemp.Append(string.Format(TdHtml, attrib, arrValues[x, y].Content));
                    }
                }
                sbFinal.Append(string.Format(TrHtmlNoAttrib, sbTemp.ToString()));
                sbTemp.Clear();
            }

            return string.Format(TableHtml, tableattrib, sbFinal.ToString());
        }

        public CalendarHTML GenerateYearHTML(CalendarRequestStruct rqStruct)
        {
            string[] monthNames = rqStruct.Info.DateTimeFormat.MonthNames;
            string[] weekdaysNames = GetWeekDaysByName(rqStruct.Info);
            int[] noDaysInMonths = new int[12];

            
            string tempval = string.Empty;
            var currentTdId = string.Empty;
            var firstDay = rqStruct.Info.DateTimeFormat.FirstDayOfWeek;
            int daysInCurrentMonth = 0;
            int indexOfFirstDay;
            int offset;
            int topList = 0;
            CalendarCellDetails[,] cells = null;
            int ClientPageId = rqStruct.ClientConfig.PageClientId == 0 ? 1 : rqStruct.ClientConfig.PageClientId;
            var sbJavascript = new StringBuilder(" var " + rqStruct.ClientConfig.JsArrayEventName + " = new Array(); " + Environment.NewLine);
            DateTime currentdate;
            string todayHtml = string.Empty;
            //currentTdId = "td_" + ClientPageId + "_" + i;
            Dictionary<int, List<int>> dico = new Dictionary<int, List<int>>();
             

            // calculate offset to add into calendar matrix
            for (int y = 1; y <= noOfMonthsinYear; y++)
            {
                //get first day of month            
                indexOfFirstDay = (int)((new DateTime(rqStruct.Year, y, 1)).DayOfWeek);
                indexOfFirstDay = (indexOfFirstDay == 0 ? 7 : indexOfFirstDay);
                //offset of first day of month in the week.
                offset = indexOfFirstDay - (int)firstDay;                
                // get no of days in the month
                daysInCurrentMonth = DateTime.DaysInMonth(rqStruct.Year, y);
                List<int> currentLst = new List<int>();

                for (int i = 0; i < offset; i++)
                {
                    currentLst.Add(0);
                }

                // fill this last.
                for (int x = 1; x <= daysInCurrentMonth; x++)
                {
                    currentLst.Add(x);
                }

                dico[y] = currentLst;
                topList = (currentLst.Count > topList) ? currentLst.Count : topList;
            }

            cells = new CalendarCellDetails[topList + 1, noOfMonthsinYear + 1];


            for (int x = 0; x <= topList; x++)
            {
                for (int y = 0; y <= noOfMonthsinYear; y++)
                {
                    if (x == 0)
                    {
                        tempval = (y > 0) ? monthNames[y - 1] : "";
                    }
                    else
                    {
                        CalendarCellReturn result = null;
                        currentTdId = string.Empty;

                        if (y == 0)
                        {
                            tempval = weekdaysNames[(x - 1) % NoWeekDays];
                        }
                        else if (y > 0)
                        {
                            
                            if(dico[y].Count >= x && dico[y][x - 1] != 0){

                                currentdate = new DateTime(rqStruct.Year, y, dico[y][x - 1]);
                                currentTdId = "td_" + ClientPageId + "_" + dico[y][x - 1] + "_" + y;

                                if (DateTime.Now.Date == currentdate.Date)
                                {
                                    todayHtml = string.Format(@" class=""{0}"" ", rqStruct.ClientConfig.CurrentDayCssClass);
                                }
                                else
                                {
                                    todayHtml = string.Empty;
                                }

                                result = MakeHTMLForCalendarCell(currentdate, rqStruct, ClientPageId);
                                sbJavascript.Append(result.Scripts);
                                tempval = @"<span class=""day"">" + dico[y][x - 1] + "</span>" + result.Events;
                            }else{
                                tempval = "";
                            }
                        }
                      
                    }

                   
                    cells[x, y] = new CalendarCellDetails {Id =currentTdId, Content = tempval, CSSClass = todayHtml, CallbackScript = rqStruct.ClientConfig.ActiveTdClickCallBack };
                }
            }

             sbJavascript.AppendLine(string.Format(" CalendarId = {0};", rqStruct.CalendarId));
             
           
                //sbTemp.Append(string.Format(TdHtml, todayHtml + @" id=""" + currentTdId + @""" " +
                  //  rqStruct.ClientConfig.ActiveTdClickCallBack, spDate +
               

                //sbEvents.Clear();

            return new CalendarHTML
            {
                GridHTML = BuildTableGrid(cells, topList, noOfMonthsinYear, rqStruct.TableTemplate),
                NextRequest = string.Format(CalendarYearViewLink, rqStruct.Year + 1, rqStruct.CalendarId),
                PreviousRequest = string.Format(CalendarYearViewLink, rqStruct.Year - 1, rqStruct.CalendarId),
                Javascript = sbJavascript.ToString(),
                ViewType = Variables.CalendarViewType.YearView
            };

        }

        private class CalendarCellDetails
        {
            public string CSSClass { get; set; }
            public string Id { get; set; }
            public string CallbackScript { get; set; }
            public string Content{get;set;}
        }

        private CalendarCellReturn MakeHTMLForCalendarCell(DateTime date, CalendarRequestStruct rqStruct, int clientpageid)
        {
           var currentTdId = "td_" + clientpageid + "_" + date.Day + "_" + date.Month;
            StringBuilder sbEvents = new StringBuilder();
            StringBuilder sbJavascript = new StringBuilder();

            if (rqStruct.RelatedEvents != null)
            {
                var foundEvents = rqStruct.RelatedEvents.FindAll(x => x.StartOfEvent.Date <= date.Date
                    && x.EndOfEvent.Date >= date.Date).ToList();
                var tempspan = string.Empty;
                var evtDetails = new StringBuilder();
                var edithtml = string.Empty;

                foreach (CalendarEventCache ce in foundEvents)
                {
                    //use delegates for event details
                    evtDetails.Append(string.Format("<b>{0}: {1}</b> <br/>", rqStruct.LabelsAndNames["EventTitle"], ce.Title));
                    evtDetails.Append(string.Format("<b>{0}: {1}</b> <br/>", rqStruct.LabelsAndNames["EventLocation"], ce.Location));
                    evtDetails.Append(string.Format("<b>{0}: {1}</b> <br/>", rqStruct.LabelsAndNames["EventStart"], ce.StartOfEvent));
                    evtDetails.Append(string.Format("<b>{0}: {1}</b> <br/>", rqStruct.LabelsAndNames["EventEnd"], ce.EndOfEvent));
                    evtDetails.Append(string.Format("<b>{0}: {1}</b> <br/>", rqStruct.LabelsAndNames["EventDescription"], ce.Description.Replace(Environment.NewLine, "<br/>")));

                    if (ce.CreatedBy == rqStruct.UserId)
                    {
                        edithtml = string.Format(@"<br/><span class=""editevent"" id=""evt_{0}_{1}"" {2} title=""{3}""></span>",
                           clientpageid, ce.id, rqStruct.ClientConfig.EventClickCallBack, rqStruct.LabelsAndNames["TooltipEditEvent"]);
                    }
                    tempspan = string.Format(@"<span class=""previewevent"" id=""sp_preview_{0}""  style=""display:none""> 
                                <span class=""spdelete"" onclick=""ClosePreview($('#sp_preview_{0}'));"" title=""{3}"">&nbsp;</span>{1} {2}</span>",
                                            ce.id, edithtml, evtDetails.ToString(), rqStruct.LabelsAndNames["ClosePreview"]);


                    sbEvents.Append(string.Format(@"<span class=""event"" id=""evt_{0}_{1}"" {2}>{3}</span>", clientpageid, ce.id, rqStruct.ClientConfig.PreviewCallBack,
                         ce.Title + tempspan));

                    sbJavascript.Append(string.Format(@" {3}['evt_{0}_{1}']=[{0},{1},'{2}'];" + Environment.NewLine, clientpageid, ce.id, currentTdId, rqStruct.ClientConfig.JsArrayEventName));
                }

                string spDate = string.Format(@"<span style=""display:none"" id=""sp_{0}"">{1}/{2}/{3}</span>", currentTdId, rqStruct.Year, date.Month, date.Day);
                sbEvents.Append(spDate);

                return new CalendarCellReturn
                {
                    Events = sbEvents.ToString(),
                    Scripts = sbJavascript.ToString()
                };
            }
            else { return null; }

        }

        private class CalendarCellReturn
        {
            public string Events { get; set; }
            public string Scripts { get; set; }
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
            var noRows = 0;
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
                var todayHtml = string.Empty;

                if (DateTime.Now.Date == currentdate.Date)
                {
                    todayHtml = string.Format(@" class=""{0}"" ", rqStruct.ClientConfig.CurrentDayCssClass);
                }

                //***consider putting events in chuncks of days at start. then just inject them when day matches.
                if (lstEvents != null)
                {
                    var foundEvents = lstEvents.FindAll(x => x.StartOfEvent.Date == currentdate.Date
                        || x.EndOfEvent.Date == currentdate.Date).ToList();
                    var tempspan = string.Empty;
                    var evtDetails = new StringBuilder();
                    var edithtml = string.Empty;

                    foreach (CalendarEventCache ce in foundEvents)
                    {
                        //use delegates for event details
                        evtDetails.Append(string.Format("<b>{0}: {1}</b> <br/>", rqStruct.LabelsAndNames["EventTitle"], ce.Title));
                        evtDetails.Append(string.Format("<b>{0}: {1}</b> <br/>", rqStruct.LabelsAndNames["EventLocation"], ce.Location));
                        evtDetails.Append(string.Format("<b>{0}: {1}</b> <br/>", rqStruct.LabelsAndNames["EventStart"], ce.StartOfEvent));
                        evtDetails.Append(string.Format("<b>{0}: {1}</b> <br/>", rqStruct.LabelsAndNames["EventEnd"], ce.EndOfEvent));
                        evtDetails.Append(string.Format("<b>{0}: {1}</b> <br/>", rqStruct.LabelsAndNames["EventDescription"], ce.Description.Replace(Environment.NewLine, "<br/>")));

                        if (ce.CreatedBy == rqStruct.UserId)
                        {
                            edithtml = string.Format(@"<br/><span class=""editevent"" id=""evt_{0}_{1}"" {2} title=""{3}""></span>",
                               ClientPageId, ce.id, rqStruct.ClientConfig.EventClickCallBack, rqStruct.LabelsAndNames["TooltipEditEvent"]);
                        }
                        tempspan = string.Format(@"<span class=""previewevent"" id=""sp_preview_{0}""  style=""display:none""> 
                                <span class=""spdelete"" onclick=""ClosePreview($('#sp_preview_{0}'));"" title=""{3}"">&nbsp;</span>{1} {2}</span>",
                                                ce.id, edithtml, evtDetails.ToString(), rqStruct.LabelsAndNames["ClosePreview"]);


                        sbEvents.Append(string.Format(@"<span class=""event"" id=""evt_{0}_{1}"" {2}>{3}</span>", ClientPageId, ce.id, rqStruct.ClientConfig.PreviewCallBack,
                             ce.Title + tempspan));

                        sbJavascript.Append(string.Format(@" {3}['evt_{0}_{1}']=[{0},{1},'{2}'];" + Environment.NewLine, ClientPageId, ce.id, currentTdId, rqStruct.ClientConfig.JsArrayEventName));
                    }
                }

                string spDate = string.Format(@"<span style=""display:none"" id=""sp_{0}"">{1}/{2}/{3}</span>", currentTdId, rqStruct.Year, rqStruct.Month, i);

                sbTemp.Append(string.Format(TdHtml, todayHtml + @" id=""" + currentTdId + @""" " +
                     rqStruct.ClientConfig.ActiveTdClickCallBack, spDate +
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

            return new CalendarHTML
            {
                GridHTML = sbAllHtml.ToString(),
                NextRequest = string.Format(CalendarMonthViewLink, nextMonthDate.Year, nextMonthDate.Month, rqStruct.CalendarId),
                PreviousRequest = string.Format(CalendarMonthViewLink, prevMonthDate.Year, prevMonthDate.Month, rqStruct.CalendarId),
                Javascript = sbJavascript.ToString(),
                ViewType = Variables.CalendarViewType.MonthView
            };
        }

        public CalendarHTML GenerateWeekHTML(CalendarWeekRequestStruct rqStruct)
        {
            var sbHtml = new StringBuilder();
            var sbTemp = new StringBuilder();
            var sbTD = new StringBuilder();
            var nextweek = rqStruct.DateRequested.AddDays(NoWeekDays);
            var prevweek = rqStruct.DateRequested.AddDays(-NoWeekDays);
            var info = rqStruct.Info ?? _info;
            var lstEvents = rqStruct.RelatedEvents;
            var sbJavascript = new StringBuilder(" var " + rqStruct.ClientConfig.JsArrayEventName + " = new Array(); " + Environment.NewLine);
            var currentTdId = string.Empty;
            var ClientPageId = 0;

            //insert table left top edge
            sbTemp.Append(string.Format(ThHtmlNoAttrib, rqStruct.CrossEdgeContent));
            ClientPageId = rqStruct.ClientConfig.PageClientId == 0 ? 1 : rqStruct.ClientConfig.PageClientId;

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
                sbTemp.Append(string.Format(TdHtml, "", +hour + " " + (hour < 12 ? LocalizedAm : LocalizedPm)));
                //add other cells
                for (int i = 0; i < NoWeekDays; i++)
                {
                    var currentdate = tempDate.AddDays(i);
                    currentTdId = "td_" + ClientPageId + "_" + i;

                    if (lstEvents != null)
                    {
                        var foundEvents = lstEvents.FindAll((
                            x => (x.StartOfEvent.Date == currentdate.Date || x.EndOfEvent.Date == currentdate.Date)
                            &&
                            (x.StartOfEvent.Hour == hour || x.EndOfEvent.Hour == hour)
                            )).ToList();

                        foreach (var ce in foundEvents)
                        {
                            sbTD.Append(string.Format(@"<span class=""event"" id=""evt_{0}_{1}"" {2}>", ClientPageId, ce.id, rqStruct.ClientConfig.EventClickCallBack)
                                + ce.Title + "</span>");
                            sbJavascript.Append(string.Format(@" {3}['evt_{0}_{1}']=[{0},{1},'{2}'];" + Environment.NewLine, ClientPageId, ce.id, currentTdId, rqStruct.ClientConfig.JsArrayEventName));
                        }
                    }


                    sbTemp.Append(string.Format(TdHtml, "", sbTD));
                    sbTD.Clear();
                }

                //add the whole row
                sbHtml.Append(string.Format(TrHtmlNoAttrib, sbTemp.ToString()));
                sbTemp.Clear();
            }

            var html = new CalendarHTML
            {
                GridHTML = string.Format(TableHtml, rqStruct.TableTemplate, sbHtml.ToString()),
                NextRequest = string.Format(CalendarWeekViewLink, nextweek.Year, nextweek.Month, nextweek.Day, rqStruct.CalendarId),
                PreviousRequest = string.Format(CalendarWeekViewLink, prevweek.Year, prevweek.Month, prevweek.Day, rqStruct.CalendarId),
                Javascript = sbJavascript.ToString(),
                ViewType = Variables.CalendarViewType.WeekView
            };

            return html;
        }

        public CalendarHTML GenerateDayHTML(CalendarWeekRequestStruct rqStruct)
        {
            var sbHtml = new StringBuilder();
            var sbTemp = new StringBuilder();
            var sbTD = new StringBuilder();

            var nextday = rqStruct.DateRequested.AddDays(1);
            var prevday = rqStruct.DateRequested.AddDays(-1);
            var info = rqStruct.Info ?? _info;
            var lstEvents = rqStruct.RelatedEvents;
            var sbJavascript = new StringBuilder(" var " + rqStruct.ClientConfig.JsArrayEventName + " = new Array(); " + Environment.NewLine);
            var currentTdId = string.Empty;
            var ClientPageId = 0;
            int i = 0;

            //insert table left top edge
            sbTemp.Append(string.Format(ThHtml, @" width=""50px"" ", rqStruct.CrossEdgeContent));

            //add requested day headers
            sbTemp.Append(string.Format(ThHtmlNoAttrib, rqStruct.DateRequested
                    .ToString(rqStruct.DayHeaderFormat, info)));
            //close header row
            sbHtml.Append(string.Format(TrHtmlNoAttrib, sbTemp.ToString()));

            //clean the temp SB
            sbTemp.Clear();
            var foundEvents = new List<CalendarEventCache>();

            if (lstEvents != null)
            {
                foundEvents = lstEvents.FindAll((
                     x => (x.StartOfEvent.Date == rqStruct.DateRequested.Date || x.EndOfEvent.Date == rqStruct.DateRequested.Date)
                     )).ToList();
            }

            //add row for each hour
            foreach (var hour in _dayHours)
            {
                currentTdId = "td_" + ClientPageId + "_" + (i++);

                var eventbyhour = foundEvents.FindAll(x => x.StartOfEvent.Hour == hour || x.EndOfEvent.Hour == hour);

                foreach (var ce in eventbyhour)
                {
                    sbTD.Append(string.Format(@"<span class=""event"" id=""evt_{0}_{1}"" {2}>", ClientPageId, ce.id, rqStruct.ClientConfig.EventClickCallBack)
                        + ce.Title + "</span>");
                    sbJavascript.Append(string.Format(@" {3}['evt_{0}_{1}']=[{0},{1},'{2}'];" + Environment.NewLine, ClientPageId, ce.id, currentTdId, rqStruct.ClientConfig.JsArrayEventName));
                }



                //add hour cell
                sbTemp.Append(string.Format(TdHtml, "", +hour + " " + (hour < 12 ? LocalizedAm : LocalizedPm)));
                //add other cells              
                sbTemp.Append(string.Format(TdHtml, "", sbTD.ToString()));

                //add the whole row
                sbHtml.Append(string.Format(TrHtmlNoAttrib, sbTemp.ToString()));
                sbTemp.Clear();
                sbTD.Clear();
            }

            return new CalendarHTML
            {
                GridHTML = string.Format(TableHtml, rqStruct.TableTemplate, sbHtml.ToString()),
                NextRequest = string.Format(CalendarDayViewLink, nextday.Year, nextday.Month, nextday.Day, rqStruct.CalendarId),
                PreviousRequest = string.Format(CalendarDayViewLink, prevday.Year, prevday.Month, prevday.Day, rqStruct.CalendarId),
                ViewType = Variables.CalendarViewType.DayView
            };
        }

    }
}
