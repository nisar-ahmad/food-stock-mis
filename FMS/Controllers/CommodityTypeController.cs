using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FMS.Models;

namespace FMS.Controllers
{
    [Authorize]
    public class CommodityTypeController : Controller
    {
        private FMSDbContext db = new FMSDbContext();

        public JsonResult List()
        {
            return Json(db.CommodityTypes.Select(o => new { id = o.CommodityTypeId, kg = o.KgConversionFactor, rate = o.RatePerBag }).ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: CommodityType
        public ActionResult Index()
        {
            return View(db.CommodityTypes.ToList());
        }

        // GET: CommodityType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommodityType commodityType = db.CommodityTypes.Find(id);
            if (commodityType == null)
            {
                return HttpNotFound();
            }
            return View(commodityType);
        }

        // GET: CommodityType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CommodityType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommodityTypeId,Name,Description")] CommodityType commodityType)
        {
            if (ModelState.IsValid)
            {
                db.CommodityTypes.Add(commodityType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(commodityType);
        }

        // GET: CommodityType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommodityType commodityType = db.CommodityTypes.Find(id);
            if (commodityType == null)
            {
                return HttpNotFound();
            }
            return View(commodityType);
        }

        // POST: CommodityType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommodityTypeId,Name,Description")] CommodityType commodityType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commodityType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(commodityType);
        }

        // GET: CommodityType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommodityType commodityType = db.CommodityTypes.Find(id);
            if (commodityType == null)
            {
                return HttpNotFound();
            }
            return View(commodityType);
        }

        // POST: CommodityType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CommodityType commodityType = db.CommodityTypes.Find(id);
            db.CommodityTypes.Remove(commodityType);
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
