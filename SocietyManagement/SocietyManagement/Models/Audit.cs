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
    
    public partial class Audit
    {
        public decimal AuditID { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public decimal ID { get; set; }
        public string UserID { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public System.DateTime ActionDate { get; set; }
    }
}