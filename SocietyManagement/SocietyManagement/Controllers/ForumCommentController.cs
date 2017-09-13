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
    public class ForumCommentController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: ForumComment
        public ActionResult Index(int id)
        {
            var forumComments = db.ForumComments.Where(c=>c.ForumID==id & c.IsDeleted == false).Include(f => f.Author).Include(f => f.Forum).OrderBy(o=>o.CreatedDate);
            return View(forumComments.ToList());
        }

        // GET: ForumComment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumComment forumComment = db.ForumComments.Find(id);
            if (forumComment == null)
            {
                return HttpNotFound();
            }
            return View(forumComment);
        }

        // GET: ForumComment/Create
        public ActionResult Create(int id)
        {
            ViewBag.ForumID = id;
            return View();
        }

        // POST: ForumComment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id,[Bind(Include = "CommentID,ForumID,Comment,UDK1,UDK2,UDK3")] ForumComment forumComment)
        {
            Helper.AssignUserInfo(forumComment, User);
            forumComment.AuthorID = forumComment.UserID;
            forumComment.ForumID = id;
            if (ModelState.IsValid)
            {
                db.ForumComments.Add(forumComment);
                db.SaveChanges();
                return RedirectToAction("Details","Forum",new {id = forumComment.ForumID });
            }
            ViewBag.ForumID = forumComment.ForumID;
            return View(forumComment);
        }

        // GET: ForumComment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumComment forumComment = db.ForumComments.Find(id);
            if (forumComment == null)
            {
                return HttpNotFound();
            }            
            return View(forumComment);
        }

        // POST: ForumComment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentID,ForumID,AuthorID,Comment,UDK1,UDK2,UDK3,CreatedDate")] ForumComment forumComment)
        {
            Helper.AssignUserInfo(forumComment, User, false);
            if (ModelState.IsValid)
            {
                db.Entry(forumComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Forum", new { id = forumComment.ForumID });
            }            
            return View(forumComment);
        }

        // GET: ForumComment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumComment forumComment = db.ForumComments.Find(id);
            if (forumComment == null)
            {
                return HttpNotFound();
            }
            return View(forumComment);
        }

        // POST: ForumComment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ForumComment forumComment = db.ForumComments.Find(id);
            Helper.SoftDelete(forumComment, User);
            db.Entry(forumComment).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", "Forum", new { id = forumComment.ForumID });
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
