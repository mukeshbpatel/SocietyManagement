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
    
    public partial class ExpenseMedia
    {
        public decimal MediaID { get; set; }
        public decimal ExpenseID { get; set; }
        public string MediaType { get; set; }
        public string MediaTitle { get; set; }
        public byte[] MediaData { get; set; }
        public string Details { get; set; }
        public bool IsDeleted { get; set; }
        public string UDK1 { get; set; }
        public string UDK2 { get; set; }
        public string UDK3 { get; set; }
        public string UserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string FileName { get; set; }
    
        public virtual Expense Expens { get; set; }
    }
}
