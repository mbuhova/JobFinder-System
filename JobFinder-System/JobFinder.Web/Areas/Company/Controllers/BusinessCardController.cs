using JobFinder.Data;
using JobFinder.Web.Areas.Company.Models;
using JobFinder.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using JobFinder.Models;

namespace JobFinder.Web.Areas.Company.Controllers
{
    [Authorize(Roles="Company")]
    public class BusinessCardController : BaseController
    {

        public BusinessCardController(IJobFinderData data) : base(data)
        {
        }

        // GET: Company/BussinessCard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBusinessCard()
        {
            string companyId = this.User.Identity.GetUserId();

            return RedirectToAction("GetCompanyBusinessCard", "PublicOffer", new { area = "", id = companyId });
        }

        public ActionResult EditCard(string id)
        {
            TempData["Sectors"] = this.data.BusinessSectors.All()
                .Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() });
            return PartialView("_EditCardPartial", id);
        }

        [HttpPost]
        public ActionResult EditCard(EditBussinessCardViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["InvalidModel"] = "Company name is required. Please try again.";
                return RedirectToAction("GetBusinessCard", "BusinessCard");
            }

            if (model.BusinessSectors == null)
            {
                TempData["InvalidModel"] = "Please select at least one business sector.";
                return RedirectToAction("GetBusinessCard", "BusinessCard");
            }

            JobFinder.Models.Company company = this.data.Companies.Find(model.Id);
            company.CompanyName = model.CompanyName;
            company.AboutUs = model.AboutUs;
            company.Address = model.Address;
            company.WebSite = model.WebSite;
            AddBusinessSectors(model.BusinessSectors, company.Id);

            TempData["Success"] = "Your changes are saved.";
            return RedirectToAction("GetBusinessCard", "BusinessCard");
        }

        private void AddBusinessSectors(int[] sectors, string userId)
        {
            var sectorsToAdd = new List<BusinessSector>();

            foreach (var id in sectors)
            {
                var sector = this.data.BusinessSectors.Find(id);
                sectorsToAdd.Add(sector);
            }

            var user = this.data.Companies.Find(userId);
            user.BusinessSectors.Clear();
            user.BusinessSectors = sectorsToAdd;
            this.data.SaveChanges();
        }
    }
}