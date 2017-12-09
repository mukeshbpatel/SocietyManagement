using SocietyManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocietyManagement.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();

        public DataTable ToDataTable(IEnumerable<dynamic> items)
        {
            if (items == null) return null;
            var data = items.ToArray();
            if (data.Length == 0) return null;

            var dt = new DataTable();
            foreach (var pair in ((IDictionary<string, object>)data[0]))
            {
                dt.Columns.Add(pair.Key, (pair.Value ?? string.Empty).GetType());
            }
            foreach (var d in data)
            {
                dt.Rows.Add(((IDictionary<string, object>)d).Values.ToArray());
            }
            return dt;
        }

        public ActionResult ExpenseReport()
        {
           DataTable dt = new DataTable();
            using (var cnn = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                cnn.Open();
                using (var cmd = new SqlCommand("EXEC[dbo].[SP_ExpenseReport] @YearID = " + SiteSetting.FinancialYearID, cnn))
                {
                    using (var adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }
            }
            return View(dt);
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