using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;
using System.Web;
using totalhr.Shared.Infrastructure;
using totalhr.Shared;
using totalhr.Shared.Models;
using AutoMapper;
using totalhr.Shared.Models.Mappers;

namespace totalhr.data.Repositories.Implementation
{
    public class CalendarEventRepository : GenericRepository<TotalHREntities, CalendarEvent>, ICalendarEventRepository
    {
        const string calendarEventCacheKey = "AllCalendarEvents";
        private static readonly object CacheLockObject = new object();
        int _defaultCacheDuration = 60;

        public ICacheHelper CacheHelper { get; set; }
        public int CacheDuration { get { return _defaultCacheDuration; } set { _defaultCacheDuration = value; } }

        /** move this to injection */
        public void SetCacheDuration(int duration)
        {
            CacheDuration = duration;
        }

        /** move this to injection */
        public void SetCachHelper(ICacheHelper helper)
        {
            CacheHelper = helper;
        }

        public CalendarEventRepository()
        {
            if (CacheHelper == null)
                CacheHelper = new HttpCacheHelper();
        }

       

        //we need to get details of user or companyid, we might bind cache object to companyid
        public List<CalendarEventCache> GetAllCalendarEvents(int companyId, int calendarid = 0)
        {
            var result = CacheHelper.Get<List<CalendarEventCache>>(calendarEventCacheKey);

            if (result == null)
            {
                lock (CacheLockObject)
                {
                    result = new List<CalendarEventCache>();
                    var allevents = this.FindBy(x => (companyId == x.Calendar.CompanyId && (calendarid == 0 || x.CalendarId == calendarid))).ToList();
                    //use mapping to map event to eventcache

                    Mapper.CreateMap<CalendarEvent, CalendarEventCache>().ConvertUsing<CalendarEventToCachedEventMapper>();
                    //put loop here
                    foreach (CalendarEvent calevent in allevents)
                    {
                        result.Add(Mapper.Map<CalendarEvent, CalendarEventCache>(calevent));
                    }

                    CacheHelper.Add<List<CalendarEventCache>>(result, calendarEventCacheKey);                    
                }
            }
           
            return result;
        }

        public void ClearCache()
        {
            CacheHelper.Clear(calendarEventCacheKey);
        }

        public void DeleteEventAssociation(CalendarEvent evt)
        {
            foreach (CalendarAssociation assocs in evt.CalendarAssociations)
            {
                Context.CalendarAssociations.Remove(assocs);
            }
            Context.SaveChanges();
        }

        public void RequestEventRemindersSceduling(CalendarEvent evt, int companyid)
        {
            EventToSchedule evtToSchedule = new EventToSchedule();
            evtToSchedule.EventId = evt.id;
            evtToSchedule.CompanyId = companyid;
            evtToSchedule.RecipientListName = string.Format("Calendar event reminder recipient list for Event Id # {0}", evt.id);            
            evtToSchedule.CreatedBy = evt.CreatedBy;
            evtToSchedule.Created = DateTime.Now;

            Context.EventToSchedules.Add(evtToSchedule);
            Context.SaveChanges();
        }

        public List<CalendarEventCache> GetMonthlyCalendarEvents(int userid, int year, int month, int calendarid=0)
        {
            
            var foundEvents = new List<CalendarEventCache>();
            var user = Context.Users.FirstOrDefault(x => x.id == userid);
            var lstEvents = GetAllCalendarEvents(user.CompanyId);

            foreach (CalendarEventCache assocevt in lstEvents)
            {
                if (
                    (calendarid == 0 || assocevt.CalendarId == calendarid)
                    &&
                    (month == 0 || (assocevt.StartOfEvent.Month == month || assocevt.EndOfEvent.Month == month))
                    &&
                    (assocevt.StartOfEvent.Year == year || assocevt.EndOfEvent.Year == year)
                  )
                {
                    //check event associations
                    if(assocevt.Associations != null && assocevt.Associations.Count > 0){
                        
                        var targetassoc = assocevt.Associations.FirstOrDefault
                            (x => x.AssociationTypeid == (int)Variables.CalendarEventAssociationType.Attendee);
                        
                        //there must be some invite or don't show event
                        if(targetassoc == null){
                            continue;
                        }

                        if (CanUserViewEvent(targetassoc, userid, assocevt.CreatedBy, user.departmentid))
                        {
                            foundEvents.Add(assocevt);
                        }
                    }
                }
            }

            return foundEvents;           

        }

        private bool CanUserViewEvent(CalendarAssociationCache targetassoc, int userid, int creatorid, int departmentid)
        {
            var assocvalue = targetassoc.AssociationValue;

            return (
                      targetassoc.SubTypeId == (int)Variables.CalendarEventTarget.Company ||
                      (targetassoc.SubTypeId == (int)Variables.CalendarEventTarget.MyselfOnly && creatorid == userid) ||
                      (targetassoc.SubTypeId == (int)Variables.CalendarEventTarget.User && assocvalue.Split(',').Select(int.Parse).ToList().Contains(userid)) ||
                      (targetassoc.SubTypeId == (int)Variables.CalendarEventTarget.User && userid == creatorid) ||
                      (targetassoc.SubTypeId == (int)Variables.CalendarEventTarget.Department && assocvalue.Split(',').Select(int.Parse).ToList().Contains(departmentid))
                   );
        }

       

        public List<CalendarAssociation> GetCalendarEventAssociations(int eventid)
        {
            return this.Context.CalendarAssociations.Where(x => x.EventId == eventid).ToList();
        }

        public List<CalendarEventCache> GetCalendarDailyEventsByUser(int userid, DateTime date, int calendarid=0){
            
            var foundEvents = new List<CalendarEventCache>();
            var user = Context.Users.FirstOrDefault(x => x.id == userid);
            var lstEvents = GetAllCalendarEvents(user.CompanyId);

            foreach (CalendarEventCache assocevt in lstEvents)
            {
                if (
                    (calendarid == 0 || assocevt.CalendarId == calendarid)
                    &&
                    (assocevt.StartOfEvent.Date == date.Date || assocevt.EndOfEvent.Date == date.Date)
                  )
                {
                    //check event associations
                    if (assocevt.Associations != null && assocevt.Associations.Count > 0)
                    {

                        var targetassoc = assocevt.Associations.FirstOrDefault
                            (x => x.AssociationTypeid == (int)Variables.CalendarEventAssociationType.Attendee);

                        //there must be some invite or don't show event
                        if (targetassoc == null)
                        {
                            continue;
                        }

                        if (CanUserViewEvent(targetassoc, userid, assocevt.CreatedBy, user.departmentid))
                        {
                            foundEvents.Add(assocevt);
                        }
                    }
                }
            }

            return foundEvents;
        }

        
        public CalendarAssociation CreateEventAssociation(CalendarAssociation assoc)
        {
            this.Context.CalendarAssociations.Add(assoc);
            this.Context.SaveChanges();
            return assoc;
        }

    }
}
