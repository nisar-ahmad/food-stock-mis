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
    public class FlourMillChokarController : BaseController
    {
        private void PopulateDropDowns(FlourMillChokar flourMillChokar)
        {
            var flourMills = db.FlourMills.Include(o => o.Place);

            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
                flourMills = flourMills.Where(o => o.DistrictOfficeId == districtOfficeId);

            ViewBag.FlourMillId = new SelectList(flourMills.Include(o => o.Place), "FlourMillId", "Place.Name", flourMillChokar.FlourMillId);
            ViewBag.UnitId = new SelectList(db.Units, "UnitId", "Name", flourMillChokar.UnitId);
        }

        // GET: FlourMillChokar
        public ActionResult Index()
        {
            var query = db.FlourMillChokars.Include(f => f.FlourMill).Include(f => f.Unit);

            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
            {
                var flourMillPlaceIds = db.FlourMills.Where(o => o.DistrictOfficeId == districtOfficeId).Select(o => o.PlaceId);
                query = query.Where(o => flourMillPlaceIds.Any(a => a == o.FlourMill.PlaceId));
            }

            return View(query.ToList());
        }

        // GET: FlourMillChokar/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlourMillChokar flourMillChokar = db.FlourMillChokars.Find(id);
            if (flourMillChokar == null)
            {
                return HttpNotFound();
            }
            return View(flourMillChokar);
        }

        // GET: FlourMillChokar/Create
        public ActionResult Create()
        {
            var flourMillChokar = new FlourMillChokar();
            flourMillChokar.ProductionDate = DateTime.Now;

            PopulateDropDowns(flourMillChokar);
            return View(flourMillChokar);
        }

        // POST: FlourMillChokar/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FlourMillChokar flourMillChokar)
        {
            if (ModelState.IsValid)
            {
                db.FlourMillChokars.Add(flourMillChokar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(flourMillChokar);
        }

        // GET: FlourMillChokar/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlourMillChokar flourMillChokar = db.FlourMillChokars.Find(id);
            if (flourMillChokar == null)
            {
                return HttpNotFound();
            }
            PopulateDropDowns(flourMillChokar);
            return View(flourMillChokar);
        }

        // POST: FlourMillChokar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FlourMillChokar flourMillChokar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flourMillChokar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateDropDowns(flourMillChokar);
            return View(flourMillChokar);
        }

        // GET: FlourMillChokar/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlourMillChokar flourMillChokar = db.FlourMillChokars.Find(id);
            if (flourMillChokar == null)
            {
                return HttpNotFound();
            }
            return View(flourMillChokar);
        }

        // POST: FlourMillChokar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FlourMillChokar flourMillChokar = db.FlourMillChokars.Find(id);
            db.FlourMillChokars.Remove(flourMillChokar);
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
