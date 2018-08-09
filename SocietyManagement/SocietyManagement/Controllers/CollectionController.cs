using System;
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
    [Authorize()]
    public class CollectionController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: Collection
        [Authorize(Roles = "Super,Admin,Manager")]
        public ActionResult Index(string CollectionDate)
        {
            DateTime dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (!string.IsNullOrEmpty(CollectionDate))
            {
                dt = DateTime.Parse(CollectionDate);
            }
            var collections = db.Collections.Where(d=>d.IsDeleted==false && d.YearID == SiteSetting.FinancialYearID && d.CollectionDate.Month == dt.Date.Month && d.CollectionDate.Year == dt.Date.Year).Include(c => c.BuildingUnit).Include(c => c.PaymentMode).OrderByDescending(o=>o.CollectionDate);
            ViewBag.Months = Helper.GetMonths(dt);
            return View(collections.ToList());
        }

        public ActionResult MyPayment(int id = 0)
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
                    var collections = db.Collections.Where(d => d.IsDeleted == false && d.YearID == SiteSetting.FinancialYearID).Include(c => c.BuildingUnit).Where(b => b.BuildingUnit.OwnerID == UserID).Include(c => c.PaymentMode).OrderBy(o => o.CollectionDate);
                    return View(collections.ToList());
                }
                else
                {                    
                    var collections = db.Collections.Where(d => d.IsDeleted == false && d.YearID == SiteSetting.FinancialYearID && d.UnitID == id).Include(c => c.BuildingUnit).Where(b => b.BuildingUnit.OwnerID == UserID).Include(c => c.PaymentMode).OrderBy(o => o.CollectionDate);
                    return View(collections.ToList());
                }
            }
            else
            {
                return View();
            }
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
        [Authorize(Roles ="Super,Admin")]
        public ActionResult Create(Int64? id)
        {
            Collection collection = new Collection();
            collection.CollectionDate = DateTime.Today.Date;
            collection.Discount = 0;
            if (id != null)
            {
                var due = db.Dues.Find(id);
                if (due != null)
                {
                    collection.Amount = due.DueAmount;
                    collection.FromDate = due.BillDate;
                    collection.ToDate = due.BillDate.AddMonths(1).AddDays(-1);
                    var dues = due.Penalties.Where(d => d.IsDeleted == false);
                    if (dues.Any())
                        collection.LateFeeFine = dues.Sum(p => p.Amount);
                    collection.UnitID = due.UnitID;
                }
            }
            ViewBag.UnitID = new SelectList(db.BuildingUnits.Where(d=>d.IsDeleted==false).OrderBy(o=>o.UnitName), "UnitID", "UnitName", collection.UnitID);
            ViewBag.PaymentModeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "PaymentMode"), "KeyID", "KeyValues");
            return View(collection);
        }

        // POST: Collection/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Super,Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CollectionID,CollectionDate,UnitID,FromDate,ToDate,Amount,Discount,Other,OtherAmount,LateFeeFine,ReceiptNumber,PaymentModeID,Reference,ChequeBank,ChequeDate,ChequeNumber,ChequeName,Details,Files,UDK1,UDK2,UDK3,UDK4,UDK5")] Collection collection)
        {
            Helper.AssignUserInfo(collection, User);
            if (ModelState.IsValid)
            {
                db.Collections.Add(collection);
                db.SaveChanges();

                foreach (HttpPostedFileBase file in collection.Files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var media = new CollectionMedia();
                        media.CollectionID = collection.CollectionID;
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
                        db.CollectionMedias.Add(media);
                        db.SaveChanges();
                    }
                }

                EmailNotification notification = new EmailNotification();
                notification.SendPaymentNotification(collection);
                notification = null;

                return RedirectToAction("Details", "BuildingUnit", new { id = collection.UnitID });
            }

            ViewBag.UnitID = new SelectList(db.BuildingUnits.Where(d => d.IsDeleted == false).OrderBy(o => o.UnitName), "UnitID", "UnitName",collection.UnitID);
            ViewBag.PaymentModeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "PaymentMode"), "KeyID", "KeyValues",collection.PaymentModeID);
            return View(collection);
        }

        // GET: Collection/Edit/5
        [Authorize(Roles = "Super,Admin")]
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
        [Authorize(Roles = "Super,Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CollectionID,CollectionDate,UnitID,FromDate,ToDate,Amount,Discount,Other,OtherAmount,LateFeeFine,ReceiptNumber,PaymentModeID,Reference,ChequeBank,ChequeDate,ChequeNumber,ChequeName,ChequeCleared,ChequeEncashmentDate,Details,YearID,Files,UDK1,UDK2,UDK3,UDK4,UDK5,CreatedDate")] Collection collection)
        {
            Helper.AssignUserInfo(collection, User, false);
            if (ModelState.IsValid)
            {
                db.Entry(collection).State = EntityState.Modified;
                db.SaveChanges();

                foreach (HttpPostedFileBase file in collection.Files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var media = new CollectionMedia();
                        media.CollectionID = collection.CollectionID;
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
                        db.CollectionMedias.Add(media);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Details", "BuildingUnit", new { id = collection.UnitID });
            }
            ViewBag.UnitID = new SelectList(db.BuildingUnits.Where(d => d.IsDeleted == false).OrderBy(o => o.UnitName), "UnitID", "UnitName", collection.UnitID);
            ViewBag.PaymentModeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "PaymentMode"), "KeyID", "KeyValues", collection.PaymentModeID);
            return View(collection);
        }

        // GET: Collection/Delete/5
        [Authorize(Roles = "Super,Admin")]
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
        [Authorize(Roles = "Super,Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Int64? id)
        {
            Collection collection = db.Collections.Find(id);
            db.Collections.Remove(collection);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public FileResult DownloadFile(int? id)
        {
            if (id != null)
            {
                var media = db.CollectionMedias.Find(id);
                if (media == null)
                {
                    return null;
                }
                return File(media.MediaData, media.MediaType, media.FileName);
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
            CollectionMedia collectionMedia = db.CollectionMedias.Find(id);
            if (collectionMedia == null)
            {
                return HttpNotFound();
            }
            return View(collectionMedia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,Super")]
        public ActionResult EditFile(CollectionMedia collectionMedia)
        {
            Helper.AssignUserInfo(collectionMedia, User, false);
            if (ModelState.IsValid)
            {
                var media = db.CollectionMedias.Find(collectionMedia.MediaID);
                media.MediaTitle = collectionMedia.MediaTitle;
                media.FileName = collectionMedia.FileName;
                Helper.AssignUserInfo(media, User, false);
                db.Entry(media).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = collectionMedia.CollectionID });
            }
            return View(collectionMedia);
        }

        [Authorize(Roles = "Admin,Manager,Super")]
        public ActionResult DeleteFile(Int64? id)
        {
            CollectionMedia collectionMedia = db.CollectionMedias.Find(id);
            Helper.SoftDelete(collectionMedia, User);
            db.Entry(collectionMedia).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = collectionMedia.CollectionID });
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
