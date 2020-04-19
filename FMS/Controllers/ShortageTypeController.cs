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
    public class ShortageTypeController : Controller
    {
        private FMSDbContext db = new FMSDbContext();

        // GET: ShortageType
        public ActionResult Index()
        {
            return View(db.ShortageTypes.ToList());
        }

        // GET: ShortageType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShortageType ShortageType = db.ShortageTypes.Find(id);
            if (ShortageType == null)
            {
                return HttpNotFound();
            }
            return View(ShortageType);
        }

        // GET: ShortageType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShortageType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShortageTypeId,Name,Description")] ShortageType ShortageType)
        {
            if (ModelState.IsValid)
            {
                db.ShortageTypes.Add(ShortageType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ShortageType);
        }

        // GET: ShortageType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShortageType ShortageType = db.ShortageTypes.Find(id);
            if (ShortageType == null)
            {
                return HttpNotFound();
            }
            return View(ShortageType);
        }

        // POST: ShortageType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShortageTypeId,Name,Description")] ShortageType ShortageType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ShortageType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ShortageType);
        }

        // GET: ShortageType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShortageType ShortageType = db.ShortageTypes.Find(id);
            if (ShortageType == null)
            {
                return HttpNotFound();
            }
            return View(ShortageType);
        }

        // POST: ShortageType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShortageType ShortageType = db.ShortageTypes.Find(id);
            db.ShortageTypes.Remove(ShortageType);
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
