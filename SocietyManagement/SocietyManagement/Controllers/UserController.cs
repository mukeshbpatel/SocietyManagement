using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SocietyManagement.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace SocietyManagement.Controllers
{
    [Authorize(Roles ="Super,Admin")]
    public class UserController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        // GET: User
        public ActionResult Index()
        {
            return View(db.AspNetUsers.Where(u => u.UserName != "SuperUser").Include(b=>b.BuildingUnits).Include(r=>r.AspNetRoles).OrderBy(o=>o.UserName).ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: User/Create
        public ActionResult Create()
        {            
            ViewBag.Role = new SelectList(db.AspNetRoles.Where(r => r.Name != "Super"), "Name", "Name","User");
            ViewBag.Gender = new SelectList(Helper.FilterKeyValues(db.KeyValues, "Gender"), "KeyValues", "KeyValues");
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email,PhoneNumber = model.Mobile, FirstName = model.FirstName,MiddleName = model.MiddleName, LastName = model.LastName, Gender = model.Gender };
                var result = UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {                    
                   var roles = UserManager.AddToRole(user.Id, model.Role);
                    if (roles.Succeeded && ! string.IsNullOrWhiteSpace(model.Email))
                    {
                        EmailNotification emailNotification = new EmailNotification();
                        emailNotification.SendWelcomeEmail(user, model.Password);
                        emailNotification = null;
                    }
                    return RedirectToAction("Details", new { id = user.Id });
                }
                AddErrors(result);
            }
            if (Helper.IsInRole("Super"))
            {
                ViewBag.Role = new SelectList(db.AspNetRoles, "Name", "Name", model.Role);
            }
            else
            {
                ViewBag.Role = new SelectList(db.AspNetRoles.Where(r => r.Name != "Super"), "Name", "Name", model.Role);
            }
            // If we got this far, something failed, redisplay form
            ViewBag.Gender = new SelectList(Helper.FilterKeyValues(db.KeyValues, "Gender"), "KeyValues", "KeyValues", model.Gender);
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            if (Helper.IsInRole("Super"))
            {
                ViewBag.Role = new SelectList(db.AspNetRoles, "Name", "Name", aspNetUser.AspNetRoles.FirstOrDefault().Name);
            }
            else
            {
                ViewBag.Role = new SelectList(db.AspNetRoles.Where(r => r.Name != "Super"), "Name", "Name", aspNetUser.AspNetRoles.FirstOrDefault().Name);
            }
            ViewBag.Gender = new SelectList(Helper.FilterKeyValues(db.KeyValues, "Gender"), "KeyValues", "KeyValues", aspNetUser.Gender);
            return View(aspNetUser);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AspNetUser NetUser)
        {
            if (ModelState.IsValid)
            {
                AspNetUser aspNetUser = db.AspNetUsers.Find(NetUser.Id);
                if (aspNetUser == null)
                {
                    return HttpNotFound();
                }
                aspNetUser.FirstName = NetUser.FirstName;
                aspNetUser.MiddleName = NetUser.MiddleName;
                aspNetUser.LastName = NetUser.LastName;
                aspNetUser.Gender = NetUser.Gender;
                aspNetUser.PhoneNumber = NetUser.PhoneNumber;
                aspNetUser.Email = NetUser.Email;
                aspNetUser.DOB = NetUser.DOB;
                aspNetUser.Profession = NetUser.Profession;
                aspNetUser.Occupation = NetUser.Occupation;
                db.Entry(aspNetUser).State = EntityState.Modified;
                db.SaveChanges();

                //Change User Role
                if (aspNetUser.AspNetRoles.FirstOrDefault().Name !=  NetUser.Role)
                {
                    UserManager.RemoveFromRole(aspNetUser.Id, aspNetUser.AspNetRoles.FirstOrDefault().Name);
                    UserManager.AddToRole(aspNetUser.Id, NetUser.Role);
                }

                //Reset Password
                if (NetUser.ResetPassword)
                {
                    string token = UserManager.GeneratePasswordResetToken(aspNetUser.Id);
                    var result = UserManager.ResetPassword(aspNetUser.Id,token, "abcd@1234");
                    if (result.Succeeded)
                    {
                        var user = UserManager.FindByName(aspNetUser.UserName);
                        EmailNotification emailNotification = new EmailNotification();
                        emailNotification.SendPasswordChangedEmail(user, "abcd@1234");
                        emailNotification = null;
                    }
                }
                return RedirectToAction("Details",new {id = aspNetUser.Id });
            }            
            if (Helper.IsInRole("Super"))
            {
                ViewBag.Role = new SelectList(db.AspNetRoles, "Name", "Name", NetUser.Role);
            }
            else
            {
                ViewBag.Role = new SelectList(db.AspNetRoles.Where(r => r.Name != "Super"), "Name", "Name", NetUser.Role);
            }
            ViewBag.Gender = new SelectList(Helper.FilterKeyValues(db.KeyValues, "Gender"), "KeyValues", "KeyValues", NetUser.Gender);
            return View(NetUser);
        }

        // GET: User/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUser);
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
