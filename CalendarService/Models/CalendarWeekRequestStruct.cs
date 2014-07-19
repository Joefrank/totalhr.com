using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Models
{
    public class CalendarWeekRequestStruct : CalendarRequestStruct
    {
        public DateTime DateRequested { get; set; }

        public string DayHeaderFormat { get; set; }

        public string CrossEdgeContent { get; set; }       
    }
}
