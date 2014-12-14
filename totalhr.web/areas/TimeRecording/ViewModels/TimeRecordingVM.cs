using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TRData = totalhr.data.TimeRecordingSystem.EF;

namespace totalhr.web.Areas.TimeRecording.ViewModels
{
    public class TimeRecordingVM
    {
        [Required(ErrorMessage="Please login to record time")]
        public int UserId { get; set; }

        public int CompanyId { get; set; }

        [Required(ErrorMessage="Please Enter Start Date Time")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Please Enter End Date Time")]
        public DateTime EndTime { get; set; }

        public TimeRecordingVM() { }
        public TimeRecordingVM(TRData.TimeRecording entity) 
        {
            this.StartTime = entity.StartTime;
            this.EndTime = entity.EndTime.Value;
            this.UserId = entity.UserId;
        }

        public static List<TimeRecordingVM> Build(List<TRData.TimeRecording> entities)
        {
            return entities.ConvertAll(x => new TimeRecordingVM(x));
        }

    }
}