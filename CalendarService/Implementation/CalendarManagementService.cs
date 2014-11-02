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
using totalhr.Shared.Models;


namespace Calendar.Implementation
{
    public class CalendarManagementService : ICalendarManagementService
    {
        private readonly ICalendarRepository _calrepos;
        private readonly ICalendarEventRepository _calEventRepos;
        private const string ReminderAssocFormat = @"<reminder><type>{0}</type><frequencytype>{1}</frequencytype><frequency>{2}</frequency><notification>{3}</notification></reminder>";

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
                    StartOfEvent = info.StartDate,
                    EndOfEvent = info.EndDate,
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
               
                if (info.Reminders != null && info.Reminders.Count > 0)
                {
                    foreach (CalendarEventReminder reminder in info.Reminders)
                    {
                        var eventAssociation = new CalendarAssociation
                            {
                                EventId = cevent.id,
                                AssociationTypeid = (int) Variables.CalendarEventAssociationType.Reminder,
                                AssociationValue = string.Format(ReminderAssocFormat, reminder.ReminderType, 
                                reminder.FrequencyType, reminder.Frequency, reminder.NotificationType),
                                Created = DateTime.Now,
                                CreatedBy = info.CreatedBy
                            };

                        _calEventRepos.CreateEventAssociation(eventAssociation);
                    }

                    //adds entry for reminders to be scheduled.
                    _calEventRepos.RequestEventRemindersSceduling(cevent, info.CompanyId);
                }

                //save attendees. *** include notification
                if (info.TargetAttendeeGroupId > 0)
                {
                    var assocValue = string.Empty;
                    var targetlist = string.Empty;

                    if (info.TargetAttendeeGroupId == (int) Variables.CalendarEventTarget.User || 
                        info.TargetAttendeeGroupId == (int)Variables.CalendarEventTarget.Department)
                    {
                         targetlist =  string.Join<int>(",", info.TargetAttendeeIdList);                    
                    }

                    assocValue = string.Format("<target><type>{0}</type><value>{1}</value></target>", info.TargetAttendeeGroupId, targetlist);
                   
                                       
                        var attendeeEvtAssociation = new CalendarAssociation
                            {
                                EventId = cevent.id,
                                AssociationTypeid = (int)Variables.CalendarEventAssociationType.Attendee,
                                AssociationValue = assocValue,
                                Created = DateTime.Now,
                                CreatedBy = info.CreatedBy
                            };

                        _calEventRepos.CreateEventAssociation(attendeeEvtAssociation);
                   

                }

                //***save repeat for event if they have been specified
                if (info.RepeatType > 0)
                {
                    var assocvalue = string.Empty;
                    var temp = new StringBuilder();

                    if (info.RepeatType == (int) Variables.RepeatType.OnDates
                        || info.RepeatType == (int)Variables.RepeatType.MonthlyOnDates)
                    {
                        foreach (string rdate in info.RepeatDates)
                        {
                            temp.Append(string.Format("<date>{0}</date>", rdate));
                        }
                       
                    }else if (info.RepeatType == (int) Variables.RepeatType.DailyMonToFri
                        || info.RepeatType == (int)Variables.RepeatType.OnDayOfTheWeek)
                    {
                        temp.Append("<date>" +  info.RepeatUntil + "</date>"); 
                    }
                    else if (info.RepeatType == (int) Variables.RepeatType.YearlyOnSameDate)
                    {
                        foreach (int year in info.RepeatYears)
                        {
                            temp.Append(string.Format("<date>{0}</date>", year));
                        }
                       
                    }
                  
                  assocvalue = string.Format("<repeat><type>{0}</type><dates>{1}</dates></repeat>", info.RepeatType, temp.ToString());

                    
                            var eventAssociation = new CalendarAssociation
                            {
                                EventId = cevent.id,
                                AssociationTypeid = (int)Variables.CalendarEventAssociationType.Repeat,
                                AssociationValue = assocvalue,
                                Created = DateTime.Now,
                                CreatedBy = info.CreatedBy
                            };

                            _calEventRepos.CreateEventAssociation(eventAssociation);
                        
                   
                }
               

            }

            return cevent;
        }

        public CalendarEvent SaveEvent(totalhr.Shared.Models.CalendarEventInfo info)
        {
            CalendarEvent cevent = _calEventRepos.FindBy(x => x.id == info.EventId).FirstOrDefault();
            
            if (cevent != null)
            {
                cevent.Title = info.Title;
                cevent.Description = info.Description;
                cevent.StartOfEvent = info.StartDate;
                cevent.EndOfEvent = info.EndDate;
                cevent.Location = info.Location;
                cevent.LastModifed = DateTime.Now;
                cevent.LastModifiedBy = info.LastModifiedBy;

                //*** check attendees before clearing for notification
                _calEventRepos.DeleteEventAssociation(cevent);
                //cevent.CalendarAssociations.Clear();
                _calEventRepos.Save();
                
                //***delete all old reminder scheduled tasks

                if (info.Reminders != null && info.Reminders.Count > 0)
                {
                    foreach (CalendarEventReminder reminder in info.Reminders)
                    {
                        var eventAssociation = new CalendarAssociation
                        {
                            EventId = cevent.id,
                            AssociationTypeid = (int)Variables.CalendarEventAssociationType.Reminder,
                            AssociationValue = string.Format(ReminderAssocFormat, reminder.ReminderType,
                            reminder.FrequencyType, reminder.Frequency, reminder.NotificationType),
                            Created = DateTime.Now,
                            CreatedBy = info.LastModifiedBy
                        };

                        cevent.CalendarAssociations.Add(eventAssociation);
                    }
                    //*** create new reminder schedules for event _calEventRepos.RequestEventRemindersSceduling(cevent, info.CompanyId);
                }

                    //save attendees.
                if (info.TargetAttendeeGroupId > 0)
                {
                    var assocValue = string.Empty;
                    var targetlist = string.Empty;

                    if (info.TargetAttendeeGroupId == (int)Variables.CalendarEventTarget.User ||
                        info.TargetAttendeeGroupId == (int)Variables.CalendarEventTarget.Department)
                    {
                        targetlist = string.Join<int>(",", info.TargetAttendeeIdList);
                    }

                    assocValue = string.Format("<target><type>{0}</type><value>{1}</value></target>", info.TargetAttendeeGroupId, targetlist);


                    var attendeeEvtAssociation = new CalendarAssociation
                    {
                        EventId = cevent.id,
                        AssociationTypeid = (int)Variables.CalendarEventAssociationType.Attendee,
                        AssociationValue = assocValue,
                        Created = DateTime.Now,
                        CreatedBy = info.CreatedBy
                    };

                    cevent.CalendarAssociations.Add(attendeeEvtAssociation);
                }

                //***save repeat for event if they have been specified
                if (info.RepeatType > 0)
                {
                    var assocvalue = string.Empty;
                    var temp = new StringBuilder();

                    if (info.RepeatType == (int)Variables.RepeatType.OnDates
                        || info.RepeatType == (int)Variables.RepeatType.MonthlyOnDates)
                    {
                        foreach (string rdate in info.RepeatDates)
                        {
                            temp.Append(string.Format("<date>{0}</date>", rdate));
                        }

                    }
                    else if (info.RepeatType == (int)Variables.RepeatType.DailyMonToFri
                       || info.RepeatType == (int)Variables.RepeatType.OnDayOfTheWeek)
                    {
                        temp.Append("<date>" + info.RepeatUntil + "</date>");
                    }
                    else if (info.RepeatType == (int)Variables.RepeatType.YearlyOnSameDate)
                    {
                        foreach (int year in info.RepeatYears)
                        {
                            temp.Append(string.Format("<date>{0}</date>", year));
                        }

                    }

                    assocvalue = string.Format("<repeat><type>{0}</type><dates>{1}</dates></repeat>", info.RepeatType, temp.ToString());


                    var eventAssociation = new CalendarAssociation
                    {
                        EventId = cevent.id,
                        AssociationTypeid = (int)Variables.CalendarEventAssociationType.Repeat,
                        AssociationValue = assocvalue,
                        Created = DateTime.Now,
                        CreatedBy = info.CreatedBy
                    };

                    cevent.CalendarAssociations.Add(eventAssociation);
                }
            }

            _calEventRepos.Save();
            return cevent;
        }

        public CalendarEventInfo GetEventInfo(int eventid)
        {
            var evt = _calEventRepos.FindBy(x => x.id == eventid).FirstOrDefault();
            var associations = _calEventRepos.GetCalendarEventAssociations(eventid);

            if (evt != null)
            {
                var calEventInfo = new CalendarEventInfo
                {
                    EventId = evt.id,
                    Title = evt.Title,
                    Location = evt.Location,
                    Description = evt.Description,
                    StartDate = evt.StartOfEvent,
                    EndDate = evt.EndOfEvent,
                    CreatedBy = evt.CreatedBy                    
                };

                foreach (CalendarAssociation assoc in associations)
                {
                    //calEventInfo
                    switch (assoc.AssociationTypeid)
                    {
                        case (int)Variables.CalendarEventAssociationType.Attendee :
                            calEventInfo = MakeAttendees(assoc, calEventInfo); break;

                        case (int)Variables.CalendarEventAssociationType.Reminder :
                            calEventInfo = MakeReminders(assoc, calEventInfo); break;

                        case (int)Variables.CalendarEventAssociationType.Repeat:
                            calEventInfo = MakeRepeats(assoc, calEventInfo); break;
                    }
                }

                return calEventInfo; 
            }
            else { return null; }
        }

        private CalendarEventInfo MakeAttendees(CalendarAssociation assoc, CalendarEventInfo info)
        {
            var doc = new XmlDocument();
            doc.LoadXml(assoc.AssociationValue);
            int attendeeTypeId = 0;
            var root = doc.DocumentElement;
            var attendeeTypeNode = root.SelectSingleNode("//type");

            if (attendeeTypeNode != null)
            {
                Int32.TryParse(attendeeTypeNode.InnerText, out attendeeTypeId);
                if (attendeeTypeId > 0)
                {
                    info.TargetAttendeeGroupId = attendeeTypeId;
                    if (attendeeTypeId == (int)Variables.CalendarEventTarget.User ||
                        attendeeTypeId == (int)Variables.CalendarEventTarget.Department)
                    {
                        var csv = root.SelectSingleNode("//value").InnerText;
                        var lst = csv.Split(',').Select(n => int.Parse(n)).ToList();
                        info.TargetAttendeeIdList = lst;
                    }
                    else { info.TargetAttendeeIdList = new List<int>(); }                                     
                }
            }
            return info;

        }

        private CalendarEventInfo MakeReminders(CalendarAssociation assoc, CalendarEventInfo info)
        {
            var doc = new XmlDocument();
            doc.LoadXml(assoc.AssociationValue);
            int reminderTypeId = 0;
            var root = doc.DocumentElement;
            var reminderTypeNode = root.SelectSingleNode("//type");

            if (reminderTypeNode != null)
            {
                Int32.TryParse(reminderTypeNode.InnerText, out reminderTypeId);
                if (reminderTypeId > 0)
                {
                    var reminder = new CalendarEventReminder
                    {
                        ReminderType =reminderTypeId,
                        Frequency = Convert.ToInt32(root.SelectSingleNode("//frequency").InnerText),
                        FrequencyType = Convert.ToInt32(root.SelectSingleNode("//frequencytype").InnerText),
                        NotificationType = Convert.ToInt32(root.SelectSingleNode("//notification").InnerText)
                    };

                    if (info.Reminders == null)
                    {
                        info.Reminders = new List<CalendarEventReminder>();
                    }
                    info.Reminders.Add(reminder);                    
                }
            }
            return info;

        }

        private CalendarEventInfo MakeRepeats(CalendarAssociation assoc, CalendarEventInfo info)
        {
            var doc = new XmlDocument();
            doc.LoadXml(assoc.AssociationValue);
            int repeatTypeId = 0;
            var root = doc.DocumentElement;
            var repeatTypeNode = root.SelectSingleNode("//type");


            if (info.RepeatType != null)
            {
                var assocvalue = string.Empty;
                var temp = new StringBuilder();
                Int32.TryParse(repeatTypeNode.InnerText, out repeatTypeId);
                info.RepeatType = repeatTypeId;

                //list of chosen repeat dates
                if (repeatTypeId == (int)Variables.RepeatType.OnDates || repeatTypeId == (int)Variables.RepeatType.MonthlyOnDates)
                {
                    var lst = root.SelectNodes("//date");
                    foreach (XmlNode node in lst)
                    {
                        info.RepeatDates.Add(node.InnerText);
                    }
                }
                    //repeat until date
                else if (repeatTypeId == (int)Variables.RepeatType.DailyMonToFri || repeatTypeId == (int)Variables.RepeatType.OnDayOfTheWeek)
                {
                    info.RepeatUntil = Convert.ToDateTime(root.SelectSingleNode("//date").InnerText);
                }
                else if (repeatTypeId == (int)Variables.RepeatType.YearlyOnSameDate)//list of repeat years
                {
                    var lst = root.SelectNodes("//date");
                    foreach (XmlNode node in lst)
                    {
                        info.RepeatYears.Add(Int32.Parse(node.InnerText));
                    }

                }
            }

            return info;
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
               

        public List<CalendarEventCache> GetUserCalendarEvents(int userid, int year, int month, int calendarid=0)
        {
            return _calEventRepos.GetMonthlyCalendarEvents( userid, year,month, calendarid);
        }
        
        public List<CalendarEventCache> GetUserDayCalendarEvents(int userid, DateTime date, int calendarid=0)
        {
            return _calEventRepos.GetCalendarDailyEventsByUser(userid, date, calendarid);
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
