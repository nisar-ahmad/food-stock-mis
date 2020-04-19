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
    public class GodownController : BaseController
    {
        private void PopulateDropDowns(Godown godown)
        {
            base.PopulateDropDowns(godown.Place);
            ViewBag.StorageOfficeId = new SelectList(db.StorageOffices.Include(o => o.Place), "StorageOfficeId", "Place.Name", godown.StorageOfficeId);
            ViewBag.UnitId = new SelectList(db.Units, "UnitId", "Name", godown.UnitId);
        }

        // GET: Godown
        public ActionResult Index()
        {
            var godowns = db.Godowns.Include(g => g.Place).Include(g => g.StorageOffice).Include(g => g.Unit);

            if (!string.IsNullOrEmpty(Request["GodownType"]))
            {
                var type = (GodownType)Enum.Parse(typeof(GodownType), Request["GodownType"]);
                godowns = godowns.Where(o => o.Type == type);
            }

            return View(godowns.ToList());
        }

        // GET: Godown/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Godown godown = db.Godowns.Include(g => g.Place).Include(g => g.Unit).FirstOrDefault(o=> o.GodownId == id);

            if (godown == null)
            {
                return HttpNotFound();
            }

            return View(godown);
        }

        // GET: Godown/Create
        public ActionResult Create()
        {
            var godown = new Godown();
            godown.Place = new Place();

            PopulateDropDowns(godown);
            return View(godown);
        }

        // POST: Godown/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Godown godown)
        {
            if (ModelState.IsValid)
            {
                AddPlace(godown.Place, PlaceType.Godown);
                db.Godowns.Add(godown);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateDropDowns(godown);
            return View(godown);
        }

        // GET: Godown/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Godown godown = db.Godowns.Include(g => g.Place).Include(g => g.Unit).FirstOrDefault(o => o.GodownId == id);
            if (godown == null)
            {
                return HttpNotFound();
            }

            PopulateDropDowns(godown);
            return View(godown);
        }

        // POST: Godown/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Godown godown)
        {
            if (ModelState.IsValid)
            {
                EditPlace(godown.Place);
                db.Entry(godown).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateDropDowns(godown);
            return View(godown);
        }

        // GET: Godown/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Godown godown = db.Godowns.Include(g => g.Place).Include(g => g.Unit).FirstOrDefault(o => o.GodownId == id);
            if (godown == null)
            {
                return HttpNotFound();
            }
            return View(godown);
        }

        // POST: Godown/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Godown godown = db.Godowns.Find(id);
            db.Godowns.Remove(godown);
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
