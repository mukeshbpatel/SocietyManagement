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
    public class PollOptionController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

      
        // GET: PollOption
        public ActionResult Index(int? PollID)
        {
            List<PollOption> pollOptions;
            if (PollID != null)
                pollOptions =db.PollOptions.Where(p=>p.PollID == PollID).Include(p => p.Poll).ToList();
            else
                pollOptions = db.PollOptions.Include(p => p.Poll).ToList();

            return View(pollOptions);
        }

        // GET: PollOption/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PollOption pollOption = db.PollOptions.Find(id);
            if (pollOption == null)
            {
                return HttpNotFound();
            }
            return View(pollOption);
        }

        // GET: PollOption/Create
        public ActionResult Create()
        {
            ViewBag.PollID = new SelectList(db.Polls, "PollID", "PollTitle");
            return View();
        }

        // POST: PollOption/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PollOptionID,PollID,PollOption1,OptionOrder,UDK1,UDK2,UDK3")] PollOption pollOption)
        {
            if (ModelState.IsValid)
            {
                db.PollOptions.Add(pollOption);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PollID = new SelectList(db.Polls, "PollID", "PollTitle", pollOption.PollID);
            return View(pollOption);
        }

        // GET: PollOption/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PollOption pollOption = db.PollOptions.Find(id);
            if (pollOption == null)
            {
                return HttpNotFound();
            }
            ViewBag.PollID = new SelectList(db.Polls, "PollID", "PollTitle", pollOption.PollID);
            return View(pollOption);
        }

        // POST: PollOption/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PollOptionID,PollID,PollOption1,OptionOrder,UDK1,UDK2,UDK3")] PollOption pollOption)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pollOption).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PollID = new SelectList(db.Polls, "PollID", "PollTitle", pollOption.PollID);
            return View(pollOption);
        }

        // GET: PollOption/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PollOption pollOption = db.PollOptions.Find(id);
            if (pollOption == null)
            {
                return HttpNotFound();
            }
            return View(pollOption);
        }

        // POST: PollOption/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PollOption pollOption = db.PollOptions.Find(id);
            db.PollOptions.Remove(pollOption);
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
