using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.Shared.Models
{
    public class CalendarEventReminder
    {
        public DateTime StartDate { get; set; }

        public int Frequency { get; set; }

        public string FrequencyType { get; set; }

        public int CreatedBy { get; set; }

        public string MessageTemplateId { get; set; }

        public int ReminderType { get; set; }
    }
}
