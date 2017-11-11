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
    [Authorize(Roles = "SuperUser")]
    public class KeyValueController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: KeyValue
        public ActionResult Index()
        {
            return View(db.KeyValues.OrderBy(o=>o.KeyOrder).OrderBy(o=>o.KeyName).ToList());
        }

        // GET: KeyValue/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KeyValue keyValue = db.KeyValues.Find(id);
            if (keyValue == null)
            {
                return HttpNotFound();
            }
            return View(keyValue);
        }

        // GET: KeyValue/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KeyValue/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KeyID,KeyName,KeyValues,KeyOrder,UDK1,UDK2,UDK3,Details")] KeyValue keyValue)
        {
            if (ModelState.IsValid)
            {
                db.KeyValues.Add(keyValue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(keyValue);
        }

        // GET: KeyValue/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KeyValue keyValue = db.KeyValues.Find(id);
            if (keyValue == null)
            {
                return HttpNotFound();
            }
            return View(keyValue);
        }

        // POST: KeyValue/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KeyID,KeyName,KeyValues,KeyOrder,UDK1,UDK2,UDK3,Details")] KeyValue keyValue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(keyValue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(keyValue);
        }

        // GET: KeyValue/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KeyValue keyValue = db.KeyValues.Find(id);
            if (keyValue == null)
            {
                return HttpNotFound();
            }
            return View(keyValue);
        }

        // POST: KeyValue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KeyValue keyValue = db.KeyValues.Find(id);
            db.KeyValues.Remove(keyValue);
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
