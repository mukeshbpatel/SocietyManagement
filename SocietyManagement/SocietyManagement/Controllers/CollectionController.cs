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
    [Authorize()]
    public class CollectionController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: Collection
        [Authorize(Roles = "Admin,Manager,Comity")]
        public ActionResult Index()
        {
            var collections = db.Collections.Where(d=>d.IsDeleted==false).Include(c => c.BuildingUnit).Include(c => c.PaymentMode).OrderByDescending(o=>o.CollectionDate);
            return View(collections.ToList());
        }

        public ActionResult MyPayment()
        {
            string UserID = Helper.GetUserID(User);                        
            var collections = db.Collections.Where(d => d.IsDeleted == false).Include(c => c.BuildingUnit).Where(b => b.BuildingUnit.OwnerID == UserID).Include(c => c.PaymentMode).OrderBy(o => o.CollectionDate);
            return View(collections.ToList());
        }

        // GET: Collection/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Collection collection = db.Collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            return View(collection);
        }

        // GET: Collection/Create
        [Authorize(Roles ="Manager,Admin")]
        public ActionResult Create()
        {
            ViewBag.UnitID = new SelectList(db.BuildingUnits.Where(d=>d.IsDeleted==false).OrderBy(o=>o.UnitName), "UnitID", "UnitName");
            ViewBag.PaymentModeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "PaymentMode"), "KeyID", "KeyValues");
            return View();
        }

        // POST: Collection/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CollectionID,CollectionDate,UnitID,Amount,Discount,ReceiptNumber,PaymentModeID,Reference,ChequeBank,ChequeDate,ChequeNumber,ChequeName,Details,UDK1,UDK2,UDK3,UDK4,UDK5")] Collection collection)
        {
            Helper.AssignUserInfo(collection, User);
            if (ModelState.IsValid)
            {
                db.Collections.Add(collection);
                db.SaveChanges();

                EmailNotification notification = new EmailNotification();
                notification.SendPaymentNotification(collection);
                notification = null;

                return RedirectToAction("Index");
            }

            ViewBag.UnitID = new SelectList(db.BuildingUnits.Where(d => d.IsDeleted == false).OrderBy(o => o.UnitName), "UnitID", "UnitName",collection.UnitID);
            ViewBag.PaymentModeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "PaymentMode"), "KeyID", "KeyValues",collection.PaymentModeID);
            return View(collection);
        }

        // GET: Collection/Edit/5
        [Authorize(Roles = "Manager,Admin")]
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Collection collection = db.Collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            ViewBag.UnitID = new SelectList(db.BuildingUnits.Where(d => d.IsDeleted == false).OrderBy(o => o.UnitName), "UnitID", "UnitName", collection.UnitID);
            ViewBag.PaymentModeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "PaymentMode"), "KeyID", "KeyValues", collection.PaymentModeID);
            return View(collection);
        }

        // POST: Collection/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CollectionID,CollectionDate,UnitID,Amount,Discount,ReceiptNumber,PaymentModeID,Reference,ChequeBank,ChequeDate,ChequeNumber,ChequeName,ChequeCleared,ChequeEncashmentDate,Details,UDK1,UDK2,UDK3,UDK4,UDK5,CreatedDate")] Collection collection)
        {
            Helper.AssignUserInfo(collection, User, false);
            if (ModelState.IsValid)
            {
                db.Entry(collection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UnitID = new SelectList(db.BuildingUnits.Where(d => d.IsDeleted == false).OrderBy(o => o.UnitName), "UnitID", "UnitName", collection.UnitID);
            ViewBag.PaymentModeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "PaymentMode"), "KeyID", "KeyValues", collection.PaymentModeID);
            return View(collection);
        }

        // GET: Collection/Delete/5
        [Authorize(Roles = "Manager,Admin")]
        public ActionResult Delete(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Collection collection = db.Collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            return View(collection);
        }

        // POST: Collection/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Manager,Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Int64? id)
        {
            Collection collection = db.Collections.Find(id);
            db.Collections.Remove(collection);
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
