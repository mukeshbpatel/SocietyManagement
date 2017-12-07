using SocietyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocietyManagement.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private SocietyManagementEntities db = new SocietyManagementEntities();
        // GET: Search
        public ActionResult Index()
        {
            if(Request.QueryString["q"] != null)
            {
                string UnitName = Request.QueryString["q"].ToString();
                BuildingUnit buildingUnit = db.BuildingUnits.Where(d=>d.IsDeleted==false && d.UnitName == UnitName).FirstOrDefault();
                if (buildingUnit != null)
                {
                    return RedirectToAction("Details","BuildingUnit",new {id = buildingUnit.UnitID });
                }
            }
            return HttpNotFound();
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