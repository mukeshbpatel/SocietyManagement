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
    public class CommitteeMemberController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: CommitteeMember
        public ActionResult Index()
        {
            var committeeMembers = db.CommitteeMembers.Where(d=>d.IsDeleted==false).Include(c => c.BuildingUnit).Include(c => c.Designation).OrderBy(o=>o.Designation.KeyOrder);
            return View(committeeMembers.ToList());
        }

        // GET: CommitteeMember/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommitteeMember committeeMember = db.CommitteeMembers.Find(id);
            if (committeeMember == null)
            {
                return HttpNotFound();
            }
            return View(committeeMember);
        }

        // GET: CommitteeMember/Create
        [Authorize(Roles = "Super,Admin")]
        public ActionResult Create()
        {
            ViewBag.UnitID = new SelectList(db.BuildingUnits, "UnitID", "UnitName");
            ViewBag.DesignationID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "CommitteeDesignation"), "KeyID", "KeyValues");
            ViewBag.Gender = new SelectList(Helper.FilterKeyValues(db.KeyValues, "Gender"), "KeyValues", "KeyValues");
            return View();
        }

        // POST: CommitteeMember/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super,Admin")]
        public ActionResult Create([Bind(Include = "MemberID,UnitID,FirstName,MiddleName,LastName,Gender,DesignationID,EmailID,Mobile,Profession,TenureFrom,TenureTo")] CommitteeMember committeeMember)
        {
            Helper.AssignUserInfo(committeeMember, User);
            if (ModelState.IsValid)
            {
                db.CommitteeMembers.Add(committeeMember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UnitID = new SelectList(db.BuildingUnits, "UnitID", "UnitName", committeeMember.UnitID);
            ViewBag.DesignationID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "CommitteeDesignation"), "KeyID", "KeyValues", committeeMember.DesignationID);
            ViewBag.Gender = new SelectList(Helper.FilterKeyValues(db.KeyValues, "Gender"), "KeyValues", "KeyValues", committeeMember.Gender);
            return View(committeeMember);
        }

        // GET: CommitteeMember/Edit/5
        [Authorize(Roles = "Super,Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommitteeMember committeeMember = db.CommitteeMembers.Find(id);
            if (committeeMember == null)
            {
                return HttpNotFound();
            }
            ViewBag.UnitID = new SelectList(db.BuildingUnits, "UnitID", "UnitName", committeeMember.UnitID);
            ViewBag.DesignationID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "CommitteeDesignation"), "KeyID", "KeyValues", committeeMember.DesignationID);
            ViewBag.Gender = new SelectList(Helper.FilterKeyValues(db.KeyValues, "Gender"), "KeyValues", "KeyValues", committeeMember.Gender);
            return View(committeeMember);
        }

        // POST: CommitteeMember/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super,Admin")]
        public ActionResult Edit([Bind(Include = "MemberID,UnitID,FirstName,MiddleName,LastName,Gender,DesignationID,EmailID,Mobile,Profession,TenureFrom,TenureTo,CreatedDate")] CommitteeMember committeeMember)
        {
            Helper.AssignUserInfo(committeeMember, User, false);
            if (ModelState.IsValid)
            {
                db.Entry(committeeMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UnitID = new SelectList(db.BuildingUnits, "UnitID", "UnitName", committeeMember.UnitID);
            ViewBag.DesignationID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "CommitteeDesignation"), "KeyID", "KeyValues", committeeMember.DesignationID);
            ViewBag.Gender = new SelectList(Helper.FilterKeyValues(db.KeyValues, "Gender"), "KeyValues", "KeyValues", committeeMember.Gender);
            return View(committeeMember);
        }

        // GET: CommitteeMember/Delete/5
        [Authorize(Roles = "Super,Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommitteeMember committeeMember = db.CommitteeMembers.Find(id);
            if (committeeMember == null)
            {
                return HttpNotFound();
            }
            return View(committeeMember);
        }

        // POST: CommitteeMember/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Super,Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CommitteeMember committeeMember = db.CommitteeMembers.Find(id);
            Helper.SoftDelete(committeeMember, User);
            db.Entry(committeeMember).State = EntityState.Modified;
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
