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
    public class ComplaintController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: Complaint
        public ActionResult Index()
        {
            var complaints = db.Complaints.Where(d=>d.IsDeleted==false).Include(c => c.AssignTo).Include(c => c.Author).Include(c => c.ComplaintType).OrderByDescending(o=>o.ModifiedDate);
            return View(complaints.ToList());
        }

        // GET: Complaint/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Complaint complaint = db.Complaints.Find(id);
            if (complaint == null)
            {
                return HttpNotFound();
            }
            return View(complaint);
        }

        // GET: Complaint/Create
        public ActionResult Create()
        {
            ViewBag.AssignToID = new SelectList(db.AspNetUsers, "Id", "FirstName");
            ViewBag.ComplaintTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "ComplaintType"), "KeyID", "KeyValues");
            return View();
        }

        // POST: Complaint/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ComplaintID,ComplaintDate,ComplaintTypeID,AssignToID,Title,Details,UDK1,UDK2,UDK3,UDK4,UDK5")] Complaint complaint)
        {
            Helper.AssignUserInfo(complaint, User);
            complaint.AuthorID = complaint.UserID;
            complaint.Status = 1;
            if (ModelState.IsValid)
            {
                db.Complaints.Add(complaint);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssignToID = new SelectList(db.AspNetUsers, "Id", "FirstName", complaint.AssignToID);
            ViewBag.ComplaintTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "ComplaintType"), "KeyID", "KeyValues", complaint.ComplaintTypeID);
            return View(complaint);
        }

        // GET: Complaint/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Complaint complaint = db.Complaints.Find(id);
            if (complaint == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssignToID = new SelectList(db.AspNetUsers, "Id", "FirstName", complaint.AssignToID);
            ViewBag.ComplaintTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "ComplaintType"), "KeyID", "KeyValues", complaint.ComplaintTypeID);
            return View(complaint);
        }

        // POST: Complaint/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ComplaintID,AuthorID,ComplaintDate,ComplaintTypeID,AssignToID,Title,Details,Status,UDK1,UDK2,UDK3,UDK4,UDK5,CreatedDate")] Complaint complaint)
        {
            Helper.AssignUserInfo(complaint, User, true);
            if (ModelState.IsValid)
            {
                db.Entry(complaint).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssignToID = new SelectList(db.AspNetUsers, "Id", "FirstName", complaint.AssignToID);
            ViewBag.ComplaintTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "ComplaintType"), "KeyID", "KeyValues", complaint.ComplaintTypeID);
            return View(complaint);
        }

        // GET: Complaint/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Complaint complaint = db.Complaints.Find(id);
            if (complaint == null)
            {
                return HttpNotFound();
            }
            return View(complaint);
        }

        // POST: Complaint/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Complaint complaint = db.Complaints.Find(id);
            Helper.SoftDelete(complaint, User);
            db.Entry(complaint).State = EntityState.Modified;
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
