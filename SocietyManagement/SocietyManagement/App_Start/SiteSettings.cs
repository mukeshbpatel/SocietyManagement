using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SocietyManagement
{
    public static class SiteSetting
    {
        public static string SocietyName { get; set; }
        public static string SocietyShortName { get; set; }
        public static string SocietyAddress { get; set; }
        public static string SocietyURL { get; set; }
        public static string SocietyFromEmailAddress { get; set; }
        public static string SocietyFromEmailName { get; set; }
        public static string SocietyReplyToEmailAddress { get; set; }
        public static string SocietyReplyToEmailName { get; set; }

        public static string SiteName { get; set; }
        public static string SiteURL { get; set; }
        public static string SiteBCCAddress { get; set; }
        public static string SiteBCCName { get; set; }

        public static Boolean IsTestMode { get; set; }

        public static Boolean SMTPEnableSsl {get; set;}
        public static string SMTPHost { get; set; }
        public static int SMTPPort { get; set; }
        public static string SMTPUserID { get; set; }
        public static string SMTPPassword { get; set; }

        public static bool SetSiteSettings()
        {
            SocietyName = "Sample Co-Op Society";
            SocietyShortName = "SamSoc";
            SocietyAddress = "Kale Padal, Tukai Nager, Hadapsar, Pune - 411028";
            SocietyURL = "http://SampleSociety.SocietyInbox.com";
            SocietyFromEmailAddress = "ygcfund@yahoo.com";
            SocietyFromEmailName = "YGC Fund";
            SocietyReplyToEmailAddress = "ygcfund@yahoo.com";
            SocietyReplyToEmailName = "YGC Fund";

            SiteName = "Society Inbox";
            SiteURL = "http://www.SocietyInbox.com";
            SiteBCCAddress = "mukeshbpatel@gmail.com";
            SiteBCCName = "Mukesh Patel";

            SMTPHost = "smtp.mail.yahoo.com";
            SMTPUserID = "ygcfund@yahoo.com";
            SMTPPassword = "y@cfund!@#$";
            SMTPPort = 587;
            SMTPEnableSsl = true;

            IsTestMode = true;
            return true;
        }
    }
}