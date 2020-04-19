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
    public class DivisionalOfficeController : BaseController
    {
        // GET: DivisionalOffice
        public ActionResult Index()
        {
            var divisionalOffices = db.DivisionalOffices.Include(d => d.Place);
            return View(divisionalOffices.ToList());
        }

        // GET: DivisionalOffice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DivisionalOffice divisionalOffice = db.DivisionalOffices.Include(d => d.Place).FirstOrDefault(o => o.DivisionalOfficeId == id);
            if (divisionalOffice == null)
            {
                return HttpNotFound();
            }
            return View(divisionalOffice);
        }

        // GET: DivisionalOffice/Create
        public ActionResult Create()
        {
            var divisionalOffice = new DivisionalOffice();
            divisionalOffice.Place = new Place();

            PopulateDropDowns(divisionalOffice.Place);
            return View(divisionalOffice);
        }

        // POST: DivisionalOffice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DivisionalOffice divisionalOffice)
        {
            if (ModelState.IsValid)
            {
                AddPlace(divisionalOffice.Place, PlaceType.DivisionalOffice);
                db.DivisionalOffices.Add(divisionalOffice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateDropDowns(divisionalOffice.Place);
            return View(divisionalOffice);
        }

        // GET: DivisionalOffice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DivisionalOffice divisionalOffice = db.DivisionalOffices.Include(d => d.Place).FirstOrDefault(o => o.DivisionalOfficeId == id);
            if (divisionalOffice == null)
            {
                return HttpNotFound();
            }
            PopulateDropDowns(divisionalOffice.Place);
            return View(divisionalOffice);
        }

        // POST: DivisionalOffice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DivisionalOffice divisionalOffice)
        {
            if (ModelState.IsValid)
            {
                EditPlace(divisionalOffice.Place);
                db.Entry(divisionalOffice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateDropDowns(divisionalOffice.Place);
            return View(divisionalOffice);
        }

        // GET: DivisionalOffice/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DivisionalOffice divisionalOffice = db.DivisionalOffices.Include(d => d.Place).FirstOrDefault(o => o.DivisionalOfficeId == id);
            if (divisionalOffice == null)
            {
                return HttpNotFound();
            }
            return View(divisionalOffice);
        }

        // POST: DivisionalOffice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DivisionalOffice divisionalOffice = db.DivisionalOffices.Find(id);
            db.DivisionalOffices.Remove(divisionalOffice);
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
