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
    [Authorize(Roles = "Super,Admin")]
    public class EmailTemplateController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: EmailTemplate
        public ActionResult Index()
        {
            var emailTemplates = db.EmailTemplates.Include(e => e.TemplateType);
            return View(emailTemplates.ToList());
        }

        // GET: EmailTemplate/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailTemplate emailTemplate = db.EmailTemplates.Find(id);
            if (emailTemplate == null)
            {
                return HttpNotFound();
            }
            return View(emailTemplate);
        }

        // GET: EmailTemplate/Create
        public ActionResult Create()
        {
            ViewBag.TemplateTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "EmailTemplateType"), "KeyID", "KeyValues");
            return View();
        }

        // POST: EmailTemplate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize(Roles ="Super")]
        public ActionResult Create([Bind(Include = "TemplateID,TemplateTypeID,TemplateSubject,TemplateBody,FromEmail,FromName,ReplyToEmail,ReplyToName,UDK1,UDK2,UDK3,UDK4,UDK5")] EmailTemplate emailTemplate)
        {
            Helper.AssignUserInfo(emailTemplate, User);
            if (ModelState.IsValid)
            {
                db.EmailTemplates.Add(emailTemplate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TemplateTypeID = new SelectList(Helper.FilterKeyValues(db.KeyValues, "EmailTemplateType"), "KeyID", "KeyValues", emailTemplate.TemplateTypeID);
            return View(emailTemplate);
        }

        // GET: EmailTemplate/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailTemplate emailTemplate = db.EmailTemplates.Find(id);
            if (emailTemplate == null)
            {
                return HttpNotFound();
            }            
            return View(emailTemplate);
        }

        // POST: EmailTemplate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "TemplateID,TemplateTypeID,TemplateSubject,TemplateBody,FromEmail,FromName,ReplyToEmail,ReplyToName,UDK1,UDK2,UDK3,UDK4,UDK5,CreatedDate")] EmailTemplate emailTemplate)
        {
            Helper.AssignUserInfo(emailTemplate, User, false);
            if (ModelState.IsValid)
            {
                db.Entry(emailTemplate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }            
            return View(emailTemplate);
        }

        // GET: EmailTemplate/Delete/5
        [Authorize(Roles ="Super")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailTemplate emailTemplate = db.EmailTemplates.Find(id);
            if (emailTemplate == null)
            {
                return HttpNotFound();
            }
            return View(emailTemplate);
        }

        // POST: EmailTemplate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super")]
        public ActionResult DeleteConfirmed(int id)
        {
            EmailTemplate emailTemplate = db.EmailTemplates.Find(id);
            db.EmailTemplates.Remove(emailTemplate);
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
