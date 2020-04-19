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
    public class UnionCouncilController : Controller
    {
        private FMSDbContext db = new FMSDbContext();

        // GET: UnionCouncil
        public ActionResult Index()
        {
            var unionCouncils = db.UnionCouncils.Include(u => u.Tehsil);
            return View(unionCouncils.ToList());
        }

        // GET: UnionCouncil/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnionCouncil unionCouncil = db.UnionCouncils.Find(id);
            if (unionCouncil == null)
            {
                return HttpNotFound();
            }
            return View(unionCouncil);
        }

        // GET: UnionCouncil/Create
        public ActionResult Create()
        {
            ViewBag.TehsilId = new SelectList(db.Tehsils, "TehsilId", "Name");
            return View();
        }

        // POST: UnionCouncil/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UnionCouncilId,Name,Description,TehsilId")] UnionCouncil unionCouncil)
        {
            if (ModelState.IsValid)
            {
                db.UnionCouncils.Add(unionCouncil);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TehsilId = new SelectList(db.Tehsils, "TehsilId", "Name", unionCouncil.TehsilId);
            return View(unionCouncil);
        }

        // GET: UnionCouncil/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnionCouncil unionCouncil = db.UnionCouncils.Find(id);
            if (unionCouncil == null)
            {
                return HttpNotFound();
            }
            ViewBag.TehsilId = new SelectList(db.Tehsils, "TehsilId", "Name", unionCouncil.TehsilId);
            return View(unionCouncil);
        }

        // POST: UnionCouncil/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UnionCouncilId,Name,Description,TehsilId")] UnionCouncil unionCouncil)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unionCouncil).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TehsilId = new SelectList(db.Tehsils, "TehsilId", "Name", unionCouncil.TehsilId);
            return View(unionCouncil);
        }

        // GET: UnionCouncil/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnionCouncil unionCouncil = db.UnionCouncils.Find(id);
            if (unionCouncil == null)
            {
                return HttpNotFound();
            }
            return View(unionCouncil);
        }

        // POST: UnionCouncil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UnionCouncil unionCouncil = db.UnionCouncils.Find(id);
            db.UnionCouncils.Remove(unionCouncil);
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
