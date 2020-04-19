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
    public class DistrictController : Controller
    {
        private FMSDbContext db = new FMSDbContext();

        // GET: District
        public ActionResult Index()
        {
            var districts = db.Districts.Include(d => d.Province);
            return View(districts.ToList());
        }

        // GET: District/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            return View(district);
        }

        // GET: District/Create
        public ActionResult Create()
        {
            ViewBag.ProvinceId = new SelectList(db.Provinces, "ProvinceId", "Name");
            return View();
        }

        // POST: District/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DistrictId,Name,Description,ProvinceId")] District district)
        {
            if (ModelState.IsValid)
            {
                db.Districts.Add(district);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProvinceId = new SelectList(db.Provinces, "ProvinceId", "Name", district.ProvinceId);
            return View(district);
        }

        // GET: District/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProvinceId = new SelectList(db.Provinces, "ProvinceId", "Name", district.ProvinceId);
            return View(district);
        }

        // POST: District/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DistrictId,Name,Description,ProvinceId")] District district)
        {
            if (ModelState.IsValid)
            {
                db.Entry(district).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProvinceId = new SelectList(db.Provinces, "ProvinceId", "Name", district.ProvinceId);
            return View(district);
        }

        // GET: District/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            return View(district);
        }

        // POST: District/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            District district = db.Districts.Find(id);
            db.Districts.Remove(district);
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
