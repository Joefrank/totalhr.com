using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.TimeRecordingSystem.Repositories.Infrastructure;
using totalhr.data.Repositories.Implementation;
using totalhr.data.TimeRecordingSystem.EF;
using System.Data.Entity;

namespace totalhr.data.TimeRecordingSystem.Repositories.Implementation
{
    public class TimeRecordingRepository : GenericRepository<TimeRecordingEntities, TimeRecording>, ITimeRecordingRepository
    {
        private TimeRecordingEntities _dbContext = new TimeRecordingEntities();
    //public class TimeRecordingRepository :  ITimeRecordingRepository
    
        //public TimeRecording Add (TimeRecording entity)
        //{
        //    using(var context = new TimeRecordingEntities())
        //    {
        //        context.TimeRecordings.Add(entity);
        //        context.SaveChanges();
        //    }
        //    return entity;
        //}

        public IList<TimeRecording> GetAllTRs()
        {
            using (var context = new TimeRecordingEntities())
            {
                return context.TimeRecordings.ToList();
            }

        }

        public IList<TimeRecording> Search(DateTime startDate, DateTime endDate, int skip, int take )
        {
            
                var results = _dbContext.TimeRecordings.Where(x => x.StartTime <= startDate && x.EndTime >= endDate)
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(take);
                return results.Count() > 0 ? results.ToList() : new List<TimeRecording>();
            
        }
    }
}
