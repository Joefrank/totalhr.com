using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace totalhr.web.Areas.TimeRecording.ViewModels
{
    public class TimeRecordingViewModel
    {
        [Required(ErrorMessage="Please login to record time")]
        public int UserId { get; set; }

        public int CompanyId { get; set; }

        [Required(ErrorMessage="Please Enter Start Date Time")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Please Enter End Date Time")]
        public DateTime EndTime { get; set; }

    }
}