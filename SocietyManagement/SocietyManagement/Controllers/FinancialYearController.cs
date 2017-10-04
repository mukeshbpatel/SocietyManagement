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
    [Authorize(Roles ="Manager, Admin")]
    public class FinancialYearController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: FinancialYear
        public ActionResult Index()
        {
            return View(db.FinancialYears.Where(d=>d.IsDeleted==false).OrderByDescending(o=>o.StartDate).ToList());
        }

        // GET: FinancialYear/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinancialYear financialYear = db.FinancialYears.Find(id);
            if (financialYear == null)
            {
                return HttpNotFound();
            }
            return View(financialYear);
        }

        // GET: FinancialYear/Create
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: FinancialYear/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "YearID,YearName,StartDate,EndDate,IsActive,IsClosed,UDK1,UDK2,UDK3,UDK4,UDK5")] FinancialYear financialYear)
        {
            Helper.AssignUserInfo(financialYear, User);
            if (ModelState.IsValid)
            {
                db.FinancialYears.Add(financialYear);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(financialYear);
        }

        // GET: FinancialYear/Edit/5
        [Authorize(Roles ="Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinancialYear financialYear = db.FinancialYears.Find(id);
            if (financialYear == null)
            {
                return HttpNotFound();
            }
            return View(financialYear);
        }

        // POST: FinancialYear/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "YearID,YearName,StartDate,EndDate,IsActive,IsClosed,UDK1,UDK2,UDK3,UDK4,UDK5,CreatedDate")] FinancialYear financialYear)
        {
            Helper.AssignUserInfo(financialYear, User, false);
            if (ModelState.IsValid)
            {
                db.Entry(financialYear).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(financialYear);
        }

        // GET: FinancialYear/Delete/5
        [Authorize(Roles ="Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinancialYear financialYear = db.FinancialYears.Find(id);
            if (financialYear == null)
            {
                return HttpNotFound();
            }
            return View(financialYear);
        }

        // POST: FinancialYear/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            FinancialYear financialYear = db.FinancialYears.Find(id);
            Helper.SoftDelete(financialYear, User);
            db.SaveChanges();
            db.Entry(financialYear).State = EntityState.Modified;
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
