//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace totalhr.data.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Calendar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Calendar()
        {
            this.CalendarEvents = new HashSet<CalendarEvent>();
        }
    
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<int> TemplateId { get; set; }
        public bool OpenToAll { get; set; }
        public int CompanyId { get; set; }
    
        public virtual Company Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CalendarEvent> CalendarEvents { get; set; }
    }
}
