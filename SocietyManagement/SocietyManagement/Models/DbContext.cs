using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SocietyManagement.Models
{
    enum Gender
    {
        Male,
        Female
    }

    enum ComplaintStatus
    {
        None = 0,
        Open = 1,
        [Display(Name = "In Progress")]
        InProgress = 2,
        Resolved = 3,
        Rejected = 4,        
    }

    public static class Helper
    {        
        public static DateTime GetCurrentDate()
        {
            return DateTime.Now;
        }

        public static List<AspNetUser> GetUsers(DbSet<AspNetUser> Users)
        {
            List<AspNetUser> FilterUsers = Users.Where(u => u.PhoneNumber!="9860002040" && u.Email!="mukeshbpatel@gmail.com").OrderBy(o=>o.FirstName).ToList();
            return FilterUsers;
        }

        public static List<KeyValue> FilterKeyValues(DbSet<KeyValue> KeyValues,string KeyName)
        {            
            return KeyValues.Where(k => k.KeyName == KeyName).OrderBy(o => o.KeyOrder).ToList();
        }

        private static void AssignValue(object obj,string PropertyName,object value)
        {
            Type objType = obj.GetType();
            System.Reflection.PropertyInfo objInfo = objType.GetProperty(PropertyName);
            object val = objInfo.GetValue(obj);
            objInfo.SetValue(obj, value, null);
        }

        public static string GetUserID(IPrincipal User)
        {
            return User.Identity.GetUserId();
        }

        public static void AssignUserInfo(object obj, IPrincipal User, bool IsNew = true)
        {
            DateTime CurrentDate = GetCurrentDate();
            if (IsNew)
                AssignValue(obj, "CreatedDate", CurrentDate);
            AssignValue(obj, "ModifiedDate", CurrentDate);
            AssignValue(obj, "UserID", User.Identity.GetUserId());            
        }

        public static void SoftDelete(object obj, IPrincipal User)
        {
            DateTime CurrentDate = GetCurrentDate();
            AssignValue(obj, "IsDeleted", true);
            AssignValue(obj, "ModifiedDate", CurrentDate);
            AssignValue(obj, "UserID", User.Identity.GetUserId());
        }

        public static string FormatAmount(object Amount)
        {
            if (Amount == null)
                return string.Empty;
            else
                return string.Format(CultureInfo.CreateSpecificCulture("hi-IN"), "{0:#,#}", Amount);
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}