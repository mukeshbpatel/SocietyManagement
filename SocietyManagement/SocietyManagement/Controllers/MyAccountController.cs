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

        public ActionResult MyBill(int id = 0)
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

        public ActionResult BalanceSheet(int id = 0)
        {
            ViewBag.ID = id;
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
                    ViewBag.ID = id;
                    return View(data);
                }
            }
            return View();
        }

        public FileResult BalanceSheetDownload(int id = 0, String Type = "PDF")
        {

           
                string html = @"<div class='title'>Balance Sheet</div><table class='BalanceSheet'><tr><th class='left'>Date</th><th class='left'>Transaction Details</th><th class='right'>Credits</th><th class='right'>Debits</th><th class='right'>Balance</th><th></th></tr>";
                decimal Balance = 0;

                var Units = db.BuildingUnits.Find(id);
                if (Units != null)
                {
                    html = html.Replace("Balance Sheet", "Balance Sheet (" + Units.UnitName + ")");
                    var data = db.Database.SqlQuery<SP_BuildingUnit_BalanceSheet_Result>("Exec SP_BuildingUnit_BalanceSheet @UnitID = " + id + ",@YearID = " + SiteSetting.FinancialYearID);
                    foreach (var item in data)
                    {
                        Balance = Balance + item.Credit - item.Debit;
                        html += "<tr><td class='left'>" + item.BDate.ToString("dd/MM/yyyy") + "</td><td class='left'>" + item.Details + "</td><td class='right'>" + item.Credit.ToString("0.00") + "</td><td class='right'>" + item.Debit.ToString("0.00") + "</td><td class='right'>" + (Balance >= 0 ? Balance : (Balance * -1)).ToString("0.00") + "</td><td class='left'>" + (Balance == 0 ? string.Empty : (Balance > 0 ? "Cr" : "Dr")) + "</td></tr>";
                    }
                    html += "<tr><td colspan='6'>&nbsp;</td></tr><tr class='bold'><td colspan='2'>Total</td><td class='right'>" + data.Sum(s => s.Credit) + "</td><td class='right'>" + data.Sum(s => s.Debit) + "</td><td class='right'>" + (Balance >= 0 ? Balance : (Balance * -1)).ToString("0.00") + "</td><td class='left'>" + (Balance == 0 ? string.Empty : (Balance > 0 ? "Cr" : "Dr")) + "</td></tr>";

                }
                html += "</table>";
                if (Type == "Excel")
                    return File(PdfGenrator.HTMLtoExcel(html, false), "application/vnd.ms-excel", "BalanceSheet.xls");
                else
                    return File(PdfGenrator.HTMLtoPDF(html, false), "application/pdf", "BalanceSheet.pdf");            
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
