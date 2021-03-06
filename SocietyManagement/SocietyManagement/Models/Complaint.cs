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
    
    public partial class Complaint
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Complaint()
        {
            this.ComplaintComments = new HashSet<ComplaintComment>();
        }
    
        public int ComplaintID { get; set; }
        public string AuthorID { get; set; }
        public System.DateTime ComplaintDate { get; set; }
        public int ComplaintTypeID { get; set; }
        public string AssignToID { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public bool IsDeleted { get; set; }
        public string UDK1 { get; set; }
        public string UDK2 { get; set; }
        public string UDK3 { get; set; }
        public string UDK4 { get; set; }
        public string UDK5 { get; set; }
        public string UserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int StatusID { get; set; }
    
        public virtual AspNetUser AssignTo { get; set; }
        public virtual AspNetUser Author { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComplaintComment> ComplaintComments { get; set; }
        public virtual KeyValue ComplaintType { get; set; }
        public virtual KeyValue Status { get; set; }
    }
}
