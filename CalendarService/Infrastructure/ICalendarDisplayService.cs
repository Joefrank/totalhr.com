using Calendar.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Infrastructure
{
    public interface ICalendarDisplayService
    {
        CalendarHTML GenerateYearHTML(CalendarRequestStruct rqStruct);

        CalendarHTML GenerateCalendarHTML(CalendarRequestStruct rqStruct);

        CalendarHTML GenerateWeekHTML(CalendarWeekRequestStruct rqStruct);

        CalendarHTML GenerateDayHTML(CalendarWeekRequestStruct rqStruct);

        string[] GetWeekDaysByName(CultureInfo info);
    }
}
