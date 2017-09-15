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
    public class BuildingController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: Building
        public ActionResult Index()
        {
            var buildings = db.Buildings.Where(d => d.IsDeleted == false).Include(b => b.BuildingType).OrderBy(o=>o.BuildingName);
            return View(buildings.ToList());
        }

        // GET: Building/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        [Authorize(Roles = "Admin")]
        // GET: Building/Create
        public ActionResult Create()
        {
            ViewBag.BuildingTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues,"BuildingType"), "KeyID", "KeyValues");
            return View();
        }

        // POST: Building/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BuildingID,BuildingName,BuildingTypeID,Details,UDK1,UDK2,UDK3,UDK4,UDK5")] Building building)
        {
            Helper.AssignUserInfo(building, User);
            if (ModelState.IsValid)
            {
                db.Buildings.Add(building);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuildingTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "BuildingType"), "KeyID", "KeyValues", building.BuildingTypeID);            
            return View(building);
        }

        // GET: Building/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            ViewBag.BuildingTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "BuildingType"), "KeyID", "KeyValues", building.BuildingTypeID);
            return View(building);
        }

        // POST: Building/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BuildingID,BuildingName,BuildingTypeID,Details,UDK1,UDK2,UDK3,UDK4,UDK5,CreatedDate")] Building building)
        {
            Helper.AssignUserInfo(building, User, false);
            if (ModelState.IsValid)
            {
                db.Entry(building).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuildingTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "BuildingType"), "KeyID", "KeyValues", building.BuildingTypeID);
            return View(building);
        }

        [Authorize(Roles = "Admin")]
        // GET: Building/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // POST: Building/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Building building = db.Buildings.Find(id);
            Helper.SoftDelete(building, User);
            db.Entry(building).State = EntityState.Modified;
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
