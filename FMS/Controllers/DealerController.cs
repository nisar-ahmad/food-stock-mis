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
    public class DealerController : BaseController
    {
        private void PopulateDropDowns(Dealer dealer)
        {
            base.PopulateDropDowns(dealer.Place);

            var depots = db.Depots.Include(d => d.Place);
            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
                depots = depots.Where(o => o.DistrictOfficeId == districtOfficeId);

            ViewBag.DepotId = new SelectList(depots, "DepotId", "Place.Name", dealer.DepotId);
        }

        // GET: Dealer
        public ActionResult Index()
        {
            var dealers = db.Dealers.Include(d => d.Depot).Include(d => d.Place);

            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
                dealers = dealers.Where(o => o.Depot.DistrictOfficeId == districtOfficeId);

            return View(dealers.ToList());
        }

        // GET: Dealer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = db.Dealers.Include(d => d.Depot).Include(d => d.Place).FirstOrDefault(o => o.DealerId == id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // GET: Dealer/Create
        public ActionResult Create()
        {
            var dealer = new Dealer();
            dealer.Place = new Place();

            PopulateDropDowns(dealer);
            return View(dealer);
        }

        // POST: Dealer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dealer dealer)
        {
            if (ModelState.IsValid)
            {
                AddPlace(dealer.Place, PlaceType.Dealer);
                db.Dealers.Add(dealer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateDropDowns(dealer);
            return View(dealer);
        }

        // GET: Dealer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = db.Dealers.Include(d => d.Depot).Include(d => d.Place).FirstOrDefault(o => o.DealerId == id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            PopulateDropDowns(dealer);
            return View(dealer);
        }

        // POST: Dealer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Dealer dealer)
        {
            if (ModelState.IsValid)
            {
                EditPlace(dealer.Place);
                db.Entry(dealer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateDropDowns(dealer);
            return View(dealer);
        }

        // GET: Dealer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = db.Dealers.Include(d => d.Depot).Include(d => d.Place).FirstOrDefault(o=> o.DealerId == id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // POST: Dealer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dealer dealer = db.Dealers.Find(id);
            db.Dealers.Remove(dealer);
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
