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
    [Authorize]
    public class BuildingUnitController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: BuildingUnit
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                var buildingUnits = db.BuildingUnits.Where(d => d.IsDeleted == false).Include(b => b.Owner).Include(b => b.Building).Include(b => b.UnitType).OrderBy(o => o.UnitName);
                return View(buildingUnits.ToList());
            }
            else
            {
                var buildingUnits = db.BuildingUnits.Where(d => d.IsDeleted == false & d.BuildingID == id).Include(b => b.Owner).Include(b => b.Building).Include(b => b.UnitType).OrderBy(o => o.UnitName);
                return View(buildingUnits.ToList());
            }
        }

        // GET: BuildingUnit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuildingUnit buildingUnit = db.BuildingUnits.Find(id);
            if (buildingUnit == null)
            {
                return HttpNotFound();
            }

            var data = db.Database.SqlQuery<SP_BuildingUnit_BalanceSheet_Result>("Exec SP_BuildingUnit_BalanceSheet @UnitID = " + id + ",@YearID = " + SiteSetting.FinancialYearID);
            ViewBag.BalanceSheet = data;
            return View(buildingUnit);
        }

        // GET: BuildingUnit/Create
        [Authorize(Roles ="SuperUser")]
        public ActionResult Create()
        {
            ViewBag.OwnerID = new SelectList(Helper.GetUsers(db.AspNetUsers), "Id", "Name");
            ViewBag.BuildingID = new SelectList(db.Buildings.Where(d => d.IsDeleted == false).OrderBy(o=>o.BuildingName) , "BuildingID", "BuildingName");
            ViewBag.UnitTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "BuildingUnitType"), "KeyID", "KeyValues");
            return View();
        }

        // POST: BuildingUnit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperUser")]
        public ActionResult Create([Bind(Include = "UnitID,BuildingID,UnitName,UnitTypeID,Details,OwnerID,OneTimeMaintenance,UnitArea,Files,UDK1,UDK2,UDK3,UDK4,UDK5")] BuildingUnit buildingUnit)
        {
            Helper.AssignUserInfo(buildingUnit,User);
            if (ModelState.IsValid)
            {
                db.BuildingUnits.Add(buildingUnit);
                db.SaveChanges();

                foreach (HttpPostedFileBase file in buildingUnit.Files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var media = new BuildingUnitMedia();
                        media.UnitID = buildingUnit.UnitID;
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
                        db.BuildingUnitMedias.Add(media);
                        db.SaveChanges();                        
                    }

                }

                return RedirectToAction("Index");
            }

            ViewBag.OwnerID = new SelectList(Helper.GetUsers(db.AspNetUsers), "Id", "Name", buildingUnit.OwnerID);
            ViewBag.BuildingID = new SelectList(db.Buildings.Where(d => d.IsDeleted == false).OrderBy(o=>o.BuildingName), "BuildingID", "BuildingName", buildingUnit.BuildingID);
            ViewBag.UnitTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "BuildingUnitType"), "KeyID", "KeyValues", buildingUnit.UnitTypeID);
            return View(buildingUnit);
        }
        
        public FileResult DownloadFile(int? id)
        {
            if (id != null)
            {
                var media = db.BuildingUnitMedias.Find(id);
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
        [Authorize(Roles = "Admin,Manager,SuperUser")]
        public ActionResult EditFile(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuildingUnitMedia buildingUnitMedia = db.BuildingUnitMedias.Find(id);
            if (buildingUnitMedia == null)
            {
                return HttpNotFound();
            }
            return View(buildingUnitMedia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,SuperUser")]
        public ActionResult EditFile(BuildingUnitMedia buildingUnitMedia)
        {
            Helper.AssignUserInfo(buildingUnitMedia, User, false);
            if (ModelState.IsValid)
            {
                var media = db.BuildingUnitMedias.Find(buildingUnitMedia.MediaID);
                media.MediaTitle = buildingUnitMedia.MediaTitle;
                media.FileName = buildingUnitMedia.FileName;
                Helper.AssignUserInfo(media, User, false);
                db.Entry(media).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = buildingUnitMedia.UnitID });
            }
            return View(buildingUnitMedia);
        }

        [Authorize(Roles = "Admin,Manager,SuperUser")]
        public ActionResult DeleteFile(Int64? id)
        {
            BuildingUnitMedia buildingUnitMedia = db.BuildingUnitMedias.Find(id);
            Helper.SoftDelete(buildingUnitMedia, User);
            db.Entry(buildingUnitMedia).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = buildingUnitMedia.UnitID });
        }

        // GET: BuildingUnit/Edit/5
        [Authorize(Roles = "SuperUser,Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuildingUnit buildingUnit = db.BuildingUnits.Find(id);
            if (buildingUnit == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnerID = new SelectList(Helper.GetUsers(db.AspNetUsers), "Id", "Name", buildingUnit.OwnerID);
            ViewBag.BuildingID = new SelectList(db.Buildings.Where(d => d.IsDeleted == false).OrderBy(o => o.BuildingName), "BuildingID", "BuildingName", buildingUnit.BuildingID);
            ViewBag.UnitTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "BuildingUnitType"), "KeyID", "KeyValues", buildingUnit.UnitTypeID);
            return View(buildingUnit);
        }

        // POST: BuildingUnit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperUser,Admin")]
        public ActionResult Edit([Bind(Include = "UnitID,BuildingID,UnitName,UnitTypeID,Details,OwnerID,OneTimeMaintenance,UnitArea,Files,UDK1,UDK2,UDK3,UDK4,UDK5,CreatedDate")] BuildingUnit buildingUnit)
        {
            Helper.AssignUserInfo(buildingUnit, User,false);
            if (ModelState.IsValid)
            {
                db.Entry(buildingUnit).State = EntityState.Modified;
                db.SaveChanges();

                foreach (HttpPostedFileBase file in buildingUnit.Files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var media = new BuildingUnitMedia();
                        media.UnitID = buildingUnit.UnitID;
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
                        db.BuildingUnitMedias.Add(media);
                        db.SaveChanges();
                    }

                }

                return RedirectToAction("Index");
            }
            ViewBag.OwnerID = new SelectList(Helper.GetUsers(db.AspNetUsers), "Id", "Name", buildingUnit.OwnerID);
            ViewBag.BuildingID = new SelectList(db.Buildings.Where(d => d.IsDeleted == false).OrderBy(o => o.BuildingName), "BuildingID", "BuildingName", buildingUnit.BuildingID);
            ViewBag.UnitTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "BuildingUnitType"), "KeyID", "KeyValues", buildingUnit.UnitTypeID);
            return View(buildingUnit);
        }

        // GET: BuildingUnit/Delete/5
        [Authorize(Roles = "SuperUser")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuildingUnit buildingUnit = db.BuildingUnits.Find(id);
            if (buildingUnit == null)
            {
                return HttpNotFound();
            }
            return View(buildingUnit);
        }

        // POST: BuildingUnit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperUser")]
        public ActionResult DeleteConfirmed(int id)
        {
            BuildingUnit buildingUnit = db.BuildingUnits.Find(id);
            Helper.SoftDelete(buildingUnit, User);
            db.Entry(buildingUnit).State = EntityState.Modified;
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
