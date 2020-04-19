using FMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace FMS.Controllers
{
    public static class Alerts
    {
        public const string SUCCESS = "success";
        public const string ATTENTION = "attention";
        public const string ERROR = "error";
        public const string INFORMATION = "info";

        public static string[] ALL
        {
            get { return new[] { SUCCESS, ATTENTION, INFORMATION, ERROR }; }
        }
    }

    [Authorize]
    public class BaseController : Controller
    {
        protected FMSDbContext db = new FMSDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUser CurrentUser
        {
            get
            {
                if (_userManager == null)
                    _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                return _userManager.FindByName(HttpContext.User.Identity.Name);
            }
        }

        protected virtual void PopulateDropDowns(Place place)
        {
            ViewBag.ProvinceId = new SelectList(db.Provinces, "ProvinceId", "Name", place.ProvinceId);
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name", place.DistrictId);
            ViewBag.TehsilId = new SelectList(db.Tehsils, "TehsilId", "Name", place.TehsilId);
            ViewBag.UnionCouncilId = new SelectList(db.UnionCouncils, "UnionCouncilId", "Name", place.UnionCouncilId);
        }

        protected virtual void AddPlace(Place place, PlaceType placeType, PersonType personType = PersonType.Incharge)
        {
            place.Type = placeType;
            place.InchargePerson.Type = personType;

            db.People.Add(place.InchargePerson);
            db.Places.Add(place);
        }

        protected virtual void EditPlace(Place place)
        {
            db.Entry(place.InchargePerson).State = EntityState.Modified;
            db.Entry(place).State = EntityState.Modified;
        }

        private void ShowMessage(string key, string value)
        {
            if (TempData.ContainsKey(key) == true)
                TempData[key] = value;
            else
                TempData.Add(key, value);
        }

        public void Attention(string message)
        {
            ShowMessage(Alerts.ATTENTION, message);
        }

        public void Success(string message = "Saved Successfully!")
        {
            ShowMessage(Alerts.SUCCESS, message);
        }

        public void Information(string message)
        {
            ShowMessage(Alerts.INFORMATION, message);
        }

        public void Error(string message)
        {
            ShowMessage(Alerts.ERROR, message);
        }
    }
}