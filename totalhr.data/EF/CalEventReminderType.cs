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
    
    public partial class CalEventReminderType
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int frequency { get; set; }
        public int frequencytype { get; set; }
        public System.DateTime created { get; set; }
        public int createdby { get; set; }
        public Nullable<System.DateTime> lastmodified { get; set; }
        public Nullable<int> lastmodifiedby { get; set; }
        public bool obsolete { get; set; }
    }
}