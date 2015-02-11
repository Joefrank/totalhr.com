using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using totalhr.Resources;

namespace totalhr.Shared.Models
{
    public class UserContractInfo
    {
        [Required(ErrorMessageResourceType = typeof(Contract), ErrorMessageResourceName = "Error_User_Req")]
        public int UserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Contract), ErrorMessageResourceName = "Error_Template_Rq")]
        public int TemplateId { get; set; }

        public int ContractId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Contract), ErrorMessageResourceName = "Error_Admin_UserId_Rq")]
        public int CreatedBy { get; set; }

    }
}