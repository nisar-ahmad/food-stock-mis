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
    public class CommodityTransactionController : BaseController
    {
        #region COMMON METHODS

        private void PopulateFilters(CommodityTransactionFilter filter, IQueryable<CommodityTransaction> query)
        {
            ViewBag.Type = new SelectList(Enum.GetValues(typeof(CommodityTransactionType)));
            ViewBag.CommodityTypeId = new SelectList(db.CommodityTypes, "CommodityTypeId", "Name", filter.CommodityTypeId);
            ViewBag.FromPlaceId = new SelectList(db.Places.Where(o => query.Select(c => c.FromPlaceId).Contains(o.PlaceId)), "PlaceId", "Name", filter.FromPlaceId);
            ViewBag.ToPlaceId = new SelectList(db.Places.Where(o => query.Select(c => c.ToPlaceId).Contains(o.PlaceId)), "PlaceId", "Name", filter.FromPlaceId);
            ViewBag.ContractorId = new SelectList(db.Contractors.Include(o => o.Place).Where(o => query.Select(c => c.ContractorId).Contains(o.ContractorId)), "ContractorId", "Place.Name", filter.ContractorId);
            ViewBag.DispatchDateStart = filter.DispatchDateStart;
            ViewBag.DispatchDateEnd = filter.DispatchDateEnd;
            ViewBag.ReceiveDateStart = filter.ReceiveDateStart;
            ViewBag.ReceiveDateEnd = filter.ReceiveDateEnd;
        }

        private void PopulateDropDowns(CommodityTransaction commodityTransaction, IEnumerable<Place> fromPlaces = null, IEnumerable<Place> toPlaces = null, IEnumerable<CommodityType> commodityTypes = null, bool populateShortageTypes = false)
        {
            if (fromPlaces == null)
                fromPlaces = db.Places;

            if (toPlaces == null)
                toPlaces = db.Places;

            if (commodityTypes == null)
                commodityTypes = db.CommodityTypes;

            ViewBag.CommodityTypeId = new SelectList(commodityTypes, "CommodityTypeId", "Name", commodityTransaction.CommodityTypeId);
            ViewBag.ContractorId = new SelectList(db.Contractors, "ContractorId", "Place.Name", commodityTransaction.ContractorId);

            ViewBag.FromPlaceId = new SelectList(fromPlaces, "PlaceId", "Name", commodityTransaction.FromPlaceId);
            ViewBag.ToPlaceId = new SelectList(toPlaces, "PlaceId", "Name", commodityTransaction.ToPlaceId);

            ViewBag.ReceivingOfficerId = new SelectList(db.People, "PersonId", "Name", commodityTransaction.ReceivingOfficerId);
            ViewBag.IssuingOfficerId = new SelectList(db.People, "PersonId", "Name", commodityTransaction.IssuingOfficerId);

            ViewBag.UnitId = new SelectList(db.Units, "UnitId", "Name", commodityTransaction.UnitId);

            if (populateShortageTypes)
                ViewBag.ShortageTypeId = new SelectList(db.ShortageTypes, "ShortageTypeId", "Name", commodityTransaction.ShortageTypeId);
        }

        private IQueryable<CommodityTransaction> GetFilteredQuery(IQueryable<CommodityTransaction> query, CommodityTransactionFilter filter)
        {
            if(filter != null && query != null)
            { 
                if (filter.Type != null)
                    query = query.Where(o => o.Type == filter.Type);

                if (filter.CommodityTypeId != null)
                    query = query.Where(o => o.CommodityTypeId == filter.CommodityTypeId);

                if (filter.FromPlaceId != null)
                    query = query.Where(o => o.FromPlaceId == filter.FromPlaceId);

                if (filter.ToPlaceId != null)
                    query = query.Where(o => o.ToPlaceId == filter.ToPlaceId);

                if (filter.ContractorId != null)
                    query = query.Where(o => o.ContractorId == filter.ContractorId);

                if (filter.DispatchDateStart != null)
                    query = query.Where(o => o.DispatchDate >= filter.DispatchDateStart);

                if (filter.DispatchDateEnd != null)
                    query = query.Where(o => o.DispatchDate <= filter.DispatchDateEnd);

                if (filter.ReceiveDateStart != null)
                    query = query.Where(o => o.ReceiveDate >= filter.ReceiveDateStart);

                if (filter.ReceiveDateEnd != null)
                    query = query.Where(o => o.ReceiveDate <= filter.ReceiveDateEnd);
            }

            return query;
        }

        private SortableList<CommodityTransaction> GetSortableList(IQueryable<CommodityTransaction> query, CommodityTransactionType? Type = null, int? CommodityTypeId = null, int? FromPlaceId = null,
                                          int? ToPlaceId = null, int? ContractorId = null, DateTime? DispatchDateStart = null, DateTime? DispatchDateEnd = null,
                                          DateTime? ReceiveDateStart = null, DateTime? ReceiveDateEnd = null, int page = 1, string sortBy = null, bool ascending = true)
        {
            var filter = new CommodityTransactionFilter
            {
                Type = Type,
                CommodityTypeId = CommodityTypeId,
                FromPlaceId = FromPlaceId,
                ToPlaceId = ToPlaceId,
                ContractorId = ContractorId,
                DispatchDateStart = DispatchDateStart,
                DispatchDateEnd = DispatchDateEnd,
                ReceiveDateStart = ReceiveDateStart,
                ReceiveDateEnd = ReceiveDateEnd
            };

            query = GetFilteredQuery(query, filter);
            PopulateFilters(filter, query);

            return new SortableList<CommodityTransaction>(query, page, sortBy, ascending);
        }

        public ActionResult PrintVoucher(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommodityTransaction commodityTransaction = db.CommodityTransactions.Find(id);
            if (commodityTransaction == null)
            {
                return HttpNotFound();
            }
            return View(commodityTransaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region STORAGE OFFICE

        private CommodityTransaction GetShipment(int id)
        {
            CommodityTransaction commodityTransaction = null;

            if (id == 0)
            {
                commodityTransaction = new CommodityTransaction();
                commodityTransaction.DispatchDate = DateTime.Now;
            }
            else
            {
                commodityTransaction = db.CommodityTransactions.FirstOrDefault(o => o.CommodityTransactionId == id);
            }

            return commodityTransaction;
        }

        public ActionResult StorageOffice(CommodityTransactionType? Type = null, int? CommodityTypeId = null, int? FromPlaceId = null,
                                          int? ToPlaceId = null, int? ContractorId = null, DateTime? DispatchDateStart = null, DateTime? DispatchDateEnd = null,
                                          DateTime? ReceiveDateStart = null, DateTime? ReceiveDateEnd = null, int page = 1, string sortBy = null, bool ascending = true)
        {
            var query = db.CommodityTransactions.Include(c => c.FromPlace).Include(c => c.ToPlace).Include(c => c.CommodityType).Include(c => c.Contractor).Include(c => c.IssuingOfficer).Include(c => c.ReceivingOfficer).Include(c => c.Unit);
            query = query.Where(o => o.ToPlace.Type == PlaceType.Godown || o.ToPlace.Type == PlaceType.FlourMill);

            var list = GetSortableList(query, Type, CommodityTypeId, FromPlaceId, ToPlaceId, ContractorId, DispatchDateStart, DispatchDateEnd,
                                       ReceiveDateStart, ReceiveDateEnd, page, sortBy, ascending);
            return View(list);
        }

        public ActionResult SOReceive(int id = 0)
        {
            var commodityTransaction = GetShipment(id);

            var pascoGodowns = db.Godowns.Where(g => g.Type == GodownType.PASSCO).Select(g => g.PlaceId);
            var fromPlaces = db.Places.Where(o => pascoGodowns.Any(a => a == o.PlaceId));

            var soGodowns = db.Godowns.Where(g => g.Type == GodownType.StorageOffice).Select(g => g.PlaceId);
            var toPlaces = db.Places.Where(o => soGodowns.Any(a => a == o.PlaceId));
            var types = db.CommodityTypes.Where(o => o.Name.ToLower().Contains("wheat"));

            PopulateDropDowns(commodityTransaction, fromPlaces, toPlaces, types, true);

            return View(commodityTransaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SOReceive(CommodityTransaction commodityTransaction)
        {
            if (ModelState.IsValid)
            {
                int id = commodityTransaction.CommodityTransactionId;

                if (id == 0)
                {
                    commodityTransaction.Type = CommodityTransactionType.Receive;
                    commodityTransaction.Status = CommodityTransactionStatus.Received;

                    db.CommodityTransactions.Add(commodityTransaction);
                }
                else
                {
                    var shipment = db.CommodityTransactions.Find(id);

                    shipment.DispatchDate = commodityTransaction.DispatchDate;
                    shipment.FromPlaceId = commodityTransaction.FromPlaceId;
                    shipment.ToPlaceId = commodityTransaction.ToPlaceId;
                    shipment.CommodityTypeId = commodityTransaction.CommodityTypeId;
                    shipment.Trucks = commodityTransaction.Trucks;
                    shipment.Bags = commodityTransaction.Bags;
                    shipment.Quantity = commodityTransaction.Quantity;
                    shipment.BagsReceived = commodityTransaction.BagsReceived;
                    shipment.QuantityReceived = commodityTransaction.QuantityReceived;
                    shipment.ShortageTypeId = commodityTransaction.ShortageTypeId;
                    shipment.ReasonsForShortage = commodityTransaction.ReasonsForShortage;
                    shipment.UnitId = commodityTransaction.UnitId;
                    shipment.Rate = commodityTransaction.Rate;
                    shipment.BookNumber = commodityTransaction.BookNumber;
                    shipment.VoucherNo = commodityTransaction.VoucherNo;
                    shipment.ContractorId = commodityTransaction.ContractorId;
                    shipment.ReceiveDate = commodityTransaction.ReceiveDate;
                    shipment.ReceivingOfficerId = commodityTransaction.ReceivingOfficerId;
                    shipment.Remarks = commodityTransaction.Remarks;
                }

                db.SaveChanges();
                Success();
                return RedirectToAction("SOReceive"); // StorageOffice
            }

            var pascoGodowns = db.Godowns.Where(g => g.Type == GodownType.PASSCO).Select(g => g.PlaceId);
            var fromPlaces = db.Places.Where(o => pascoGodowns.Any(a => a == o.PlaceId));

            var soGodowns = db.Godowns.Where(g => g.Type == GodownType.StorageOffice).Select(g => g.PlaceId);
            var toPlaces = db.Places.Where(o => soGodowns.Any(a => a == o.PlaceId));
            var types = db.CommodityTypes.Where(o => o.Name.ToLower().Contains("wheat"));

            PopulateDropDowns(commodityTransaction, fromPlaces, toPlaces, types, true);

            return View(commodityTransaction);
        }

        public ActionResult SODispatch(int id = 0)
        {
            var commodityTransaction = GetShipment(id);

            var soGodowns = db.Godowns.Where(g => g.Type == GodownType.StorageOffice).Select(g => g.PlaceId);
            var flourMills = db.FlourMills.Select(g => g.PlaceId);

            var fromPlaces = db.Places.Where(o => soGodowns.Any(a => a == o.PlaceId));
            var toPlaces = db.Places.Where(o => flourMills.Any(a => a == o.PlaceId));
            var types = db.CommodityTypes.Where(o => o.Name.ToLower().Contains("wheat"));

            PopulateDropDowns(commodityTransaction, fromPlaces, toPlaces, types);

            return View(commodityTransaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SODispatch(CommodityTransaction commodityTransaction)
        {
            if (ModelState.IsValid)
            {
                int id = commodityTransaction.CommodityTransactionId;

                if (id == 0)
                {
                    commodityTransaction.Type = CommodityTransactionType.Dispatch;
                    commodityTransaction.Status = CommodityTransactionStatus.Dispatched;

                    db.CommodityTransactions.Add(commodityTransaction);
                }
                else
                {
                    var shipment = db.CommodityTransactions.Find(id);

                    shipment.BookNumber = commodityTransaction.BookNumber;
                    shipment.VoucherNo = commodityTransaction.VoucherNo;

                    shipment.DispatchDate = commodityTransaction.DispatchDate;
                    shipment.FromPlaceId = commodityTransaction.FromPlaceId;
                    shipment.ToPlaceId = commodityTransaction.ToPlaceId;
                    shipment.CommodityTypeId = commodityTransaction.CommodityTypeId;
                    shipment.Trucks = commodityTransaction.Trucks;
                    shipment.Bags = commodityTransaction.Bags;
                    shipment.Quantity = commodityTransaction.Quantity;
                    shipment.UnitId = commodityTransaction.UnitId;
                    shipment.Rate = commodityTransaction.Rate;

                    shipment.ContractorId = commodityTransaction.ContractorId;
                    shipment.IssuingOfficerId = commodityTransaction.IssuingOfficerId;

                    shipment.Remarks = commodityTransaction.Remarks;
                }

                db.SaveChanges();
                Success();
                return RedirectToAction("SODispatch"); // StorageOffice
            }

            var soGodowns = db.Godowns.Where(g => g.Type == GodownType.StorageOffice).Select(g => g.PlaceId);
            var flourMills = db.FlourMills.Select(g => g.PlaceId);

            var fromPlaces = db.Places.Where(o => soGodowns.Any(a => a == o.PlaceId));
            var toPlaces = db.Places.Where(o => flourMills.Any(a => a == o.PlaceId));
            var types = db.CommodityTypes.Where(o => o.Name.ToLower().Contains("wheat"));

            PopulateDropDowns(commodityTransaction, fromPlaces, toPlaces, types);

            return View(commodityTransaction);
        } 

        #endregion

        #region FLOUR MILL

        private void PopulateFlourMillDropDowns(CommodityTransaction commodityTransaction)
        {
            var types = db.CommodityTypes.Where(o => !o.Name.ToLower().Contains("wheat"));
            var flourMills = db.FlourMills.AsQueryable();
            var depots = db.Depots.AsQueryable();

            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
            {
                flourMills = flourMills.Where(o => o.DistrictOfficeId == districtOfficeId);
                depots = depots.Where(o => o.DistrictOfficeId == districtOfficeId);
            }

            var fromPlaces = db.Places.Where(o => flourMills.Select(g => g.PlaceId).Any(a => a == o.PlaceId));
            var toPlaces = db.Places.Where(o => depots.Select(g => g.PlaceId).Any(a => a == o.PlaceId));

            PopulateDropDowns(commodityTransaction, fromPlaces, toPlaces, types);
        }

        public ActionResult FlourMill(CommodityTransactionType? Type = null, int? CommodityTypeId = null, int? FromPlaceId = null,
                                          int? ToPlaceId = null, int? ContractorId = null, DateTime? DispatchDateStart = null, DateTime? DispatchDateEnd = null,
                                          DateTime? ReceiveDateStart = null, DateTime? ReceiveDateEnd = null, int page = 1, string sortBy = null, bool ascending = true)
        {
            var query = db.CommodityTransactions.Include(c => c.FromPlace).Include(c => c.ToPlace).Include(c => c.CommodityType).Include(c => c.Contractor).Include(c => c.IssuingOfficer).Include(c => c.ReceivingOfficer).Include(c => c.Unit);
            query = query.Where(o => o.ToPlace.Type == PlaceType.FlourMill || o.FromPlace.Type == PlaceType.FlourMill);

            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
            {
                var flourMillPlaceIds = db.FlourMills.Where(o => o.DistrictOfficeId == districtOfficeId).Select(o => o.PlaceId);
                query = query.Where(o => flourMillPlaceIds.Any(a => a == o.ToPlaceId));
            }

            var list = GetSortableList(query, Type, CommodityTypeId, FromPlaceId, ToPlaceId, ContractorId, DispatchDateStart, DispatchDateEnd,
                                       ReceiveDateStart, ReceiveDateEnd, page, sortBy, ascending);
            return View(list);
        }

        //public ActionResult FlourMill()
        //{
        //    var commodityTransactions = db.CommodityTransactions.Include(c => c.FromPlace).Include(c => c.ToPlace).Include(c => c.CommodityType).Include(c => c.Contractor).Include(c => c.IssuingOfficer).Include(c => c.ReceivingOfficer).Include(c => c.Unit);
        //    commodityTransactions = commodityTransactions.Where(o => o.ToPlace.Type == PlaceType.FlourMill || o.FromPlace.Type == PlaceType.FlourMill);
        //    return View(commodityTransactions.ToList());
        //}

        public ActionResult FlourMillReceive(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CommodityTransaction commodityTransaction = db.CommodityTransactions.Find(id);

            if (commodityTransaction == null)
            {
                return HttpNotFound();
            }

            PopulateDropDowns(commodityTransaction, null, null, null, true);

            if (commodityTransaction.ReceiveDate == null)
            { 
                commodityTransaction.ReceiveDate = DateTime.Now;
                commodityTransaction.BagsReceived = commodityTransaction.Bags;
                commodityTransaction.QuantityReceived = commodityTransaction.Quantity;
            }

            return View(commodityTransaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FlourMillReceive(CommodityTransaction commodityTransaction)
        {
            //if (ModelState.IsValid)
            {
                var received = db.CommodityTransactions.FirstOrDefault(o => o.CommodityTransactionId == commodityTransaction.CommodityTransactionId);

                if (received != null)
                {
                    if (commodityTransaction.ReceiveDate != null && commodityTransaction.ReceiveDate >= received.DispatchDate)
                    {
                        received.ReceiveDate = commodityTransaction.ReceiveDate;
                        received.ReceivingOfficerId = commodityTransaction.ReceivingOfficerId;
                        received.QuantityReceived = commodityTransaction.QuantityReceived;
                        received.BagsReceived = commodityTransaction.BagsReceived;
                        received.ShortageTypeId = commodityTransaction.ShortageTypeId;
                        received.ReasonsForShortage = commodityTransaction.ReasonsForShortage;
                        received.Remarks = commodityTransaction.Remarks;
                        received.Status = CommodityTransactionStatus.Received;

                        db.SaveChanges();
                    }
                    else
                    {
                        ModelState.AddModelError("ReceiveDate", "Receive Date cannot be less than Dispatch Date");
                        PopulateDropDowns(received, null, null, null, true);
                        return View(received);
                    }
                }

                Success();
                return RedirectToAction("FlourMillReceive", new { id = commodityTransaction.CommodityTransactionId }); // FlourMill
            }
        }

        public ActionResult FlourMillDispatch(int id = 0)
        {
            var commodityTransaction = GetShipment(id);
            PopulateFlourMillDropDowns(commodityTransaction);
            return View(commodityTransaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FlourMillDispatch(CommodityTransaction commodityTransaction)
        {
            if (ModelState.IsValid)
            {
                int id = commodityTransaction.CommodityTransactionId;

                if (id == 0)
                {
                    commodityTransaction.Type = CommodityTransactionType.Dispatch;
                    commodityTransaction.Status = CommodityTransactionStatus.Dispatched;

                    db.CommodityTransactions.Add(commodityTransaction);
                }
                else
                {
                    var shipment = db.CommodityTransactions.Find(id);

                    shipment.BookNumber = commodityTransaction.BookNumber;
                    shipment.VoucherNo = commodityTransaction.VoucherNo;

                    shipment.DispatchDate = commodityTransaction.DispatchDate;
                    shipment.FromPlaceId = commodityTransaction.FromPlaceId;
                    shipment.ToPlaceId = commodityTransaction.ToPlaceId;
                    shipment.CommodityTypeId = commodityTransaction.CommodityTypeId;
                    shipment.Trucks = commodityTransaction.Trucks;
                    shipment.Bags = commodityTransaction.Bags;
                    shipment.Quantity = commodityTransaction.Quantity;
                    shipment.UnitId = commodityTransaction.UnitId;
                    shipment.Rate = commodityTransaction.Rate;

                    shipment.ContractorId = commodityTransaction.ContractorId;
                    shipment.IssuingOfficerId = commodityTransaction.IssuingOfficerId;

                    shipment.Remarks = commodityTransaction.Remarks;
                }

                db.SaveChanges();
                Success();
                return RedirectToAction("FlourMillDispatch");
            }

            PopulateFlourMillDropDowns(commodityTransaction);
            return View(commodityTransaction);
        } 

        #endregion

        #region DEPOT
        
        public ActionResult Depot(CommodityTransactionType? Type = null, int? CommodityTypeId = null, int? FromPlaceId = null,
                                          int? ToPlaceId = null, int? ContractorId = null, DateTime? DispatchDateStart = null, DateTime? DispatchDateEnd = null,
                                          DateTime? ReceiveDateStart = null, DateTime? ReceiveDateEnd = null, int page = 1, string sortBy = null, bool ascending = true)
        {
            var query = db.CommodityTransactions.Include(c => c.FromPlace).Include(c => c.ToPlace).Include(c => c.CommodityType).Include(c => c.Contractor).Include(c => c.IssuingOfficer).Include(c => c.ReceivingOfficer).Include(c => c.Unit);
            query = query.Where(o => o.ToPlace.Type == PlaceType.Depot || o.FromPlace.Type == PlaceType.Depot);

            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
            {
                var depotPlaceIds = db.Depots.Where(o => o.DistrictOfficeId == districtOfficeId).Select(o => o.PlaceId);
                query = query.Where(o => depotPlaceIds.Any(a => a == o.ToPlaceId));
            }

            var list = GetSortableList(query, Type, CommodityTypeId, FromPlaceId, ToPlaceId, ContractorId, DispatchDateStart, DispatchDateEnd,
                                       ReceiveDateStart, ReceiveDateEnd, page, sortBy, ascending);
            return View(list);
        }

        public ActionResult DepotReceive(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommodityTransaction commodityTransaction = db.CommodityTransactions.Find(id);
            if (commodityTransaction == null)
            {
                return HttpNotFound();
            }
            
            var flourMills = db.FlourMills.Select(g => g.PlaceId);
            var fromPlaces = db.Places.Where(o => flourMills.Any(a => a == o.PlaceId));

            var depots = db.Depots.Select(g => g.PlaceId);
            var toPlaces = db.Places.Where(o => depots.Any(a => a == o.PlaceId));

            if (commodityTransaction.ReceiveDate == null)
                commodityTransaction.ReceiveDate = DateTime.Now;

            if (commodityTransaction.BagsReceived == 0)
                commodityTransaction.BagsReceived = commodityTransaction.Bags;

            if (commodityTransaction.QuantityReceived == 0)
                commodityTransaction.QuantityReceived = commodityTransaction.Quantity;

            PopulateDropDowns(commodityTransaction, fromPlaces, toPlaces, null, true);
            return View(commodityTransaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DepotReceive(CommodityTransaction commodityTransaction)
        {
            //if (ModelState.IsValid)
            {
                var received = db.CommodityTransactions.FirstOrDefault(o => o.CommodityTransactionId == commodityTransaction.CommodityTransactionId);

                if (received != null)
                {
                    received.ReceiveDate = commodityTransaction.ReceiveDate;
                    received.ReceivingOfficerId = commodityTransaction.ReceivingOfficerId;
                    received.QuantityReceived = commodityTransaction.QuantityReceived;
                    received.BagsReceived = commodityTransaction.BagsReceived;
                    received.ShortageTypeId = commodityTransaction.ShortageTypeId;
                    received.ReasonsForShortage = commodityTransaction.ReasonsForShortage;
                    received.Remarks = commodityTransaction.Remarks;
                    received.Status = CommodityTransactionStatus.Received;

                    db.SaveChanges();
                }

                Success();
                return RedirectToAction("DepotReceive", new { id = commodityTransaction.CommodityTransactionId });  // Depot
            }

            //return View(commodityTransaction);
        }

        #endregion

        #region DEALER

        public ActionResult Dealer()
        {
            var commodityTransactions = db.CommodityTransactions.Include(c => c.FromPlace).Include(c => c.ToPlace).Include(c => c.CommodityType).Include(c => c.Contractor).Include(c => c.IssuingOfficer).Include(c => c.ReceivingOfficer).Include(c => c.Unit);
            commodityTransactions = commodityTransactions.Where(o => o.ToPlace.Type == PlaceType.Dealer);

            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
            {
                var dealers = db.Dealers.Where(o => o.Depot.DistrictOfficeId == districtOfficeId);
                commodityTransactions = commodityTransactions.Where(o => dealers.Any(a => a.PlaceId == o.ToPlaceId));
            }

            return View(commodityTransactions.ToList());
        } 

        #endregion

        //// DEFAULT ACTIONS
        //// GET: CommodityTransaction
        //public ActionResult Index()
        //{
        //    var commodityTransactions = db.CommodityTransactions.Include(c => c.FromPlace).Include(c => c.ToPlace).Include(c => c.CommodityType).Include(c => c.Contractor).Include(c => c.IssuingOfficer).Include(c => c.ReceivingOfficer).Include(c => c.Unit);
        //    return View(commodityTransactions.ToList());
        //}

        //// GET: CommodityTransaction/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CommodityTransaction commodityTransaction = db.CommodityTransactions.Find(id);
        //    if (commodityTransaction == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(commodityTransaction);
        //}

        //// GET: CommodityTransaction/Create
        //public ActionResult Create()
        //{
        //    var commodityTransaction = new CommodityTransaction();
        //    commodityTransaction.DispatchDate = DateTime.Now;

        //    PopulateDropDowns(commodityTransaction);

        //    return View(commodityTransaction);
        //}

        //// POST: CommodityTransaction/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(CommodityTransaction commodityTransaction)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.CommodityTransactions.Add(commodityTransaction);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(commodityTransaction);
        //}

        //// GET: CommodityTransaction/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CommodityTransaction commodityTransaction = db.CommodityTransactions.Find(id);
        //    if (commodityTransaction == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    PopulateDropDowns(commodityTransaction);
        //    return View(commodityTransaction);
        //}

        //// POST: CommodityTransaction/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(CommodityTransaction commodityTransaction)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(commodityTransaction).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    PopulateDropDowns(commodityTransaction);
        //    return View(commodityTransaction);
        //}

        //// GET: CommodityTransaction/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CommodityTransaction commodityTransaction = db.CommodityTransactions.Find(id);
        //    if (commodityTransaction == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(commodityTransaction);
        //}

        //// POST: CommodityTransaction/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    CommodityTransaction commodityTransaction = db.CommodityTransactions.Find(id);
        //    db.CommodityTransactions.Remove(commodityTransaction);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
    }
}
