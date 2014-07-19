using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using totalhr.Resources;

namespace totalhr.Shared.Models
{
    public class LoginStruct
    {
        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Username_Rq")]
        public string UserName { get; set; }
        [Required(ErrorMessageResourceType = typeof(FormMessages), ErrorMessageResourceName = "Error_Password_Rq")]
        public string Password { get; set; }
        public int UserType { get; set; }
    }
}
