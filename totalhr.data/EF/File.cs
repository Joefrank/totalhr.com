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
    
    public partial class File
    {
        public File()
        {
            this.CompanyDocuments = new HashSet<CompanyDocument>();
        }
    
        public int id { get; set; }
        public string shortname { get; set; }
        public int typeid { get; set; }
        public long size { get; set; }
        public System.DateTime created { get; set; }
        public int createdby { get; set; }
        public string extension { get; set; }
        public Nullable<System.DateTime> lastupdated { get; set; }
        public Nullable<int> lastupdatedby { get; set; }
    
        public virtual ICollection<CompanyDocument> CompanyDocuments { get; set; }
    }
}
