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
    public class PlaceController : BaseController
    {   
        // GET: Place
        public ActionResult Index()
        {
            var places = db.Places.Include(p => p.District).Include(p => p.InchargePerson).Include(p => p.Province).Include(p => p.Tehsil).Include(p => p.UnionCouncil);
            return View(places.ToList());
        }

        // GET: Place/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = db.Places.Find(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            return View(place);
        }

        // GET: Place/Create
        public ActionResult Create()
        {
            var place = new Place();
            PopulateDropDowns(place);
            return View(place);
        }

        // POST: Place/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Place place)
        {
            if (ModelState.IsValid)
            {
                AddPlace(place, PlaceType.Depot);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            PopulateDropDowns(place);
            return View(place);
        }

        // GET: Place/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = db.Places.Find(id);

            if (place == null)
            {
                return HttpNotFound();
            }

            PopulateDropDowns(place);
            return View(place);
        }

        // POST: Place/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Place place)
        {
            if (ModelState.IsValid)
            {
                EditPlace(place);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateDropDowns(place);
            return View(place);
        }

        // GET: Place/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Place place = db.Places.Find(id);
            if (place == null)
            {
                return HttpNotFound();
            }
            return View(place);
        }

        // POST: Place/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Place place = db.Places.Find(id);
            db.Places.Remove(place);
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
