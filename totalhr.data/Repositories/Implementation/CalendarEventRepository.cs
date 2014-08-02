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
        string calendarEventCacheKey = "AllCalendarEvents";
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

        public List<CalendarEventCache> GetAllCalendarEvents(int calendarid = 0)
        {
            var result = CacheHelper.Get<List<CalendarEventCache>>(calendarEventCacheKey);

            if (result == null)
            {
                lock (CacheLockObject)
                {
                    result = new List<CalendarEventCache>();
                    var allevents = this.FindBy(x => (calendarid == 0 || x.CalendarId == calendarid)).ToList();
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

        private List<CalendarEvent> List<T1>(IQueryable<CalendarEvent> queryable)
        {
            throw new NotImplementedException();
        }

        //add filtering by user
        public List<CalendarEvent> GetCalendarMonthlyEventsByUser(int userid, int year, int month)
        {
            return FindBy(x =>
                (x.StartOfEvent.Month == month || x.EndOfEvent.Month == month)
                &&
                (x.StartOfEvent.Year == year || x.EndOfEvent.Year == year)
                ).ToList();                

        }

        //add filtering by user
        public List<CalendarEvent> GetCalendarMonthlyEventsByUserAndCalendar(int calendarId, int userid, int year, int month)
        {
            return FindBy(x => x.CalendarId == calendarId &&
                (x.StartOfEvent.Month == month || x.EndOfEvent.Month == month)
                &&
                (x.StartOfEvent.Year == year || x.EndOfEvent.Year == year)
                ).ToList();

        }

        public List<CalendarEventCache> GetMonthlyCalendarEvents(int userid, int year, int month)
        {
            var lstEvents = GetAllCalendarEvents();
            var user = Context.Users.FirstOrDefault(x => x.id == userid);

            return lstEvents.FindAll(x =>  
                (x.StartOfEvent.Month == month || x.EndOfEvent.Month == month)
                && (x.StartOfEvent.Year == year || x.EndOfEvent.Year == year)
                && //check that event is for all company or user personal event or user department or in list of invited users
                (
                x.CreatedBy == userid //is it my own event
                ||
                    x.Associations.Count(a =>                     
                        (a.AssociationTypeid == (int)Variables.CalendarEventAssociationType.Attendee)
                            &&
                        (
                            a.SubTypeId == (int)Variables.CalendarEventTarget.Company ||
                            (a.SubTypeId == (int)Variables.CalendarEventTarget.MyselfOnly && x.CreatedBy == userid) ||                         
                            (a.SubTypeId == (int)Variables.CalendarEventTarget.User && a.AssociationValue.Split(',').Select(int.Parse).ToList().Contains(userid)) ||
                            (a.SubTypeId == (int)Variables.CalendarEventTarget.Department && a.AssociationValue.Split(',').Select(int.Parse).ToList().Contains(user.departmentid))
                         )
                        ) > 0
                    )
                );
        }

        public List<CalendarEventCache> GetMonthlyCalendarEvents(int calendarId, int userid, int year, int month)
        {
            var lstEvents = GetAllCalendarEvents();           
            var user = Context.Users.FirstOrDefault(x => x.id == userid);

            return lstEvents.FindAll(x =>  x.CalendarId == calendarId &&
                (x.StartOfEvent.Month == month || x.EndOfEvent.Month == month) &&
                (x.StartOfEvent.Year == year || x.EndOfEvent.Year == year) &&
                 
                (
                x.Associations == null ||
                (
                x.CreatedBy == userid //is it my own event
                ||

                    x.Associations.Select(a => 
                        (a.AssociationTypeid == (int)Variables.CalendarEventAssociationType.Attendee)
                            &&
                        (
                            a.SubTypeId == (int)Variables.CalendarEventTarget.Company ||
                            (a.SubTypeId == (int)Variables.CalendarEventTarget.MyselfOnly && x.CreatedBy == userid) ||
                            (a.SubTypeId == (int)Variables.CalendarEventTarget.User && a.AssociationValue.Split(',').Select(int.Parse).ToList().Contains(userid)) ||
                            (a.SubTypeId == (int)Variables.CalendarEventTarget.Department && a.AssociationValue.Split(',').Select(int.Parse).ToList().Contains(user.departmentid))
                         )
                        ).Count() >0
                  )
                  )
                );

        }

        public CalendarAssociation CreateEventAssociation(CalendarAssociation assoc)
        {
            this.Context.CalendarAssociations.Add(assoc);
            this.Context.SaveChanges();
            return assoc;
        }

        public List<CalendarAssociation> GetCalendarEventAssociations(int eventid)
        {
            return this.Context.CalendarAssociations.Where(x => x.EventId == eventid).ToList();
        }

        public List<CalendarEventCache> GetCalendarDailyEventsByUser(int userid, DateTime date, int calendarid=0){
            var lstEvents = GetAllCalendarEvents();

            return lstEvents.FindAll(x =>
                (x.StartOfEvent.Date == date.Date || 
                    x.EndOfEvent.Date == date.Date)
                && (calendarid == 0 || x.CalendarId == calendarid)
                ); 
        }

    }
}
