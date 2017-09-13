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
    [Authorize(Roles = "Admin")]
    public class BuildingUnitController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: BuildingUnit
        public ActionResult Index()
        {
            var buildingUnits = db.BuildingUnits.Where(d => d.IsDeleted == false).Include(b => b.Owner).Include(b => b.Building).Include(b => b.UnitType).OrderBy(o=>o.UnitName);
            return View(buildingUnits.ToList());
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
            return View(buildingUnit);
        }

        // GET: BuildingUnit/Create
        public ActionResult Create()
        {
            ViewBag.OwnerID = new SelectList(db.AspNetUsers, "Id", "FirstName");
            ViewBag.BuildingID = new SelectList(db.Buildings.Where(d => d.IsDeleted == false).OrderBy(o=>o.BuildingName) , "BuildingID", "BuildingName");
            ViewBag.UnitTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "BuildingUnitType"), "KeyID", "KeyValues");
            return View();
        }

        // POST: BuildingUnit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UnitID,BuildingID,UnitName,UnitTypeID,Details,OwnerID,OneTimeMaintenance,UnitArea,UDK1,UDK2,UDK3,UDK4,UDK5")] BuildingUnit buildingUnit)
        {
            Helper.AssignUserInfo(buildingUnit,User);
            if (ModelState.IsValid)
            {
                db.BuildingUnits.Add(buildingUnit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OwnerID = new SelectList(db.AspNetUsers, "Id", "FirstName", buildingUnit.OwnerID);
            ViewBag.BuildingID = new SelectList(db.Buildings.Where(d => d.IsDeleted == false).OrderBy(o=>o.BuildingName), "BuildingID", "BuildingName", buildingUnit.BuildingID);
            ViewBag.UnitTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "BuildingUnitType"), "KeyID", "KeyValues", buildingUnit.UnitTypeID);
            return View(buildingUnit);
        }

        // GET: BuildingUnit/Edit/5
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
            ViewBag.OwnerID = new SelectList(db.AspNetUsers, "Id", "FirstName", buildingUnit.OwnerID);
            ViewBag.BuildingID = new SelectList(db.Buildings.Where(d => d.IsDeleted == false).OrderBy(o => o.BuildingName), "BuildingID", "BuildingName", buildingUnit.BuildingID);
            ViewBag.UnitTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "BuildingUnitType"), "KeyID", "KeyValues", buildingUnit.UnitTypeID);
            return View(buildingUnit);
        }

        // POST: BuildingUnit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UnitID,BuildingID,UnitName,UnitTypeID,Details,OwnerID,OneTimeMaintenance,UnitArea,UDK1,UDK2,UDK3,UDK4,UDK5,CreatedDate")] BuildingUnit buildingUnit)
        {
            Helper.AssignUserInfo(buildingUnit, User,false);
            if (ModelState.IsValid)
            {
                db.Entry(buildingUnit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerID = new SelectList(db.AspNetUsers, "Id", "FirstName", buildingUnit.OwnerID);
            ViewBag.BuildingID = new SelectList(db.Buildings.Where(d => d.IsDeleted == false).OrderBy(o => o.BuildingName), "BuildingID", "BuildingName", buildingUnit.BuildingID);
            ViewBag.UnitTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "BuildingUnitType"), "KeyID", "KeyValues", buildingUnit.UnitTypeID);
            return View(buildingUnit);
        }

        // GET: BuildingUnit/Delete/5
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
