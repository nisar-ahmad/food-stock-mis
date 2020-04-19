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
    public class FlourMillController : BaseController
    {
        private void PopulateDropDowns(FlourMill flourMill)
        {
            base.PopulateDropDowns(flourMill.Place);
            ViewBag.DistrictOfficeId = new SelectList(db.DistrictOffices.Include(f => f.Place), "DistrictOfficeId", "Place.Name", flourMill.DistrictOfficeId);
        }

        // GET: FlourMill
        public ActionResult Index()
        {
            var flourMills = db.FlourMills.Include(f => f.DistrictOffice).Include(f => f.Place);
            return View(flourMills.ToList());
        }

        // GET: FlourMill/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlourMill flourMill = db.FlourMills.Include(f => f.DistrictOffice).Include(f => f.Place).FirstOrDefault(o => o.FlourMillId == id);
            if (flourMill == null)
            {
                return HttpNotFound();
            }
            return View(flourMill);
        }

        // GET: FlourMill/Create
        public ActionResult Create()
        {
            var flourMill = new FlourMill();
            flourMill.Place = new Place();
            PopulateDropDowns(flourMill);

            return View(flourMill);
        }

        // POST: FlourMill/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FlourMill flourMill)
        {
            if (ModelState.IsValid)
            {
                AddPlace(flourMill.Place, PlaceType.FlourMill);
                db.FlourMills.Add(flourMill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            PopulateDropDowns(flourMill);
            return View(flourMill);
        }

        // GET: FlourMill/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlourMill flourMill = db.FlourMills.Include(f => f.DistrictOffice).Include(f => f.Place).FirstOrDefault(o => o.FlourMillId == id);
            if (flourMill == null)
            {
                return HttpNotFound();
            }
            PopulateDropDowns(flourMill);
            return View(flourMill);
        }

        // POST: FlourMill/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FlourMill flourMill)
        {
            if (ModelState.IsValid)
            {
                EditPlace(flourMill.Place);
                db.Entry(flourMill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateDropDowns(flourMill);
            return View(flourMill);
        }

        // GET: FlourMill/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlourMill flourMill = db.FlourMills.Include(f => f.DistrictOffice).Include(f => f.Place).FirstOrDefault(o => o.FlourMillId == id);
            if (flourMill == null)
            {
                return HttpNotFound();
            }
            return View(flourMill);
        }

        // POST: FlourMill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FlourMill flourMill = db.FlourMills.Find(id);
            db.FlourMills.Remove(flourMill);
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
