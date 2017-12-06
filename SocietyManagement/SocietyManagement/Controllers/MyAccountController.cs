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
    public class MyAccountController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();
        
        public ActionResult MyBill(int id=0)
        {
            string UserID = Helper.GetUserID(User);
            var appUser = db.AspNetUsers.Find(UserID);
            if (appUser != null)
            {
                var Units = appUser.BuildingUnits.Where(b => b.IsDeleted == false).OrderBy(o => o.UnitName).ToList();
                if (Units.Count > 1)
                {
                    Units.Add(new BuildingUnit { UnitID = 0, UnitName = "All" });
                    ViewBag.UnitID = new SelectList(Units, "UnitID", "UnitName", id);
                }
                else if (Units.Count == 1)
                {
                    id = Units.FirstOrDefault().UnitID;
                    ViewBag.UnitID = new SelectList(Units, "UnitID", "UnitName", id);
                }                

                if (id == 0)
                {
                    var dues = db.Dues.Where(d => d.IsDeleted == false && d.YearID == SiteSetting.FinancialYearID).Include(d => d.BuildingUnit).Where(b => b.BuildingUnit.OwnerID == UserID).Include(d => d.DueType).OrderBy(o => o.BillDate);
                    return View(dues.ToList());
                }
                else
                {
                    var dues = db.Dues.Where(d => d.IsDeleted == false && d.YearID == SiteSetting.FinancialYearID && d.UnitID == id).Include(d => d.BuildingUnit).Where(b => b.BuildingUnit.OwnerID == UserID).Include(d => d.DueType).OrderBy(o => o.BillDate);
                    return View(dues.ToList());
                }
            }
            else
            {
                return View();
            }
        }

        public ActionResult BalanceSheet(int id=0)
        {
            string UserID = Helper.GetUserID(User);
            var appUser = db.AspNetUsers.Find(UserID);
            if (appUser != null)
            {
                var Units = appUser.BuildingUnits.Where(b => b.IsDeleted == false).OrderBy(o => o.UnitName);
                if (Units.Count() > 0)
                {
                    if (id == 0)
                    {
                        id = Units.FirstOrDefault().UnitID;
                    }

                    ViewBag.UnitID = new SelectList(Units, "UnitID", "UnitName", id);
                    var data = db.Database.SqlQuery<SP_BuildingUnit_BalanceSheet_Result>("Exec SP_BuildingUnit_BalanceSheet @UnitID = " + id + ",@YearID = " + SiteSetting.FinancialYearID);
                    return View(data);
                }
            }
            return View();
        }

        public ActionResult MyPayment(int id = 0)
        {
            string UserID = Helper.GetUserID(User);
            var appUser = db.AspNetUsers.Find(UserID);
            if (appUser != null)
            {
                var Units = appUser.BuildingUnits.Where(b => b.IsDeleted == false).OrderBy(o => o.UnitName).ToList();
                if (Units.Count > 1)
                {
                    Units.Add(new BuildingUnit { UnitID = 0, UnitName = "All" });
                    ViewBag.UnitID = new SelectList(Units, "UnitID", "UnitName", id);
                }
                else if (Units.Count == 1)
                {
                    id = Units.FirstOrDefault().UnitID;
                    ViewBag.UnitID = new SelectList(Units, "UnitID", "UnitName", id);
                }



                if (id == 0)
                {
                    var collections = db.Collections.Where(d => d.IsDeleted == false && d.YearID == SiteSetting.FinancialYearID).Include(c => c.BuildingUnit).Where(b => b.BuildingUnit.OwnerID == UserID).Include(c => c.PaymentMode).OrderBy(o => o.CollectionDate);
                    return View(collections.ToList());
                }
                else
                {
                    var collections = db.Collections.Where(d => d.IsDeleted == false && d.YearID == SiteSetting.FinancialYearID && d.UnitID == id).Include(c => c.BuildingUnit).Where(b => b.BuildingUnit.OwnerID == UserID).Include(c => c.PaymentMode).OrderBy(o => o.CollectionDate);
                    return View(collections.ToList());
                }
            }
            else
            {
                return View();
            }
        }
    }
}
