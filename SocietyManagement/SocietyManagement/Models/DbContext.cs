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
using System.Web.Mvc;

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
        public static SelectList GetMonths(DateTime Current)
        {
            List<DateTime> dtList = new List<DateTime>();
            DateTime dt = SiteSetting.FinancialYear_StartDate;
            while (dt <= SiteSetting.FinancialYear_EndDate)
            {
                dtList.Add(dt);
                dt = dt.AddMonths(1);
            }

            return new SelectList(dtList, Current);
        }

        public static DateTime GetCurrentDate()
        {
            return DateTime.Now;
        }

        public static List<AspNetUser> GetUsers(DbSet<AspNetUser> Users)
        {
            List<AspNetUser> FilterUsers = Users.Where(u => u.UserName != "SuperUser").OrderBy(o=>o.FirstName).ToList();            
            return FilterUsers;
        }

        public static AspNetUser CurrentUser()
        {
            AspNetUser aspNetUser;
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                aspNetUser = new AspNetUser();
                aspNetUser.FirstName = string.Empty;
                aspNetUser.LastName = string.Empty;
                aspNetUser.Role = string.Empty;
                aspNetUser.Gender = "Male";
                HttpContext.Current.Session["UserInfo"] = null;
            }
            else if (HttpContext.Current.Session["UserInfo"] != null)
            {
                aspNetUser = (AspNetUser)HttpContext.Current.Session["UserInfo"];                
            }
            else
            {
                using (SocietyManagementEntities db = new SocietyManagementEntities())
                {
                    aspNetUser = db.AspNetUsers.Find(HttpContext.Current.User.Identity.GetUserId());
                    if (aspNetUser != null)
                        aspNetUser.Role = aspNetUser.AspNetRoles.FirstOrDefault().Name;
                    HttpContext.Current.Session["UserInfo"] = aspNetUser;
                }                
            }
            return aspNetUser;
        }

        public static string GetUserID(IPrincipal User)
        {
            return User.Identity.GetUserId();
        }        

        public static List<KeyValue> FilterKeyValues(DbSet<KeyValue> KeyValues,string KeyName)
        {            
            return KeyValues.Where(k => k.KeyName == KeyName).OrderBy(o => o.KeyOrder).ToList();
        }

        private static void AssignValue(object obj,string PropertyName,object value)
        {
            try
            {
                Type objType = obj.GetType();
                System.Reflection.PropertyInfo objInfo = objType.GetProperty(PropertyName);
                if (objInfo != null)
                {
                    object val = objInfo.GetValue(obj);
                    objInfo.SetValue(obj, value, null);
                }
            }
            catch (Exception)
            {                
            }
            
        }
        
        public static void AssignUserInfo(object obj, IPrincipal User, bool IsNew = true)
        {
            DateTime CurrentDate = GetCurrentDate();
            if (IsNew)
            {
                AssignValue(obj, "CreatedDate", CurrentDate);
                AssignValue(obj, "YearID",SiteSetting.FinancialYearID);
            }
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

        public static string GetFileType(string MediaType,string MediaTitle)
        {
            string FileType = MediaType.ToLower();
            MediaType = MediaType.ToLower();
            if (FileType.StartsWith("image")|| MediaType.EndsWith("png") || MediaType.EndsWith("jpg") || MediaType.EndsWith("jpeg") || MediaType.EndsWith("gif") || MediaType.EndsWith("bmp"))
            {
                FileType = "Image";
            }
            else if (FileType.EndsWith("document") || MediaType.EndsWith("doc") || MediaType.EndsWith("docx"))
            {
                FileType = "Document";
            }
            else if (FileType.EndsWith("sheet") || MediaType.EndsWith("xls") || MediaType.EndsWith("xlsx"))
            {
                FileType = "Excel";
            }
            else if (FileType.EndsWith("pdf") || MediaType.EndsWith("pdf"))
            {
                FileType = "PDF";
            }
            else
            {
                FileType = "File";
            }
            return FileType;
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