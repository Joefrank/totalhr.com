using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Resources;

namespace totalhr.Shared.Models
{
    public class CalendarEventInfo
    {
        public int EventId { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_EventTitle_Rq")]
        public string Title { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_EventStartDate_Rq")]       
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

       
        public string StartTime { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_EventEndDate_Rq")]
        public DateTime EndDate { get; set; }

        
        public string EndTime { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Event_UserNotLogged")]
        public int CreatedBy { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Event_Target_Required")]
        public int TargetAttendeeGroupId { get; set; }

        public string InvitedUserIds { get; set; }

        public string InvitedDepartmentIds { get; set; }

        public bool NewlyCreated { get; set; }

        public List<CalendarEventReminder> Reminders { get; set; }

        public int ReminderType { get; set; }

        public int ReminderFrequencyType { get; set; }

        public int ReminderFrequency { get; set; }

        public string ReminderXML { get; set; }

        public int CalendarId { get; set; }

        public string CalendarName { get; set; }

        public int RepeatType { get; set; }

        public DateTime RepeatDate { get; set; }

        public string RepeatXML { get; set; }

        public string RepeatValue { get; set; }
        
    }
}
