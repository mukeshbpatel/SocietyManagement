﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SocietyManagement.Models;
using System.IO;

namespace SocietyManagement.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: Expense
        [Authorize(Roles = "Super,Admin,Manager")]
        public ActionResult Index(string date)
        {
            DateTime dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (!string.IsNullOrEmpty(date))
            {
                dt = DateTime.Parse(date);
            }

            var expenses = db.Expenses.Where(d => d.IsDeleted == false && d.YearID == SiteSetting.FinancialYearID && d.ExpenseDate.Month == dt.Date.Month && d.ExpenseDate.Year == dt.Date.Year).Include(e => e.PaymentMode).Include(e => e.ExpenseType).OrderBy(o => o.ExpenseDate);
            ViewBag.Months = Helper.GetMonths(dt);
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
        [Authorize(Roles = "Admin,Manager,Super")]
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
        [Authorize(Roles = "Admin,Manager,Super")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExpenseID,ExpenseDate,PaidTo,ExpenseTypeID,Details,Amount,TDS,PaymentModeID,ChequeBank,ChequeName,ChequeDate,ChequeNumber,ChequeCleared,ChequeEncashmentDate,Files,UDK1,UDK2,UDK3,UDK4,UDK5")] Expense expense)
        {
            Helper.AssignUserInfo(expense, User);
            if (ModelState.IsValid)
            {
                db.Expenses.Add(expense);
                db.SaveChanges();

                foreach (HttpPostedFileBase file in expense.Files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var media = new ExpenseMedia();
                        media.ExpenseID = expense.ExpenseID;
                        media.MediaType = file.ContentType;
                        media.FileName = Path.GetFileName(file.FileName);
                        media.MediaTitle = Path.GetFileNameWithoutExtension(file.FileName);
                        byte[] bytes = null;
                        using (BinaryReader br = new BinaryReader(file.InputStream))
                        {
                            bytes = br.ReadBytes(file.ContentLength);
                        }
                        media.MediaData = bytes;
                        Helper.AssignUserInfo(media, User);
                        db.ExpenseMedias.Add(media);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }
            ViewBag.PaymentModeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "PaymentMode"), "KeyID", "KeyValues",expense.PaymentModeID);
            ViewBag.ExpenseTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "ExpenseType"), "KeyID", "KeyValues",expense.ExpenseTypeID);
            return View(expense);
        }

        // GET: Expense/Edit/5
        [Authorize(Roles = "Admin,Manager,Super")]
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
        [Authorize(Roles = "Admin,Manager,Super")]
        public ActionResult Edit([Bind(Include = "ExpenseID,ExpenseDate,PaidTo,ExpenseTypeID,Details,Amount,TDS,PaymentModeID,ChequeBank,ChequeName,ChequeDate,ChequeNumber,ChequeCleared,ChequeEncashmentDate,YearID,Files,UDK1,UDK2,UDK3,UDK4,UDK5,CreatedDate")] Expense expense)
        {
            Helper.AssignUserInfo(expense, User, false);
            if (ModelState.IsValid)
            {
                db.Entry(expense).State = EntityState.Modified;
                db.SaveChanges();

                foreach (HttpPostedFileBase file in expense.Files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var media = new ExpenseMedia();
                        media.ExpenseID = expense.ExpenseID;
                        media.MediaType = file.ContentType;
                        media.FileName = Path.GetFileName(file.FileName);
                        media.MediaTitle = Path.GetFileNameWithoutExtension(file.FileName);
                        byte[] bytes = null;
                        using (BinaryReader br = new BinaryReader(file.InputStream))
                        {
                            bytes = br.ReadBytes(file.ContentLength);
                        }
                        media.MediaData = bytes;
                        Helper.AssignUserInfo(media, User);
                        db.ExpenseMedias.Add(media);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }
            ViewBag.PaymentModeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "PaymentMode"), "KeyID", "KeyValues", expense.PaymentModeID);
            ViewBag.ExpenseTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "ExpenseType"), "KeyID", "KeyValues", expense.ExpenseTypeID);
            return View(expense);
        }

        // GET: Expense/Delete/5
        [Authorize(Roles = "Admin,Manager,Super")]
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
        [Authorize(Roles = "Admin,Manager,Super")]
        public ActionResult DeleteConfirmed(Int64 id)
        {
            Expense expense = db.Expenses.Find(id);
            Helper.SoftDelete(expense, User);
            db.Entry(expense).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public FileResult DownloadFile(Int64? id)
        {
            if (id != null)
            {
                var media = db.ExpenseMedias.Find(id);
                if (media == null)
                {
                    return null;
                }
                return File(media.MediaData, media.MediaType, media.MediaTitle);
            }
            else
                return null;
        }

        // GET: Expense/Edit/5
        [Authorize(Roles = "Admin,Manager,Super")]
        public ActionResult EditFile(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseMedia expenseMedia = db.ExpenseMedias.Find(id);
            if (expenseMedia == null)
            {
                return HttpNotFound();
            }
            return View(expenseMedia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,Super")]
        public ActionResult EditFile(ExpenseMedia expenseMedia)
        {
            Helper.AssignUserInfo(expenseMedia, User, false);
            if (ModelState.IsValid)
            {
                var media = db.ExpenseMedias.Find(expenseMedia.MediaID);
                media.MediaTitle = expenseMedia.MediaTitle;
                media.FileName = expenseMedia.FileName;
                Helper.AssignUserInfo(media, User, false);
                db.Entry(media).State = EntityState.Modified;
                db.SaveChanges();                
                return RedirectToAction("Details",new {id = expenseMedia.ExpenseID });
            }       
            return View(expenseMedia);
        }

        [Authorize(Roles = "Admin,Manager,Super")]
        public ActionResult DeleteFile(Int64? id)
        {
            ExpenseMedia expenseMedia = db.ExpenseMedias.Find(id);
            Helper.SoftDelete(expenseMedia, User);
            db.Entry(expenseMedia).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = expenseMedia.ExpenseID });
        }

        [Authorize]
        public ActionResult ExpenseReport()
        {
            var data = db.Database.SqlQuery<dynamic>("EXEC [dbo].[SP_ExpenseReport] @YearID = " + SiteSetting.FinancialYearID);
            return View(data);
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
