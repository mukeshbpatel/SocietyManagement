using SocietyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocietyManagement.Controllers
{
    public class FileDownloadController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        public FileResult Bill(int id = 0)
        {
            string html = System.IO.File.ReadAllText(Server.MapPath("~/Template/Bill.html"));
            var due = db.Dues.Find(id);
            if (due != null)
            {
                html = html.Replace("{{Name}}", Helper.NullToString(due.BuildingUnit.OwnerName));
                html = html.Replace("{{UnitName}}", due.BuildingUnit.UnitName);
                html = html.Replace("{{BillNumber}}", due.DueID.ToString());
                html = html.Replace("{{BillAmount}}", Helper.FormatMoney(due.DueAmount));
                html = html.Replace("{{BillType}}", due.DueType.KeyValues);
                html = html.Replace("{{BillDetail}}", due.Details);
                html = html.Replace("{{BillDate}}", due.BillDate.ToString("dd-MMM-yyyy"));
                html = html.Replace("{{BillMonth}}", due.BillDate.ToString("MMM-yyyy"));
                html = html.Replace("{{DueDate}}", due.DueDate.ToString("dd-MMM-yyyy"));
                html = html.Replace("{{DueDate}}", due.DueDate.ToString("dd-MMM-yyyy"));
                html = html.Replace("{{OpeningBalance}}", Helper.FormatMoney(0d));
                html = html.Replace("{{TotalAmount}}", Helper.FormatMoney(due.DueAmount));
            }
            else
            {
                html = "<b>Invalid Bill</b>";
            }
            return File(PdfGenrator.HTMLtoPDF(html, true, true), "application/pdf", due.DueType.KeyValues + ".pdf");
        }

        public FileResult BalanceSheet(int id = 0)
        {
            string html = @"<div class='maindiv'><div class='title'>BALANCE SHEET</div><table class='BalanceSheet'><tr><th class='left'>Date</th><th class='left'>Transaction Details</th><th class='right'>Credits</th><th class='right'>Debits</th><th class='right'>Balance</th><th></th></tr>";
            decimal Balance = 0;

            var Units = db.BuildingUnits.Find(id);
            if (Units != null)
            {
                html = html.Replace("BALANCE SHEET", "BALANCE SHEET (" + Units.UnitName + ")");
                var data = db.Database.SqlQuery<SP_BuildingUnit_BalanceSheet_Result>("Exec SP_BuildingUnit_BalanceSheet @UnitID = " + id + ",@YearID = " + SiteSetting.FinancialYearID);
                foreach (var item in data)
                {
                    Balance = Balance + item.Credit - item.Debit;
                    html += "<tr><td class='left'>" + item.BDate.ToString("dd/MM/yyyy") + "</td><td class='left'>" + item.Details + "</td><td class='right'>" + (item.Credit > 0 ? item.Credit.ToString("0.00") : string.Empty) + "</td><td class='right'>" + (item.Debit > 0 ? item.Debit.ToString("0.00") : string.Empty) + "</td><td class='right'>" + (Balance >= 0 ? Balance : (Balance * -1)).ToString("0.00") + "</td><td class='left'>" + (Balance == 0 ? string.Empty : (Balance > 0 ? "Cr" : "Dr")) + "</td></tr>";
                }
                html += "<tr><td colspan='6'>&nbsp;</td></tr><tr class='bold'><td colspan='2'>Total</td><td class='right'>" + data.Sum(s => s.Credit) + "</td><td class='right'>" + data.Sum(s => s.Debit) + "</td><td class='right'>" + (Balance >= 0 ? Balance : (Balance * -1)).ToString("0.00") + "</td><td class='left'>" + (Balance == 0 ? string.Empty : (Balance > 0 ? "Cr" : "Dr")) + "</td></tr>";

            }
            else
            {
                html = "<b>Invalid Balance Sheet</b>";
            }
            html += "</table></div>";
            return File(PdfGenrator.HTMLtoPDF(html, false, true), "application/pdf", "BalanceSheet.pdf");
        }

        public FileResult Receipt(int id = 0)
        {
            string html = System.IO.File.ReadAllText(Server.MapPath("~/Template/Receipt.html"));
            var receipt = db.Collections.Find(id);
            if (receipt != null)
            {
                html = html.Replace("{{Name}}", Helper.NullToString(receipt.BuildingUnit.OwnerName));
                html = html.Replace("{{UnitName}}", receipt.BuildingUnit.UnitName);
                html = html.Replace("{{ReceiptNumber}}", receipt.ReceiptNumber.ToString());
                html = html.Replace("{{ReceiptDate}}", receipt.CollectionDate.ToString("dd-MMM-yyyy"));
                if (receipt.FromDate!= null)
                    html = html.Replace("{{FromMonth}}", receipt.FromDate.Value.ToString("MMM-yyyy"));
                else
                    html = html.Replace("{{FromMonth}}", receipt.CollectionDate.ToString("MMM-yyyy"));
                if(receipt.ToDate!=null)
                    html = html.Replace("{{ToMonth}}", receipt.ToDate.Value.ToString("MMM-yyyy"));
                else
                    html = html.Replace("{{ToMonth}}", receipt.CollectionDate.ToString("MMM-yyyy"));
                html = html.Replace("{{PaymentMode}}", receipt.PaymentMode.KeyValues);
                html = html.Replace("{{PaymentAmount}}", Helper.FormatMoney(receipt.Amount));
                if (receipt.Discount>0)
                    html = html.Replace("{{Discount}}", "Advance Payment Discount");
                else
                    html = html.Replace("{{Discount}}", "");

                html = html.Replace("{{DiscountAmount}}", Helper.FormatMoney(receipt.Discount));
                html = html.Replace("{{Other}}", receipt.Other);
                html = html.Replace("{{OtherAmount}}", Helper.FormatMoney(receipt.OtherAmount));
                html = html.Replace("{{FineAmount}}", Helper.FormatMoney(receipt.LateFeeFine));
                html = html.Replace("{{TotalAmount}}", Helper.FormatMoney(receipt.CollectionAmount));
                html = html.Replace("{{AmountInWord}}", Helper.NumbersToWords(receipt.CollectionAmount));

                string details = string.Empty;
                if (receipt.PaymentMode.KeyValues == "Cheque Payment")
                {
                    details = Helper.NullToString(receipt.ChequeNumber) + ", " + Helper.NullToString(receipt.ChequeBank) + ", " + Helper.NullToDate(receipt.ChequeDate);
                }
                else if (receipt.PaymentMode.KeyValues != "Cash Payment")
                {
                    details = Helper.NullToString(receipt.Reference) + ", " + Helper.NullToString(receipt.ChequeBank) + ", " + Helper.NullToDate(receipt.ChequeDate);
                }
                html = html.Replace("{{PaymentDetails}}", details);
            }
            else
            {
                html = "<b>Invalid Receipt</b>";
            }
            return File(PdfGenrator.HTMLtoPDF(html, true, true), "application/pdf", "Receipt.pdf");
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