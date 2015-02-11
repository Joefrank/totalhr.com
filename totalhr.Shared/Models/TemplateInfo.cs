using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Resources;

namespace totalhr.Shared.Models
{
    public class TemplateInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Contract), ErrorMessageResourceName = "Error_Template_Name_Rq")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(Contract), ErrorMessageResourceName = "Error_Template_Creator_Rq")]
        public int CreatedBy { get; set; }

        [Required(ErrorMessageResourceType = typeof(Contract), ErrorMessageResourceName = "Error_Template_Form_Rq")]
        public int FormId { get; set; }

        public int? LastUpdatedBy { get; set; }

        public DateTime? LastUpdated { get; set; }
    }
}
