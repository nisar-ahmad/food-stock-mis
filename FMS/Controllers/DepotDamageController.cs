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
    public class DepotDamageController : BaseController
    {
        private void PopulateDropDowns(DepotDamage depotDamage)
        {
            var depots = db.Depots.AsQueryable();

            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
                depots = depots.Where(o => o.DistrictOfficeId == districtOfficeId);

            ViewBag.CommodityTypeId = new SelectList(db.CommodityTypes, "CommodityTypeId", "Name", depotDamage.CommodityTypeId);
            ViewBag.DamageTypeId = new SelectList(db.DamageTypes, "DamageTypeId", "Name", depotDamage.DamageTypeId);
            ViewBag.DepotId = new SelectList(depots.Include(o => o.Place), "DepotId", "Place.Name", depotDamage.DepotId);
            ViewBag.UnitId = new SelectList(db.Units, "UnitId", "Name", depotDamage.UnitId);
        }

        // GET: DepotDamage
        public ActionResult Index()
        {
            var depotDamages = db.DepotDamages.Include(d => d.CommodityType).Include(d => d.Depot).Include(d => d.Unit);

            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
                depotDamages = depotDamages.Where(o => o.Depot.DistrictOfficeId == districtOfficeId);

            return View(depotDamages.ToList());
        }

        // GET: DepotDamage/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepotDamage depotDamage = db.DepotDamages.Find(id);
            if (depotDamage == null)
            {
                return HttpNotFound();
            }
            return View(depotDamage);
        }

        // GET: DepotDamage/Create
        public ActionResult Create()
        {
            var depotDamage = new DepotDamage();
            PopulateDropDowns(depotDamage);
            return View(depotDamage);
        }

        // POST: DepotDamage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepotDamage depotDamage)
        {
            if (ModelState.IsValid)
            {
                db.DepotDamages.Add(depotDamage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateDropDowns(depotDamage);
            return View(depotDamage);
        }

        // GET: DepotDamage/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepotDamage depotDamage = db.DepotDamages.Find(id);
            if (depotDamage == null)
            {
                return HttpNotFound();
            }
            PopulateDropDowns(depotDamage);
            return View(depotDamage);
        }

        // POST: DepotDamage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DepotDamage depotDamage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(depotDamage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateDropDowns(depotDamage);
            return View(depotDamage);
        }

        // GET: DepotDamage/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepotDamage depotDamage = db.DepotDamages.Find(id);
            if (depotDamage == null)
            {
                return HttpNotFound();
            }
            PopulateDropDowns(depotDamage);
            return View(depotDamage);
        }

        // POST: DepotDamage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DepotDamage depotDamage = db.DepotDamages.Find(id);
            db.DepotDamages.Remove(depotDamage);
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
