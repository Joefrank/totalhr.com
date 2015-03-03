using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Resources;
using System.ComponentModel.DataAnnotations;

namespace totalhr.Shared.Models
{
    public class AbsenceInfo
    {
        [Required(ErrorMessageResourceType = typeof(Absence), ErrorMessageResourceName = "Error_StarDate_Rq")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Absence), ErrorMessageResourceName = "Error_EndDate_Rq")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Absence), ErrorMessageResourceName = "Error_Type_Rq")]
        public int TypeId { get; set; }

        public int CreatorId { get; set; }

        public int UserId { get; set; }

        public bool IncludeSaturday { get; set; }

        public bool IncludeSunday { get; set; }

        public int StatusId { get; set; }

        public string Reason { get; set; }

    }
}
