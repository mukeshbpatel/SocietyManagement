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
    public class ComplaintCommentController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: ComplaintComment
        public ActionResult Index(int id)
        {
            var complaintComments = db.ComplaintComments.Where(d=>d.IsDeleted==false & d.CommentID == id).Include(c => c.Author).Include(c => c.Complaint).Include(c => c.Status).Include(c => c.AssignTo);
            return View(complaintComments.ToList());
        }

        // GET: ComplaintComment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComplaintComment complaintComment = db.ComplaintComments.Find(id);
            if (complaintComment == null)
            {
                return HttpNotFound();
            }
            return View(complaintComment);
        }

        // GET: ComplaintComment/Create
        public ActionResult Create(int id)
        {
            Complaint complaint = db.Complaints.Find(id);
            if (complaint == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComplaintID = complaint.ComplaintID;
            ViewBag.AssignToID = new SelectList(db.AspNetUsers, "Id", "FirstName");
            ViewBag.StatusID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "ComplaintStatus"), "KeyID", "KeyValues", complaint.StatusID);
            return View();
        }

        // POST: ComplaintComment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id,[Bind(Include = "CommentID,ComplaintID,AssignToID,Comment,StatusID")] ComplaintComment complaintComment)
        {
            Helper.AssignUserInfo(complaintComment, User);
            complaintComment.ComplaintID = id;
            complaintComment.AuthorID = complaintComment.UserID;
            if (ModelState.IsValid)
            {
                db.ComplaintComments.Add(complaintComment);
                db.SaveChanges();

                var complaint = db.Complaints.Find(complaintComment.ComplaintID);
                if (complaint != null)
                {
                    complaint.StatusID = complaintComment.StatusID;
                    db.Entry(complaintComment).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Details","Complaint",new {id = complaintComment.ComplaintID });
            }

            ViewBag.AssignToID = new SelectList(db.AspNetUsers, "Id", "FirstName", complaintComment.AssignToID);
            ViewBag.StatusID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "ComplaintStatus"), "KeyID", "KeyValues", complaintComment.StatusID);
            return View(complaintComment);
        }

        // GET: ComplaintComment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComplaintComment complaintComment = db.ComplaintComments.Find(id);
            if (complaintComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssignToID = new SelectList(db.AspNetUsers, "Id", "FirstName", complaintComment.AssignToID);
            ViewBag.StatusID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "ComplaintStatus"), "KeyID", "KeyValues", complaintComment.StatusID);
            return View(complaintComment);
        }

        // POST: ComplaintComment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentID,ComplaintID,AuthorID,AssignToID,Comment,StatusID,CreatedDate")] ComplaintComment complaintComment)
        {
            Helper.AssignUserInfo(complaintComment, User);
            if (ModelState.IsValid)
            {
                db.Entry(complaintComment).State = EntityState.Modified;
                db.SaveChanges();

                var complaint = db.Complaints.Find(complaintComment.ComplaintID);
                if (complaint != null)
                {
                    complaint.StatusID = complaintComment.StatusID;
                    db.Entry(complaintComment).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Details", "Complaint", new { id = complaintComment.ComplaintID });
            }
            ViewBag.AssignToID = new SelectList(db.AspNetUsers, "Id", "FirstName", complaintComment.AssignToID);
            ViewBag.StatusID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "ComplaintStatus"), "KeyID", "KeyValues", complaintComment.StatusID);
            return View(complaintComment);
        }

        // GET: ComplaintComment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComplaintComment complaintComment = db.ComplaintComments.Find(id);
            if (complaintComment == null)
            {
                return HttpNotFound();
            }
            return View(complaintComment);
        }

        // POST: ComplaintComment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ComplaintComment complaintComment = db.ComplaintComments.Find(id);
            Helper.SoftDelete(complaintComment,User);
            db.Entry(complaintComment).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details","Complaint",new {id = complaintComment.ComplaintID });
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
