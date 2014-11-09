using JobFinder.Models;
using JobFinder.Web.Areas.Company.Models;
using JobFinder.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using JobFinder.Data;
using JobFinder.Web.Models;

namespace JobFinder.Web.Areas.Company.Controllers
{
    [Authorize(Roles="Company")]
    public class JobOfferController : BaseController
    {
        //private User currentUser;

        private const int OffersPerPage = 5;

        public JobOfferController(IJobFinderData data) : base(data)
        {            
        }

        // GET: Company/JobOffer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetOffers(int? page)
        {
            string companyId = User.Identity.GetUserId();
            //currentUser = this.data.Users.Find(id);

            int skipPages = page == null ? 0 : (int)page - 1;
            IEnumerable<ListOfferViewModel> model = this.data.JobOffers.All().Where(o => o.CompanyId == companyId)
                .OrderByDescending(o => o.DateCreated).Skip(skipPages * OffersPerPage).Take(OffersPerPage).
                Select(ListOfferViewModel.FromJobOffer);
            return View(model);
        }

        public ActionResult OfferDetails(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("GetOffers");
            }

            string companyId = User.Identity.GetUserId();
            
            DetailsOfferViewModel model = this.data.JobOffers.All().Where(o => o.Id == id && o.CompanyId == companyId)
                .Select(DetailsOfferViewModel.FromJobOffer).FirstOrDefault();

            return View(model);
        }

        public ActionResult CreateOffer()
        {
            TempData["Towns"] = this.data.Towns.All().Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() });
            TempData["BusinessSectors"] = this.data.BusinessSectors.All().Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOffer(CreateOfferViewModel model)
        {
            if (ModelState.IsValid)
            {
                string companyId = User.Identity.GetUserId();

                JobOffer offer = new JobOffer();
                offer.Title = model.Title;
                offer.Description = model.Description;
                offer.DateCreated = DateTime.Now;
                offer.TownId = model.TownId;
                //offer.Company = (JobFinder.Models.Company)this.currentUser;
                offer.CompanyId = companyId;
                offer.BusinessSectorId = model.BusinessSectorId;
                this.data.JobOffers.Add(offer);
                TempData["Success"] = "You have successfully created your job offer.";
                return RedirectToAction("CreateOffer");
            }

            return View(model);
        }
    }
}