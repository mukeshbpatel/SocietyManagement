using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;

namespace SocietyManagement.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.PaymentNotification = true;
            this.PaymentReminder = true;
            this.BillNotification = true;
            this.ForumNotification = true;
            this.PollNotification = true;
            this.NoticeBoardNotification = true;
            this.EventNotification = true;
        }
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here                        
            return userIdentity;
        }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(10)]
        public string Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }
        [StringLength(500)]
        public string Profession { get; set; }
        [StringLength(500)]
        public string Occupation { get; set; }
        public int? UnitID { get; set; }
        [Display(Name = "Bill Notification")]
        public Boolean BillNotification { get; set; }
        [Display(Name = "Payment Notification")]
        public Boolean PaymentNotification { get; set; }
        [Display(Name = "Notice Notification")]
        public Boolean NoticeBoardNotification { get; set; }
        [Display(Name = "Poll Notification")]
        public Boolean PollNotification { get; set; }
        [Display(Name = "Event Notification")]
        public Boolean EventNotification { get; set; }
        [Display(Name = "Forum Notification")]
        public Boolean ForumNotification { get; set; }
        [Display(Name = "Payment Reminder")]
        public Boolean PaymentReminder { get; set; }
    }

    public class UserProfile
    {
        public UserProfile()
        {
            this.PaymentNotification = true;
            this.PaymentReminder = true;
            this.BillNotification = true;
            this.ForumNotification = true;
            this.PollNotification = true;
            this.NoticeBoardNotification = true;
            this.EventNotification = true;
        }

        public UserProfile(AspNetUser UserInfo)
        {
            this.FirstName = UserInfo.FirstName;
            this.LastName = UserInfo.LastName;
            this.Gender = UserInfo.Gender;
            this.DOB = UserInfo.DOB;
            this.Profession = UserInfo.Profession;
            this.Occupation = UserInfo.Occupation;
            this.Email = UserInfo.Email;
            this.PhoneNumber = UserInfo.PhoneNumber;
            this.BillNotification = UserInfo.BillNotification;
            this.PaymentNotification = UserInfo.PaymentNotification;
            this.NoticeBoardNotification = UserInfo.NoticeBoardNotification;
            this.PaymentReminder = UserInfo.PaymentReminder;
            this.PollNotification = UserInfo.PollNotification;
            this.ForumNotification = UserInfo.ForumNotification;
            this.EventNotification = UserInfo.EventNotification;
            this.NoticeBoardNotification = UserInfo.NoticeBoardNotification;
        }
        
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]        
        public string Gender { get; set; }
        [DataType(dataType: DataType.Date)]
        public DateTime? DOB { get; set; }
        [StringLength(500)]
        public string Profession { get; set; }
        [StringLength(500)]
        public string Occupation { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Mobile")]
        [StringLength(10,MinimumLength =10,ErrorMessage ="Invalid mobile number")]
        [Required]
        public string PhoneNumber { get; set; }
        [Display(Name = "Bill Notification")]
        public Boolean BillNotification { get; set; }
        [Display(Name = "Payment Notification")]
        public Boolean PaymentNotification { get; set; }
        [Display(Name = "Notice Notification")]
        public Boolean NoticeBoardNotification { get; set; }
        [Display(Name = "Poll Notification")]
        public Boolean PollNotification { get; set; }
        [Display(Name = "Event Notification")]
        public Boolean EventNotification { get; set; }
        [Display(Name = "Forum Notification")]
        public Boolean ForumNotification { get; set; }
        [Display(Name = "Payment Reminder")]
        public Boolean PaymentReminder { get; set; }
    }
}