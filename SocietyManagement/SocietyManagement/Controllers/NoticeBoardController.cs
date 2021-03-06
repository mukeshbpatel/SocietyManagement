﻿using System;
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
    public class NoticeBoardController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: NoticeBoard
        public ActionResult Index()
        {
            return View(db.NoticeBoards.Where(n=>n.ExpiryDate>=DateTime.Now & n.IsDeleted == false).OrderByDescending(o=>o.NoticeDate).ToList());
        }

        // GET: NoticeBoard/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoticeBoard noticeBoard = db.NoticeBoards.Find(id);
            if (noticeBoard == null)
            {
                return HttpNotFound();
            }
            return View(noticeBoard);
        }

        [Authorize(Roles = "Super,Admin,Manager")]
        // GET: NoticeBoard/Create
        public ActionResult Create()
        {
            NoticeBoard nb = new NoticeBoard();
            nb.NoticeDate = DateTime.Today;
            nb.ExpiryDate = DateTime.Today.AddMonths(1);
            nb.Notice = "<div style='width:auto; border:1px solid rgb(128,98,16); margin:2px; padding:5px; box-shadow:0 8px 4px 0 rgba(0,0,0,0.2), 0 6px 20px 0 rgba(0,0,0,0.19);'>" + Environment.NewLine +
                        Helper.GetHeader() + Environment.NewLine +
                        "<div style='width:100%; color:rgb(128,98,16); text-align: center;'><h3><strong><em><span id='htmltitle'>TITLE HERE</span></em></strong></h3></div>" + Environment.NewLine +
                        "<div style='width:100%; padding:30px; text-align: right;'><strong>Date : " + nb.NoticeDate.ToString("dd-MMM-yyyy")  + "</strong></div>" + Environment.NewLine +
                        "<div style='width:100%; padding:30px;'><p>Type Notice Here 1<br /></p><p>Type Notice Here 2<br /></p></div>" + Environment.NewLine +
                        "<br /><br />" + Helper.GetSignature() + "<br /><br />" + Environment.NewLine +
                        Helper.GetFooter() + "</div>";
                        
            return View(nb);
        }

        // POST: NoticeBoard/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Super,Admin,Manager")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "NoticeID,NoticeDate,NoticeHeading,Notice,ExpiryDate,IsDeleted,UDK1,UDK2,UDK3")] NoticeBoard noticeBoard)
        {
            Helper.AssignUserInfo(noticeBoard, User);
            if (ModelState.IsValid)
            {
                db.NoticeBoards.Add(noticeBoard);
                db.SaveChanges();

                EmailNotification emailNotification = new EmailNotification();
                emailNotification.SendNoticeBoardEmail(noticeBoard);
                emailNotification = null;

                return RedirectToAction("Index");
            }

            return View(noticeBoard);
        }

        [Authorize(Roles = "Super,Admin,Manager")]
        // GET: NoticeBoard/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoticeBoard noticeBoard = db.NoticeBoards.Find(id);
            if (noticeBoard == null)
            {
                return HttpNotFound();
            }
            return View(noticeBoard);
        }

        // POST: NoticeBoard/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Super,Admin,Manager")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "NoticeID,NoticeDate,NoticeHeading,Notice,ExpiryDate,IsDeleted,UDK1,UDK2,UDK3,CreatedDate")] NoticeBoard noticeBoard)
        {
            Helper.AssignUserInfo(noticeBoard, User, false);
            if (ModelState.IsValid)
            {
                db.Entry(noticeBoard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(noticeBoard);
        }

        // GET: NoticeBoard/Delete/5
        [Authorize(Roles = "Super,Admin,Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoticeBoard noticeBoard = db.NoticeBoards.Find(id);
            if (noticeBoard == null)
            {
                return HttpNotFound();
            }
            return View(noticeBoard);
        }

        // POST: NoticeBoard/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Super,Admin,Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NoticeBoard noticeBoard = db.NoticeBoards.Find(id);
            Helper.SoftDelete(noticeBoard, User);            
            db.Entry(noticeBoard).State = EntityState.Modified;
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
