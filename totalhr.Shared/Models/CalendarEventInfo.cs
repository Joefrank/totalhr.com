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
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
       
        public string StartTime { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_EventEndDate_Rq")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        
        public string EndTime { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Event_UserNotLogged")]
        public int CreatedBy { get; set; }

        public string UserCulture { get; set; }

        public List<CalendarEventReminder> Reminders { get; set; }

        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Event_Target_Required")]
        public int TargetAttendeeGroupId { get; set; }
                
        public List<int> TargetAttendeeIdList { get; set; }
       

        public int CalendarId { get; set; }

        public string CalendarName { get; set; }

        public int RepeatType { get; set; }

        public DateTime RepeatDate { get; set; }

        public string RepeatXML { get; set; }

        public int CompanyId { get; set; }


    }
}
