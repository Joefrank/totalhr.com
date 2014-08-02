using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.Shared.Models
{
    public class CalendarEventCache
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime Created { get; set; }       
        public System.DateTime StartOfEvent { get; set; }
        public System.DateTime EndOfEvent { get; set; }
        public string Location { get; set; }
        public int CalendarId { get; set; }
        public string CalendarName { get; set; }
        public List<CalendarAssociationCache> Associations { get; set; }
    }

    public class CalendarAssociationCache
    {
        public int EventAssociationId { get; set; }
        public int EventId { get; set; }
        public int AssociationTypeid { get; set; }
        public int SubTypeId { get; set; }
        public string AssociationValue { get; set; }
        public int FrequencyType { get; set; }
        public System.DateTime Created { get; set; }
        public int CreatedBy { get; set; }
    }
   
}
