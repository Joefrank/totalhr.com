using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TRData = totalhr.data.TimeRecordingSystem.EF;

namespace totalhr.web.ViewModels
{
    public class TimeRecordingDetailsVM : TimeRecordingVM
    {
        public string AddedByUser { get; set; }
        public string TimeRecordingType { get; set; }

        public string RecordedDuration 
        {
            get 
            {
                var timeDiff = EndTime.Subtract(StartTime);
                return string.Concat(timeDiff.Days, " days ", timeDiff.Hours, " hrs ",timeDiff.Minutes , " mins ",timeDiff.Seconds , " secs ");
            } 
        }


        public TimeRecordingDetailsVM() { }
        public TimeRecordingDetailsVM(TRData.TimeRecording entity):base(entity)
        {
            if(entity != null)
            {
                this.AddedByUser = entity.Audit.AddedByUserName;
                this.TimeRecordingType = entity.TimeRecordingType.Type;
            }
        }

        public static List<TimeRecordingDetailsVM> Build(List<TRData.TimeRecording> entities)
        {
            return entities.ConvertAll(x => new TimeRecordingDetailsVM(x));
        }

    }
}