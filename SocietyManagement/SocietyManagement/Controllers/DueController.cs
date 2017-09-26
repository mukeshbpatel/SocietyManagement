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
    public class DueController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: Due
        [Authorize(Roles = "Admin,Manager,Comity")]
        public ActionResult Index()
        {
            var dues = db.Dues.Where(d=>d.IsDeleted == false).Include(d => d.BuildingUnit).Include(d => d.DueType);
            return View(dues.ToList());
        }

        public ActionResult MyBill()
        {
            string UserID = Helper.GetUserID(User);            
            var dues = db.Dues.Where(d => d.IsDeleted == false).Include(d => d.BuildingUnit).Where(b => b.BuildingUnit.OwnerID == UserID).Include(d => d.DueType).OrderBy(o=>o.BillDate);
            return View(dues.ToList());
        }

        // GET: Due/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Due due = db.Dues.Find(id);
            if (due == null)
            {
                return HttpNotFound();
            }
            return View(due);
        }

        // GET: Due/Create
        [Authorize(Roles = "Manager,Admin")]
        public ActionResult Create()
        {
            ViewBag.UnitID = new SelectList(db.BuildingUnits, "UnitID", "UnitName");            
            ViewBag.DueTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "DueType"), "KeyID", "KeyValues");
            return View();
        }

        // POST: Due/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager,Admin")]
        public ActionResult Create([Bind(Include = "DueID,BillDate,UnitID,DueTypeID,DueAmount,Details,DueDate,UDK1,UDK2,UDK3,UDK4,UDK5")] Due due)
        {
            Helper.AssignUserInfo(due, User);
            if (ModelState.IsValid)
            {
                db.Dues.Add(due);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UnitID = new SelectList(db.BuildingUnits, "UnitID", "UnitName", due.UnitID);
            ViewBag.DueTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "DueType"), "KeyID", "KeyValues", due.DueTypeID);
            return View(due);
        }

        // GET: Due/Edit/5
        [Authorize(Roles = "Manager,Admin")]
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Due due = db.Dues.Find(id);
            if (due == null)
            {
                return HttpNotFound();
            }
            ViewBag.UnitID = new SelectList(db.BuildingUnits, "UnitID", "UnitName", due.UnitID);
            ViewBag.DueTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "DueType"), "KeyID", "KeyValues", due.DueTypeID);
            return View(due);
        }

        // POST: Due/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager,Admin")]
        public ActionResult Edit([Bind(Include = "DueID,BillDate,UnitID,DueTypeID,DueAmount,Details,DueDate,UDK1,UDK2,UDK3,UDK4,UDK5,CreatedDate")] Due due)
        {
            Helper.AssignUserInfo(due, User, false);
            if (ModelState.IsValid)
            {
                db.Entry(due).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UnitID = new SelectList(db.BuildingUnits, "UnitID", "UnitName", due.UnitID);
            ViewBag.DueTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "DueType"), "KeyID", "KeyValues", due.DueTypeID);
            return View(due);
        }

        // GET: Due/Delete/5
        [Authorize(Roles = "Manager,Admin")]
        public ActionResult Delete(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Due due = db.Dues.Find(id);
            if (due == null)
            {
                return HttpNotFound();
            }
            return View(due);
        }

        // POST: Due/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Manager,Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Int64 id)
        {
            Due due = db.Dues.Find(id);
            Helper.SoftDelete(due, User);
            db.Entry(due).State = EntityState.Modified;
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
