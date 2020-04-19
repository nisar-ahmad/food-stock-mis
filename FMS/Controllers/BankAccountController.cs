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
    public class BankAccountController : BaseController
    {
        private void PopulateDropDowns(BankAccount bankAccount)
        {
            base.PopulateDropDowns(bankAccount.Place);
            ViewBag.BankId = new SelectList(db.Banks, "BankId", "BankName", bankAccount.BankId);
        }

        // GET: BankAccount
        public ActionResult Index()
        {
            var bankAccounts = db.BankAccounts.Include(b => b.Bank);
            return View(bankAccounts.ToList());
        }

        // GET: BankAccount/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Include(o => o.Place).FirstOrDefault(o => o.BankAccountId == id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // GET: BankAccount/Create
        public ActionResult Create()
        {
            var bankAccount = new BankAccount();
            bankAccount.Place = new Place();

            PopulateDropDowns(bankAccount);

            return View(bankAccount);
        }

        // POST: BankAccount/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                AddPlace(bankAccount.Place, PlaceType.BankAccount);
                db.BankAccounts.Add(bankAccount);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            PopulateDropDowns(bankAccount);
            return View(bankAccount);
        }

        // GET: BankAccount/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Include(o => o.Place).FirstOrDefault(o => o.BankAccountId == id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            PopulateDropDowns(bankAccount);
            return View(bankAccount);
        }

        // POST: BankAccount/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                EditPlace(bankAccount.Place);
                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            PopulateDropDowns(bankAccount);
            return View(bankAccount);
        }

        // GET: BankAccount/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Include(o => o.Place).FirstOrDefault(o => o.BankAccountId == id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankAccount bankAccount = db.BankAccounts.Find(id);
            db.BankAccounts.Remove(bankAccount);
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
