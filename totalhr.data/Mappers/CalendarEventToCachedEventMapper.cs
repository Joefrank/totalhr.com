using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using totalhr.data.EF;

namespace totalhr.Shared.Models.Mappers
{
    public class CalendarEventToCachedEventMapper : ITypeConverter<CalendarEvent, CalendarEventCache>
    {
        public CalendarEventCache Convert(ResolutionContext context)
        {
            CalendarEventCache eventcache = new CalendarEventCache();
            CalendarEvent evtSource = (CalendarEvent)context.SourceValue;
             eventcache.id = evtSource.id;
             eventcache.Title = evtSource.Title;
             eventcache.Description = evtSource.Description;
             eventcache.CreatedBy= evtSource.CreatedBy;
             eventcache.Created       = evtSource.Created;
             eventcache.StartOfEvent = evtSource.StartOfEvent;
             eventcache.EndOfEvent = evtSource.EndOfEvent;
             eventcache.Location = evtSource.Location;
             eventcache.CalendarId = evtSource.CalendarId;
             eventcache.CalendarName = evtSource.Calendar.Name;
             eventcache.Associations = MapEventAssociations(evtSource);

             return eventcache;
        }

        public List<CalendarAssociationCache> MapEventAssociations(CalendarEvent calendarevent)
        {
            List<CalendarAssociationCache> lstAssociations = new List<CalendarAssociationCache>();
            

            foreach (CalendarAssociation assoc in calendarevent.CalendarAssociations)
            {
                 Mapper.CreateMap<CalendarAssociation, CalendarAssociationCache>().ConvertUsing<CalendarAssociationToCacheMapper>();
                 lstAssociations.Add(Mapper.Map<CalendarAssociation, CalendarAssociationCache>(assoc));
               
            }

            return lstAssociations;
        }
       
    }

    public class CalendarAssociationToCacheMapper : ITypeConverter<CalendarAssociation, CalendarAssociationCache>
    {
        public CalendarAssociationCache Convert(ResolutionContext context)
        {
            CalendarAssociationCache assocCache = new CalendarAssociationCache();
            CalendarAssociation assocSource = (CalendarAssociation)context.SourceValue;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(assocSource.AssociationValue);
            XmlNode root = doc.DocumentElement;

             assocCache.EventAssociationId  = assocSource.EventAssociationId;
             assocCache.EventId = assocSource.EventId;
             assocCache.AssociationTypeid = assocSource.AssociationTypeid;             
             assocCache.Created = assocSource.Created;
             assocCache.CreatedBy = assocSource.CreatedBy;
             if (assocSource.AssociationTypeid == (int)Variables.CalendarEventAssociationType.Attendee)
                 DecodeAttendee(root, ref assocCache);
             else if (assocSource.AssociationTypeid == (int)Variables.CalendarEventAssociationType.Repeat)
                 DecodeRepeat(root, ref assocCache);
             else if (assocSource.AssociationTypeid == (int)Variables.CalendarEventAssociationType.Reminder)
                 DecodeReminder(root, ref assocCache);

            return assocCache;
        }

        public void DecodeAttendee(XmlNode rootnode,ref CalendarAssociationCache assoccache)
        {            
            try
            {
                int typeofattendee = Int32.Parse(rootnode.SelectSingleNode("//type").InnerText);
                assoccache.SubTypeId = typeofattendee;
                assoccache.AssociationValue = rootnode.SelectSingleNode("//value").InnerText;
            }
            catch (Exception ex)
            {
                assoccache.AssociationTypeid = -1;
                assoccache.AssociationValue = "";
            }
        }

        public void DecodeRepeat(XmlNode rootnode, ref CalendarAssociationCache assoccache)
        {
            try
            {
                int typeofrepeat = Int32.Parse(rootnode.SelectSingleNode("//type").InnerText);
                assoccache.SubTypeId = typeofrepeat;
                XmlNodeList lstNodes = rootnode.SelectNodes("//date");
                foreach(XmlNode node in lstNodes)
                {
                    assoccache.AssociationValue += !string.IsNullOrEmpty(assoccache.AssociationValue)?
                        "," + node.InnerText : node.InnerText;
                }
                
            }
            catch (Exception ex)
            {
                assoccache.AssociationTypeid = -1;
                assoccache.AssociationValue = "";
            }
        }

        public void DecodeReminder(XmlNode rootnode, ref CalendarAssociationCache assoccache)
        {
            try
            {
                int typeofreminder = Int32.Parse(rootnode.SelectSingleNode("//type").InnerText);
                assoccache.SubTypeId = typeofreminder;
                assoccache.AssociationValue = rootnode.SelectSingleNode("//frequency").InnerText;
                assoccache.FrequencyType = Int32.Parse(rootnode.SelectSingleNode("//frequencytype").InnerText);
            }
            catch (Exception ex)
            {
                assoccache.AssociationTypeid = -1;
                assoccache.AssociationValue = "";
            }
        }
    }

   
}
