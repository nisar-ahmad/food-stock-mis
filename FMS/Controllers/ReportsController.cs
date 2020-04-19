using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using FMS.Models;

namespace FMS.Controllers
{
    public class ReportsController : BaseController
    {
        //private ActionResult GetDivisionByDistrictCash(int? DivisionalOfficeId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        //{
        //    if (StartDate == null)
        //        StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        //    if (EndDate == null)
        //        EndDate = DateTime.Now;

        //    var depots = db.DistrictOffices.Where(o => o.DivisionalOfficeId == DivisionalOfficeId).Select(o => o.PlaceId);

        //    var vm = new CashReport();

        //    var sold = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.ToPlace).Where(o => depots.Contains(o.FromPlaceId.Value)
        //                && o.ToPlace.Type == PlaceType.Dealer && o.DispatchDate >= StartDate && o.DispatchDate <= EndDate);

        //    var soldGroup = sold.GroupBy(o => new { Place = o.FromPlace.Name, Commodity = o.CommodityType.Name }).Select(s => new
        //    {
        //        Place = s.Key.Place,
        //        Commodity = s.Key.Commodity,
        //        Bags = s.Sum(o => o.Bags),
        //        Quantity = s.Sum(o => o.Quantity * o.Unit.ConversionFactor),
        //        AmountReleased = s.Sum(o => o.Bags * (o.Rate ?? 0)),
        //        RateMin = s.Min(o => o.Rate ?? 0),
        //        RateMax = s.Max(o => o.Rate ?? 0)
        //    });


        //    var cashDeposited = db.CashTransactions.Where(o => depots.Contains(o.ToPlaceId.Value)
        //                        && o.FromPlace.Type == PlaceType.Dealer && o.TransactionDate >= StartDate && o.TransactionDate <= EndDate);


        //    var cashGroup = cashDeposited.GroupBy(o => new { Place = o.ToPlace.Name }).Select(s => new
        //    {
        //        Place = s.Key.Place,
        //        Bank = s.FirstOrDefault().BankAccount.Bank.BankName,
        //        Amount = s.Sum(o => o.Amount)
        //    }).ToList();

        //    foreach (var i in soldGroup)
        //    {
        //        if (!vm.CashDetail.ContainsKey(i.Place))
        //            vm.CashDetail[i.Place] = new PlaceCash();

        //        var placeCash = vm.CashDetail[i.Place];
        //        var commoditySold = new CommoditySold();

        //        commoditySold.Bags = i.Bags;
        //        commoditySold.Quantity = Convert.ToInt32(i.Quantity);
        //        commoditySold.RatePerBag = i.RateMin == i.RateMax ? i.RateMax.ToString() : string.Format("{0}-{1}", i.RateMin, i.RateMax);
        //        commoditySold.AmountReleased = i.AmountReleased;

        //        var cash = cashGroup.Find(o => o.Place == i.Place);

        //        if (cash != null)
        //        {
        //            placeCash.Bank = cash.Bank;
        //            placeCash.AmountDeposited = cash.Amount;
        //        }

        //        placeCash.CommoditiesSold.Add(i.Commodity, commoditySold);
        //    }

        //    ViewBag.StartDate = StartDate;
        //    ViewBag.EndDate = EndDate;

        //    ViewBag.DistrictOfficeId = new SelectList(db.DistrictOffices.Include(o => o.Place), "DistrictOfficeId", "Place.Name", DistrictOfficeId);

        //    return View(vm);
        //}
      
        #region DISTRICT

        private ActionResult GetDistrictByDepot(int? DistrictOfficeId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            var districtOffices = db.DistrictOffices.Include(o => o.Place);
            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
            {
                districtOffices = districtOffices.Where(o => o.DistrictOfficeId == districtOfficeId);
                DistrictOfficeId = districtOfficeId.Value;
            }

            if (StartDate == null)
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (EndDate == null)
                EndDate = DateTime.Now;

            var depots = db.Depots.Where(o => o.DistrictOfficeId == DistrictOfficeId).Select(o => o.PlaceId);

            var received = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.FromPlace).Where(o => depots.Contains(o.ToPlaceId.Value) && o.ReceiveDate >= StartDate && o.ReceiveDate <= EndDate);
            var dispatched = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.ToPlace).Where(o => depots.Contains(o.FromPlaceId.Value) && o.DispatchDate >= StartDate && o.DispatchDate <= EndDate);

            var receivedBefore = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.FromPlace).Where(o => depots.Contains(o.ToPlaceId.Value) && o.ReceiveDate < StartDate);
            var dispatchedBefore = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.ToPlace).Where(o => depots.Contains(o.FromPlaceId.Value) && o.DispatchDate < StartDate);

            var vm = new DistrictByDepot();

            var groupReceived = received.GroupBy(o => new { Place = o.ToPlace.Name, Commodity = o.CommodityType.Name }).Select(s => new
            {
                Place = s.Key.Place,
                Commodity = s.Key.Commodity,
                Bags = s.Sum(o => o.BagsReceived),
                Quantity = s.Sum(o => o.QuantityReceived * o.Unit.ConversionFactor)
            });

            var groupRecvBefore = receivedBefore.GroupBy(o => new { Place = o.ToPlace.Name, Commodity = o.CommodityType.Name }).Select(s => new
            {
                Place = s.Key.Place,
                Commodity = s.Key.Commodity,
                Bags = s.Sum(o => o.BagsReceived),
                Quantity = s.Sum(o => o.QuantityReceived * o.Unit.ConversionFactor)
            });

            var groupDispatched = dispatched.GroupBy(o => new { Place = o.FromPlace.Name, Commodity = o.CommodityType.Name }).Select(s => new
            {
                Place = s.Key.Place,
                Commodity = s.Key.Commodity,
                Bags = s.Sum(o => o.Bags),
                Quantity = s.Sum(o => o.Quantity * o.Unit.ConversionFactor)
            });

            var groupDispBefore = dispatchedBefore.GroupBy(o => new { Place = o.FromPlace.Name, Commodity = o.CommodityType.Name }).Select(s => new
            {
                Place = s.Key.Place,
                Commodity = s.Key.Commodity,
                Bags = s.Sum(o => o.Bags),
                Quantity = s.Sum(o => o.Quantity * o.Unit.ConversionFactor)
            });

            foreach (var i in groupReceived)
            {
                if (!vm.DepotSummary.ContainsKey(i.Place))
                    vm.DepotSummary[i.Place] = new PlaceSummary();

                var list = vm.DepotSummary[i.Place];
                var item = new SummaryItem();

                item.Commodity = i.Commodity;
                item.ReceivedBags = i.Bags;
                item.ReceivedQuantity = Convert.ToInt32(i.Quantity);

                list.CommoditySummary.Add(i.Commodity, item);
            }

            foreach (var i in groupDispatched)
            {
                if (!vm.DepotSummary.ContainsKey(i.Place))
                    vm.DepotSummary[i.Place] = new PlaceSummary();

                var list = vm.DepotSummary[i.Place];

                if (!list.CommoditySummary.ContainsKey(i.Commodity))
                    list.CommoditySummary[i.Commodity] = new SummaryItem();

                var item = list.CommoditySummary[i.Commodity];

                item.Commodity = i.Commodity;
                item.DispatchedBags = i.Bags;
                item.DispatchedQuantity = Convert.ToInt32(i.Quantity);
            }

            foreach (var depot in vm.DepotSummary.Keys)
            {
                var depotSummary = vm.DepotSummary[depot];

                foreach (var commodity in depotSummary.CommoditySummary.Keys)
                {
                    var summary = depotSummary.CommoditySummary[commodity];

                    var recv = groupRecvBefore.Where(o => o.Place == depot && o.Commodity == commodity);
                    var disp = groupDispBefore.Where(o => o.Place == depot && o.Commodity == commodity);
                    var damg = db.DepotDamages.Where(o => o.Depot.Place.Name == depot && o.CommodityType.Name == commodity && o.DamageDate >= StartDate && o.DamageDate <= EndDate);

                    summary.OpeningBags = recv.Select(o => o.Bags).DefaultIfEmpty(0).Sum() - disp.Select(o => o.Bags).DefaultIfEmpty(0).Sum();
                    summary.OpeningQuantity = Convert.ToInt32(recv.Select(o => o.Quantity).DefaultIfEmpty(0).Sum() - disp.Select(o => o.Quantity).DefaultIfEmpty(0).Sum());

                    summary.ShortageBags = damg.Select(o => o.Bags).DefaultIfEmpty(0).Sum();
                    summary.ShortageQuantity = damg.Select(o => o.Quantity).DefaultIfEmpty(0).Sum();
                }
            }

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            ViewBag.DistrictOfficeId = new SelectList(districtOffices, "DistrictOfficeId", "Place.Name", DistrictOfficeId);

            return View(vm);
        }

        private ActionResult GetDistrictByDepotCash(int? DistrictOfficeId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            var districtOffices = db.DistrictOffices.Include(o => o.Place);

            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
            {
                districtOffices = districtOffices.Where(o => o.DistrictOfficeId == districtOfficeId);
                DistrictOfficeId = districtOfficeId.Value;
            }

            if (StartDate == null)
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (EndDate == null)
                EndDate = DateTime.Now;

            var depots = db.Depots.Where(o => o.DistrictOfficeId == DistrictOfficeId).Select(o => o.PlaceId);

            var vm = new CashReport();

            var sold = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.ToPlace).Where(o => depots.Contains(o.FromPlaceId.Value)
                        && o.ToPlace.Type == PlaceType.Dealer && o.DispatchDate >= StartDate && o.DispatchDate <= EndDate);

            var soldGroup = sold.GroupBy(o => new { Place = o.FromPlace.Name, Commodity = o.CommodityType.Name }).Select(s => new
            {
                Place = s.Key.Place,
                Commodity = s.Key.Commodity,
                Bags = s.Sum(o => o.Bags),
                Quantity = s.Sum(o => o.Quantity * o.Unit.ConversionFactor),
                AmountReleased = s.Sum(o => o.Bags * (o.Rate ?? 0)),
                RateMin = s.Min(o => o.Rate ?? 0),
                RateMax = s.Max(o => o.Rate ?? 0)
            });


            var cashDeposited = db.CashTransactions.Where(o => depots.Contains(o.ToPlaceId.Value)
                                && o.FromPlace.Type == PlaceType.Dealer && o.TransactionDate >= StartDate && o.TransactionDate <= EndDate);


            var cashGroup = cashDeposited.GroupBy(o => new { Place = o.ToPlace.Name }).Select(s => new
            {
                Place = s.Key.Place,
                Bank = s.FirstOrDefault().BankAccount.Bank.BankName,
                Amount = s.Sum(o => o.Amount)
            }).ToList();

            foreach (var i in soldGroup)
            {
                if (!vm.CashDetail.ContainsKey(i.Place))
                    vm.CashDetail[i.Place] = new PlaceCash();

                var placeCash = vm.CashDetail[i.Place];
                var commoditySold = new CommoditySold();

                commoditySold.Bags = i.Bags;
                commoditySold.Quantity = Convert.ToInt32(i.Quantity);
                commoditySold.RatePerBag = i.RateMin == i.RateMax ? i.RateMax.ToString() : string.Format("{0}-{1}", i.RateMin, i.RateMax);
                commoditySold.AmountReleased = i.AmountReleased;

                var cash = cashGroup.Find(o => o.Place == i.Place);

                if(cash != null)
                {
                    placeCash.Bank = cash.Bank;
                    placeCash.AmountDeposited = cash.Amount;
                }

                placeCash.CommoditiesSold.Add(i.Commodity, commoditySold);
            }

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.DistrictOfficeId = new SelectList(districtOffices, "DistrictOfficeId", "Place.Name", DistrictOfficeId);

            return View(vm);
        }
       
        public ActionResult DistrictByDepot()
        {
            return GetDistrictByDepot();
        }

        [HttpPost]
        public ActionResult DistrictByDepot(int? DistrictOfficeId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            return GetDistrictByDepot(DistrictOfficeId, StartDate, EndDate);
        }

        public ActionResult DistrictByDepotCash()
        {
            return GetDistrictByDepotCash();
        }

        [HttpPost]
        public ActionResult DistrictByDepotCash(int? DistrictOfficeId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            return GetDistrictByDepotCash(DistrictOfficeId, StartDate, EndDate);
        }

        #endregion

        #region FLOUR MILL

        private ActionResult GetFlourMillByVoucher(int? PlaceId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            if (StartDate == null)
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (EndDate == null)
                EndDate = DateTime.Now;

            var received = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.FromPlace).Where(o => o.ToPlaceId == PlaceId && o.ReceiveDate >= StartDate && o.ReceiveDate <= EndDate);
            var dispatched = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.ToPlace).Where(o => o.FromPlaceId == PlaceId && o.DispatchDate >= StartDate && o.DispatchDate <= EndDate);

            var vm = new FlourMillByVoucher();

            foreach (var i in received)
            {
                vm.VoucherDetail.ReceivedItems.Add(new VoucherItem(i.ReceiveDate.Value, string.Format("{0}/{1}", i.BookNumber, i.VoucherNo),
                                                                   i.FromPlace.Name, i.CommodityType.Name, i.BagsReceived, Convert.ToInt32(i.QuantityReceived * i.Unit.ConversionFactor)));
            }

            foreach (var i in dispatched)
            {
                vm.VoucherDetail.DispatchedItems.Add(new VoucherItem(i.DispatchDate, string.Format("{0}/{1}", i.BookNumber, i.VoucherNo),
                                                                   i.ToPlace.Name, i.CommodityType.Name, i.Bags, Convert.ToInt32(i.Quantity * i.Unit.ConversionFactor)));
            }

            var receivedBefore = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.FromPlace).Where(o => o.ToPlaceId == PlaceId && o.ReceiveDate < StartDate);
            var dispatchedBefore = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.ToPlace).Where(o => o.FromPlaceId == PlaceId && o.DispatchDate < StartDate);

            var before = new VoucherDetail();

            foreach (var i in receivedBefore)
            {
                before.ReceivedItems.Add(new VoucherItem(i.ReceiveDate.Value, string.Format("{0}/{1}", i.BookNumber, i.VoucherNo),
                                                                   i.FromPlace.Name, i.CommodityType.Name, i.BagsReceived, Convert.ToInt32(i.QuantityReceived * i.Unit.ConversionFactor)));
            }

            foreach (var i in dispatchedBefore)
            {
                before.DispatchedItems.Add(new VoucherItem(i.DispatchDate, string.Format("{0}/{1}", i.BookNumber, i.VoucherNo),
                                                                   i.ToPlace.Name, i.CommodityType.Name, i.Bags, Convert.ToInt32(i.Quantity * i.Unit.ConversionFactor)));
            }

            var groupDisp = dispatched.GroupBy(o => o.CommodityType.Name).Select(s => new
            {
                Commodity = s.Key,
                Bags = s.Sum(o => o.Bags),
                Quantity = s.Sum(o => o.Quantity * o.Unit.ConversionFactor)
            });

            foreach (var i in groupDisp)
            {
                vm.Summary.DispatchedCommodities.Add(new CommodityItem(i.Commodity, i.Bags, Convert.ToInt32(i.Quantity)));
            }

            var beforeChokar = db.FlourMillChokars.Where(o => o.FlourMill.PlaceId == PlaceId && o.ProductionDate < StartDate).Select(o => o.Quantity).DefaultIfEmpty(0).Sum();
            var beforeCrushedBags = db.FlourMillChokars.Where(o => o.FlourMill.PlaceId == PlaceId && o.ProductionDate < StartDate).Select(o => o.Bags).DefaultIfEmpty(0).Sum();

            vm.Summary.OpeningBags = before.GrandTotalBagsReceived - beforeCrushedBags;
            vm.Summary.OpeningQuantity = before.GrandTotalQuantityReceived - (before.GrandTotalQuantityDispatched + beforeChokar);

            vm.Summary.ChokarQuantity = db.FlourMillChokars.Where(o => o.FlourMill.PlaceId == PlaceId && o.ProductionDate >= StartDate && o.ProductionDate <= EndDate).Select(o => o.Quantity).DefaultIfEmpty(0).Sum();
            vm.Summary.CrushedBags = db.FlourMillChokars.Where(o => o.FlourMill.PlaceId == PlaceId && o.ProductionDate >= StartDate && o.ProductionDate <= EndDate).Select(o => o.Bags).DefaultIfEmpty(0).Sum();

            vm.Summary.ReceivedBags = vm.VoucherDetail.GrandTotalBagsReceived;
            vm.Summary.ReceivedQuantity = vm.VoucherDetail.GrandTotalQuantityReceived;

            vm.Summary.BalanceChokar = beforeChokar + vm.Summary.ChokarQuantity;

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            var flourMills = db.FlourMills.Include(o => o.Place);

            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
                flourMills = flourMills.Where(o => o.DistrictOfficeId == districtOfficeId);

            ViewBag.PlaceId = new SelectList(flourMills, "PlaceId", "Place.Name", PlaceId);

            return View(vm);
        }

        public ActionResult FlourMillByVoucher()
        {
            //var id = db.FlourMills.Select(o => o.PlaceId).FirstOrDefault();
            return GetFlourMillByVoucher();
        }

        [HttpPost]
        public ActionResult FlourMillByVoucher(int? PlaceId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            return GetFlourMillByVoucher(PlaceId, StartDate, EndDate);
        }

        #endregion

        #region DEPOT

        private ActionResult GetDepotByVoucher(int? PlaceId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            if (StartDate == null)
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (EndDate == null)
                EndDate = DateTime.Now;

            var received = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.FromPlace).Where(o => o.ToPlaceId == PlaceId && o.ReceiveDate >= StartDate && o.ReceiveDate <= EndDate);
            var dispatched = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.ToPlace).Where(o => o.FromPlaceId == PlaceId && o.DispatchDate >= StartDate && o.DispatchDate <= EndDate);

            var vm = new DepotByVoucher();

            foreach (var i in received)
            {
                vm.VoucherDetail.ReceivedItems.Add(new VoucherItem(i.ReceiveDate.Value, string.Format("{0}/{1}", i.BookNumber, i.VoucherNo),
                                                                   i.FromPlace.Name, i.CommodityType.Name, i.BagsReceived, Convert.ToInt32(i.QuantityReceived * i.Unit.ConversionFactor)));
            }

            foreach (var i in dispatched)
            {
                vm.VoucherDetail.DispatchedItems.Add(new VoucherItem(i.DispatchDate, string.Format("{0}/{1}", i.BookNumber, i.VoucherNo),
                                                                   i.ToPlace.Name, i.CommodityType.Name, i.Bags, Convert.ToInt32(i.Quantity * i.Unit.ConversionFactor)));
            }

            var receivedBefore = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.FromPlace).Where(o => o.ToPlaceId == PlaceId && o.ReceiveDate < StartDate);
            var dispatchedBefore = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.ToPlace).Where(o => o.FromPlaceId == PlaceId && o.DispatchDate < StartDate);

            var before = new VoucherDetail();

            foreach (var i in receivedBefore)
            {
                before.ReceivedItems.Add(new VoucherItem(i.ReceiveDate.Value, string.Format("{0}/{1}", i.BookNumber, i.VoucherNo),
                                                                   i.FromPlace.Name, i.CommodityType.Name, i.BagsReceived, Convert.ToInt32(i.QuantityReceived * i.Unit.ConversionFactor)));
            }

            foreach (var i in dispatchedBefore)
            {
                before.DispatchedItems.Add(new VoucherItem(i.DispatchDate, string.Format("{0}/{1}", i.BookNumber, i.VoucherNo),
                                                                   i.ToPlace.Name, i.CommodityType.Name, i.Bags, Convert.ToInt32(i.Quantity * i.Unit.ConversionFactor)));
            }

            foreach (var commodity in before.TotalReceived.Keys)
                vm.SummaryItems[commodity] = new SummaryItem();

            foreach (var commodity in before.TotalDispatched.Keys)
                vm.SummaryItems[commodity] = new SummaryItem();

            foreach (var commodity in vm.VoucherDetail.TotalReceived.Keys)
                vm.SummaryItems[commodity] = new SummaryItem();

            foreach (var commodity in vm.VoucherDetail.TotalDispatched.Keys)
                vm.SummaryItems[commodity] = new SummaryItem();

            foreach (var si in vm.SummaryItems)
            {
                si.Value.Commodity = si.Key;

                if (before.TotalReceived.ContainsKey(si.Key))
                {
                    si.Value.OpeningBags = before.TotalReceived[si.Key].Bags;
                    si.Value.OpeningQuantity = before.TotalReceived[si.Key].Quantity;
                }

                if (before.TotalDispatched.ContainsKey(si.Key))
                {
                    si.Value.OpeningBags -= before.TotalDispatched[si.Key].Bags;
                    si.Value.OpeningQuantity -= before.TotalDispatched[si.Key].Quantity;
                }

                if (vm.VoucherDetail.TotalReceived.ContainsKey(si.Key))
                {
                    si.Value.ReceivedBags = vm.VoucherDetail.TotalReceived[si.Key].Bags;
                    si.Value.ReceivedQuantity = vm.VoucherDetail.TotalReceived[si.Key].Quantity;
                }

                if (vm.VoucherDetail.TotalDispatched.ContainsKey(si.Key))
                {
                    si.Value.DispatchedBags = vm.VoucherDetail.TotalDispatched[si.Key].Bags;
                    si.Value.DispatchedQuantity = vm.VoucherDetail.TotalDispatched[si.Key].Quantity;
                }
            }

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            var depots = db.Depots.Include(o => o.Place);
            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
                depots = depots.Where(o => o.DistrictOfficeId == districtOfficeId);

            ViewBag.PlaceId = new SelectList(depots, "PlaceId", "Place.Name", PlaceId);

            return View(vm);
        }

        private ActionResult GetDepotByDealer(int? PlaceId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            if (StartDate == null)
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (EndDate == null)
                EndDate = DateTime.Now;

            var received = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.FromPlace).Where(o => o.ToPlaceId == PlaceId && o.ReceiveDate >= StartDate && o.ReceiveDate <= EndDate);
            var dispatched = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.ToPlace).Where(o => o.FromPlaceId == PlaceId && o.DispatchDate >= StartDate && o.DispatchDate <= EndDate);

            var vm = new DepotByDealer();

            var groupRecv = received.GroupBy(o => o.FromPlace.Name).Select(s => new
            {
                Place = s.Key,
                Commodity = s.FirstOrDefault().CommodityType.Name,
                Bags = s.Sum(o => o.BagsReceived),
                Quantity = s.Sum(o => o.QuantityReceived * o.Unit.ConversionFactor)
            });

            var groupDisp = dispatched.GroupBy(o => o.ToPlace.Name).Select(s => new
            {
                Place = s.Key,
                Commodity = s.FirstOrDefault().CommodityType.Name,
                Bags = s.Sum(o => o.Bags),
                Quantity = s.Sum(o => o.Quantity * o.Unit.ConversionFactor)
            });

            var prevPlace = "";
            CommoditySummary summary = null;

            foreach (var i in groupRecv)
            {
                if (prevPlace != i.Place)
                {
                    summary = new CommoditySummary() { PlaceName = i.Place };
                    vm.CommodityDetail.ReceivedItems.Add(summary);
                }

                summary.Items.Add(new CommodityItem(i.Commodity, i.Bags, Convert.ToInt32(i.Quantity)));

                prevPlace = i.Place;
            }

            prevPlace = "";
            summary = null;

            foreach (var i in groupDisp)
            {
                if (prevPlace != i.Place)
                {
                    summary = new CommoditySummary() { PlaceName = i.Place };
                    vm.CommodityDetail.DispatchedItems.Add(summary);
                }

                summary.Items.Add(new CommodityItem(i.Commodity, i.Bags, Convert.ToInt32(i.Quantity)));

                prevPlace = i.Place;
            }

            var receivedBefore = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.FromPlace).Where(o => o.ToPlaceId == PlaceId && o.ReceiveDate < StartDate);
            var dispatchedBefore = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.ToPlace).Where(o => o.FromPlaceId == PlaceId && o.DispatchDate < StartDate);

            var before = new VoucherDetail();

            foreach (var i in receivedBefore)
            {
                before.ReceivedItems.Add(new VoucherItem(i.ReceiveDate.Value, string.Format("{0}/{1}", i.BookNumber, i.VoucherNo),
                                                                   i.FromPlace.Name, i.CommodityType.Name, i.BagsReceived, Convert.ToInt32(i.QuantityReceived * i.Unit.ConversionFactor)));
            }

            foreach (var i in dispatchedBefore)
            {
                before.DispatchedItems.Add(new VoucherItem(i.DispatchDate, string.Format("{0}/{1}", i.BookNumber, i.VoucherNo),
                                                                   i.ToPlace.Name, i.CommodityType.Name, i.Bags, Convert.ToInt32(i.Quantity * i.Unit.ConversionFactor)));
            }

            foreach (var commodity in before.TotalReceived.Keys)
                vm.SummaryItems[commodity] = new SummaryItem();

            foreach (var commodity in before.TotalDispatched.Keys)
                vm.SummaryItems[commodity] = new SummaryItem();

            foreach (var commodity in vm.CommodityDetail.TotalReceived.Keys)
                vm.SummaryItems[commodity] = new SummaryItem();

            foreach (var commodity in vm.CommodityDetail.TotalDispatched.Keys)
                vm.SummaryItems[commodity] = new SummaryItem();

            foreach (var si in vm.SummaryItems)
            {
                si.Value.Commodity = si.Key;

                if (before.TotalReceived.ContainsKey(si.Key))
                {
                    si.Value.OpeningBags = before.TotalReceived[si.Key].Bags;
                    si.Value.OpeningQuantity = before.TotalReceived[si.Key].Quantity;
                }

                if (before.TotalDispatched.ContainsKey(si.Key))
                {
                    si.Value.OpeningBags -= before.TotalDispatched[si.Key].Bags;
                    si.Value.OpeningQuantity -= before.TotalDispatched[si.Key].Quantity;
                }

                if (vm.CommodityDetail.TotalReceived.ContainsKey(si.Key))
                {
                    si.Value.ReceivedBags = vm.CommodityDetail.TotalReceived[si.Key].Bags;
                    si.Value.ReceivedQuantity = vm.CommodityDetail.TotalReceived[si.Key].Quantity;
                }

                if (vm.CommodityDetail.TotalDispatched.ContainsKey(si.Key))
                {
                    si.Value.DispatchedBags = vm.CommodityDetail.TotalDispatched[si.Key].Bags;
                    si.Value.DispatchedQuantity = vm.CommodityDetail.TotalDispatched[si.Key].Quantity;
                }
            }

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            var depots = db.Depots.Include(o => o.Place);
            int? districtOfficeId = CurrentUser.PlaceId;

            if (districtOfficeId != null)
                depots = depots.Where(o => o.DistrictOfficeId == districtOfficeId);

            ViewBag.PlaceId = new SelectList(depots, "PlaceId", "Place.Name", PlaceId);

            return View(vm);
        }

        public ActionResult DepotByVoucher()
        {
            //var id = db.Depots.Select(o => o.PlaceId).FirstOrDefault();
            return GetDepotByVoucher();
        }

        [HttpPost]
        public ActionResult DepotByVoucher(int? PlaceId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            return GetDepotByVoucher(PlaceId, StartDate, EndDate);
        }

        public ActionResult DepotByDealer()
        {
            //var id = db.Depots.Select(o => o.PlaceId).FirstOrDefault();
            return GetDepotByDealer();
        }

        [HttpPost]
        public ActionResult DepotByDealer(int? PlaceId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            return GetDepotByDealer(PlaceId, StartDate, EndDate);
        }

        #endregion

        #region STORAGE OFFICE

        private ActionResult GetStorageOfficeReport(int? StorageOfficeId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            if (StartDate == null)
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (EndDate == null)
                EndDate = DateTime.Now;

            var dictionary = new Dictionary<string, SOGodownViewModel>();

            var soGodowns = db.Godowns.Where(o => o.Type == GodownType.StorageOffice).Select(o => o.PlaceId);

            if (StorageOfficeId != null)
                soGodowns = db.Godowns.Where(o => o.StorageOfficeId == StorageOfficeId).Select(o => o.PlaceId);

            var soReceived = db.CommodityTransactions.Include(o => o.Unit).Include(o => o.FromPlace).Include(o => o.ToPlace).Where(o => soGodowns.Contains(o.ToPlaceId.Value) && o.ReceiveDate >= StartDate && o.ReceiveDate <= EndDate);
            var soDispatched = db.CommodityTransactions.Include(o => o.Unit).Include(o => o.FromPlace).Include(o => o.ToPlace).Where(o => soGodowns.Contains(o.FromPlaceId.Value) && o.DispatchDate >= StartDate && o.DispatchDate <= EndDate);

            var groupByReceived = soReceived.GroupBy(o => new { o.ToPlaceId, o.FromPlaceId }).Select(s => new
            {
                ToPlaceId = s.FirstOrDefault().ToPlaceId,
                FromPlace = s.FirstOrDefault().FromPlace.Name,
                ToPlace = s.FirstOrDefault().ToPlace.Name,
                BagsReceived = s.Sum(c => c.BagsReceived),
                QuantityReceived = s.Sum(c => c.QuantityReceived * c.Unit.ConversionFactor)
            });

            foreach (var item in groupByReceived)
            {
                var godown = item.ToPlace;
                SOGodownViewModel vm = null;

                if (dictionary.ContainsKey(godown))
                    vm = dictionary[godown];
                else
                {
                    vm = new SOGodownViewModel();
                    vm.PlaceId = item.ToPlaceId.Value;
                    dictionary.Add(godown, vm);
                }

                var r = new QuantityItem();
                r.Name = item.FromPlace;
                r.Bags = item.BagsReceived;
                r.Quantity = Convert.ToInt32(item.QuantityReceived);

                vm.ReceivedItems.Add(r);
            }

            var groupByDispatched = soDispatched.GroupBy(o => new { o.FromPlaceId, o.ToPlaceId }).Select(s => new
            {
                FromPlaceId = s.FirstOrDefault().FromPlaceId,
                FromPlace = s.FirstOrDefault().FromPlace.Name,
                ToPlace = s.FirstOrDefault().ToPlace.Name,
                Bags = s.Sum(c => c.Bags),
                Quantity = s.Sum(c => c.Quantity * c.Unit.ConversionFactor)
            });

            foreach (var item in groupByDispatched)
            {
                var godown = item.FromPlace;
                SOGodownViewModel vm = null;

                if (dictionary.ContainsKey(godown))
                    vm = dictionary[godown];
                else
                {
                    vm = new SOGodownViewModel();
                    vm.PlaceId = item.FromPlaceId.Value;
                    dictionary.Add(godown, vm);
                }

                var r = new QuantityItem();
                r.Name = item.ToPlace;
                r.Bags = item.Bags;
                r.Quantity = Convert.ToInt32(item.Quantity);

                vm.DispatchedItems.Add(r);
            }

            foreach (var vm in dictionary.Values)
            {
                var rec = db.CommodityTransactions.Where(o => o.ToPlaceId == vm.PlaceId && o.ReceiveDate < StartDate);
                var dis = db.CommodityTransactions.Where(o => o.FromPlaceId == vm.PlaceId && o.DispatchDate < StartDate);

                vm.OpeningBags = rec.Select(o => o.BagsReceived).DefaultIfEmpty(0).Sum() - dis.Select(o => o.Bags).DefaultIfEmpty(0).Sum();
                vm.OpeningQuantity = rec.Select(o => o.QuantityReceived).DefaultIfEmpty(0).Sum() - dis.Select(o => o.Quantity).DefaultIfEmpty(0).Sum();
            }

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            ViewBag.StorageOfficeId = new SelectList(db.StorageOffices.Include(o => o.Place), "StorageOfficeId", "Place.Name", StorageOfficeId);
            return View(dictionary);
        } 

        public ActionResult StorageOffice()
        {
            return GetStorageOfficeReport();
        }

        [HttpPost]
        public ActionResult StorageOffice(int? StorageOfficeId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            return GetStorageOfficeReport(StorageOfficeId, StartDate, EndDate);
        }

        #endregion

        #region DAILY STOCK REPORT

        public ActionResult DailyStockReport()
        {
            return GetDailyStockReport();
        }

        [HttpPost]
        public ActionResult DailyStockReport(DateTime? SelectedDate = null)
        {
            return GetDailyStockReport(SelectedDate);
        }

        private ActionResult GetDailyStockReport(DateTime? SelectedDate = null)
        {
            var vm = new DailyStockReport();

            if (SelectedDate == null)
                SelectedDate = DateTime.Today;

            GetPASSCOStockReport(vm, SelectedDate.Value);
            GetStorageOfficeStockReport(vm, SelectedDate.Value);
            GetFlourMillStockReport(vm, SelectedDate.Value);
            GetDepotStockReport(vm, SelectedDate.Value);

            ViewBag.SelectedDate = SelectedDate;

            return View(vm);
        } 

        private void GetDepotStockReport(DailyStockReport vm, DateTime SelectedDate)
        {
            var places = db.Depots.Select(o => o.PlaceId);
            GetStockReport(vm.StockByDepot, places, SelectedDate);

            foreach (var depot in vm.StockByDepot.Keys)
            {
                var depotSummary = vm.StockByDepot[depot];

                foreach (var commodity in depotSummary.CommoditySummary.Keys)
                {
                    var summary = depotSummary.CommoditySummary[commodity];
                    var damg = db.DepotDamages.Where(o => o.Depot.Place.Name == depot && o.CommodityType.Name == commodity && DbFunctions.TruncateTime(o.DamageDate) <= SelectedDate);

                    summary.ShortageBags = damg.Select(o => o.Bags).DefaultIfEmpty(0).Sum();
                    summary.ShortageQuantity = damg.Select(o => o.Quantity).DefaultIfEmpty(0).Sum();
                }
            }
        }

        private void GetStorageOfficeStockReport(DailyStockReport vm, DateTime SelectedDate)
        {
            var places = db.Godowns.Where(o => o.Type == GodownType.StorageOffice).Select(o => o.PlaceId);
            GetStockReport(vm.StockByStorageOffice, places, SelectedDate);
        }

        private void GetPASSCOStockReport(DailyStockReport vm, DateTime SelectedDate)
        {
            var places = db.Godowns.Where(o => o.Type == GodownType.PASSCO).Select(o => o.PlaceId);
            GetStockReport(vm.StockByPASSCOCenter, places, SelectedDate);
        }

        private void GetFlourMillStockReport(DailyStockReport vm, DateTime SelectedDate)
        {
            var places = db.FlourMills.Select(o => o.PlaceId);
            GetStockReport(vm.StockByFlourMill, places, SelectedDate);
        }

        private void GetStockReport(Dictionary<string, PlaceSummary> vm, IQueryable<int> places, DateTime SelectedDate)
        {
            var received = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.FromPlace).Where(o => places.Contains(o.ToPlaceId.Value) && DbFunctions.TruncateTime(o.ReceiveDate) == DbFunctions.TruncateTime(SelectedDate));
            var dispatched = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.ToPlace).Where(o => places.Contains(o.FromPlaceId.Value) && DbFunctions.TruncateTime(o.DispatchDate) == DbFunctions.TruncateTime(SelectedDate));

            var receivedBefore = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.FromPlace).Where(o => places.Contains(o.ToPlaceId.Value) && DbFunctions.TruncateTime(o.ReceiveDate) < DbFunctions.TruncateTime(SelectedDate));
            var dispatchedBefore = db.CommodityTransactions.Include(o => o.CommodityType).Include(o => o.Unit).Include(o => o.ToPlace).Where(o => places.Contains(o.FromPlaceId.Value) && DbFunctions.TruncateTime(o.DispatchDate) < DbFunctions.TruncateTime(SelectedDate));

            var groupReceived = received.GroupBy(o => new { Place = o.ToPlace.Name, Commodity = o.CommodityType.Name }).Select(s => new
            {
                Place = s.Key.Place,
                Commodity = s.Key.Commodity,
                Bags = s.Sum(o => o.BagsReceived),
                Quantity = s.Sum(o => o.QuantityReceived * o.Unit.ConversionFactor)
            });

            var groupRecvBefore = receivedBefore.GroupBy(o => new { Place = o.ToPlace.Name, Commodity = o.CommodityType.Name }).Select(s => new
            {
                Place = s.Key.Place,
                Commodity = s.Key.Commodity,
                Bags = s.Sum(o => o.BagsReceived),
                Quantity = s.Sum(o => o.QuantityReceived * o.Unit.ConversionFactor)
            });

            var groupDispatched = dispatched.GroupBy(o => new { Place = o.FromPlace.Name, Commodity = o.CommodityType.Name }).Select(s => new
            {
                Place = s.Key.Place,
                Commodity = s.Key.Commodity,
                Bags = s.Sum(o => o.Bags),
                Quantity = s.Sum(o => o.Quantity * o.Unit.ConversionFactor)
            });

            var groupDispBefore = dispatchedBefore.GroupBy(o => new { Place = o.FromPlace.Name, Commodity = o.CommodityType.Name }).Select(s => new
            {
                Place = s.Key.Place,
                Commodity = s.Key.Commodity,
                Bags = s.Sum(o => o.Bags),
                Quantity = s.Sum(o => o.Quantity * o.Unit.ConversionFactor)
            });

            foreach (var i in groupReceived)
            {
                if (!vm.ContainsKey(i.Place))
                    vm[i.Place] = new PlaceSummary();

                var list = vm[i.Place];
                var item = new SummaryItem();

                item.Commodity = i.Commodity;
                item.ReceivedBags = i.Bags;
                item.ReceivedQuantity = Convert.ToInt32(i.Quantity);

                list.CommoditySummary.Add(i.Commodity, item);
            }

            foreach (var i in groupDispatched)
            {
                if (!vm.ContainsKey(i.Place))
                    vm[i.Place] = new PlaceSummary();

                var list = vm[i.Place];

                if (!list.CommoditySummary.ContainsKey(i.Commodity))
                    list.CommoditySummary[i.Commodity] = new SummaryItem();

                var item = list.CommoditySummary[i.Commodity];

                item.Commodity = i.Commodity;
                item.DispatchedBags = i.Bags;
                item.DispatchedQuantity = Convert.ToInt32(i.Quantity);
            }

            foreach (var i in groupDispBefore)
            {
                if (!vm.ContainsKey(i.Place))
                    vm[i.Place] = new PlaceSummary();

                var list = vm[i.Place];

                if (!list.CommoditySummary.ContainsKey(i.Commodity))
                    list.CommoditySummary[i.Commodity] = new SummaryItem();

                var item = list.CommoditySummary[i.Commodity];
                item.Commodity = i.Commodity;
            }

            foreach (var i in groupRecvBefore)
            {
                if (!vm.ContainsKey(i.Place))
                    vm[i.Place] = new PlaceSummary();

                var list = vm[i.Place];

                if (!list.CommoditySummary.ContainsKey(i.Commodity))
                    list.CommoditySummary[i.Commodity] = new SummaryItem();

                var item = list.CommoditySummary[i.Commodity];
                item.Commodity = i.Commodity;
            }

            foreach (var place in vm.Keys)
            {
                var placeSummary = vm[place];

                foreach (var commodity in placeSummary.CommoditySummary.Keys)
                {
                    var summary = placeSummary.CommoditySummary[commodity];

                    var recv = groupRecvBefore.Where(o => o.Place == place && o.Commodity == commodity);
                    var disp = groupDispBefore.Where(o => o.Place == place && o.Commodity == commodity);

                    summary.OpeningBags = recv.Select(o => o.Bags).DefaultIfEmpty(0).Sum() - disp.Select(o => o.Bags).DefaultIfEmpty(0).Sum();
                    summary.OpeningQuantity = Convert.ToInt32(recv.Select(o => o.Quantity).DefaultIfEmpty(0).Sum() - disp.Select(o => o.Quantity).DefaultIfEmpty(0).Sum());
                }
            }
        }

        #endregion

        public ActionResult Index()
        {
            return View();
        }
    }
}