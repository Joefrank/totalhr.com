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
    
    public partial class FormField
    {
        public FormField()
        {
            this.CTSectionFieldLinks = new HashSet<CTSectionFieldLink>();
        }
    
        public int id { get; set; }
        public int FormControlId { get; set; }
        public Nullable<int> LabelId { get; set; }
        public Nullable<int> TooltipId { get; set; }
        public System.DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public Nullable<int> LastUpdatedBy { get; set; }
        public bool obsolete { get; set; }
    
        public virtual ICollection<CTSectionFieldLink> CTSectionFieldLinks { get; set; }
    }
}