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
    public class PenaltyController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: Penalty
        public ActionResult Index(Int64? id)
        {
            if (id == null)
            {
                var penalties = db.Penalties.Where(d => d.IsDeleted == false).Include(p => p.BuildingUnit).Include(p => p.Due).OrderBy(o => o.PenaltyDate);
                return View(penalties.ToList());
            } else
            {
                var penalties = db.Penalties.Where(d=>d.IsDeleted==false && d.DueID == id).Include(p => p.BuildingUnit).Include(p => p.Due).OrderBy(o=>o.PenaltyDate);
                ViewBag.DueID = id;
                return View(penalties.ToList());
            }
        }

        // GET: Penalty/Details/5
        public ActionResult Details(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Penalty penalty = db.Penalties.Find(id);
            if (penalty == null)
            {
                return HttpNotFound();
            }
            return View(penalty);
        }

        // GET: Penalty/Create
        [Authorize(Roles = "Super,Admin")]
        public ActionResult Create(Int64 id)
        {
            Due due = db.Dues.Find(id);
            if (due == null)
            {
                return HttpNotFound();
            }
            Penalty penalty = new Penalty();
            penalty.DueID = due.DueID;
            penalty.UnitID = due.UnitID;
            penalty.Details = "Late Payment penalty for " + due.Details;
            penalty.PenaltyDate = DateTime.Today.Date;
            return View(penalty);
        }

        // POST: Penalty/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Super,Admin")]
        public ActionResult Create([Bind(Include = "PenaltyID,PenaltyDate,UnitID,DueID,Amount,Details,UDK1,UDK2,UDK3,UDK4,UDK5")] Penalty penalty)
        {
            Helper.AssignUserInfo(penalty, User);
            if (ModelState.IsValid)
            {
                db.Penalties.Add(penalty);
                db.SaveChanges();
                return RedirectToAction("Details","Due",new {id = penalty.DueID });
            }
            return View(penalty);
        }

        // GET: Penalty/Edit/5
        [Authorize(Roles = "Super,Admin")]
        public ActionResult Edit(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Penalty penalty = db.Penalties.Find(id);
            if (penalty == null)
            {
                return HttpNotFound();
            }
            return View(penalty);
        }

        // POST: Penalty/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super,Admin")]
        public ActionResult Edit([Bind(Include = "PenaltyID,PenaltyDate,UnitID,DueID,Amount,Details,UDK1,UDK2,UDK3,UDK4,UDK5,CreatedDate")] Penalty penalty)
        {
            Helper.AssignUserInfo(penalty, User, false);
            if (ModelState.IsValid)
            {
                db.Entry(penalty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Due", new { id = penalty.DueID });
            }
            return View(penalty);
        }

        // GET: Penalty/Delete/5
        [Authorize(Roles = "Super,Admin")]
        public ActionResult Delete(Int64? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Penalty penalty = db.Penalties.Find(id);
            if (penalty == null)
            {
                return HttpNotFound();
            }
            return View(penalty);
        }

        // POST: Penalty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super,Admin")]
        public ActionResult DeleteConfirmed(Int64? id)
        {
            Penalty penalty = db.Penalties.Find(id);
            Helper.SoftDelete(penalty, User);
            db.Entry(penalty).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", "Due", new { id = penalty.DueID });
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
