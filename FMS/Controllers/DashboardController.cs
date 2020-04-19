using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using FMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace FMS.Controllers
{
    public class DashboardController : BaseController
    {
        private void CreateCharts(IQueryable<int> placeIds, string fromPlace, string toPlace, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            if (StartDate == null)
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (EndDate == null)
                EndDate = DateTime.Now;

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            var received = db.CommodityTransactions.Include(o => o.Unit).Include(o => o.FromPlace).Include(o => o.ToPlace).Where(o => placeIds.Contains(o.ToPlaceId.Value) && o.ReceiveDate >= StartDate && o.ReceiveDate <= EndDate);
            var dispatched = db.CommodityTransactions.Include(o => o.Unit).Include(o => o.FromPlace).Include(o => o.ToPlace).Where(o => placeIds.Contains(o.FromPlaceId.Value) && o.DispatchDate >= StartDate && o.DispatchDate <= EndDate);

            var groupByReceived = received.GroupBy(o => new { o.FromPlaceId }).Select(s => new
            {
                Place = s.FirstOrDefault().FromPlace.Name,
                Bags = s.Sum(c => c.BagsReceived),
                Quantity = s.Sum(c => c.QuantityReceived * c.Unit.ConversionFactor)
            }).ToList();

            var groupByDispatched = dispatched.GroupBy(o => new { o.ToPlaceId }).Select(s => new
            {
                Place = s.FirstOrDefault().ToPlace.Name,
                Bags = s.Sum(c => c.Bags),
                Quantity = s.Sum(c => c.Quantity * c.Unit.ConversionFactor)
            }).ToList();

            var placesReceived = groupByReceived.Select(o => o.Place).ToArray();
            var bagsReceived = groupByReceived.Select(o => o.Bags).Cast<object>().ToArray();
            var quantityReceived = groupByReceived.Select(o => o.Quantity).Cast<object>().ToArray();

            var placesDispatched = groupByDispatched.Select(o => o.Place).ToArray();
            var bagsDispatched = groupByDispatched.Select(o => o.Bags).Cast<object>().ToArray();
            var quantityDispatched = groupByDispatched.Select(o => o.Quantity).Cast<object>().ToArray();

            ViewBag.BagsReceived = new Highcharts("BagsReceived")
                                .InitChart(new Chart() { DefaultSeriesType = ChartTypes.Column })
                                .SetTitle(new Title() { Text = string.Format("Bags Received from {0}", fromPlace) })
                                .SetXAxis(new XAxis { Title = new XAxisTitle { Text = fromPlace }, Categories = placesReceived, })
                                .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Bags Received" }, Labels = new YAxisLabels { Format = "{value:,.0f}" } })
                                .SetSeries(new[] { new Series { Name = "Bags Received", Data = new Data(bagsReceived) } })
                                .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { ColorByPoint = true } });

            ViewBag.QuantityReceived = new Highcharts("QuantityReceived")
                                .InitChart(new Chart() { DefaultSeriesType = ChartTypes.Column })
                                .SetTitle(new Title() { Text = string.Format("Quantity Received from {0} (kg)", fromPlace) })
                                .SetXAxis(new XAxis { Title = new XAxisTitle { Text = fromPlace }, Categories = placesReceived })
                                .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Quantity Received (kg)" }, Labels = new YAxisLabels { Format = "{value:,.0f}" } })
                                .SetSeries(new[] { new Series { Name = "Quantity Received", Data = new Data(quantityReceived) } })
                                .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { ColorByPoint = true } });

            ViewBag.BagsDispatched = new Highcharts("BagsDispatched")
                                .InitChart(new Chart() { DefaultSeriesType = ChartTypes.Column })
                                .SetTitle(new Title() { Text = string.Format("Bags Dispatched to {0}", toPlace) })
                                .SetXAxis(new XAxis { Title = new XAxisTitle { Text = toPlace }, Categories = placesDispatched, })
                                .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Bags Dispatched" }, Labels = new YAxisLabels { Format = "{value:,.0f}" } })
                                .SetSeries(new[] { new Series { Name = "Bags Dispatched", Data = new Data(bagsDispatched) } })
                                .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { ColorByPoint = true } });

            ViewBag.QuantityDispatched = new Highcharts("QuantityDispatched")
                                .InitChart(new Chart() { DefaultSeriesType = ChartTypes.Column })
                                .SetTitle(new Title() { Text = string.Format("Quantity Dispatched to {0} (kg)", toPlace) })
                                .SetXAxis(new XAxis { Title = new XAxisTitle { Text = toPlace }, Categories = placesDispatched })
                                .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Quantity Dispatched (kg)" }, Labels = new YAxisLabels { Format = "{value:,.0f}" } })
                                .SetSeries(new[] { new Series { Name = "Quantity Dispatched", Data = new Data(quantityDispatched) } })
                                .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { ColorByPoint = true } });
        }

        private void GetDirectorate(int? StorageOfficeId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            var placeIds = db.Godowns.Where(o => o.Type == GodownType.StorageOffice).Select(o => o.PlaceId);

            if (StorageOfficeId != null)
                placeIds = db.Godowns.Where(o => o.StorageOfficeId == StorageOfficeId).Select(o => o.PlaceId);

            CreateCharts(placeIds, "PASSCO Centers", "Flour Mills", StartDate, EndDate);
            ViewBag.StorageOfficeId = new SelectList(db.StorageOffices.Include(o => o.Place), "StorageOfficeId", "Place.Name", StorageOfficeId);
        }

        private void GetFlourMill(int? FlourMillId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            int? districtOfficeId = CurrentUser.PlaceId;

            var flourMills = db.FlourMills.AsQueryable();

            if (districtOfficeId != null)
                flourMills = flourMills.Where(o => o.DistrictOfficeId == districtOfficeId);

            var placeIds = db.FlourMills.Select(o => o.PlaceId);

            if (FlourMillId != null)
                placeIds = flourMills.Where(o => o.FlourMillId == FlourMillId).Select(o => o.PlaceId);

            CreateCharts(placeIds, "Storage Offices", "Depots", StartDate, EndDate);
            
            ViewBag.FlourMillId = new SelectList(flourMills.Include(o => o.Place), "FlourMillId", "Place.Name", FlourMillId);
        }

        private void GetDepot(int? DepotId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            int? districtOfficeId = CurrentUser.PlaceId;

            var depots = db.Depots.AsQueryable();

            if (districtOfficeId != null)
                depots = depots.Where(o => o.DistrictOfficeId == districtOfficeId);

            var placeIds = db.Depots.Select(o => o.PlaceId);

            if (DepotId != null)
                placeIds = depots.Where(o => o.DepotId == DepotId).Select(o => o.PlaceId);

            CreateCharts(placeIds, "Flour Mills", "Dealers", StartDate, EndDate);
            
            ViewBag.DepotId = new SelectList(depots.Include(o => o.Place), "DepotId", "Place.Name", DepotId);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Directorate()
        {
            GetDirectorate();
            return View();
        }

        [HttpPost]
        public ActionResult Directorate(int? StorageOfficeId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            GetDirectorate(StorageOfficeId, StartDate, EndDate);
            return View();
        }

        public ActionResult FlourMill()
        {
            GetFlourMill();
            return View();
        }

        [HttpPost]
        public ActionResult FlourMill(int? FlourMillId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            GetFlourMill(FlourMillId, StartDate, EndDate);
            return View();
        }

        public ActionResult Depot()
        {
            GetDepot();
            return View();
        }

        [HttpPost]
        public ActionResult Depot(int? DepotId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            GetDepot(DepotId, StartDate, EndDate);
            return View();
        }
    }
}