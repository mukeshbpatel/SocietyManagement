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
    
    public partial class Collection
    {
        public decimal CollectionID { get; set; }
        public System.DateTime CollectionDate { get; set; }
        public int UnitID { get; set; }
        public decimal Amount { get; set; }
        public decimal LatePaymentCharges { get; set; }
        public string ReceiptNumber { get; set; }
        public int PaymentModeID { get; set; }
        public string Reference { get; set; }
        public string ChequeBank { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        public string ChequeNumber { get; set; }
        public bool ChequeCleared { get; set; }
        public Nullable<System.DateTime> ChequeEncashmentDate { get; set; }
        public string Details { get; set; }
        public string UDK1 { get; set; }
        public string UDK2 { get; set; }
        public string UDK3 { get; set; }
        public string UDK4 { get; set; }
        public string UDK5 { get; set; }
        public string UserID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ChequeName { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual BuildingUnit BuildingUnit { get; set; }
        public virtual KeyValue PaymentMode { get; set; }
    }
}