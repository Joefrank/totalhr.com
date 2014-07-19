﻿using Calendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Infrastructure
{
    public interface ICalendarService
    {
        CalendarHTML GenerateCalendarHTML(CalendarRequestStruct rqStruct);

        CalendarHTML GenerateWeekHTML(CalendarWeekRequestStruct rqStruct);

        CalendarHTML GenerateDayHTML(CalendarWeekRequestStruct rqStruct);
    }
}
