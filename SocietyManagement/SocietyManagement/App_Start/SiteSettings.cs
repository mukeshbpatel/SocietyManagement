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

        //public static string SocietyName { get; set; }
        //public static string SocietyShortName { get; set; }
        //public static string SocietyAddress { get; set; }
        //public static string SocietyURL { get; set; }
        //public static string SocietyFromEmailAddress { get; set; }
        //public static string SocietyFromEmailName { get; set; }
        //public static string SocietyReplyToEmailAddress { get; set; }
        //public static string SocietyReplyToEmailName { get; set; }

        //public static string SiteName { get; set; }
        //public static string SiteURL { get; set; }
        //public static string SiteBCCAddress { get; set; }
        //public static string SiteBCCName { get; set; }

        //public static Boolean IsTestMode { get; set; }

        //public static Boolean SMTPEnableSsl { get; set; }
        //public static string SMTPHost { get; set; }
        //public static int SMTPPort { get; set; }
        //public static string SMTPUserID { get; set; }
        //public static string SMTPPassword { get; set; }

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