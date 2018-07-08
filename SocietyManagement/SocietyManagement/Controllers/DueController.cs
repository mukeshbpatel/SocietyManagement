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
        [Authorize(Roles = "Super,Admin,Manager")]
        public ActionResult Index(string BillDate)
        {
            DateTime dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (!string.IsNullOrEmpty(BillDate))
            {
                dt = DateTime.Parse(BillDate);                
            }
            var dues = db.Dues.Where(d => d.IsDeleted == false && d.YearID == SiteSetting.FinancialYearID && d.BillDate.Month == dt.Date.Month && d.BillDate.Year == dt.Date.Year).Include(d => d.BuildingUnit).Include(d => d.DueType);
            ViewBag.Months = Helper.GetMonths(dt);
            return View(dues.ToList());
        }

        public ActionResult MyBill(int id)
        {
            string UserID = Helper.GetUserID(User);
            var appUser = db.AspNetUsers.Find(UserID);
            if (appUser != null)
            {
                var Units = appUser.BuildingUnits.Where(b => b.IsDeleted == false).OrderBy(o => o.UnitName).ToList();
                if (Units.Count > 1)
                {
                    Units.Add(new BuildingUnit { UnitID = 0, UnitName = "All" });
                    ViewBag.UnitID = new SelectList(Units, "UnitID", "UnitName", id);
                }
                else if (Units.Count == 1)
                {
                    id = Units.FirstOrDefault().UnitID;
                    ViewBag.UnitID = new SelectList(Units, "UnitID", "UnitName", id);
                }                

                if (id == 0)
                {
                    var dues = db.Dues.Where(d => d.IsDeleted == false && d.YearID == SiteSetting.FinancialYearID).Include(d => d.BuildingUnit).Where(b => b.BuildingUnit.OwnerID == UserID).Include(d => d.DueType).OrderBy(o => o.BillDate);
                    return View(dues.ToList());
                }
                else
                {
                    var dues = db.Dues.Where(d => d.IsDeleted == false && d.YearID == SiteSetting.FinancialYearID && d.UnitID == id).Include(d => d.BuildingUnit).Where(b => b.BuildingUnit.OwnerID == UserID).Include(d => d.DueType).OrderBy(o => o.BillDate);
                    return View(dues.ToList());
                }
            }
            else
            {
                return View();
            }
        }

        public ActionResult BalanceSheet(int id)
        {
            string UserID = Helper.GetUserID(User);
            var appUser = db.AspNetUsers.Find(UserID);
            if (appUser != null)
            {
                var Units = appUser.BuildingUnits.Where(b => b.IsDeleted == false).OrderBy(o => o.UnitName);
                if (Units.Count() > 0)
                {
                    if (id == 0)
                    {
                        id = Units.FirstOrDefault().UnitID;
                    }

                    ViewBag.UnitID = new SelectList(Units, "UnitID", "UnitName", id);
                    var data = db.Database.SqlQuery<SP_BuildingUnit_BalanceSheet_Result>("Exec SP_BuildingUnit_BalanceSheet @UnitID = " + id + ",@YearID = " + SiteSetting.FinancialYearID);
                    return View(data);
                }
            }
            return View();
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
        [Authorize(Roles = "Super,Admin")]
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
        [Authorize(Roles = "Super,Admin")]
        public ActionResult Create([Bind(Include = "DueID,BillDate,UnitID,DueTypeID,DueAmount,Details,DueDate,UDK1,UDK2,UDK3,UDK4,UDK5")] Due due)
        {
            Helper.AssignUserInfo(due, User);
            if (ModelState.IsValid)
            {
                db.Dues.Add(due);
                db.SaveChanges();

                EmailNotification emailNotification = new EmailNotification();
                emailNotification.SendBillNotification(due);
                emailNotification = null;

                return RedirectToAction("Index");
            }
            ViewBag.UnitID = new SelectList(db.BuildingUnits, "UnitID", "UnitName", due.UnitID);
            ViewBag.DueTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "DueType"), "KeyID", "KeyValues", due.DueTypeID);
            return View(due);
        }

        // GET: Due/Edit/5
        [Authorize(Roles = "Super,Admin")]
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
        [Authorize(Roles = "Super,Admin")]
        public ActionResult Edit([Bind(Include = "DueID,BillDate,UnitID,DueTypeID,DueAmount,Details,DueDate,YearID,UDK1,UDK2,UDK3,UDK4,UDK5,CreatedDate")] Due due)
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
        [Authorize(Roles = "Super,Admin")]
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
        [Authorize(Roles = "Super,Admin")]
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
