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

        public EmailNotification()
        {
        }

        public Boolean SendMail(MailMessage mailMessage)
        {
            bool result = false;
            try
            {
                if (SiteSetting.IsTestMode)
                {
                    try
                    {
                        for (int i = mailMessage.To.Count - 1; i >= 0; i--)
                        {
                            mailMessage.To.RemoveAt(i);
                        }
                        for (int i = mailMessage.CC.Count - 1; i >= 0; i--)
                        {
                            mailMessage.CC.RemoveAt(i);
                        }
                        for (int i = mailMessage.Bcc.Count - 1; i >= 0; i--)
                        {
                            mailMessage.Bcc.RemoveAt(i);
                        }
                        mailMessage.Bcc.Add(new MailAddress(SiteSetting.SiteBCCAddress, SiteSetting.SiteBCCName));
                    }
                    catch
                    {

                    }


                    SmtpClient client = new SmtpClient();
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.EnableSsl = SiteSetting.SMTPEnableSsl;
                    client.Host = SiteSetting.SMTPHost;
                    client.Port = SiteSetting.SMTPPort;
                    // setup Smtp authentication
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(SiteSetting.SMTPUserID, SiteSetting.SMTPPassword);
                    client.UseDefaultCredentials = false;
                    client.Credentials = credentials;
                    client.Send(mailMessage);
                    client.Dispose();
                    result = true;
                }
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
            string Body, Subject;
            if (collection != null && emailTemplate != null)
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
                Body = UpdateBodySubject(Body);
                
                Subject = emailTemplate.TemplateSubject;
                Subject = Subject.Replace("{{ReceiptNumber}}", collection.ReceiptNumber);
                Subject = Subject.Replace("{{PaymentDate}}", collection.CollectionDate.ToString("dd/MM/yyyy"));
                Subject = Subject.Replace("{{PaymentId}}", collection.CollectionID.ToString());
                Subject = UpdateBodySubject(Subject);

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

                MailMessage msg = CreateMessage();
                if (collection.BuildingUnit.Owner!= null && !String.IsNullOrEmpty(collection.BuildingUnit.Owner.Email))
                {
                    if (collection.BuildingUnit.Owner.PaymentNotification)
                        msg.To.Add(new MailAddress(collection.BuildingUnit.Owner.Email, collection.BuildingUnit.Owner.Name));
                }
                msg.Subject = Subject;
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
            string Body, Subject;
            if (due != null && emailTemplate != null)
            {
                Body = emailTemplate.TemplateBody;
                if (due.BuildingUnit.OwnerID != null)
                {
                    Body = Body.Replace("{{Name}}", due.BuildingUnit.Owner.Name);
                }
                else
                {
                    Body = Body.Replace("{{Name}}", string.Empty);
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
                Body = UpdateBodySubject(Body);

                Subject = emailTemplate.TemplateSubject;
                Subject = Subject.Replace("{{BillNumber}}", due.DueID.ToString());
                Subject = Subject.Replace("{{BillDate}}", due.BillDate.ToString("dd-MMM-yyyy"));
                Subject = Subject.Replace("{{BillMonth}}", due.BillDate.ToString("MMM-yyyy"));
                Subject = Subject.Replace("{{BillYear}}", due.BillDate.ToString("yyyy"));
                Subject = Subject.Replace("{{BillDetail}}", due.Details);
                Subject = Subject.Replace("{{UnitName}}", due.BuildingUnit.UnitName);
                Subject = UpdateBodySubject(Subject);

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

                MailMessage msg = CreateMessage();
                if (due.BuildingUnit.OwnerID != null && !String.IsNullOrEmpty(due.BuildingUnit.Owner.Email))
                {
                    if (due.BuildingUnit.Owner.BillNotification)
                        msg.To.Add(new MailAddress(due.BuildingUnit.Owner.Email, due.BuildingUnit.Owner.Name));
                }
                msg.Subject = Subject;
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
            string Body, Subject;
            if (user != null && emailTemplate != null)
            {
                Body = emailTemplate.TemplateBody;
                if (!string.IsNullOrEmpty(user.FirstName))
                {
                    Body = Body.Replace("{{Name}}", user.FirstName);
                }
                Body = Body.Replace("{{UserName}}", user.UserName);
                Body = Body.Replace("{{PasswordURL}}", callbackUrl);
                Body = Body.Replace("{{Email}}", user.Email);
                Body = UpdateBodySubject(Body);

                Subject = emailTemplate.TemplateSubject;
                Subject = UpdateBodySubject(Subject);

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
                
                MailMessage msg = CreateMessage();
                if (!String.IsNullOrEmpty(user.Email))
                {
                    msg.To.Add(new MailAddress(user.Email, user.FirstName + " " + user.LastName));
                }
                msg.Subject = Subject;
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
            string Body, Subject;
            if (user != null && emailTemplate != null)
            {
                Body = emailTemplate.TemplateBody;
                if (!string.IsNullOrEmpty(user.FirstName))
                {
                    Body = Body.Replace("{{Name}}", user.FirstName + " " + user.LastName);
                }
                Body = Body.Replace("{{UserName}}", user.UserName);
                Body = Body.Replace("{{Password}}", Password);
                Body = Body.Replace("{{Email}}", user.Email);
                Body = UpdateBodySubject(Body);

                Subject = emailTemplate.TemplateSubject;
                Subject = UpdateBodySubject(Subject);

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

                MailMessage msg = CreateMessage();
                if (!String.IsNullOrEmpty(user.Email))
                {
                    msg.To.Add(new MailAddress(user.Email, user.FirstName + " " + user.LastName));
                }
                msg.Subject = Subject;
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
            string Body, Subject;
            if (user != null && emailTemplate != null)
            {
                Body = emailTemplate.TemplateBody;
                Body = Body.Replace("{{Name}}", user.FirstName + " " + user.LastName);
                Body = Body.Replace("{{UserName}}", user.UserName);
                Body = Body.Replace("{{Password}}", Password);
                Body = Body.Replace("{{Email}}", user.Email);
                Body = UpdateBodySubject(Body);

                Subject = emailTemplate.TemplateSubject;
                Subject = UpdateBodySubject(Subject);

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
                MailMessage msg = CreateMessage();
                if (!String.IsNullOrEmpty(user.Email))
                {
                    msg.To.Add(new MailAddress(user.Email, user.FirstName + " " + user.LastName));
                }
                msg.Subject = Subject;
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
            string Body, Subject;
            if (noticeBoard != null && emailTemplate != null)
            {
                Body = emailTemplate.TemplateBody;
                Body = Body.Replace("{{NoticeHeading}}", noticeBoard.NoticeHeading);
                Body = Body.Replace("{{Notice}}", noticeBoard.Notice);
                Body = Body.Replace("{{NoticeDate}}", noticeBoard.NoticeDate.ToString("dd/MM/yyyy"));
                Body = UpdateBodySubject(Body); 

                Subject = emailTemplate.TemplateSubject;
                Subject = Subject.Replace("{{NoticeHeading}}", noticeBoard.NoticeHeading);
                Subject = UpdateBodySubject(Subject);

                MailMessage msg = CreateMessage();
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
                msg.Subject = Subject;
                msg.Body = Body;
                db.Dispose();
                return SendMail(msg);
            }
            db.Dispose();
            return false;
        }

        private string UpdateBodySubject(string str)
        {
            str = str.Replace("{{SocietyName}}", SiteSetting.SocietyName);
            str = str.Replace("{{SocietyAddress}}", SiteSetting.SocietyAddress);
            str = str.Replace("{{SocietyURL}}", SiteSetting.SocietyURL);
            str = str.Replace("{{SiteName}}", SiteSetting.SiteName);
            str = str.Replace("{{SiteURL}}", SiteSetting.SiteURL);
            return str;
        }
        
        private MailMessage CreateMessage()
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(SiteSetting.SocietyFromEmailAddress, SiteSetting.SocietyFromEmailName);
            msg.ReplyToList.Add(new MailAddress(SiteSetting.SocietyReplyToEmailAddress, SiteSetting.SocietyReplyToEmailName));
            msg.CC.Add(new MailAddress(SiteSetting.SocietyFromEmailAddress, SiteSetting.SocietyFromEmailName));
            msg.Bcc.Add(new MailAddress(SiteSetting.SiteBCCAddress, SiteSetting.SiteBCCName));
            msg.IsBodyHtml = true;
            return msg;
        }

        public bool SendPollEmail(Poll pollInfo)
        {
            db = new SocietyManagementEntities();
            Poll poll = db.Polls.Find(pollInfo.PollID);
            EmailTemplate emailTemplate = db.EmailTemplates.Where(t => t.TemplateType.KeyName == "EmailTemplateType" && t.TemplateType.KeyValues == "Opinion Poll").FirstOrDefault();
            string Body, Subject;
            if (poll != null && emailTemplate != null)
            {
                Body = emailTemplate.TemplateBody;
                Body = Body.Replace("{{PollTitle}}", poll.PollTitle);
                Body = Body.Replace("{{Details}}", poll.Details);
                Body = Body.Replace("{{StartDate}}", poll.StartDate.ToString("dd/MM/yyyy"));
                Body = Body.Replace("{{EndDate}}", poll.StartDate.ToString("dd/MM/yyyy"));
                Body = Body.Replace("{{Options}}", poll.PollOptions.Count().ToString());
                Body = UpdateBodySubject(Body);
                
                Subject = emailTemplate.TemplateSubject;
                Subject = Subject.Replace("{{PollTitle}}", poll.PollTitle);
                Subject = UpdateBodySubject(Subject);

                var msg = CreateMessage();
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
                msg.Subject = Subject;
                msg.Body = Body;
                db.Dispose();
                return SendMail(msg);
            }
            db.Dispose();
            return false;
        }

    }
}