using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace totalhr.web.ViewModels
{
    public class TaskSchedulerVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Description { get; set; }

        public int DepartmentId { get; set; }
        public int AssignedBy { get; set; }
        public int AssignedTo { get; set; }

        

    }
}