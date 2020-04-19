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
    public class ContractorVehicleController : Controller
    {
        private FMSDbContext db = new FMSDbContext();

        // GET: ContractorVehicle
        public ActionResult Index()
        {
            var contractorVehicles = db.ContractorVehicles.Include(c => c.Contractor);
            return View(contractorVehicles.ToList());
        }

        // GET: ContractorVehicle/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorVehicle contractorVehicle = db.ContractorVehicles.Find(id);
            if (contractorVehicle == null)
            {
                return HttpNotFound();
            }
            return View(contractorVehicle);
        }

        // GET: ContractorVehicle/Create
        public ActionResult Create()
        {
            ViewBag.ContractorId = new SelectList(db.Contractors, "ContractorId", "Place.Name");
            return View();
        }

        // POST: ContractorVehicle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContractorVehicleId,RegistrationNumber,VehicleType,Description,ContractorId")] ContractorVehicle contractorVehicle)
        {
            if (ModelState.IsValid)
            {
                db.ContractorVehicles.Add(contractorVehicle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContractorId = new SelectList(db.Contractors, "ContractorId", "Place.Name", contractorVehicle.ContractorId);
            return View(contractorVehicle);
        }

        // GET: ContractorVehicle/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorVehicle contractorVehicle = db.ContractorVehicles.Find(id);
            if (contractorVehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContractorId = new SelectList(db.Contractors, "ContractorId", "Place.Name", contractorVehicle.ContractorId);
            return View(contractorVehicle);
        }

        // POST: ContractorVehicle/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContractorVehicleId,RegistrationNumber,VehicleType,Description,ContractorId")] ContractorVehicle contractorVehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contractorVehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContractorId = new SelectList(db.Contractors, "ContractorId", "Place.Name", contractorVehicle.ContractorId);
            return View(contractorVehicle);
        }

        // GET: ContractorVehicle/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorVehicle contractorVehicle = db.ContractorVehicles.Find(id);
            if (contractorVehicle == null)
            {
                return HttpNotFound();
            }
            return View(contractorVehicle);
        }

        // POST: ContractorVehicle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContractorVehicle contractorVehicle = db.ContractorVehicles.Find(id);
            db.ContractorVehicles.Remove(contractorVehicle);
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
