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
    public class DepotAllocationController : BaseController
    {
        private void PopulateDropDowns(DepotAllocation depotAllocation)
        {
            var depots = db.Depots.AsQueryable();
            var flourMills = db.FlourMills.AsQueryable();

            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
            {
                depots = depots.Where(o => o.DistrictOfficeId == districtOfficeId);
                flourMills = flourMills.Where(o => o.DistrictOfficeId == districtOfficeId);
            }

            ViewBag.DepotId = new SelectList(depots, "DepotId", "Place.Name", depotAllocation.DepotId);
            ViewBag.FlourMillId = new SelectList(flourMills, "FlourMillId", "Place.Name", depotAllocation.FlourMillId);
            ViewBag.CommodityTypeId = new SelectList(db.CommodityTypes.Where(o => !o.Name.ToLower().Contains("wheat")), "CommodityTypeId", "Name", depotAllocation.CommodityTypeId);
            ViewBag.UnitId = new SelectList(db.Units, "UnitId", "Name", depotAllocation.UnitId);
        }

        // GET: DepotAllocation
        public ActionResult Index()
        {
            var depotAllocations = db.DepotAllocations.Include(d => d.Depot).Include(d => d.FlourMill).Include(d => d.Unit);

            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
                depotAllocations = depotAllocations.Where(o => o.Depot.DistrictOfficeId == districtOfficeId);

            return View(depotAllocations.ToList());
        }

        // GET: DepotAllocation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepotAllocation depotAllocation = db.DepotAllocations.Find(id);
            if (depotAllocation == null)
            {
                return HttpNotFound();
            }
            return View(depotAllocation);
        }

        // GET: DepotAllocation/Create
        public ActionResult Create()
        {
            var depotAllocation = new DepotAllocation();
            PopulateDropDowns(depotAllocation);
            return View(depotAllocation);
        }

        // POST: DepotAllocation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepotAllocation depotAllocation)
        {
            if (ModelState.IsValid)
            {
                db.DepotAllocations.Add(depotAllocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateDropDowns(depotAllocation);
            return View(depotAllocation);
        }

        // GET: DepotAllocation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepotAllocation depotAllocation = db.DepotAllocations.Find(id);
            if (depotAllocation == null)
            {
                return HttpNotFound();
            }
            PopulateDropDowns(depotAllocation);
            return View(depotAllocation);
        }

        // POST: DepotAllocation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DepotAllocation depotAllocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(depotAllocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateDropDowns(depotAllocation);
            return View(depotAllocation);
        }

        // GET: DepotAllocation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepotAllocation depotAllocation = db.DepotAllocations.Find(id);
            if (depotAllocation == null)
            {
                return HttpNotFound();
            }
            PopulateDropDowns(depotAllocation);
            return View(depotAllocation);
        }

        // POST: DepotAllocation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DepotAllocation depotAllocation = db.DepotAllocations.Find(id);
            db.DepotAllocations.Remove(depotAllocation);
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
