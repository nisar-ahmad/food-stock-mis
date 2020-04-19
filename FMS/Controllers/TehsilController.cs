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
    public class TehsilController : Controller
    {
        private FMSDbContext db = new FMSDbContext();

        // GET: Tehsil
        public ActionResult Index()
        {
            var tehsils = db.Tehsils.Include(t => t.District);
            return View(tehsils.ToList());
        }

        // GET: Tehsil/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tehsil tehsil = db.Tehsils.Find(id);
            if (tehsil == null)
            {
                return HttpNotFound();
            }
            return View(tehsil);
        }

        // GET: Tehsil/Create
        public ActionResult Create()
        {
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name");
            return View();
        }

        // POST: Tehsil/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TehsilId,Name,Description,DistrictId")] Tehsil tehsil)
        {
            if (ModelState.IsValid)
            {
                db.Tehsils.Add(tehsil);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name", tehsil.DistrictId);
            return View(tehsil);
        }

        // GET: Tehsil/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tehsil tehsil = db.Tehsils.Find(id);
            if (tehsil == null)
            {
                return HttpNotFound();
            }
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name", tehsil.DistrictId);
            return View(tehsil);
        }

        // POST: Tehsil/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TehsilId,Name,Description,DistrictId")] Tehsil tehsil)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tehsil).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name", tehsil.DistrictId);
            return View(tehsil);
        }

        // GET: Tehsil/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tehsil tehsil = db.Tehsils.Find(id);
            if (tehsil == null)
            {
                return HttpNotFound();
            }
            return View(tehsil);
        }

        // POST: Tehsil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tehsil tehsil = db.Tehsils.Find(id);
            db.Tehsils.Remove(tehsil);
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
