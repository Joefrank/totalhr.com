
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace totalhr.web.ViewModels
{
    public class TaskSchedulerSearchVm
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? AssignedBy { get; set; }
        public int? AssignedTo { get; set; }

        public int PageSize { get; set; }
        public int Skip { get; set; }


        public List<TaskSchedulerVM> Results { get; set; }


        public TaskSchedulerSearchVm() { }

        public void SetDefaults()
        { 
            
        }



    }
}