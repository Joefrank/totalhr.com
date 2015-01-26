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
    
    public partial class Company
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Company()
        {
            this.Calendars = new HashSet<Calendar>();
            this.Departments = new HashSet<Department>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string StateorCounty { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string PostCode { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> LastUpdateBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        public string TaxId { get; set; }
        public string VATId { get; set; }
        public string DNUSNumber { get; set; }
        public int NumberOfEmployees { get; set; }
        public string CompanyType { get; set; }
        public Nullable<decimal> AnnualRevenue { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public int PreferredLanguageId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Calendar> Calendars { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Department> Departments { get; set; }
    }
}
