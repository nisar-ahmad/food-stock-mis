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
    public class StorageOfficeController : BaseController
    {
        private void PopulateDropDowns(StorageOffice storageOffice)
        {
            base.PopulateDropDowns(storageOffice.Place);
        }

        // GET: StorageOffice
        public ActionResult Index()
        {
            var storageOffices = db.StorageOffices.Include(s => s.Directorate).Include(s => s.Place);
            return View(storageOffices.ToList());
        }

        // GET: StorageOffice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StorageOffice storageOffice = db.StorageOffices.Include(o => o.Place).FirstOrDefault(o => o.StorageOfficeId == id);
            if (storageOffice == null)
            {
                return HttpNotFound();
            }
            return View(storageOffice);
        }

        // GET: StorageOffice/Create
        public ActionResult Create()
        {
            var storageOffice = new StorageOffice();
            storageOffice.Place = new Place();

            PopulateDropDowns(storageOffice);
            return View(storageOffice);
        }

        // POST: StorageOffice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StorageOffice storageOffice)
        {
            if (ModelState.IsValid)
            {
                AddPlace(storageOffice.Place, PlaceType.StorageOffice);
                db.StorageOffices.Add(storageOffice);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            PopulateDropDowns(storageOffice);
            return View(storageOffice);
        }

        // GET: StorageOffice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StorageOffice storageOffice = db.StorageOffices.Include(o => o.Place).FirstOrDefault(o => o.StorageOfficeId == id);
            if (storageOffice == null)
            {
                return HttpNotFound();
            }

            PopulateDropDowns(storageOffice);
            return View(storageOffice);
        }

        // POST: StorageOffice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StorageOffice storageOffice)
        {
            if (ModelState.IsValid)
            {
                EditPlace(storageOffice.Place);
                db.Entry(storageOffice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateDropDowns(storageOffice);
            return View(storageOffice);
        }

        // GET: StorageOffice/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StorageOffice storageOffice = db.StorageOffices.Include(o => o.Place).FirstOrDefault(o => o.StorageOfficeId == id);
            if (storageOffice == null)
            {
                return HttpNotFound();
            }
            return View(storageOffice);
        }

        // POST: StorageOffice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StorageOffice storageOffice = db.StorageOffices.Find(id);
            db.StorageOffices.Remove(storageOffice);
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
