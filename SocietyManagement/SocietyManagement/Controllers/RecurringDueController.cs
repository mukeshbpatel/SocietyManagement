using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SocietyManagement.Models;

namespace SocietyManagement.Views
{
    [Authorize(Roles = "Manager,Admin")]
    public class RecurringDueController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        [Authorize(Roles = "Admin")]
        public ActionResult CalculateBill(int id,string RecurringDate)
        {
            DateTime dt = DateTime.Now;
            if (!string.IsNullOrEmpty(RecurringDate))
                dt = DateTime.Parse(RecurringDate);
            else
                return RedirectToAction("CalculateBill", new { RecurringDate = dt.ToString("dd-MM-yyyy") });

            RecurringBill recurringBill = new RecurringBill();            
            return View(recurringBill.CalculateRecurringBill(User, dt));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CalculateBill(string RecurringDate)
        {
            DateTime dt = DateTime.Now;
            if (!string.IsNullOrEmpty(RecurringDate))
                dt = DateTime.Parse(RecurringDate);
            RecurringBill recurringBill = new RecurringBill();
            if (recurringBill.EnterRecurringBill(User,dt))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
            }
        }

        // GET: RecurringDue
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                var recurringDues = db.RecurringDues.Where(d => d.IsDeleted == false).Include(r => r.BuildingUnit).Include(r => r.DueType).Include(r => r.RecurringType).OrderBy(o => o.BuildingUnit.UnitName);
                return View(recurringDues.ToList());
            }
            else
            {
                var recurringDues = db.RecurringDues.Where(d => d.IsDeleted == false & d.UnitID == id).Include(r => r.BuildingUnit).Include(r => r.DueType).Include(r => r.RecurringType).OrderBy(o => o.BuildingUnit.UnitName);
                return View(recurringDues.ToList());
            }
        }

        // GET: RecurringDue/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecurringDue recurringDue = db.RecurringDues.Find(id);
            if (recurringDue == null)
            {
                return HttpNotFound();
            }
            return View(recurringDue);
        }

        // GET: RecurringDue/Create
        public ActionResult Create()
        {
            ViewBag.UnitID = new SelectList(db.BuildingUnits.Where(d=>d.IsDeleted == false).OrderBy(o=>o.UnitName), "UnitID", "UnitName");
            ViewBag.DueTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "DueType"), "KeyID", "KeyValues");
            ViewBag.RecurringTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "RecurringType"), "KeyID", "KeyValues");
            return View();
        }

        // POST: RecurringDue/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RecurringID,UnitID,StartDate,EndDate,RecurringTypeID,DueTypeID,Amount,DueDays,UDK1,UDK2,UDK3,UDK4,UDK5")] RecurringDue recurringDue)
        {
            Helper.AssignUserInfo(recurringDue, User);
            if (ModelState.IsValid)
            {
                db.RecurringDues.Add(recurringDue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UnitID = new SelectList(db.BuildingUnits.Where(d => d.IsDeleted == false).OrderBy(o => o.UnitName), "UnitID", "UnitName",recurringDue.UnitID);
            ViewBag.DueTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "DueType"), "KeyID", "KeyValues",recurringDue.DueTypeID);
            ViewBag.RecurringTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "RecurringType"), "KeyID", "KeyValues",recurringDue.RecurringID);
            return View(recurringDue);
        }

        // GET: RecurringDue/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecurringDue recurringDue = db.RecurringDues.Find(id);
            if (recurringDue == null)
            {
                return HttpNotFound();
            }
            ViewBag.UnitID = new SelectList(db.BuildingUnits.Where(d => d.IsDeleted == false).OrderBy(o => o.UnitName), "UnitID", "UnitName", recurringDue.UnitID);
            ViewBag.DueTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "DueType"), "KeyID", "KeyValues", recurringDue.DueTypeID);
            ViewBag.RecurringTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "RecurringType"), "KeyID", "KeyValues", recurringDue.RecurringID);
            return View(recurringDue);
        }

        // POST: RecurringDue/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecurringID,UnitID,StartDate,EndDate,RecurringTypeID,DueTypeID,Amount,DueDays,UDK1,UDK2,UDK3,UDK4,UDK5,CreatedDate")] RecurringDue recurringDue)
        {
            Helper.AssignUserInfo(recurringDue, User, false);
            if (ModelState.IsValid)
            {
                db.Entry(recurringDue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UnitID = new SelectList(db.BuildingUnits.Where(d => d.IsDeleted == false).OrderBy(o => o.UnitName), "UnitID", "UnitName", recurringDue.UnitID);
            ViewBag.DueTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "DueType"), "KeyID", "KeyValues", recurringDue.DueTypeID);
            ViewBag.RecurringTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "RecurringType"), "KeyID", "KeyValues", recurringDue.RecurringID);
            return View(recurringDue);
        }

        // GET: RecurringDue/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecurringDue recurringDue = db.RecurringDues.Find(id);
            if (recurringDue == null)
            {
                return HttpNotFound();
            }
            return View(recurringDue);
        }

        // POST: RecurringDue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RecurringDue recurringDue = db.RecurringDues.Find(id);
            Helper.SoftDelete(recurringDue, User);
            db.Entry(recurringDue).State = EntityState.Modified;
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
