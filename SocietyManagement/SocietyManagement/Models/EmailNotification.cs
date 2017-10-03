using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace SocietyManagement.Models
{
    public class EmailNotification
    {
        private SocietyManagementEntities db;        

        public EmailNotification()
        {
            
        }

        public Boolean SendMail(MailMessage mailMessage)
        {
            bool result = false;
            try
            {
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
                    notification.UDK1 = collection.Details;
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
                    notification.UDK1 = due.Details;
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

                Subject = emailTemplate.TemplateSubject.Replace("{{SocietyName}}", settings.SettingValue);

                Notification notification = new Notification();
                notification.Body = Body;
                notification.Subject = Subject;
                notification.UserID = user.Id;
                notification.TemplateID = emailTemplate.TemplateID;
                notification.CreatedDate = DateTime.Now;
                notification.ModifiedDate = notification.CreatedDate;
                notification.UDK1 = "Forgot Password";
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
                    Body = Body.Replace("{{Name}}", user.FirstName);
                }
                Body = Body.Replace("{{UserName}}", user.UserName);
                Body = Body.Replace("{{Password}}", Password);
                Body = Body.Replace("{{Email}}", user.Email);
                Body = Body.Replace("{{SocietyName}}", settings.SettingValue);
                Body = Body.Replace("{{SocietyAddress}}", settings.UDK1);
                Body = Body.Replace("{{SocietyURL}}", settings.UDK2);

                Subject = emailTemplate.TemplateSubject.Replace("{{SocietyName}}", settings.SettingValue);

                Notification notification = new Notification();
                notification.Body = Body;
                notification.Subject = Subject;
                notification.UserID = user.Id;
                notification.TemplateID = emailTemplate.TemplateID;
                notification.CreatedDate = DateTime.Now;
                notification.ModifiedDate = notification.CreatedDate;
                notification.UDK1 = "Password Changed";
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
    }
}