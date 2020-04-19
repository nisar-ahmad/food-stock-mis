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
    public class DistrictOfficeController : BaseController
    {
        private void PopulateDropDowns(DistrictOffice districtOffice)
        {
            base.PopulateDropDowns(districtOffice.Place);
            ViewBag.DivisionalOfficeId = new SelectList(db.DivisionalOffices.Include(o => o.Place), "DivisionalOfficeId", "Place.Name", districtOffice.DivisionalOfficeId);
        }

        // GET: DistrictOffice
        public ActionResult Index()
        {
            var districtOffices = db.DistrictOffices.Include(d => d.DivisionalOffice);
            return View(districtOffices.ToList());
        }

        // GET: DistrictOffice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DistrictOffice districtOffice = db.DistrictOffices.Include(o => o.Place).FirstOrDefault(o => o.DistrictOfficeId == id);
            if (districtOffice == null)
            {
                return HttpNotFound();
            }
            return View(districtOffice);
        }

        // GET: DistrictOffice/Create
        public ActionResult Create()
        {
            var districtOffice = new DistrictOffice();
            districtOffice.Place = new Place();

            PopulateDropDowns(districtOffice);
            return View(districtOffice);
        }

        // POST: DistrictOffice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DistrictOffice districtOffice)
        {
            if (ModelState.IsValid)
            {
                AddPlace(districtOffice.Place, PlaceType.DistrictOffice);
                db.DistrictOffices.Add(districtOffice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateDropDowns(districtOffice);
            return View(districtOffice);
        }

        // GET: DistrictOffice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DistrictOffice districtOffice = db.DistrictOffices.Include(o => o.Place).FirstOrDefault(o => o.DistrictOfficeId == id);
            if (districtOffice == null)
            {
                return HttpNotFound();
            }
            PopulateDropDowns(districtOffice);
            return View(districtOffice);
        }

        // POST: DistrictOffice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DistrictOffice districtOffice)
        {
            if (ModelState.IsValid)
            {
                EditPlace(districtOffice.Place);
                db.Entry(districtOffice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateDropDowns(districtOffice);
            return View(districtOffice);
        }

        // GET: DistrictOffice/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DistrictOffice districtOffice = db.DistrictOffices.Include(o => o.Place).FirstOrDefault(o => o.DistrictOfficeId == id);
            if (districtOffice == null)
            {
                return HttpNotFound();
            }
            return View(districtOffice);
        }

        // POST: DistrictOffice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DistrictOffice districtOffice = db.DistrictOffices.Find(id);
            db.DistrictOffices.Remove(districtOffice);
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
