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


        public IList<TimeRecording> Search(DateTime startDate, DateTime endDate, int userId, int skip, int take)
        {
            var results = _dbContext.TimeRecordings.Where(x => x.StartTime <= startDate && x.EndTime >= endDate);
            if (userId > 0) results = results.Where(x => x.UserId == userId);
                   results= results.OrderBy(x => x.Id)
                   .Skip(skip)
                   .Take(take);
                   return results.Count() > 0 ? SetUserAuditAddedByName(results.ToList()) : new List<TimeRecording>();
        }

        private IList<TimeRecording> SetUserAuditAddedByName(IList<TimeRecording> results)
        {
            var addedByUserIds = results.Select(x=> x.Audit.AddedBy).ToList();
            var users = _dbContext.Users.Where(x => addedByUserIds.Contains(x.id)).ToList();
            results.ToList().ForEach(result =>
            {
                result.Audit.AddedByUserName = users.FirstOrDefault(x => x.id == result.Audit.AddedBy).FullName;
            });
            return results;
        }
    }
}
