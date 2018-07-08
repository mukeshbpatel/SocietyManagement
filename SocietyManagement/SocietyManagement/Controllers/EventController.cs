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
    public class EventController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: Event
        public ActionResult Index()
        {
            var events = db.Events.Where(d => d.IsDeleted == false).Include(e => e.EventType).OrderByDescending(o => o.EventDate);
            return View(events.ToList());
        }

        // GET: Event/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event objEvent = db.Events.Find(id);
            if (objEvent == null)
            {
                return HttpNotFound();
            }
            return View(objEvent);
        }

        // GET: Event/Create
        [Authorize(Roles = "Super,Admin,Manager")]
        public ActionResult Create()
        {
            ViewBag.EventTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "EventType"), "KeyID", "KeyValues");
            return View();
        }

        // POST: Event/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super,Admin,Manager")]
        public ActionResult Create([Bind(Include = "EventID,EventDate,EventTypeID,EventName,Details,UDK1,UDK2,UDK3,UDK4,UDK5")] Event objEvent)
        {
            Helper.AssignUserInfo(objEvent, User);
            if (ModelState.IsValid)
            {
                db.Events.Add(objEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "EventType"), "KeyID", "KeyValues", objEvent.EventTypeID);
            return View(objEvent);
        }

        // GET: Event/Edit/5
        [Authorize(Roles = "Super,Admin,Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event objEvent = db.Events.Find(id);
            if (objEvent == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "EventType"), "KeyID", "KeyValues", objEvent.EventTypeID);
            return View(objEvent);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super,Admin,Manager")]
        public ActionResult Edit([Bind(Include = "EventID,EventDate,EventTypeID,EventName,Details,UDK1,UDK2,UDK3,UDK4,UDK5,CreatedDate")] Event objEvent)
        {
            Helper.AssignUserInfo(objEvent, User, false);
            if (ModelState.IsValid)
            {
                db.Entry(objEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "EventType"), "KeyID", "KeyValues", objEvent.EventTypeID);
            return View(objEvent);
        }

        // GET: Event/Delete/5
        [Authorize(Roles = "Super,Admin,Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event objEvent = db.Events.Find(id);
            if (objEvent == null)
            {
                return HttpNotFound();
            }
            return View(objEvent);
        }

        // POST: Event/Delete/5
        [Authorize(Roles = "Super,Admin,Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event objEvent = db.Events.Find(id);
            Helper.SoftDelete(objEvent, User);
            db.Entry(objEvent).State = EntityState.Modified;
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
