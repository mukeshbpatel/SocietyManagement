using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using System.Data;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocietyManagement.Models
{
    //public partial class ComplaintMetadata
    //{

    //}
    //[MetadataType(typeof(ComplaintMetadata))]
    //public partial class Complaint
    //{
    //}

    public partial class CommitteeMemberMetadata
    {
        [Required]
        [Display(Name = "Unit")]
        public Nullable<int> UnitID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [StringLength(50)]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string EmailID { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Display(Name = "Mobile")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Invalid mobile number")]
        public string Mobile { get; set; }

        [Display(Name = "Designation")]
        [Required]
        public int DesignationID { get; set; }        
        [DataType(DataType.Date)]
        [Display(Name = "Tenure From")]
        [Required]
        public System.DateTime TenureFrom { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Tenure To")]
        [Required]
        public System.DateTime TenureTo { get; set; }
    }
    [MetadataType(typeof(CommitteeMemberMetadata))]
    public partial class CommitteeMember
    {
        [NotMapped]
        public string Name
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }
    }

    public partial class EventMetadata
    {
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        [Required]
        public System.DateTime EventDate { get; set; }
        [Display(Name = "Event Type")]
        [Required]
        public int EventTypeID { get; set; }
        [Required]
        [Display(Name = "Event")]
        public string EventName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }        
    }
    [MetadataType(typeof(EventMetadata))]
    public partial class Event
    {
    }

    public partial class SP_BuildingUnit_BalanceSheet_ResultMetadata
    {
        public decimal ID { get; set; }
        public string RecType { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public System.DateTime BDate { get; set; }
        public int UnitID { get; set; }
        [Display(Name = "Type")]
        public string BType { get; set; }
        public string Details { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
    }
    [MetadataType(typeof(SP_BuildingUnit_BalanceSheet_ResultMetadata))]
    public partial class SP_BuildingUnit_BalanceSheet_Result
    {
    }

    public partial class NotificationMetadata
    {
        [Display(Name = "Notification")]
        public int TemplateID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }
        public bool IsArchive { get; set; }
        public bool IsDeleted { get; set; }
        public string ReferenceTable { get; set; }
        public Nullable<decimal> ReferenceID { get; set; }
        public string UDK1 { get; set; }
        public string UDK2 { get; set; }
        public string UDK3 { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    }
    [MetadataType(typeof(NotificationMetadata))]
    public partial class Notification
    {
    }

    public partial class FinancialYearMetadata
    {
        [Required]
        [Display(Name = "Financial Year")]
        public string YearName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public System.DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public System.DateTime EndDate { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        [Display(Name = "Closed")]
        public bool IsClosed { get; set; }
        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }
        [Display(Name = "Modified Date")]
        public System.DateTime ModifiedDate { get; set; }

    }
    [MetadataType(typeof(FinancialYearMetadata))]
    public partial class FinancialYear
    {
    }

    public partial class EmailTemplateMetadata
    {
        [Display(Name = "Template Type")]
        public int TemplateTypeID { get; set; }
        [Required]
        [Display(Name = "Subject")]
        public string TemplateSubject { get; set; }
        [Required]
        [DataType(DataType.Html)]
        [Display(Name = "Body")]
        public string TemplateBody { get; set; }
        [EmailAddress]
        [Display(Name = "From Email ID")]
        public string FromEmail { get; set; }
        [Required]
        [Display(Name = "From Email Name")]
        public string FromName { get; set; }
        [EmailAddress]
        [Required]
        [Display(Name = "Reply To Email")]
        public string ReplyToEmail { get; set; }
        [Required]
        [Display(Name = "Reply To Name")]
        public string ReplyToName { get; set; }        
        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }
        [Display(Name = "Modified Date")]
        public System.DateTime ModifiedDate { get; set; }
    }
    [MetadataType(typeof(EmailTemplateMetadata))]
    public partial class EmailTemplate
    {
    }

    public partial class PenaltyMetadata
    {
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public System.DateTime PenaltyDate { get; set; }
        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }
        [Display(Name = "Modified Date")]
        public System.DateTime ModifiedDate { get; set; }
    }
    [MetadataType(typeof(PenaltyMetadata))]
    public partial class Penalty
    {
    }

    public partial class ExpenseMetadata
    {
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public System.DateTime ExpenseDate { get; set; }

        [Display(Name = "Paid To")]
        [Required]
        public string PaidTo { get; set; }

        [Display(Name = "Expense Type")]
        public int ExpenseTypeID { get; set; }

        [Required]
        public string Details { get; set; }

        [Range(0, 9999999)]
        [Required]
        public decimal Amount { get; set; }

        [Display(Name = "TDS")]
        [Range(0,9999999)]
        public decimal TDS { get; set; }

        [Display(Name = "Payment Mode")]
        public int PaymentModeID { get; set; }

        [Display(Name = "Cheque Bank")]
        public string ChequeBank { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Cheque Date")]
        public Nullable<System.DateTime> ChequeDate { get; set; }
        [Display(Name = "Cheque Number")]
        public string ChequeNumber { get; set; }
        [Display(Name = "Cheque Cleared")]
        public bool ChequeCleared { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Encashment Date")]
        public Nullable<System.DateTime> ChequeEncashmentDate { get; set; }
        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }
        [Display(Name = "Modified Date")]
        public System.DateTime ModifiedDate { get; set; }
    }
    [MetadataType(typeof(ExpenseMetadata))]
    public partial class Expense
    {
        [NotMapped]
        [Display(Name = "Files")]
        public HttpPostedFileBase[] Files { get; set; }
    }

    public partial class CollectionMetadata
    {
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public System.DateTime CollectionDate { get; set; }
        [Required]
        [Display(Name = "Unit")]
        public int UnitID { get; set; }

        [Display(Name = "FromDate")]
        [DataType(DataType.Date)]
        public System.DateTime FromDate { get; set; }

        [Display(Name = "ToDate")]
        [DataType(DataType.Date)]
        public System.DateTime ToDate { get; set; }

        [Required]
        [Range(0, 9999999, ErrorMessage = "Invalid Amount")]
        public decimal Amount { get; set; } 
               
        [Range(0, 9999999, ErrorMessage = "Invalid Discount Amount")]
        [Display(Name = "Discount")]
        public decimal Discount { get; set; }

        [Range(0, 9999999, ErrorMessage = "Invalid Other Amount")]
        [Display(Name = "Other Amount")]
        public decimal OtherAmount { get; set; }

        [Range(0, 9999999, ErrorMessage = "Invalid Late Fee Fine")]
        [Display(Name = "Late Fee Fine")]
        public decimal LateFeeFine { get; set; }

        [Required]
        [Display(Name = "Receipt #")]
        public string ReceiptNumber { get; set; }
        [Required]
        [Display(Name = "Mode")]
        public int PaymentModeID { get; set; }
        [Display(Name = "Reference Number")]
        public string Reference { get; set; }
        [Display(Name = "Cheque Bank")]
        public string ChequeBank { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Cheque Date")]
        public Nullable<System.DateTime> ChequeDate { get; set; }
        [Display(Name = "Cheque Number")]
        public string ChequeNumber { get; set; }
        [Display(Name = "Name on Cheque")]
        public string ChequeName { get; set; }
        [Display(Name = "Cheque Cleared")]
        public bool ChequeCleared { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Encashment Date")]
        public Nullable<System.DateTime> ChequeEncashmentDate { get; set; }
        public string Details { get; set; }
        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }
        [Display(Name = "Modified Date")]
        public System.DateTime ModifiedDate { get; set; }
    }
    [MetadataType(typeof(CollectionMetadata))]
    public partial class Collection
    {
       [NotMapped]
        [Display(Name = "Amount")]
        public decimal CollectionAmount
        {
            get
            {
                return this.Amount + this.LateFeeFine + this.OtherAmount - this.Discount;
            }
        }

        [NotMapped]
        [Display(Name = "Files")]
        public HttpPostedFileBase[] Files { get; set; }
    }

    public partial class AspNetUserMetadata
    {
        [Required]
        [Display(Name = "User Name")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "The {0} must be at least {2} characters long.")]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Display(Name = "Mobile")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Invalid mobile number")]
        public string PhoneNumber { get; set; }

       

        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }
    }

    [MetadataType(typeof(AspNetUserMetadata))]
    public partial class AspNetUser
    {
        [NotMapped]        
        public string Role { get; set; }
        [Display(Name = "Reset Password (abcd@1234)")]
        public bool ResetPassword { get; set; }
        [NotMapped]
        public string Name
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }
    }

    public partial class ComplaintCommentMetadata
    {

        public int ComplaintID { get; set; }
        [Required]
        [Display(Name = "Assign To")]
        public string AssignToID { get; set; }
        [Required]
        [Display(Name = "Status")]
        public int StatusID { get; set; }
        [Display(Name = "Comment")]
        [Required]
        public string Comment { get; set; }
        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }
        [Display(Name = "Modified Date")]
        public System.DateTime ModifiedDate { get; set; }
    }
    [MetadataType(typeof(ComplaintCommentMetadata))]
    public partial class ComplaintComment
    {
    }
    
    public partial class ComplaintMetadata
    {
        [Display(Name = "Author")]
        public string AuthorID { get; set; }
        [Display(Name = "Date")]
        [Required]        
        [DataType(dataType: DataType.Date)]
        public System.DateTime ComplaintDate { get; set; }
        [Display(Name = "Type")]
        [Required]
        public int ComplaintTypeID { get; set; }
        [Display(Name = "AssignTo")]
        public string AssignToID { get; set; }
        [Display(Name = "Status")]
        [Required]
        public string StatusID { get; set; }
        [Required]
        [Display(Name = "Complaint")]
        public string Title { get; set; }
        [Display(Name = "Detail")]
        [Required]
        public string Details { get; set; }
        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }
        [Display(Name = "Modified Date")]
        public System.DateTime ModifiedDate { get; set; }
    }

    [MetadataType(typeof(ComplaintMetadata))]
    public partial class Complaint
    {
    }


    public partial class NoticeBoardMetadata
    {
        [Display(Name = "Date")]
        [Required]
        [DataType(dataType: DataType.Date)]
        public System.DateTime NoticeDate { get; set; }
        [Display(Name = "Title")]
        [Required]
        [StringLength(1000)]
        public string NoticeHeading { get; set; }
        [Required]
        [AllowHtml]
        public string Notice { get; set; }
        [Display(Name = "Expiry")]
        [Required]
        [DataType(dataType: DataType.Date)]
        public System.DateTime ExpiryDate { get; set; }
        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public System.DateTime ModifiedDate { get; set; }
    }
    [MetadataType(typeof(NoticeBoardMetadata))]
    public partial class NoticeBoard
    {
    }

    public partial class PollMetadata
    {
        [Display(Name = "Title")]
        [Required]
        [StringLength(1000)]
        public string PollTitle { get; set; }
        [Required]
        [AllowHtml]
        public string Details { get; set; }
        [Display(Name = "Start Date")]
        [Required]
        [DataType(dataType: DataType.Date)]
        public System.DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        [Required]
        [DataType(dataType: DataType.Date)]
        public System.DateTime EndDate { get; set; }
        [Display(Name = "Poll Type")]
        [Required]
        public int PollTypeID { get; set; }
        
    }

    [MetadataType(typeof(PollMetadata))]
    public partial class Poll
    {
        [Required]
        [Display(Name = "Poll Options")]
        [NotMapped]
        public string Options { get; set; }
    }

    public partial class ForumMetadata
    {
        [Display(Name = "Title")]
        [Required]
        [StringLength(500)]
        [AllowHtml]
        public string ForumTitle { get; set; }
        [Display(Name = "Details")]
        [Required]        
        public string Details { get; set; }
        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public System.DateTime ModifiedDate { get; set; }
    }
    [MetadataType(typeof(ForumMetadata))]
    public partial class Forum
    {
    }

    public partial class ForumCommentMetadata
    {        
        [Required]
        [AllowHtml]
        public string Comment { get; set; }

        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public System.DateTime ModifiedDate { get; set; }

    }

    [MetadataType(typeof(ForumCommentMetadata))]
    public partial class ForumComment
    {

    }



    public partial class RecurringDueMetadata
    {
        [Display(Name = "Start")]
        [Required]
        [DataType(dataType: DataType.Date)]
        public System.DateTime StartDate { get; set; }
        [Display(Name = "End")]
        [Required]
        [DataType(dataType: DataType.Date)]
        public System.DateTime EndDate { get; set; }
        [Display(Name = "Recurring Type")]
        [Required]
        public int RecurringTypeID { get; set; }
        [Display(Name = "Bill Type")]
        [Required]
        public int DueTypeID { get; set; }
        [Display(Name = "Amount")]
        [Required]
        [Range(-9999999,9999999)]        
        public decimal Amount { get; set; }
        [Display(Name = "Due Days")]
        [Required]
        [Range(0,99999)]
        public int DueDays { get; set; }
        [Display(Name = "Last Bill")]
        [DataType(dataType: DataType.Date)]
        public Nullable<System.DateTime> LastRunDate { get; set; }

        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public System.DateTime ModifiedDate { get; set; }
    }
    [MetadataType(typeof(RecurringDueMetadata))]
    public partial class RecurringDue
    {
        [NotMapped]
        public int LastRunMonth
        {
            get
            {
                if (this.LastRunDate == null)
                 return 200001;
                else
                {
                    return ((this.LastRunDate.Value.Year * 100) + this.LastRunDate.Value.Month); 
                }
            }
        }
    }

    public partial class DueMetadata
    {
        [Display(Name = "Date")]
        [Required]
        [DataType(dataType: DataType.Date)]
        public System.DateTime BillDate { get; set; }
        [Required]
        public int UnitID { get; set; }
        [Display(Name = "Bill Type")]
        [Required]
        public int DueTypeID { get; set; }
        [Display(Name = "Amount")]
        [Required]
        [Range(-99999999,9999999)]
        public decimal DueAmount { get; set; }
        [Display(Name = "Particular")]
        [Required]
        [StringLength(1000)]
        public string Details { get; set; }
        [Display(Name = "Due Date")]
        [Required]
        [DataType(dataType: DataType.Date)]
        public System.DateTime DueDate { get; set; }

        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public System.DateTime ModifiedDate { get; set; }
    }
    [MetadataType(typeof(DueMetadata))]
    public partial class Due
    {
    }


    public partial class BuildingMetadata
    {
        [Display(Name = "Building")]
        [StringLength(500)]
        [Required]
        public string BuildingName { get; set; }

        [Display(Name = "Building Type")]
        [Required]
        public int BuildingTypeID { get; set; }

        [Required]
        [StringLength(1000)]
        public string Details { get; set; }

        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public System.DateTime ModifiedDate { get; set; }
    }
    [MetadataType(typeof(BuildingMetadata))]
    public partial class Building
    {
    }

    public partial class BuildingUnitMetadata
    {
        [Required]
        [Display(Name = "Building")]
        public int BuildingID { get; set; }

        [Required]
        [Display(Name = "Unit")]
        [StringLength(500)]
        public string UnitName { get; set; }

        [Required]
        [Display(Name = "Type")]
        public int UnitTypeID { get; set; }
        public string Details { get; set; }

        [Required]
        [Display(Name = "Owner")]
        [StringLength(500)]
        public string OwnerName { get; set; }
        
        [Display(Name = "Co-Owner")]
        [StringLength(500)]
        public string CoOwnerName { get; set; }

        [Required]
        [Display(Name = "User")]
        public string OwnerID { get; set; }

        [Required]
        [Display(Name = "Onetime Maintenance")]
        [Range(0,999999)]
        public decimal OneTimeMaintenance { get; set; }

        [Required]
        [Display(Name = "Monthly Maintenance")]
        [Range(0, 999999)]
        public decimal MonthlyMaintenance { get; set; }

        [Range(0, 999999)]
        [Display(Name = "Area")]
        public decimal UnitArea { get; set; }
        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public System.DateTime ModifiedDate { get; set; }
    }

    [MetadataType(typeof(BuildingUnitMetadata))]
    public partial class BuildingUnit
    {
        [NotMapped]
        [Display(Name = "Files")]
        public HttpPostedFileBase[] Files { get; set; }
    }

    public partial class KeyValueMetadata
    {
        [Required]
        [StringLength(500)]
        [Display(Name = "Key Name")]
        public string KeyName { get; set; }
        [Required]
        [StringLength(500)]
        [Display(Name = "Key Value")]
        public string KeyValues { get; set; }
        [Required]
        [Range(0, 10000)]
        [Display(Name = "Key Order")]
        public int KeyOrder { get; set; }
        [StringLength(1000)]
        public string Details { get; set; }
    }
    [MetadataType(typeof(KeyValueMetadata))]
    public partial class KeyValue
    {
    }
    
}