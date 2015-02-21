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
        public long Id { get; set; }

        [Required(ErrorMessage="Please login to record time")]
        public int UserId { get; set; }
        
        [Required(ErrorMessage="Please Enter Start Date Time")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Please Enter End Date Time")]
        public DateTime EndTime { get; set; }

        public string ErrorMessage { get; private set; }

        public TimeRecordingVM() { }
        public TimeRecordingVM(TRData.TimeRecording entity)
        {
            if (entity != null)
            {
                this.Id = entity.Id;
                this.StartTime = entity.StartTime;
                this.EndTime = entity.EndTime.Value;
                this.UserId = entity.UserId;
            }
            else this.ErrorMessage = "No Records found";
        }

        public static List<TimeRecordingVM> Build(List<TRData.TimeRecording> entities)
        {
            return entities.ConvertAll(x => new TimeRecordingVM(x));
        }

    }
}