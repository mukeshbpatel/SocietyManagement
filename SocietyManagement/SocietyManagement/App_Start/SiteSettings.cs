using SocietyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SocietyManagement
{
    public class SiteSetting
    {
        static Dictionary<string, string> Items = new Dictionary<string, string>();

        public static int FinancialYearID { get; set; }
        public static string FinancialYear { get; set; }
        public static DateTime FinancialYear_StartDate { get; set; }
        public static DateTime FinancialYear_EndDate { get; set; }
        
        public static bool LoadFinancialYearSettings()
        {
            bool _result = false;
            SocietyManagementEntities db = new SocietyManagementEntities();
            FinancialYear fy = db.FinancialYears.Where(f => f.IsDeleted == false && f.IsActive == true).FirstOrDefault();
            if (fy != null)
            {
                FinancialYearID = fy.YearID;
                FinancialYear = fy.YearName;
                FinancialYear_StartDate = fy.StartDate;
                FinancialYear_EndDate = fy.EndDate;
                _result = true;
            }
            else
            {
                FinancialYearID = 0;
                FinancialYear = string.Empty;
                FinancialYear_StartDate = DateTime.MinValue;
                FinancialYear_EndDate = DateTime.MinValue;
            }
            db.Dispose();
            return _result;
        }

        public static bool SetSiteSettings()
        {
            SocietyManagementEntities db = new SocietyManagementEntities();
            Items.Clear();
            foreach (var item in db.SystemSettings)
            {
                Items.Add(item.SettingName, item.SettingValue);
            }
            db.Dispose();
            return true;
        }

        public static string Item(string SettingName)
        {
            try
            {
                return Items[SettingName];
            }
            catch (Exception)
            {
                return string.Empty;
            }            
        }

        public static Boolean ItemBoolean(string SettingName)
        {
            try
            {
                if (Items[SettingName].ToLower() == "true" || Items[SettingName].ToLower() == "yes" || Items[SettingName].ToLower() == "y")
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void UpdateSettings(string SettingName, string SettingValue)
        {
            try
            {
                Items[SettingName] = SettingValue;
            }
            catch (Exception)
            {
            }
            
        }
        public static void AddSettings(string SettingName, string SettingValue)
        {
            try
            {
                Items.Add(SettingName,SettingValue);
            }
            catch (Exception)
            {
            }
        }
    }
}