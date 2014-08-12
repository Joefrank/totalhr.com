using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.Shared.Models
{
    public class CalendarEventReminder
    {
        public int Frequency { get; set; }

        public int FrequencyType { get; set; }

        public int ReminderType { get; set; }

        public int NotificationType { get; set; }
    }
}
