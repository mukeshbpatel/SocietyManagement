using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.Configuration;

namespace SocietyManagement.Models
{
    public class EmailNotification
    {
        private SocietyManagementEntities db;
        private string SiteName = "";
        private string SiteURL = "";
        public EmailNotification()
        {
            SiteName = WebConfigurationManager.AppSettings["SiteName"].ToString();
            SiteURL = WebConfigurationManager.AppSettings["SiteURL"].ToString();
        }

        public Boolean SendMail(MailMessage mailMessage)
        {
            bool result = false;
            try
            {
                if (WebConfigurationManager.AppSettings["TestMode"] == "true")
                {
                    try
                    {
                        mailMessage.To.RemoveAt(0);
                        mailMessage.CC.RemoveAt(0);
                    }
                    catch
                    {

                    }
                }

                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Host = "smtp.mail.yahoo.com";
                client.Port = 587;
                // setup Smtp authentication
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("ygcfund@yahoo.com", "y@cfund!@#$");
                client.UseDefaultCredentials = false;
                client.Credentials = credentials;
                client.Send(mailMessage);
                client.Dispose();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public bool SendPaymentNotification(Collection PaymentDetails)
        {
            db = new SocietyManagementEntities();
            EmailTemplate emailTemplate = db.EmailTemplates.Where(t => t.TemplateType.KeyName == "EmailTemplateType" && t.TemplateType.KeyValues == "Payment Receipt").FirstOrDefault();
            Collection collection = db.Collections.Find(PaymentDetails.CollectionID);
            SystemSetting settings = db.SystemSettings.Where(s => s.SettingName == "SocietyName").FirstOrDefault();
            string Body, Subject;
            if (collection != null && emailTemplate != null && settings != null)
            {
                Body = emailTemplate.TemplateBody;
                if (collection.BuildingUnit.OwnerID != null)
                {
                    Body = Body.Replace("{{Name}}", collection.BuildingUnit.Owner.Name);
                }
                Body = Body.Replace("{{UnitName}}", collection.BuildingUnit.UnitName);              
                Body = Body.Replace("{{ReceiptNumber}}", collection.ReceiptNumber);
                Body = Body.Replace("{{PaymentAmount}}", Helper.FormatAmount(collection.CollectionAmount));
                Body = Body.Replace("{{PaymentDate}}", collection.CollectionDate.ToString("dd-MMM-yyyy"));
                Body = Body.Replace("{{PaymentMode}}", collection.PaymentMode.KeyValues);
                Body = Body.Replace("{{PaymentID}}", collection.CollectionID.ToString());
                Body = Body.Replace("{{SocietyName}}", settings.SettingValue);
                Body = Body.Replace("{{SocietyAddress}}", settings.UDK1);
                Body = Body.Replace("{{SocietyURL}}", settings.UDK2);
                Body = Body.Replace("{{SiteName}}", SiteName);
                Body = Body.Replace("{{SiteURL}}", SiteURL);

                Subject = emailTemplate.TemplateSubject.Replace("{{SocietyName}}", settings.SettingValue);
                Subject = Subject.Replace("{{ReceiptNumber}}", collection.ReceiptNumber);
                Subject = Subject.Replace("{{PaymentDate}}", collection.CollectionDate.ToString("dd/MM/yyyy"));
                Subject = Subject.Replace("{{PaymentId}}", collection.CollectionID.ToString());

                if (collection.BuildingUnit.OwnerID != null)
                {
                    Notification notification = new Notification();
                    notification.Body = Body;
                    notification.Subject = Subject;
                    notification.UserID = collection.BuildingUnit.Owner.Id;
                    notification.TemplateID = emailTemplate.TemplateID;
                    notification.CreatedDate = collection.CreatedDate;
                    notification.ModifiedDate = collection.CreatedDate;
                    notification.ReferenceTable = "Collection";
                    notification.ReferenceID = collection.CollectionID;
                    db.Notifications.Add(notification);
                    db.SaveChanges();
                }

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(emailTemplate.FromEmail, emailTemplate.FromName);                
                msg.ReplyToList.Add(new MailAddress(emailTemplate.ReplyToEmail, emailTemplate.ReplyToName));
                msg.CC.Add(new MailAddress(emailTemplate.ReplyToEmail, emailTemplate.ReplyToName));
                if (collection.BuildingUnit.Owner!= null && !String.IsNullOrEmpty(collection.BuildingUnit.Owner.Email))
                {
                    if (collection.BuildingUnit.Owner.PaymentNotification)
                        msg.To.Add(new MailAddress(collection.BuildingUnit.Owner.Email, collection.BuildingUnit.Owner.Name));
                }
                msg.Bcc.Add(new MailAddress("societyinbox@outlook.com", "Society Inbox"));                
                msg.Subject = Subject;
                msg.IsBodyHtml = true;
                msg.Body = Body;                
                return SendMail(msg);
            }
            return false;
        }

        public bool SendBillNotification(Due BillDetails)
        {
            db = new SocietyManagementEntities();
            EmailTemplate emailTemplate = db.EmailTemplates.Where(t => t.TemplateType.KeyName == "EmailTemplateType" && t.TemplateType.KeyValues == "Maintance Bill").FirstOrDefault();
            Due due = db.Dues.Find(BillDetails.DueID);
            SystemSetting settings = db.SystemSettings.Where(s => s.SettingName == "SocietyName").FirstOrDefault();
            string Body, Subject;
            if (due != null && emailTemplate != null && settings != null)
            {
                Body = emailTemplate.TemplateBody;
                if (due.BuildingUnit.OwnerID != null)
                {
                    Body = Body.Replace("{{Name}}", due.BuildingUnit.Owner.Name);
                }
                Body = Body.Replace("{{UnitName}}", due.BuildingUnit.UnitName);
                Body = Body.Replace("{{BillNumber}}", due.DueID.ToString());
                Body = Body.Replace("{{BillAmount}}", Helper.FormatAmount(due.DueAmount));                
                Body = Body.Replace("{{BillType}}", due.DueType.KeyValues);
                Body = Body.Replace("{{BillDetail}}", due.Details);
                Body = Body.Replace("{{BillDate}}", due.BillDate.ToString("dd-MMM-yyyy"));
                Body = Body.Replace("{{BillMonth}}", due.BillDate.ToString("MMM-yyyy"));
                Body = Body.Replace("{{BillYear}}", due.BillDate.ToString("yyyy"));
                Body = Body.Replace("{{DueDate}}", due.DueDate.ToString("dd-MMM-yyyy"));
                Body = Body.Replace("{{SocietyName}}", settings.SettingValue);
                Body = Body.Replace("{{SocietyAddress}}", settings.UDK1);
                Body = Body.Replace("{{SocietyURL}}", settings.UDK2);
                Body = Body.Replace("{{SiteName}}", SiteName);
                Body = Body.Replace("{{SiteURL}}", SiteURL);

                Subject = emailTemplate.TemplateSubject.Replace("{{SocietyName}}", settings.SettingValue);
                Subject = Subject.Replace("{{BillNumber}}", due.DueID.ToString());
                Subject = Subject.Replace("{{BillDate}}", due.BillDate.ToString("dd-MMM-yyyy"));
                Subject = Subject.Replace("{{BillMonth}}", due.BillDate.ToString("MMM-yyyy"));
                Subject = Subject.Replace("{{BillYear}}", due.BillDate.ToString("yyyy"));
                Subject = Subject.Replace("{{BillDetail}}", due.Details);

                if (due.BuildingUnit.OwnerID != null)
                {
                    Notification notification = new Notification();
                    notification.Body = Body;
                    notification.Subject = Subject;
                    notification.UserID = due.BuildingUnit.Owner.Id;
                    notification.TemplateID = emailTemplate.TemplateID;
                    notification.CreatedDate = due.CreatedDate;
                    notification.ModifiedDate = due.CreatedDate;
                    notification.ReferenceTable = "Due";
                    notification.ReferenceID = due.DueID;
                    db.Notifications.Add(notification);
                    db.SaveChanges();
                }

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(emailTemplate.FromEmail, emailTemplate.FromName);
                msg.ReplyToList.Add(new MailAddress(emailTemplate.ReplyToEmail, emailTemplate.ReplyToName));
                msg.CC.Add(new MailAddress(emailTemplate.ReplyToEmail, emailTemplate.ReplyToName));
                if (due.BuildingUnit.OwnerID != null && !String.IsNullOrEmpty(due.BuildingUnit.Owner.Email))
                {
                    if (due.BuildingUnit.Owner.BillNotification)
                        msg.To.Add(new MailAddress(due.BuildingUnit.Owner.Email, due.BuildingUnit.Owner.Name));
                }
                msg.Bcc.Add(new MailAddress("societyinbox@outlook.com", "Society Inbox"));
                msg.Subject = Subject;
                msg.IsBodyHtml = true;
                msg.Body = Body;
                db.Dispose();
                return SendMail(msg);
            }
            return false;
        }

        public bool SendForgotPasswordEmail(ApplicationUser user,string callbackUrl)
        {
            db = new SocietyManagementEntities();
            EmailTemplate emailTemplate = db.EmailTemplates.Where(t => t.TemplateType.KeyName == "EmailTemplateType" && t.TemplateType.KeyValues == "Forgot Password").FirstOrDefault();
            SystemSetting settings = db.SystemSettings.Where(s => s.SettingName == "SocietyName").FirstOrDefault();
            string Body, Subject;
            if (user != null && emailTemplate != null && settings != null)
            {
                Body = emailTemplate.TemplateBody;
                if (!string.IsNullOrEmpty(user.FirstName))
                {
                    Body = Body.Replace("{{Name}}", user.FirstName);
                }
                Body = Body.Replace("{{UserName}}", user.UserName);
                Body = Body.Replace("{{PasswordURL}}", callbackUrl);
                Body = Body.Replace("{{Email}}", user.Email);
                Body = Body.Replace("{{SocietyName}}", settings.SettingValue);
                Body = Body.Replace("{{SocietyAddress}}", settings.UDK1);
                Body = Body.Replace("{{SocietyURL}}", settings.UDK2);
                Body = Body.Replace("{{SiteName}}", SiteName);
                Body = Body.Replace("{{SiteURL}}", SiteURL);

                Subject = emailTemplate.TemplateSubject.Replace("{{SocietyName}}", settings.SettingValue);

                Notification notification = new Notification();
                notification.Body = Body;
                notification.Subject = Subject;
                notification.UserID = user.Id;
                notification.TemplateID = emailTemplate.TemplateID;
                notification.CreatedDate = DateTime.Now;
                notification.ModifiedDate = notification.CreatedDate;
                notification.ReferenceTable = "Account";                
                db.Notifications.Add(notification);
                db.SaveChanges();
                
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(emailTemplate.FromEmail, emailTemplate.FromName);
                msg.ReplyToList.Add(new MailAddress(emailTemplate.ReplyToEmail, emailTemplate.ReplyToName));
                msg.CC.Add(new MailAddress(emailTemplate.ReplyToEmail, emailTemplate.ReplyToName));
                if (!String.IsNullOrEmpty(user.Email))
                {
                    msg.To.Add(new MailAddress(user.Email, user.FirstName));
                }
                msg.Bcc.Add(new MailAddress("societyinbox@outlook.com", "Society Inbox"));
                msg.Subject = Subject;
                msg.IsBodyHtml = true;
                msg.Body = Body;
                db.Dispose();
                return SendMail(msg);
            }
            db.Dispose();
            return false;
        }

        public bool SendPasswordChangedEmail(ApplicationUser user, string Password)
        {
            db = new SocietyManagementEntities();
            EmailTemplate emailTemplate = db.EmailTemplates.Where(t => t.TemplateType.KeyName == "EmailTemplateType" && t.TemplateType.KeyValues == "Password Changed").FirstOrDefault();
            SystemSetting settings = db.SystemSettings.Where(s => s.SettingName == "SocietyName").FirstOrDefault();
            string Body, Subject;
            if (user != null && emailTemplate != null && settings != null)
            {
                Body = emailTemplate.TemplateBody;
                if (!string.IsNullOrEmpty(user.FirstName))
                {
                    Body = Body.Replace("{{Name}}", user.FirstName + " " + user.LastName);
                }
                Body = Body.Replace("{{UserName}}", user.UserName);
                Body = Body.Replace("{{Password}}", Password);
                Body = Body.Replace("{{Email}}", user.Email);
                Body = Body.Replace("{{SocietyName}}", settings.SettingValue);
                Body = Body.Replace("{{SocietyAddress}}", settings.UDK1);
                Body = Body.Replace("{{SocietyURL}}", settings.UDK2);
                Body = Body.Replace("{{SiteName}}", SiteName);
                Body = Body.Replace("{{SiteURL}}", SiteURL);

                Subject = emailTemplate.TemplateSubject.Replace("{{SocietyName}}", settings.SettingValue);

                Notification notification = new Notification();
                notification.Body = Body;
                notification.Subject = Subject;
                notification.UserID = user.Id;
                notification.TemplateID = emailTemplate.TemplateID;
                notification.CreatedDate = DateTime.Now;
                notification.ModifiedDate = notification.CreatedDate;
                notification.ReferenceTable = "Account";
                db.Notifications.Add(notification);
                db.SaveChanges();

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(emailTemplate.FromEmail, emailTemplate.FromName);
                msg.ReplyToList.Add(new MailAddress(emailTemplate.ReplyToEmail, emailTemplate.ReplyToName));
                msg.CC.Add(new MailAddress(emailTemplate.ReplyToEmail, emailTemplate.ReplyToName));
                if (!String.IsNullOrEmpty(user.Email))
                {
                    msg.To.Add(new MailAddress(user.Email, user.FirstName + " " + user.LastName));
                }
                msg.Bcc.Add(new MailAddress("societyinbox@outlook.com", "Society Inbox"));
                msg.Subject = Subject;
                msg.IsBodyHtml = true;
                msg.Body = Body;
                db.Dispose();
                return SendMail(msg);
            }
            db.Dispose();
            return false;
        }

        public bool SendWelcomeEmail(ApplicationUser user, string Password)
        {
            db = new SocietyManagementEntities();
            EmailTemplate emailTemplate = db.EmailTemplates.Where(t => t.TemplateType.KeyName == "EmailTemplateType" && t.TemplateType.KeyValues == "Welcome Message").FirstOrDefault();
            SystemSetting settings = db.SystemSettings.Where(s => s.SettingName == "SocietyName").FirstOrDefault();
            string Body, Subject;
            if (user != null && emailTemplate != null && settings != null)
            {
                Body = emailTemplate.TemplateBody;
                Body = Body.Replace("{{Name}}", user.FirstName + " " + user.LastName);
                Body = Body.Replace("{{UserName}}", user.UserName);
                Body = Body.Replace("{{Password}}", Password);
                Body = Body.Replace("{{Email}}", user.Email);
                Body = Body.Replace("{{SocietyName}}", settings.SettingValue);
                Body = Body.Replace("{{SocietyAddress}}", settings.UDK1);
                Body = Body.Replace("{{SocietyURL}}", settings.UDK2);
                Body = Body.Replace("{{SiteName}}", SiteName);
                Body = Body.Replace("{{SiteURL}}", SiteURL);

                Subject = emailTemplate.TemplateSubject.Replace("{{SocietyName}}", settings.SettingValue);

                Notification notification = new Notification();
                notification.Body = Body;
                notification.Subject = Subject;
                notification.UserID = user.Id;
                notification.TemplateID = emailTemplate.TemplateID;
                notification.CreatedDate = DateTime.Now;
                notification.ModifiedDate = notification.CreatedDate;
                notification.ReferenceTable = "Account";
                db.Notifications.Add(notification);
                db.SaveChanges();

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(emailTemplate.FromEmail, emailTemplate.FromName);
                msg.ReplyToList.Add(new MailAddress(emailTemplate.ReplyToEmail, emailTemplate.ReplyToName));
                msg.CC.Add(new MailAddress(emailTemplate.ReplyToEmail, emailTemplate.ReplyToName));
                if (!String.IsNullOrEmpty(user.Email))
                {
                    msg.To.Add(new MailAddress(user.Email, user.FirstName + " " + user.LastName));
                }
                msg.Bcc.Add(new MailAddress("societyinbox@outlook.com", "Society Inbox"));
                msg.Subject = Subject;
                msg.IsBodyHtml = true;
                msg.Body = Body;
                db.Dispose();
                return SendMail(msg);
            }
            db.Dispose();
            return false;
        }

        public bool SendNoticeBoardEmail(NoticeBoard noticeBoardInfo)
        {
            db = new SocietyManagementEntities();
            NoticeBoard noticeBoard = db.NoticeBoards.Find(noticeBoardInfo.NoticeID);
            EmailTemplate emailTemplate = db.EmailTemplates.Where(t => t.TemplateType.KeyName == "EmailTemplateType" && t.TemplateType.KeyValues == "Notice Board").FirstOrDefault();
            SystemSetting settings = db.SystemSettings.Where(s => s.SettingName == "SocietyName").FirstOrDefault();
            string Body, Subject;
            if (noticeBoard != null && emailTemplate != null && settings != null)
            {
                Body = emailTemplate.TemplateBody;
                Body = Body.Replace("{{NoticeHeading}}", noticeBoard.NoticeHeading);
                Body = Body.Replace("{{Notice}}", noticeBoard.Notice);
                Body = Body.Replace("{{NoticeDate}}", noticeBoard.NoticeDate.ToString("dd/MM/yyyy"));
                Body = Body.Replace("{{SocietyName}}", settings.SettingValue);
                Body = Body.Replace("{{SocietyAddress}}", settings.UDK1);
                Body = Body.Replace("{{SocietyURL}}", settings.UDK2);
                Body = Body.Replace("{{SiteName}}", SiteName);
                Body = Body.Replace("{{SiteURL}}", SiteURL);

                Subject = emailTemplate.TemplateSubject.Replace("{{SocietyName}}", settings.SettingValue);
                Subject = Subject.Replace("{{NoticeHeading}}", noticeBoard.NoticeHeading);
                
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(emailTemplate.FromEmail, emailTemplate.FromName);
                msg.ReplyToList.Add(new MailAddress(emailTemplate.ReplyToEmail, emailTemplate.ReplyToName));
                msg.CC.Add(new MailAddress(emailTemplate.ReplyToEmail, emailTemplate.ReplyToName));
                msg.To.Add(new MailAddress(emailTemplate.ReplyToEmail, emailTemplate.ReplyToName));
                if (WebConfigurationManager.AppSettings["TestMode"] != "true")
                {
                    var SiteUsers = db.AspNetUsers.Where(e => e.Email != null && e.Email != string.Empty & e.NoticeBoardNotification == true).ToList();
                    int count = SiteUsers.Count();
                    foreach (var siteuser in SiteUsers)
                    {
                        msg.Bcc.Add(new MailAddress(siteuser.Email, siteuser.FirstName + " " + siteuser.LastName));

                        Notification notification = new Notification();
                        notification.Body = Body;
                        notification.Subject = Subject;
                        notification.UserID = siteuser.Id;
                        notification.TemplateID = emailTemplate.TemplateID;
                        notification.CreatedDate = DateTime.Now;
                        notification.ModifiedDate = notification.CreatedDate;
                        notification.ReferenceTable = "NoticeBoard";
                        notification.ReferenceID = notification.ReferenceID;
                        db.Notifications.Add(notification);
                        db.SaveChanges();
                    }
                }
                msg.Bcc.Add(new MailAddress("societyinbox@outlook.com", "Society Inbox"));
                msg.Subject = Subject;
                msg.IsBodyHtml = true;
                msg.Body = Body;
                db.Dispose();
                return SendMail(msg);
            }
            db.Dispose();
            return false;
        }

        public bool SendPollEmail(Poll pollInfo)
        {
            db = new SocietyManagementEntities();
            Poll poll = db.Polls.Find(pollInfo.PollID);
            EmailTemplate emailTemplate = db.EmailTemplates.Where(t => t.TemplateType.KeyName == "EmailTemplateType" && t.TemplateType.KeyValues == "Opinion Poll").FirstOrDefault();
            SystemSetting settings = db.SystemSettings.Where(s => s.SettingName == "SocietyName").FirstOrDefault();
            string Body, Subject;
            if (poll != null && emailTemplate != null && settings != null)
            {
                Body = emailTemplate.TemplateBody;
                Body = Body.Replace("{{PollTitle}}", poll.PollTitle);
                Body = Body.Replace("{{Details}}", poll.Details);
                Body = Body.Replace("{{StartDate}}", poll.StartDate.ToString("dd/MM/yyyy"));
                Body = Body.Replace("{{EndDate}}", poll.StartDate.ToString("dd/MM/yyyy"));
                Body = Body.Replace("{{Options}}", poll.PollOptions.Count().ToString());
                Body = Body.Replace("{{SocietyName}}", settings.SettingValue);
                Body = Body.Replace("{{SocietyAddress}}", settings.UDK1);
                Body = Body.Replace("{{SocietyURL}}", settings.UDK2);
                Body = Body.Replace("{{SiteName}}", SiteName);
                Body = Body.Replace("{{SiteURL}}", SiteURL);

                Subject = emailTemplate.TemplateSubject.Replace("{{SocietyName}}", settings.SettingValue);
                Subject = Subject.Replace("{{NoticeHeading}}", poll.PollTitle);

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(emailTemplate.FromEmail, emailTemplate.FromName);
                msg.ReplyToList.Add(new MailAddress(emailTemplate.ReplyToEmail, emailTemplate.ReplyToName));
                msg.CC.Add(new MailAddress(emailTemplate.ReplyToEmail, emailTemplate.ReplyToName));
                msg.To.Add(new MailAddress(emailTemplate.ReplyToEmail, emailTemplate.ReplyToName));
                if (WebConfigurationManager.AppSettings["TestMode"] != "true")
                {
                    var SiteUsers = db.AspNetUsers.Where(e => e.Email != null && e.Email != string.Empty & e.NoticeBoardNotification == true).ToList();
                    int count = SiteUsers.Count();
                    foreach (var siteuser in SiteUsers)
                    {
                        msg.Bcc.Add(new MailAddress(siteuser.Email, siteuser.FirstName + " " + siteuser.LastName));

                        Notification notification = new Notification();
                        notification.Body = Body;
                        notification.Subject = Subject;
                        notification.UserID = siteuser.Id;
                        notification.TemplateID = emailTemplate.TemplateID;
                        notification.CreatedDate = DateTime.Now;
                        notification.ModifiedDate = notification.CreatedDate;
                        notification.ReferenceTable = "Poll";
                        notification.ReferenceID = poll.PollID;
                        db.Notifications.Add(notification);
                        db.SaveChanges();
                    }
                }
                msg.Bcc.Add(new MailAddress("societyinbox@outlook.com", "Society Inbox"));
                msg.Subject = Subject;
                msg.IsBodyHtml = true;
                msg.Body = Body;
                db.Dispose();
                return SendMail(msg);
            }
            db.Dispose();
            return false;
        }

    }
}