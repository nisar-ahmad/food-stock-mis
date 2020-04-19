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
    public class DamageTypeController : Controller
    {
        private FMSDbContext db = new FMSDbContext();

        // GET: DamageType
        public ActionResult Index()
        {
            return View(db.DamageTypes.ToList());
        }

        // GET: DamageType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DamageType DamageType = db.DamageTypes.Find(id);
            if (DamageType == null)
            {
                return HttpNotFound();
            }
            return View(DamageType);
        }

        // GET: DamageType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DamageType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DamageTypeId,Name,Description")] DamageType DamageType)
        {
            if (ModelState.IsValid)
            {
                db.DamageTypes.Add(DamageType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(DamageType);
        }

        // GET: DamageType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DamageType DamageType = db.DamageTypes.Find(id);
            if (DamageType == null)
            {
                return HttpNotFound();
            }
            return View(DamageType);
        }

        // POST: DamageType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DamageTypeId,Name,Description")] DamageType DamageType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(DamageType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(DamageType);
        }

        // GET: DamageType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DamageType DamageType = db.DamageTypes.Find(id);
            if (DamageType == null)
            {
                return HttpNotFound();
            }
            return View(DamageType);
        }

        // POST: DamageType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DamageType DamageType = db.DamageTypes.Find(id);
            db.DamageTypes.Remove(DamageType);
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
