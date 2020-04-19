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
    public class CashTransactionController : BaseController
    {
        private void PopulateDropDowns(CashTransaction payment)
        {
            var depots = db.Depots.AsQueryable();

            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
                depots = depots.Where(o => o.DistrictOfficeId == districtOfficeId);

            ViewBag.FromPlaceId = new SelectList(depots, "PlaceId", "Place.Name", payment.FromPlaceId);
        }

        private void PopulateDropDowns(CommodityTransaction commodityTransaction)
        {
            var depots = db.Depots.AsQueryable();
            var dealers = db.Dealers.AsQueryable();

            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
            {
                dealers = dealers.Where(o => o.Depot.DistrictOfficeId == districtOfficeId);
                depots = depots.Where(o => o.DistrictOfficeId == districtOfficeId);
            }
            
            var fromPlaces = db.Places.Where(o => depots.Any(a => a.PlaceId == o.PlaceId));
            var toPlaces = db.Places.Where(o => dealers.Any(a => a.PlaceId == o.PlaceId));

            var types = db.CommodityTypes.Where(o => !o.Name.ToLower().Contains("wheat"));

            ViewBag.CommodityTypeId = new SelectList(types, "CommodityTypeId", "Name", commodityTransaction.CommodityTypeId);

            ViewBag.FromPlaceId = new SelectList(fromPlaces, "PlaceId", "Name", commodityTransaction.FromPlaceId);
            ViewBag.ToPlaceId = new SelectList(toPlaces, "PlaceId", "Name", commodityTransaction.ToPlaceId);

            ViewBag.IssuingOfficerId = new SelectList(db.People, "PersonId", "Name", commodityTransaction.IssuingOfficerId);

            ViewBag.UnitId = new SelectList(db.Units, "UnitId", "Name", commodityTransaction.UnitId);
        }

        private void Update(CashTransaction payment, int? issuingOfficerId = null)
        {
            var shipment = payment.CommodityTransaction;

            if (payment.CommodityTransactionId != null)
                shipment.CommodityTransactionId = payment.CommodityTransactionId.Value;

            shipment.DispatchDate = payment.TransactionDate;
            shipment.ReceiveDate = shipment.DispatchDate;
            shipment.BagsReceived = shipment.Bags;
            shipment.QuantityReceived = shipment.Quantity;
            shipment.IssuingOfficerId = issuingOfficerId;

            shipment.Type = CommodityTransactionType.Dispatch;
            shipment.Status = CommodityTransactionStatus.Received;

            shipment.CommodityTypeId = payment.CommodityTypeId.Value;
            shipment.UnitId = payment.UnitId.Value;
            shipment.FromPlaceId = payment.FromPlaceId;
            shipment.ToPlaceId = payment.ToPlaceId;
            shipment.Rate = payment.Rate;
            
            payment.FromPlaceId = shipment.ToPlaceId; // swap
            payment.ToPlaceId = shipment.FromPlaceId;

            payment.BookNumber = shipment.BookNumber;
            payment.CashVoucherNo = shipment.VoucherNo;
            payment.Bags = shipment.Bags;
            payment.Quantity = shipment.Quantity;            
            payment.Remarks = shipment.Remarks;

            payment.BankAccount = db.Depots.Include(o => o.BankAccount).FirstOrDefault(o => o.PlaceId == shipment.FromPlaceId).BankAccount;
            payment.BankAccountId = payment.BankAccount.BankAccountId;

            payment.Type = CashTransactionType.Credit;
            payment.Status = CashTransactionStatus.Completed;
        }

        public ActionResult DepotSale(int id = 0)
        {
            CashTransaction payment = null;

            if (id != 0) // Edit
            {
                payment = db.CashTransactions.Include(o => o.CommodityTransaction).FirstOrDefault(o => o.CommodityTransactionId == id);
            }
            else
            {
                payment = new CashTransaction();
                payment.CommodityTransaction = new CommodityTransaction();
            }

            PopulateDropDowns(payment.CommodityTransaction);
            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DepotSale(CashTransaction payment, int? IssuingOfficerId = null)
        {
            if (payment.Rate.HasValue && payment.Rate > 0)
            {
                if (payment.CashTransactionId == 0) // Add
                {
                    Update(payment, IssuingOfficerId);

                    db.CommodityTransactions.Add(payment.CommodityTransaction);
                    db.CashTransactions.Add(payment);
                }
                else
                {
                    Update(payment, IssuingOfficerId);
                    db.Entry(payment.CommodityTransaction).State = EntityState.Modified;
                    db.Entry(payment).State = EntityState.Modified;
                }

                db.SaveChanges();
                Success();
                return RedirectToAction("DepotSale");
            }

            Error("Rate must be entered!");
            PopulateDropDowns(payment.CommodityTransaction);
            return View(payment);
        }

        // GET: CashTransaction/Dealer
        public ActionResult Dealer(int id)
        {
            var payment = db.CashTransactions.Include(o => o.CommodityTransaction).Include(o => o.BankAccount).FirstOrDefault(o=> o.CommodityTransactionId == id);

            if (payment == null)
            {
                var shipment = db.CommodityTransactions.FirstOrDefault(o => o.CommodityTransactionId == id);

                payment = new CashTransaction();
                payment.CommodityTransaction = shipment;
                payment.CommodityTransactionId = shipment.CommodityTransactionId;
                payment.BankAccount = db.Depots.Include(o => o.BankAccount).FirstOrDefault(o => o.PlaceId == shipment.FromPlaceId).BankAccount;
                payment.BankAccountId = payment.BankAccount.BankAccountId;
            }

            return View(payment);
        }

        [HttpPost]
        public ActionResult Dealer(CashTransaction payment)
        {
            if (payment.Amount > 0 && !string.IsNullOrWhiteSpace(payment.CashVoucherNo) && payment.TransactionDate != DateTime.MinValue)
            {
                if (payment.CashTransactionId == 0) // Add
                {
                    var shipment = db.CommodityTransactions.FirstOrDefault(o => o.CommodityTransactionId == payment.CommodityTransactionId);

                    payment.CommodityTypeId = shipment.CommodityTypeId;

                    payment.Bags = shipment.Bags;
                    payment.Quantity = shipment.Quantity;
                    payment.FromPlaceId = shipment.ToPlaceId;
                    payment.ToPlaceId = shipment.FromPlaceId;
                    payment.UnitId = shipment.UnitId;
                    payment.Rate = shipment.Rate;
                    payment.Status = CashTransactionStatus.Completed;
                    payment.Type = CashTransactionType.Credit;

                    db.CashTransactions.Add(payment);
                }
                else
                {
                    var p = db.CashTransactions.FirstOrDefault(o => o.CashTransactionId == payment.CashTransactionId);
                    p.CashVoucherNo = payment.CashVoucherNo;
                    p.Amount = payment.Amount;
                    p.TransactionDate = payment.TransactionDate;
                    p.Remarks = payment.Remarks;
                }

                db.SaveChanges();
                Success();
                return RedirectToAction("Dealer", new { id = payment.CommodityTransactionId });
            }

            return View(payment);
        }

        public ActionResult DealerPayments(int id = 0)
        {
            var payments = db.CashTransactions.Where(o => o.FromPlace.Type == PlaceType.Dealer);

            if (id > 0)
                payments = payments.Where(o => o.FromPlaceId == id);

            return View(payments);
        }

        public ActionResult Depot(int id = 0)
        {
            CashTransaction payment = null;

            if(id > 0)
                payment = db.CashTransactions.Include(o => o.BankAccount).FirstOrDefault(o => o.CashTransactionId == id);
            else
                payment = new CashTransaction();

            payment.BankAccount = db.Directorates.First().BankAccount;
            payment.BankAccountId = payment.BankAccount.BankAccountId;

            PopulateDropDowns(payment);
            return View(payment);
        }

        [HttpPost]
        public ActionResult Depot(CashTransaction payment)
        {
            if (payment.Amount > 0 && !string.IsNullOrWhiteSpace(payment.CashVoucherNo) && payment.TransactionDate != DateTime.MinValue)
            {
                if (payment.CashTransactionId == 0) // Add
                {
                    payment.Status = CashTransactionStatus.Completed;
                    payment.Type = CashTransactionType.Credit;

                    db.CashTransactions.Add(payment);
                }
                else
                {
                    var p = db.CashTransactions.FirstOrDefault(o => o.CashTransactionId == payment.CashTransactionId);
                    
                    p.FromPlaceId = payment.FromPlaceId;
                    p.CashVoucherNo = payment.CashVoucherNo;
                    p.Amount = payment.Amount;
                    p.TransactionDate = payment.TransactionDate;
                    p.Remarks = payment.Remarks;
                }

                payment.ToPlaceId = db.Directorates.First().PlaceId;
                db.SaveChanges();

                Success();
                return RedirectToAction("Depot"); // DepotPayments
            }

            PopulateDropDowns(payment);
            return View(payment);
        }


        public ActionResult DepotPayments(int id = 0)
        {
            var payments = db.CashTransactions.Where(o => o.FromPlace.Type == PlaceType.Depot);

            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
            {
                var depots = db.Depots.Where(o => o.DistrictOfficeId == districtOfficeId);
                payments = payments.Where(o => depots.Any(a => a.PlaceId == o.FromPlaceId));
            }

            if (id > 0)
                payments = payments.Where(o => o.FromPlaceId == id);

            return View(payments);
        }

        // GET: CashTransaction
        public ActionResult Index()
        {
            var cashTransactions = db.CashTransactions.Include(c => c.BankAccount).Include(c => c.CommodityTransaction).Include(c => c.CommodityType).Include(c => c.FromPlace).Include(c => c.ToPlace).Include(c => c.Unit);
            return View(cashTransactions.ToList());
        }

        // GET: CashTransaction/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashTransaction cashTransaction = db.CashTransactions.Find(id);
            if (cashTransaction == null)
            {
                return HttpNotFound();
            }
            return View(cashTransaction);
        }

        // GET: CashTransaction/Create
        public ActionResult Create()
        {
            ViewBag.BankAccountId = new SelectList(db.BankAccounts, "BankAccountId", "Branch");
            ViewBag.CommodityTransactionId = new SelectList(db.CommodityTransactions, "CommodityTransactionId", "CompositeKey");
            ViewBag.CommodityTypeId = new SelectList(db.CommodityTypes, "CommodityTypeId", "Name");
            ViewBag.FromPlaceId = new SelectList(db.Places, "PlaceId", "Name");
            ViewBag.UnitId = new SelectList(db.Units, "UnitId", "Name");
            return View();
        }

        // POST: CashTransaction/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CashTransactionId,CompositeKey,Type,CashVoucherNo,FromPlaceId,ToPlaceId,CommodityTypeId,TransactionDate,Amount,Bags,Quantity,UnitId,Rate,Status,CommodityTransactionId,BankAccountId")] CashTransaction cashTransaction)
        {
            if (ModelState.IsValid)
            {
                db.CashTransactions.Add(cashTransaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BankAccountId = new SelectList(db.BankAccounts, "BankAccountId", "Branch", cashTransaction.BankAccountId);
            ViewBag.CommodityTransactionId = new SelectList(db.CommodityTransactions, "CommodityTransactionId", "CompositeKey", cashTransaction.CommodityTransactionId);
            ViewBag.CommodityTypeId = new SelectList(db.CommodityTypes, "CommodityTypeId", "Name", cashTransaction.CommodityTypeId);
            ViewBag.FromPlaceId = new SelectList(db.Places, "PlaceId", "Name", cashTransaction.FromPlaceId);
            ViewBag.UnitId = new SelectList(db.Units, "UnitId", "Name", cashTransaction.UnitId);
            return View(cashTransaction);
        }

        // GET: CashTransaction/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashTransaction cashTransaction = db.CashTransactions.Find(id);
            if (cashTransaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.BankAccountId = new SelectList(db.BankAccounts, "BankAccountId", "Branch", cashTransaction.BankAccountId);
            ViewBag.CommodityTransactionId = new SelectList(db.CommodityTransactions, "CommodityTransactionId", "CompositeKey", cashTransaction.CommodityTransactionId);
            ViewBag.CommodityTypeId = new SelectList(db.CommodityTypes, "CommodityTypeId", "Name", cashTransaction.CommodityTypeId);
            ViewBag.FromPlaceId = new SelectList(db.Places, "PlaceId", "Name", cashTransaction.FromPlaceId);
            ViewBag.UnitId = new SelectList(db.Units, "UnitId", "Name", cashTransaction.UnitId);
            return View(cashTransaction);
        }

        // POST: CashTransaction/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CashTransactionId,CompositeKey,Type,CashVoucherNo,FromPlaceId,ToPlaceId,CommodityTypeId,TransactionDate,Amount,Bags,Quantity,UnitId,Rate,Status,CommodityTransactionId,BankAccountId")] CashTransaction cashTransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cashTransaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BankAccountId = new SelectList(db.BankAccounts, "BankAccountId", "Branch", cashTransaction.BankAccountId);
            ViewBag.CommodityTransactionId = new SelectList(db.CommodityTransactions, "CommodityTransactionId", "CompositeKey", cashTransaction.CommodityTransactionId);
            ViewBag.CommodityTypeId = new SelectList(db.CommodityTypes, "CommodityTypeId", "Name", cashTransaction.CommodityTypeId);
            ViewBag.FromPlaceId = new SelectList(db.Places, "PlaceId", "Name", cashTransaction.FromPlaceId);
            ViewBag.UnitId = new SelectList(db.Units, "UnitId", "Name", cashTransaction.UnitId);
            return View(cashTransaction);
        }

        // GET: CashTransaction/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashTransaction cashTransaction = db.CashTransactions.Find(id);
            if (cashTransaction == null)
            {
                return HttpNotFound();
            }
            return View(cashTransaction);
        }

        // POST: CashTransaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CashTransaction cashTransaction = db.CashTransactions.Find(id);
            db.CashTransactions.Remove(cashTransaction);
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
