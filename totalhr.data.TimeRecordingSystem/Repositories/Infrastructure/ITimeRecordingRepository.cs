using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.Repositories.Infrastructure;
using totalhr.data.TimeRecordingSystem.EF;

namespace totalhr.data.TimeRecordingSystem.Repositories.Infrastructure
{

    public interface ITimeRecordingRepository : IGenericRepository<TimeRecording>
    {
        //TimeRecording Add(TimeRecording entity);

        IList<TimeRecording> GetAllTRs();

        IList<TimeRecording> Search(DateTime startDate, DateTime endDate, int skip, int take);

    }
}
