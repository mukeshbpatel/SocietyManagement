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

    public partial class ComplaintMetadata
    {
        [Required]
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
        [Required]
        public string AssignToID { get; set; }
        [Required]
        [Display(Name = "Complaint")]
        public string Title { get; set; }
        [Display(Name = "Detail")]
        [Required]
        public string Details { get; set; }
        [Required]
        public int Status { get; set; }
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
        [Display(Name = "Sstart")]
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
        [Display(Name = "Due Type")]
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
        [Display(Name = "Last Run")]
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
    }

    public partial class DueMetadata
    {
        [Display(Name = "Date")]
        [Required]
        [DataType(dataType: DataType.Date)]
        public System.DateTime BillDate { get; set; }
        [Required]
        public int UnitID { get; set; }
        [Display(Name = "Due Type")]
        [Required]
        public int DueTypeID { get; set; }
        [Display(Name = "Amount")]
        [Required]
        [Range(-99999999,9999999)]
        public decimal DueAmount { get; set; }
        [Display(Name = "Due")]
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
        [StringLength(1000)]
        public string UnitName { get; set; }
        [Required]
        [Display(Name = "Type")]
        public int UnitTypeID { get; set; }
        public string Details { get; set; }
        [Required]
        [Display(Name = "Owner")]
        public string OwnerID { get; set; }
        [Required]
        [Display(Name = "Onetime Maintenance")]
        [Range(0,999999)]
        public decimal OneTimeMaintenance { get; set; }
        [Range(0, 999999)]
        public decimal UnitArea { get; set; }
        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public System.DateTime ModifiedDate { get; set; }

    }
    [MetadataType(typeof(BuildingUnitMetadata))]
    public partial class BuildingUnit
    {
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