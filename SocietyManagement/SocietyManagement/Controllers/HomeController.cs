using SocietyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocietyManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        public ActionResult Index()
        {
            string UserID = Helper.GetUserID(User);
            if (Helper.IsInRole("User") || Helper.IsInRole("Manager"))
            {
                var appUser = db.AspNetUsers.Find(UserID);
                if (appUser != null)
                {
                    decimal BillAmount = 0;
                    int BillCount = 0;

                    decimal PaymentAmount = 0;
                    int PaymentCount = 0;

                    decimal BalanceAmount = 0;

                    var Notifications = appUser.Notifications.Where(d => d.IsArchive == false && d.IsDeleted == false && d.IsEmailSend == false).ToList();

                    List<Due> DueList = new List<Due>();
                    List<Collection> CollectionList = new List<Collection>();

                    foreach (var unit in appUser.BuildingUnits.Where(d => d.IsDeleted == false))
                    {
                        var dues = unit.Dues.Where(d => d.IsDeleted == false && d.YearID == SiteSetting.FinancialYearID).ToList();

                        DueList.AddRange(dues);

                        var penalties = unit.Penalties.Where(d => d.IsDeleted == false).ToList();
                        var collections = unit.Collections.Where(d => d.IsDeleted == false && d.YearID == SiteSetting.FinancialYearID).ToList();

                        CollectionList.AddRange(collections);

                        BillAmount += dues.Sum(s => s.DueAmount);
                        BillAmount += penalties.Sum(s => s.Amount);
                        BillCount += dues.Count();

                        PaymentAmount += collections.Sum(s => s.Amount);
                        PaymentAmount -= collections.Sum(s => s.Discount);
                        PaymentCount += collections.Count();

                    }
                    ViewBag.Units = appUser.BuildingUnits.Where(d => d.IsDeleted == false).Count();
                    ViewBag.BillAmount = BillAmount;
                    ViewBag.BillCount = BillCount;
                    ViewBag.PaymentAmount = PaymentAmount;
                    ViewBag.PaymentCount = PaymentCount;

                    BalanceAmount = BillAmount - PaymentAmount;
                    if (BalanceAmount > 0)
                    {
                        ViewBag.BalanceAmount = BalanceAmount;
                        ViewBag.BalanceDrCr = "Credit";
                    }
                    else if (BalanceAmount < 0)
                    {
                        ViewBag.BalanceAmount = BalanceAmount * -1;
                        ViewBag.BalanceDrCr = "Debit";
                    }
                    else
                    {
                        ViewBag.BalanceAmount = 0.00;
                        ViewBag.BalanceDrCr = "Nil";
                    }

                    ViewBag.Notification = Notifications.Count();
                    int newNotifications = Notifications.Where(d => d.IsRead == false).Count();
                    if (newNotifications > 0)
                    {
                        ViewBag.NewNotification = "New : " + newNotifications.ToString();
                    }
                    else
                    {
                        ViewBag.NewNotification = "No notification ";
                    }

                    ViewBag.DueList = DueList.OrderByDescending(o=>o.DueDate).Take(6);
                    ViewBag.CollectionList = CollectionList.OrderByDescending(o => o.CollectionDate).Take(6);
                }
            }
            else if (Helper.IsInRole("Manger") || Helper.IsInRole("Super"))
            {
                var data = db.Database.SqlQuery<SP_Graph_DueCollection1_Result>("Exec SP_Graph_DueCollection @YearID = " + SiteSetting.FinancialYearID);
                ViewBag.GraphBillPayment = data;
                
                ViewBag.BillAmount = data.Sum(s=>s.DueAmount);
                ViewBag.BillCount = data.Sum(s => s.Dues);
                ViewBag.PaymentAmount = data.Sum(s => s.CollectionAmount);
                ViewBag.PaymentCount = data.Sum(s => s.Collections);
                ViewBag.ExpenseAmount = data.Sum(p => p.DueAmount);
                ViewBag.ExpenseCount = data.Sum(p=>p.Expenses);                
            }

                return View();
        }        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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