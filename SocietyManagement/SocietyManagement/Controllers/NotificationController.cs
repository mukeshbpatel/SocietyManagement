using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SocietyManagement.Models;

namespace SocietyManagement.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: Notification
        public ActionResult Index()
        {
            var userID = Helper.GetUserID(User);
            var notifications = db.Notifications.Where(d => d.UserID == userID && d.IsDeleted==false && d.IsDeleted == false).Include(n => n.AspNetUser).Include(n => n.EmailTemplate).OrderByDescending(o=>o.CreatedDate);
            return View(notifications.ToList());
        }

        [HttpPost]
        public JsonResult NotificationCount(string name)
        {
            var userID = Helper.GetUserID(User);
            var notifications = db.Notifications.Where(d => d.IsDeleted == false && d.UserID == userID && d.IsArchive==false && d.IsRead == false);
            if (notifications.Count() > 0)
                return Json(notifications.Count());
            else
                return Json(string.Empty);
        }

        [HttpPost]
        public JsonResult GetNotification(string name)
        {
            var userID = Helper.GetUserID(User);
            var notifications = db.Notifications.Where(d => d.UserID == userID && d.IsDeleted == false && d.IsDeleted == false).Include(n => n.EmailTemplate).OrderByDescending(o => o.CreatedDate);
            return Json(notifications.ToList());
        }

        // GET: Notification/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            var userID = Helper.GetUserID(User);
            if (userID != notification.UserID)
            {
                return HttpNotFound();
            }

            notification.IsRead = true;
            Helper.AssignUserInfo(notification, User, false);
            db.Entry(notification).State = EntityState.Modified;
            db.SaveChanges();
            notification = db.Notifications.Find(id);
            return View(notification);
        }

        // GET: Notification/Delete/5
        public ActionResult Delete(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            var userID = Helper.GetUserID(User);
            if (userID != notification.UserID)
            {
                return HttpNotFound();
            }
            return View(notification);
        }

        // POST: Notification/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Notification notification = db.Notifications.Find(id);
            Helper.SoftDelete(notification, User);
            db.Entry(notification).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
