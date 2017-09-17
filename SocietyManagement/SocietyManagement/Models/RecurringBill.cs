using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace SocietyManagement.Models
{
    public class RecurringBill 
    {
        

        public bool CalculateRecurringBill(IPrincipal User)
        {


            SocietyManagementEntities db = new SocietyManagementEntities();
            SocietyManagementEntities db1 = new SocietyManagementEntities();

            for (int i = 5; i <= 12; i++)
            {

                DateTime RunDate = new DateTime(2017, i, 1);
                int RunDay = RunDate.Day;
                int RunMonth = (RunDate.Year * 100) + RunDate.Month;
                int RunYear = RunDate.Year;
                var recurringDues = db.RecurringDues.Where(d => d.IsDeleted == false && d.StartDate <= RunDate && d.EndDate >= RunDate);
                var recurringDuesMonth = recurringDues.Where(m => m.RecurringType.KeyValues == "Monthly" & ((m.LastRunDate == null || (m.LastRunDate.Value.Year * 100) + m.LastRunDate.Value.Month < RunMonth)));
                var recurringDuesQuater = recurringDues.Where(m => m.RecurringType.KeyValues == "Quarterly" & ((m.LastRunDate == null || (m.LastRunDate.Value.Year * 100) + m.LastRunDate.Value.Month + 3 <= RunMonth)));
                var recurringDuesHalfYear = recurringDues.Where(m => m.RecurringType.KeyValues == "Half-Yearly" & ((m.LastRunDate == null || (m.LastRunDate.Value.Year * 100) + m.LastRunDate.Value.Month + 6 <= RunMonth)));
                var recurringDuesYear = recurringDues.Where(m => m.RecurringType.KeyValues == "Yearly" & ((m.LastRunDate == null || m.LastRunDate.Value.Year < RunYear)));

                foreach (var recurring in recurringDuesMonth)
                {
                    var bill = new Due();
                    bill.BillDate = RunDate;
                    bill.Details = recurring.RecurringType.KeyValues + " " + recurring.DueType.KeyValues + " (" + RunDate.ToString("MMM-yyyy") + ")";
                    bill.DueAmount = recurring.Amount;
                    bill.DueDate = RunDate.AddDays(recurring.DueDays - 1);
                    bill.DueTypeID = recurring.DueTypeID;
                    bill.RecurringID = recurring.RecurringID;
                    bill.UDK1 = recurring.UDK1;
                    bill.UDK2 = recurring.UDK2;
                    bill.UDK3 = recurring.UDK3;
                    bill.UDK4 = recurring.UDK4;
                    bill.UDK5 = recurring.UDK5;
                    bill.UnitID = recurring.UnitID;
                    Helper.AssignUserInfo(bill, User);
                    db1.Dues.Add(bill);
                    db1.SaveChanges();

                    RecurringDue recurringDue = db1.RecurringDues.Find(bill.RecurringID);
                    recurringDue.LastRunDate = RunDate;
                    db1.Entry(recurringDue).State = EntityState.Modified;
                    db1.SaveChanges();

                }

                foreach (var recurring in recurringDuesQuater)
                {
                    var bill = new Due();
                    bill.BillDate = RunDate;
                    bill.Details = recurring.RecurringType.KeyValues + " " + recurring.DueType.KeyValues + " (" + RunDate.ToString("MMM-yyyy") + ")";
                    bill.DueAmount = recurring.Amount;
                    bill.DueDate = RunDate.AddDays(recurring.DueDays - 1);
                    bill.DueTypeID = recurring.DueTypeID;
                    bill.RecurringID = recurring.RecurringID;
                    bill.UDK1 = recurring.UDK1;
                    bill.UDK2 = recurring.UDK2;
                    bill.UDK3 = recurring.UDK3;
                    bill.UDK4 = recurring.UDK4;
                    bill.UDK5 = recurring.UDK5;
                    bill.UnitID = recurring.UnitID;
                    Helper.AssignUserInfo(bill, User);
                    db1.Dues.Add(bill);
                    db1.SaveChanges();

                    RecurringDue recurringDue = db1.RecurringDues.Find(bill.RecurringID);
                    recurringDue.LastRunDate = RunDate;
                    db1.Entry(recurringDue).State = EntityState.Modified;
                    db1.SaveChanges();

                }

                foreach (var recurring in recurringDuesHalfYear)
                {
                    var bill = new Due();
                    bill.BillDate = RunDate;
                    bill.Details = recurring.RecurringType.KeyValues + " " + recurring.DueType.KeyValues + " (" + RunDate.ToString("MMM-yyyy") + ")";
                    bill.DueAmount = recurring.Amount;
                    bill.DueDate = RunDate.AddDays(recurring.DueDays - 1);
                    bill.DueTypeID = recurring.DueTypeID;
                    bill.RecurringID = recurring.RecurringID;
                    bill.UDK1 = recurring.UDK1;
                    bill.UDK2 = recurring.UDK2;
                    bill.UDK3 = recurring.UDK3;
                    bill.UDK4 = recurring.UDK4;
                    bill.UDK5 = recurring.UDK5;
                    bill.UnitID = recurring.UnitID;
                    Helper.AssignUserInfo(bill, User);
                    db1.Dues.Add(bill);
                    db1.SaveChanges();

                    RecurringDue recurringDue = db1.RecurringDues.Find(bill.RecurringID);
                    recurringDue.LastRunDate = RunDate;
                    db1.Entry(recurringDue).State = EntityState.Modified;
                    db1.SaveChanges();

                }

                foreach (var recurring in recurringDuesYear)
                {
                    var bill = new Due();
                    bill.BillDate = RunDate;
                    bill.Details = recurring.RecurringType.KeyValues + " " + recurring.DueType.KeyValues + " (" + RunDate.ToString("yyyy") + ")";
                    bill.DueAmount = recurring.Amount;
                    bill.DueDate = RunDate.AddDays(recurring.DueDays - 1);
                    bill.DueTypeID = recurring.DueTypeID;
                    bill.RecurringID = recurring.RecurringID;
                    bill.UDK1 = recurring.UDK1;
                    bill.UDK2 = recurring.UDK2;
                    bill.UDK3 = recurring.UDK3;
                    bill.UDK4 = recurring.UDK4;
                    bill.UDK5 = recurring.UDK5;
                    bill.UnitID = recurring.UnitID;
                    Helper.AssignUserInfo(bill, User);
                    db1.Dues.Add(bill);
                    db1.SaveChanges();

                    RecurringDue recurringDue = db1.RecurringDues.Find(bill.RecurringID);
                    recurringDue.LastRunDate = RunDate;
                    db1.Entry(recurringDue).State = EntityState.Modified;
                    db1.SaveChanges();

                }
            }
            db.Dispose();
            db1.Dispose();
            return true;
        }
        
    }
}