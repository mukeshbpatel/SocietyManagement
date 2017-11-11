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
    public class ExpenseController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: Expense
        [Authorize(Roles = "SuperUser,Admin,Manager")]
        public ActionResult Index()
        {
            var expenses = db.Expenses.Where(d=>d.IsDeleted==false).Include(e => e.PaymentMode).Include(e => e.ExpenseType).OrderBy(o=>o.ExpenseDate);
            return View(expenses.ToList());
        }

        // GET: Expense/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        // GET: Expense/Create
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Create()
        {
            ViewBag.PaymentModeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "PaymentMode"), "KeyID", "KeyValues");
            ViewBag.ExpenseTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "ExpenseType"), "KeyID", "KeyValues");
            return View();
        }

        // POST: Expense/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExpenseID,ExpenseDate,PaidTo,ExpenseTypeID,Details,Amount,TDS,PaymentModeID,ChequeBank,ChequeName,ChequeDate,ChequeNumber,ChequeCleared,ChequeEncashmentDate,UDK1,UDK2,UDK3,UDK4,UDK5")] Expense expense)
        {
            Helper.AssignUserInfo(expense, User);
            if (ModelState.IsValid)
            {
                db.Expenses.Add(expense);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PaymentModeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "PaymentMode"), "KeyID", "KeyValues",expense.PaymentModeID);
            ViewBag.ExpenseTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "ExpenseType"), "KeyID", "KeyValues",expense.ExpenseTypeID);
            return View(expense);
        }

        // GET: Expense/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaymentModeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "PaymentMode"), "KeyID", "KeyValues", expense.PaymentModeID);
            ViewBag.ExpenseTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "ExpenseType"), "KeyID", "KeyValues", expense.ExpenseTypeID);
            return View(expense);
        }

        // POST: Expense/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Edit([Bind(Include = "ExpenseID,ExpenseDate,PaidTo,ExpenseTypeID,Details,Amount,TDS,PaymentModeID,ChequeBank,ChequeName,ChequeDate,ChequeNumber,ChequeCleared,ChequeEncashmentDate,YearID,UDK1,UDK2,UDK3,UDK4,UDK5,CreatedDate")] Expense expense)
        {
            Helper.AssignUserInfo(expense, User, false);
            if (ModelState.IsValid)
            {
                db.Entry(expense).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PaymentModeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "PaymentMode"), "KeyID", "KeyValues", expense.PaymentModeID);
            ViewBag.ExpenseTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "ExpenseType"), "KeyID", "KeyValues", expense.ExpenseTypeID);
            return View(expense);
        }

        // GET: Expense/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Delete(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        // POST: Expense/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult DeleteConfirmed(Int64 id)
        {
            Expense expense = db.Expenses.Find(id);
            Helper.SoftDelete(expense, User);
            db.Entry(expense).State = EntityState.Modified;
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
