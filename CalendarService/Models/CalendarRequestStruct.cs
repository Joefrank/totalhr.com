using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;

namespace Calendar.Models
{
    public class CalendarRequestStruct 
    {
        public CultureInfo Info { get; set; }

        public string TableTemplate { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int CalendarId { get; set; }

        public List<CalendarEvent> RelatedEvents { get; set; }

        public ClientScriptConfig ClientConfig { get; set; }
    }
}
