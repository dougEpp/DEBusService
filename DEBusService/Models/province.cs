//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DEBusService.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class province
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public province()
        {
            this.drivers = new HashSet<driver>();
        }
    
        public string provinceCode { get; set; }
        public string name { get; set; }
        public string countryCode { get; set; }
        public string taxCode { get; set; }
        public double taxRate { get; set; }
        public string capital { get; set; }
    
        public virtual country country { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<driver> drivers { get; set; }
    }
}