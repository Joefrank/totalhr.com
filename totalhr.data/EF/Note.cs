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
    
    public partial class Note
    {
        public Note()
        {
            this.TimeRecordings = new HashSet<TimeRecording>();
        }
    
        public long Id { get; set; }
        public string Note1 { get; set; }
        public int AddedById { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdateLog { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
        public virtual ICollection<TimeRecording> TimeRecordings { get; set; }
    }
}
