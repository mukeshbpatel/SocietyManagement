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

        public static String GetHeader()
        {
           return UpdateSettings(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Template/Header.html")));
        }
        public static String GetFooter()
        {
            return UpdateSettings(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Template/Footer.html")));
        }
        public static String GetSignature()
        {
            return UpdateSettings(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Template/Signature.html")));
        }
        public static string UpdateSettings(string str)
        {
            str = str.Replace("{{Society.Name}}", SiteSetting.Item("Society.Name"))
                .Replace("{{Society.FullName}}", SiteSetting.Item("Society.FullName"))
                .Replace("{{Society.Address}}", SiteSetting.Item("Society.Address"))
                .Replace("{{Society.URL}}", SiteSetting.Item("Society.URL"))
                .Replace("{{Society.Email}}", SiteSetting.Item("Society.Email"))
                .Replace("{{Society.Phone}}", SiteSetting.Item("Society.Phone"))
                .Replace("{{Society.OfficePhone}}", SiteSetting.Item("Society.OfficePhone"))
                .Replace("{{Society.SecurityPhone}}", SiteSetting.Item("Society.SecurityPhone"))
                .Replace("{{Society.RegNo}}", SiteSetting.Item("Society.RegNo"))
                 .Replace("{{Society.Chairman}}", SiteSetting.Item("Society.Chairman"))
                .Replace("{{Society.Secretary}}", SiteSetting.Item("Society.Secretary"))
                .Replace("{{Society.Treasurer}}", SiteSetting.Item("Society.Treasurer"))
                .Replace("{{Site.Name}}", SiteSetting.Item("Site.Name"))
                .Replace("{{Site.URL}}", SiteSetting.Item("Site.URL"));
            return str;
        }

        public static List<AspNetUser> GetUsers(DbSet<AspNetUser> Users)
        {
            List<AspNetUser> FilterUsers = Users.Where(u => u.UserName != "Super").OrderBy(o=>o.FirstName).ToList();            
            return FilterUsers;
        }

        public static List<AspNetUser> GetUsers(DbSet<AspNetUser> Users, string RoleName, string UserID= "")
        {
            List<AspNetUser> FilterUsers = Users.Where(u => u.AspNetRoles.Any(r => r.Name == "Manager" || r.Name == "Admin")).OrderBy(o => o.FirstName).ToList();
            if (UserID != "" && FilterUsers.Where(u=>u.Id== UserID).FirstOrDefault() == null)
                FilterUsers.Add(Users.Find(UserID));
            return FilterUsers;
        }

        public static Boolean IsInRole(string Role)
        {
            if (HttpContext.Current.Session["UserRoles"] != null)
            {
                return HttpContext.Current.Session["UserRoles"].ToString().Contains(Role);
            }

            AspNetUser aspNetUser = CurrentUser();
            string Roles = string.Empty;
            foreach (var item in aspNetUser.AspNetRoles)
            {
                if (string.IsNullOrEmpty(Roles))
                    Roles += item.Name;
                else
                    Roles += "," + item.Name;
            }
            HttpContext.Current.Session["UserRoles"] = Roles;
            return Roles.Contains(Role);
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

        public static List<SelectListItem> GetUnits(DbSet<BuildingUnit> BuildingUnits, int? UnitID)
        {
            List<SelectListItem> items = new SelectList(BuildingUnits.Where(d => d.IsDeleted == false).OrderBy(o => o.UnitName), "UnitID", "UnitName", UnitID).ToList();
            items.Insert(0, (new SelectListItem { Text = "-- Select One --", Value = "" }));
            return items;
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
        
        public static String NullToString(object obj)
        {
            if (obj == null)
                return string.Empty;
            else
                return obj.ToString();
        }

        public static String NullToDate(object obj)
        {
            if (obj == null)
                return string.Empty;
            else
                return ((DateTime)obj).ToString("dd-MMM-yyyy");
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

        public static string FormatMoney(object Amount,Boolean Decimal = false)
        {
            if (Amount == null)
                return string.Empty;
            else if(Decimal)
                return string.Format(CultureInfo.CreateSpecificCulture("hi-IN"), "{0:#,0.00}", Amount);
            else
                return string.Format(CultureInfo.CreateSpecificCulture("hi-IN"), "{0:#,0}", Amount);
        }

        public static string NumbersToWords(decimal inputNumber)
        {
            int inputNo = (int)inputNumber;

            if (inputNo == 0)
                return "Zero";

            int[] numbers = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (inputNo < 0)
            {
                sb.Append("Minus ");
                inputNo = -inputNo;
            }

            string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
            "Five " ,"Six ", "Seven ", "Eight ", "Nine "};
            string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ",
            "Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};
            string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ",
            "Seventy ","Eighty ", "Ninety "};
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };

            numbers[0] = inputNo % 1000; // units
            numbers[1] = inputNo / 1000;
            numbers[2] = inputNo / 100000;
            numbers[1] = numbers[1] - 100 * numbers[2]; // thousands
            numbers[3] = inputNo / 10000000; // crores
            numbers[2] = numbers[2] - 100 * numbers[3]; // lakhs

            for (int i = 3; i > 0; i--)
            {
                if (numbers[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (numbers[i] == 0) continue;
                u = numbers[i] % 10; // ones
                t = numbers[i] / 10;
                h = numbers[i] / 100; // hundreds
                t = t - 10 * h; // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i == 0) sb.Append("and ");
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd() + " Only";
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