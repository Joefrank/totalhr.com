//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace totalhr.data.TimeRecording
{
    using System;
    using System.Collections.Generic;
    
    public partial class TaskScheduler
    {
        public TaskScheduler()
        {
            this.TimeRecordings = new HashSet<TimeRecording>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DepartmentId { get; set; }
        public bool ApprovalNeeded { get; set; }
        public Nullable<bool> Approved { get; set; }
        public int AssignedBy { get; set; }
        public int AssignedTo { get; set; }
        public Nullable<System.DateTime> ScheduledDateTime { get; set; }
        public string ScheduleLog { get; set; }
        public int AddedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
        public virtual User User3 { get; set; }
        public virtual User User4 { get; set; }
        public virtual ICollection<TimeRecording> TimeRecordings { get; set; }
    }
}
