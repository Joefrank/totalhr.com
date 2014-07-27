using System.Xml;
using Calendar.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Shared;
using TEF = totalhr.data.EF;
using totalhr.data.Repositories.Implementation;
using totalhr.data.Repositories.Infrastructure;
using totalhr.data.EF;


namespace Calendar.Implementation
{
    public class CalendarManagementService : ICalendarManagementService
    {
        private readonly ICalendarRepository _calrepos;
        private readonly ICalendarEventRepository _calEventRepos;

        public CalendarManagementService(ICalendarRepository calRepos, ICalendarEventRepository calEventRepos)
        {
            _calrepos = calRepos;
            _calEventRepos = calEventRepos;
        }

        public totalhr.data.EF.Calendar GetCalendar(int calendarid)
        {
            return _calrepos.FindBy(x => x.id == calendarid).FirstOrDefault();
        }

        private DateTime AddTimeToDate(DateTime date, string formattedTime)
        {
            string[] arrTemp = formattedTime.Split(':');
            int hour= 0, minute=0, second=0;

            if(arrTemp.Length > 0)
            {
                hour = Int32.Parse(arrTemp[0]);
                minute = Int32.Parse(arrTemp[1]);
                second = (arrTemp.Length > 2) ? Int32.Parse(arrTemp[2]) : 00;
            }
            return new DateTime(date.Year, date.Month, date.Day, hour, minute, second);
        }

        public CalendarEvent CreateEvent(totalhr.Shared.Models.CalendarEventInfo info)
        {
            var cevent = new CalendarEvent
                {
                    Title = info.Title,
                    Description = info.Description,
                    StartOfEvent = AddTimeToDate(info.StartDate, info.StartTime),
                    EndOfEvent = AddTimeToDate(info.EndDate, info.EndTime),
                    CreatedBy = info.CreatedBy,
                    Location = info.Location,
                    Created = DateTime.Now,
                    CalendarId = info.CalendarId
                };

            _calEventRepos.Add(cevent);
            _calEventRepos.Save();

            //event has been created let's create associations
            if (cevent.id > 0)
            {
                //save reminders.
                var doc = new XmlDocument();
                doc.LoadXml(info.ReminderXML);
                var rootNode =  doc.DocumentElement;

                if (rootNode != null)
                {
                    foreach (XmlNode node in rootNode.ChildNodes)
                    {
                        var eventAssociation = new CalendarAssociation
                            {
                                EventId = cevent.id,
                                AssociationTypeid = (int) Variables.CalendarEventAssociationType.Reminder,
                                AssociationValue = node.OuterXml,
                                Created = DateTime.Now,
                                CreatedBy = info.CreatedBy
                            };

                        _calEventRepos.CreateEventAssociation(eventAssociation);
                    }
                }

                //save attendees. *** include notification
                if (info.TargetAttendeeGroupId > 0)
                {
                    var assocValue = string.Empty;
                   
                    if (info.TargetAttendeeGroupId == (int) Variables.CalendarEventTarget.User)
                    {
                        assocValue = string.Format("<target><type>{0}</type><value>{1}</value></target>",
                            info.TargetAttendeeGroupId,info.InvitedUserIds);                       
                    }
                    else if (info.TargetAttendeeGroupId == (int)Variables.CalendarEventAssociationType.Department)
                    {
                        assocValue = string.Format("<target><type>{0}</type><value>{1}</value></target>", 
                            info.TargetAttendeeGroupId, info.InvitedDepartmentIds);
                    }
                    else
                    {
                        assocValue = string.Format("<target><type>{0}</type></target>", info.TargetAttendeeGroupId);
                    }

                   
                        var attendeeEvtAssociation = new CalendarAssociation
                            {
                                EventId = cevent.id,
                                AssociationTypeid = (int)Variables.CalendarEventAssociationType.UserInvite,
                                AssociationValue = assocValue,
                                Created = DateTime.Now,
                                CreatedBy = info.CreatedBy
                            };

                        _calEventRepos.CreateEventAssociation(attendeeEvtAssociation);
                   

                }

                //save repeat for event if they have been specified
                if (info.RepeatType > 0)
                {
                    doc = new XmlDocument();
                    doc.LoadXml(info.RepeatXML);
                    rootNode = doc.DocumentElement;
                    
                    if (rootNode != null)
                    {
                        
                            var eventAssociation = new CalendarAssociation
                            {
                                EventId = cevent.id,
                                AssociationTypeid = (int)Variables.CalendarEventAssociationType.Repeat,
                                AssociationValue = rootNode.OuterXml,
                                Created = DateTime.Now,
                                CreatedBy = info.CreatedBy
                            };

                            _calEventRepos.CreateEventAssociation(eventAssociation);
                        
                    }
                }
               

            }

            return cevent;
        }

        public CalendarEvent GetEvent(int eventid)
        {
            return _calEventRepos.FindBy(x => x.id == eventid).FirstOrDefault();
        }

        public List<TEF.CalendarEvent> GetCalendarEvents(int calendarid)
        {
            return _calrepos.GetCalendarEvents(calendarid);
        }

        public List<TEF.Calendar> GetCompanyCalendars(int companyid)
        {
            return _calrepos.GetCompanyCalendar(companyid);
        } 

        public List<TEF.Calendar> GetUserCalendars(int userid)
        {
            
            return _calrepos.FindBy(x => x.CreatedBy == userid).ToList();
        }

        public List<TEF.CalendarEvent> GetUserCalendarEvents(int userid, int year, int month)
        {
            return _calEventRepos.GetCalendarMonthlyEventsByUser(userid, year, month);
        }

        public List<TEF.CalendarEvent> GetUserCalendarEvents(int calendarid, int userid, int year, int month)
        {
            return _calEventRepos.GetCalendarMonthlyEventsByUserAndCalendar(calendarid, userid, year, month);
        }

        public List<TEF.CalendarAssociation> GetCalendarEventInvitees(int calendareventid)
        {
            return null;
        }

        public List<TEF.CalendarAssociation> GetCalendarEventReminders(int calendareventid)
        {
            return null;
        }
    }
}
