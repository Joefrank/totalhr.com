using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Resources;

namespace totalhr.Shared.Models
{
    public class CalendarInfo
    {
        [Required(ErrorMessageResourceType = typeof(Calendar), ErrorMessageResourceName = "Error_CalendarName_Req")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Calendar), ErrorMessageResourceName = "Error_CalendarName_Too_Long")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessageResourceType = typeof(Calendar), ErrorMessageResourceName = "Error_Calendar_Description_Too_Long")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(Calendar), ErrorMessageResourceName = "Error_Template_Req")]
        public int TemplateId { get; set; }

        public bool OpenToAll { get; set; }
        
        public int UserId { get; set; }

        public int CompanyId { get; set; }
    }
}
