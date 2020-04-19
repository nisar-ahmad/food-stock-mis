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
    public class DirectorateController : BaseController
    {
        private void PopulateDropDowns(Directorate directorate)
        {
            base.PopulateDropDowns(directorate.Place);
            ViewBag.BankAccountId = new SelectList(db.BankAccounts.Include(o => o.Place), "BankAccountId", "Place.Name", directorate.BankAccountId);
        }

        // GET: Directorate
        public ActionResult Index()
        {
            var directorates = db.Directorates.Include(d => d.BankAccount).Include(d => d.Place);
            return View(directorates.ToList());
        }

        // GET: Directorate/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Directorate directorate = db.Directorates.Include(o => o.Place).FirstOrDefault(o => o.DirectorateId == id);
            if (directorate == null)
            {
                return HttpNotFound();
            }
            return View(directorate);
        }

        // GET: Directorate/Create
        public ActionResult Create()
        {
            var directorate = new Directorate();
            directorate.Place = new Place();

            PopulateDropDowns(directorate);
            return View(directorate);
        }

        // POST: Directorate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Directorate directorate)
        {
            if (ModelState.IsValid)
            {
                AddPlace(directorate.Place, PlaceType.Directorate);
                db.Directorates.Add(directorate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            PopulateDropDowns(directorate);
            return View(directorate);
        }

        // GET: Directorate/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Directorate directorate = db.Directorates.Include(o => o.Place).FirstOrDefault(o => o.DirectorateId == id);
            if (directorate == null)
            {
                return HttpNotFound();
            }

            PopulateDropDowns(directorate);
            return View(directorate);
        }

        // POST: Directorate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Directorate directorate)
        {
            if (ModelState.IsValid)
            {
                EditPlace(directorate.Place);
                db.Entry(directorate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            PopulateDropDowns(directorate);
            return View(directorate);
        }

        // GET: Directorate/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Directorate directorate = db.Directorates.Include(o => o.Place).FirstOrDefault(o => o.DirectorateId == id);
            if (directorate == null)
            {
                return HttpNotFound();
            }
            return View(directorate);
        }

        // POST: Directorate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Directorate directorate = db.Directorates.Find(id);
            db.Directorates.Remove(directorate);
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
