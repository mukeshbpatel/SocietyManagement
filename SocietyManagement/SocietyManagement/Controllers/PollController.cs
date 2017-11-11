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
    public class PollController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();


        public ActionResult Vote(int id)
        {
            Poll poll = db.Polls.Find(id);
            if (poll == null)
            {
                return HttpNotFound();
            }

            ViewBag.IsVoted = false;
            var UserID = Helper.GetUserID(User);
            ViewBag.UserID = UserID;
            if (poll.PollOptions.Count > 0)
            {                
                var options = db.PollVotes.Where(d => d.IsDeleted == false && d.PollOption.PollID == id && d.VoteByID == UserID);
                if (options.Count() > 0)
                    ViewBag.IsVoted = true;
            }
            return View(poll);
        }

        [HttpPost]
        public ActionResult Vote(int id, FormCollection collection)
        {
            Poll poll = db.Polls.Find(id);
            if (poll == null)
            {
                return HttpNotFound();
            }

            foreach (var option in poll.PollOptions)
            {
                if (!string.IsNullOrEmpty(collection["chk_" + option.PollOptionID]))
                {
                    string chk = collection["chk_" + option.PollOptionID];
                    if (chk.ToLower() == "on")
                    {
                        PollVote pollVote = new PollVote();
                        pollVote.PollOptionID = option.PollOptionID;
                        Helper.AssignUserInfo(pollVote, User);
                        pollVote.VoteByID = pollVote.UserID;
                        pollVote.VoteDate = pollVote.CreatedDate;
                        db.PollVotes.Add(pollVote);
                        db.SaveChanges();
                    }
                    
                }
            }
            return RedirectToAction("Details", new { id = poll.PollID });            
        }


        // GET: Poll
        public ActionResult Index()
        {
            var polls = db.Polls.Where(p => p.IsDeleted == false).Include(p => p.PollType).OrderByDescending(o=>o.StartDate);
            return View(polls.ToList());
        }

        // GET: Poll/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Poll poll = db.Polls.Find(id);
            if (poll == null)
            {
                return HttpNotFound();
            }

            int TotalVote = db.PollVotes.Where(p => p.IsDeleted == false && p.PollOption.PollID == id).Count();
            ViewBag.TotalVote = TotalVote;
            return View(poll);
        }


        // GET: Poll/Create
        [Authorize(Roles = "SuperUser,Admin,Manager")]
        public ActionResult Create()
        {            
            ViewBag.PollTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "PollType"), "KeyID", "KeyValues");
            return View();
        }

        // POST: Poll/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "SuperUser,Admin,Manager")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "PollID,PollTitle,Details,StartDate,EndDate,PollTypeID,UDK1,UDK2,UDK3,Options")] Poll poll)
        {
            Helper.AssignUserInfo(poll, User);
            if (ModelState.IsValid)
            {
                db.Polls.Add(poll);
                db.SaveChanges();

                string[] lines = poll.Options.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                int i = 1;
                foreach(var item in lines)
                {
                    var pollOption = new PollOption();
                    pollOption.PollID = poll.PollID;
                    pollOption.PollOption1 = item;
                    pollOption.OptionOrder = i;
                    db.PollOptions.Add(pollOption);
                    db.SaveChanges();
                    i = +1;
                }
                return RedirectToAction("Index");
            }

            ViewBag.PollTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "PollType"), "KeyID", "KeyValues", poll.PollTypeID);
            return View(poll);
        }

        // GET: Poll/Edit/5
        [Authorize(Roles = "SuperUser,Admin,Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Poll poll = db.Polls.Find(id);
            if (poll == null)
            {
                return HttpNotFound();
            }
            ViewBag.PollTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "PollType"), "KeyID", "KeyValues", poll.PollTypeID);

            poll.Options = string.Empty;
            foreach (var pollOption in poll.PollOptions.OrderBy(o=>o.OptionOrder))
            {
                poll.Options += pollOption.PollOption1 + Environment.NewLine;                
            }
            return View(poll);
        }

        // POST: Poll/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "SuperUser,Admin,Manager")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "PollID,PollTitle,Details,StartDate,EndDate,PollTypeID,UDK1,UDK2,UDK3,CreatedDate,Options")] Poll poll)
        {
            Helper.AssignUserInfo(poll, User,true);
            if (ModelState.IsValid)
            {
                db.Entry(poll).State = EntityState.Modified;
                db.SaveChanges();

                string[] lines = poll.Options.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                int i = 1;
                foreach (var item in lines)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var pollOption = db.PollOptions.Where(p => p.PollID == poll.PollID & p.PollOption1 == item).FirstOrDefault();
                        if (pollOption == null)
                        {
                            pollOption = new PollOption();
                            pollOption.PollID = poll.PollID;
                            pollOption.PollOption1 = item;
                            pollOption.OptionOrder = i;
                            db.PollOptions.Add(pollOption);
                            db.SaveChanges();
                            i = +1;
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            ViewBag.PollTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "PollType"), "KeyID", "KeyValues", poll.PollTypeID);
            return View(poll);
        }

        // GET: Poll/Delete/5
        [Authorize(Roles = "SuperUser,Admin,Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Poll poll = db.Polls.Find(id);
            if (poll == null)
            {
                return HttpNotFound();
            }
            return View(poll);
        }

        // POST: Poll/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperUser,Admin,Manager")]
        public ActionResult DeleteConfirmed(int id)
        {
            Poll poll = db.Polls.Find(id);
            Helper.SoftDelete(poll, User);
            db.Entry(poll).State = EntityState.Modified;
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
