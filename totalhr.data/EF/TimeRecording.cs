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
    
    public partial class TimeRecording
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public System.DateTime StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public short TypeId { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public byte[] TimeStamp { get; set; }
        public Nullable<long> NoteId { get; set; }
        public int AddedById { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<int> TaskRefId { get; set; }
    
        public virtual Note Note { get; set; }
        public virtual TaskScheduler TaskScheduler { get; set; }
        public virtual TimeRecordingType TimeRecordingType { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
    }
}
