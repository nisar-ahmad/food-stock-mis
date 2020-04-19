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
    public class ContractorController : BaseController
    {
        private void PopulateDropDowns(Contractor contractor)
        {
            base.PopulateDropDowns(contractor.Place);
            ViewBag.ContractorTypeId = new SelectList(db.ContractorTypes, "ContractorTypeId", "Name");
        }

        // GET: Contractor
        public ActionResult Index()
        {
            var contractors = db.Contractors.Include(c => c.ContractorType).Include(c => c.Place);
            return View(contractors.ToList());
        }

        // GET: Contractor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contractor contractor = db.Contractors.Include(o => o.Place).FirstOrDefault(o => o.ContractorId == id);
            if (contractor == null)
            {
                return HttpNotFound();
            }
            return View(contractor);
        }

        // GET: Contractor/Create
        public ActionResult Create()
        {
            var contractor = new Contractor();
            contractor.Place = new Place();

            PopulateDropDowns(contractor);
            return View(contractor);
        }

        // POST: Contractor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contractor contractor)
        {
            if (ModelState.IsValid)
            {
                AddPlace(contractor.Place, PlaceType.Contractor);
                db.Contractors.Add(contractor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateDropDowns(contractor);
            return View(contractor);
        }

        // GET: Contractor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contractor contractor = db.Contractors.Include(o => o.Place).FirstOrDefault(o => o.ContractorId == id);
            if (contractor == null)
            {
                return HttpNotFound();
            }
            PopulateDropDowns(contractor);
            return View(contractor);
        }

        // POST: Contractor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contractor contractor)
        {
            if (ModelState.IsValid)
            {
                EditPlace(contractor.Place);
                db.Entry(contractor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateDropDowns(contractor);
            return View(contractor);
        }

        // GET: Contractor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contractor contractor = db.Contractors.Include(o => o.Place).FirstOrDefault(o => o.ContractorId == id);
            if (contractor == null)
            {
                return HttpNotFound();
            }
            return View(contractor);
        }

        // POST: Contractor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contractor contractor = db.Contractors.Find(id);
            db.Contractors.Remove(contractor);
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
