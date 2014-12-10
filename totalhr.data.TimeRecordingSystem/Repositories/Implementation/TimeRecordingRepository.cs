using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.TimeRecordingSystem.Repositories.Infrastructure;
using totalhr.data.Repositories.Implementation;
using totalhr.data.TimeRecordingSystem.EF;

namespace totalhr.data.TimeRecordingSystem.Repositories.Implementation
{
    public class TimeRecordingRepository : GenericRepository<TimeRecordingEntities, TimeRecording>, ITimeRecordingRepository
    {

    }
}
