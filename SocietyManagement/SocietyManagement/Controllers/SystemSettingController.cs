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
    [Authorize(Roles ="Super")]
    public class SystemSettingController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: SystemSetting
        public ActionResult Index()
        {
            return View(db.SystemSettings.OrderBy(o=>o.UDK1).ToList());
        }

        // GET: SystemSetting/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemSetting systemSetting = db.SystemSettings.Find(id);
            if (systemSetting == null)
            {
                return HttpNotFound();
            }
            return View(systemSetting);
        }

        // GET: SystemSetting/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SystemSetting/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SettingID,SettingName,SettingValue,UDK1,UDK2,UDK3,UDK4,UDK5,Details,UDK6,UDK7,UDK8,UDK9,UDK10")] SystemSetting systemSetting)
        {
            if (ModelState.IsValid)
            {
                db.SystemSettings.Add(systemSetting);
                db.SaveChanges();
                SiteSetting.AddSettings(systemSetting.UDK1, systemSetting.SettingName, systemSetting.SettingValue);
                return RedirectToAction("Index");
            }

            return View(systemSetting);
        }

        // GET: SystemSetting/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemSetting systemSetting = db.SystemSettings.Find(id);
            if (systemSetting == null)
            {
                return HttpNotFound();
            }
            return View(systemSetting);
        }

        // POST: SystemSetting/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SettingID,SettingName,SettingValue,UDK1,UDK2,UDK3,UDK4,UDK5,Details,UDK6,UDK7,UDK8,UDK9,UDK10")] SystemSetting systemSetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(systemSetting).State = EntityState.Modified;
                db.SaveChanges();
                SiteSetting.UpdateSettings(systemSetting.UDK1, systemSetting.SettingName, systemSetting.SettingValue);
                return RedirectToAction("Index");
            }
            return View(systemSetting);
        }

        // GET: SystemSetting/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemSetting systemSetting = db.SystemSettings.Find(id);
            if (systemSetting == null)
            {
                return HttpNotFound();
            }
            return View(systemSetting);
        }

        // POST: SystemSetting/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SystemSetting systemSetting = db.SystemSettings.Find(id);
            db.SystemSettings.Remove(systemSetting);
            db.SaveChanges();
            SiteSetting.SetSiteSettings();
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
