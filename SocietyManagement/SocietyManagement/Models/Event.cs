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
    
    public partial class Event
    {
        public int EventID { get; set; }
        public System.DateTime EventDate { get; set; }
        public int EventTypeID { get; set; }
        public string EventName { get; set; }
        public string Details { get; set; }
        public string UDK1 { get; set; }
        public string UDK2 { get; set; }
        public string UDK3 { get; set; }
        public string UDK4 { get; set; }
        public string UDK5 { get; set; }
        public bool IsDeleted { get; set; }
        public string UserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    
        public virtual KeyValue EventType { get; set; }
    }
}
