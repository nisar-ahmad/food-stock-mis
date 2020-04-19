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
    public class ContractorTypeController : Controller
    {
        private FMSDbContext db = new FMSDbContext();

        // GET: ContractorType
        public ActionResult Index()
        {
            return View(db.ContractorTypes.ToList());
        }

        // GET: ContractorType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorType contractorType = db.ContractorTypes.Find(id);
            if (contractorType == null)
            {
                return HttpNotFound();
            }
            return View(contractorType);
        }

        // GET: ContractorType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContractorType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContractorTypeId,Name,Description")] ContractorType contractorType)
        {
            if (ModelState.IsValid)
            {
                db.ContractorTypes.Add(contractorType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contractorType);
        }

        // GET: ContractorType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorType contractorType = db.ContractorTypes.Find(id);
            if (contractorType == null)
            {
                return HttpNotFound();
            }
            return View(contractorType);
        }

        // POST: ContractorType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContractorTypeId,Name,Description")] ContractorType contractorType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contractorType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contractorType);
        }

        // GET: ContractorType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorType contractorType = db.ContractorTypes.Find(id);
            if (contractorType == null)
            {
                return HttpNotFound();
            }
            return View(contractorType);
        }

        // POST: ContractorType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContractorType contractorType = db.ContractorTypes.Find(id);
            db.ContractorTypes.Remove(contractorType);
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
