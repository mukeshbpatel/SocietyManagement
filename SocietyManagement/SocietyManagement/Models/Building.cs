//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SocietyManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Building
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Building()
        {
            this.BuildingUnits = new HashSet<BuildingUnit>();
        }
    
        public int BuildingID { get; set; }
        public string BuildingName { get; set; }
        public int BuildingTypeID { get; set; }
        public string Details { get; set; }
        public string UDK1 { get; set; }
        public string UDK2 { get; set; }
        public string UDK3 { get; set; }
        public string UDK4 { get; set; }
        public string UDK5 { get; set; }
        public string UserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual KeyValue BuildingType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuildingUnit> BuildingUnits { get; set; }
    }
}
