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
    public class UserDefineFieldController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: UserDefineField
        public ActionResult Index()
        {
            return View(db.UserDefineFields.ToList());
        }

        // GET: UserDefineField/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDefineField userDefineField = db.UserDefineFields.Find(id);
            if (userDefineField == null)
            {
                return HttpNotFound();
            }
            return View(userDefineField);
        }

        // GET: UserDefineField/Create
        public ActionResult Create()
        {
            ViewBag.FieldType = new SelectList(Helper.FilterKeyValues(db.KeyValues, "FieldType"), "KeyValues", "KeyValues");
            ViewBag.ControlType = new SelectList(Helper.FilterKeyValues(db.KeyValues, "ControlType"), "KeyValues", "KeyValues");
            return View();
        }

        // POST: UserDefineField/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UDFID,TableName,FieldName,IsDispaly,IsActive,FieldLable,ControlType,FieldType,DefaultValue,IsRequired,IsRangeValidator,MinimumValue,MaximumValue,IsRegularExpression,RegularExpression")] UserDefineField userDefineField)
        {
            Helper.AssignUserInfo(userDefineField, User);
            if (ModelState.IsValid)
            {
                db.UserDefineFields.Add(userDefineField);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FieldType = new SelectList(Helper.FilterKeyValues(db.KeyValues, "FieldType"), "KeyValues", "KeyValues",userDefineField.FieldLable);
            ViewBag.ControlType = new SelectList(Helper.FilterKeyValues(db.KeyValues, "ControlType"), "KeyValues", "KeyValues",userDefineField.ControlType);
            return View(userDefineField);
        }

        // GET: UserDefineField/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDefineField userDefineField = db.UserDefineFields.Find(id);
            if (userDefineField == null)
            {
                return HttpNotFound();
            }
            ViewBag.FieldType = new SelectList(Helper.FilterKeyValues(db.KeyValues, "FieldType"), "KeyValues", "KeyValues", userDefineField.FieldLable);
            ViewBag.ControlType = new SelectList(Helper.FilterKeyValues(db.KeyValues, "ControlType"), "KeyValues", "KeyValues", userDefineField.ControlType);
            return View(userDefineField);
        }

        // POST: UserDefineField/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UDFID,TableName,FieldName,IsDispaly,IsActive,FieldLable,ControlType,FieldType,DefaultValue,IsRequired,IsRangeValidator,MinimumValue,MaximumValue,IsRegularExpression,RegularExpression,CreatedDate")] UserDefineField userDefineField)
        {
            Helper.AssignUserInfo(userDefineField, User,true);
            if (ModelState.IsValid)
            {
                db.Entry(userDefineField).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FieldType = new SelectList(Helper.FilterKeyValues(db.KeyValues, "FieldType"), "KeyValues", "KeyValues", userDefineField.FieldLable);
            ViewBag.ControlType = new SelectList(Helper.FilterKeyValues(db.KeyValues, "ControlType"), "KeyValues", "KeyValues", userDefineField.ControlType);
            return View(userDefineField);
        }

        // GET: UserDefineField/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDefineField userDefineField = db.UserDefineFields.Find(id);
            if (userDefineField == null)
            {
                return HttpNotFound();
            }
            return View(userDefineField);
        }

        // POST: UserDefineField/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserDefineField userDefineField = db.UserDefineFields.Find(id);
            db.UserDefineFields.Remove(userDefineField);
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
