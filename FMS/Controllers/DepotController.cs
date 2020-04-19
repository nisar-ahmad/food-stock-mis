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
    public class DepotController : BaseController
    {
        private void PopulateDropDowns(Depot depot)
        {
            base.PopulateDropDowns(depot.Place);
            ViewBag.BankAccountId = new SelectList(db.BankAccounts.Include(o => o.Place), "BankAccountId", "Place.Name", depot.BankAccountId);
            ViewBag.DistrictOfficeId = new SelectList(db.DistrictOffices.Include(f => f.Place), "DistrictOfficeId", "Place.Name", depot.DistrictOfficeId);
        }

        // GET: Depot
        public ActionResult Index()
        {
            var depots = db.Depots.Include(d => d.BankAccount).Include(d => d.DistrictOffice).Include(d => d.Place);
            return View(depots.ToList());
        }

        // GET: Depot/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depot depot = db.Depots.Include(d => d.BankAccount).Include(d => d.DistrictOffice).Include(d => d.Place).FirstOrDefault(o=> o.DepotId == id);
            if (depot == null)
            {
                return HttpNotFound();
            }
            return View(depot);
        }

        // GET: Depot/Create
        public ActionResult Create()
        {
            var depot = new Depot();
            depot.Place = new Place();

            PopulateDropDowns(depot);
            return View(depot);
        }

        // POST: Depot/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Depot depot)
        {
            if (ModelState.IsValid)
            {
                AddPlace(depot.Place, PlaceType.Depot);
                db.Depots.Add(depot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateDropDowns(depot);
            return View(depot);
        }

        // GET: Depot/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depot depot = db.Depots.Include(d => d.BankAccount).Include(d => d.DistrictOffice).Include(d => d.Place).FirstOrDefault(o => o.DepotId == id);
            if (depot == null)
            {
                return HttpNotFound();
            }
            PopulateDropDowns(depot);
            return View(depot);
        }

        // POST: Depot/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Depot depot)
        {
            if (ModelState.IsValid)
            {
                EditPlace(depot.Place);
                db.Entry(depot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateDropDowns(depot);
            return View(depot);
        }

        // GET: Depot/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depot depot = db.Depots.Include(d => d.BankAccount).Include(d => d.DistrictOffice).Include(d => d.Place).FirstOrDefault(o => o.DepotId == id);
            if (depot == null)
            {
                return HttpNotFound();
            }
            return View(depot);
        }

        // POST: Depot/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Depot depot = db.Depots.Find(id);
            db.Depots.Remove(depot);
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
