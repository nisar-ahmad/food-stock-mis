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
    public class WheatRequestController : BaseController
    {
        private void PopulateDropDowns(WheatRequest wheatRequest)
        {
            ViewBag.UnitId = new SelectList(db.Units, "UnitId", "Name", wheatRequest.UnitId);
        }

        // GET: WheatRequest
        public ActionResult Index()
        {
            var wheatRequests = db.WheatRequests.Include(w => w.Directorate).Include(w => w.Unit);
            return View(wheatRequests.ToList());
        }

        // GET: WheatRequest/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WheatRequest wheatRequest = db.WheatRequests.Find(id);
            if (wheatRequest == null)
            {
                return HttpNotFound();
            }
            return View(wheatRequest);
        }

        // GET: WheatRequest/Create
        public ActionResult Create()
        {
            var wheatRequest = new WheatRequest();
            wheatRequest.RequestDate = DateTime.Now;

            PopulateDropDowns(wheatRequest);
            return View(wheatRequest);
        }

        // POST: WheatRequest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WheatRequest wheatRequest)
        {
            if (ModelState.IsValid)
            {
                db.WheatRequests.Add(wheatRequest);
                db.SaveChanges();
                Success();
                return RedirectToAction("Create"); // Index
            }

            PopulateDropDowns(wheatRequest);
            return View(wheatRequest);
        }

        // GET: WheatRequest/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WheatRequest wheatRequest = db.WheatRequests.Find(id);
            if (wheatRequest == null)
            {
                return HttpNotFound();
            }
            PopulateDropDowns(wheatRequest);
            return View(wheatRequest);
        }

        // POST: WheatRequest/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WheatRequest wheatRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wheatRequest).State = EntityState.Modified;
                db.SaveChanges();
                Success();
                //return RedirectToAction("Index");
            }
            PopulateDropDowns(wheatRequest);
            return View(wheatRequest);
        }

        // GET: WheatRequest/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WheatRequest wheatRequest = db.WheatRequests.Find(id);
            if (wheatRequest == null)
            {
                return HttpNotFound();
            }
            return View(wheatRequest);
        }

        // POST: WheatRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WheatRequest wheatRequest = db.WheatRequests.Find(id);
            db.WheatRequests.Remove(wheatRequest);
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
