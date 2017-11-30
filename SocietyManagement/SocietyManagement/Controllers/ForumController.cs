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
    public class ForumController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: Forum
        public ActionResult Index()
        {
            var forums = db.Forums.Where(f=>f.IsDeleted==false).Include(f => f.Author).OrderByDescending(o=>o.CreatedDate);
            return View(forums.ToList());
        }

        // GET: Forum/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forum forum = db.Forums.Find(id);
            if (forum == null)
            {
                return HttpNotFound();
            }
            return View(forum);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Details([Bind(Include = "CommentID,ForumID,Comment,UDK1,UDK2,UDK3")] ForumComment forumComment)
        {
            Helper.AssignUserInfo(forumComment, User);
            forumComment.AuthorID = forumComment.UserID;            
            if (ModelState.IsValid)
            {
                db.ForumComments.Add(forumComment);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = forumComment.ForumID });
            }            
            return View(forumComment);
        }

        // GET: Forum/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Forum/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ForumID,ForumTitle,Details,UDK1,UDK2,UDK3")] Forum forum)
        {
            Helper.AssignUserInfo(forum, User);
            forum.AuthorID = forum.UserID;
            if (ModelState.IsValid)
            {
                db.Forums.Add(forum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(forum);
        }

        // GET: Forum/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forum forum = db.Forums.Find(id);
            if (forum == null)
            {
                return HttpNotFound();
            }                           
            return View(forum);
        }

        // POST: Forum/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ForumID,AuthorID,ForumTitle,Details,UDK1,UDK2,UDK3,CreatedDate")] Forum forum)
        {
            Helper.AssignUserInfo(forum, User, false);
            if (ModelState.IsValid)
            {
                db.Entry(forum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }            
            return View(forum);
        }

        // GET: Forum/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forum forum = db.Forums.Find(id);
            if (forum == null)
            {
                return HttpNotFound();
            }
            return View(forum);
        }

        // POST: Forum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Forum forum = db.Forums.Find(id);
            Helper.SoftDelete(forum, User);
            db.Entry(forum).State = EntityState.Modified;
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
