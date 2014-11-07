using JobFinder.Models;
using JobFinder.Web.Areas.Company.Models;
using JobFinder.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace JobFinder.Web.Areas.Company.Controllers
{
    public class JobOfferController : BaseController
    {
        private User currentUser;

        public JobOfferController() : base()
        {
            //string id = User.Identity.GetUserId();
            //currentUser = this.data.Users.Find(id);
        }

        // GET: Company/JobOffer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateOffer()
        {
            ViewBag.Towns = this.data.Towns.All().Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() });
            ViewBag.BusinessSectors = this.data.BusinessSectors.All().Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() });
            return View();
        }

        [HttpPost]
        public ActionResult CreateOffer(CreateOfferViewModel model)
        {
            if (ModelState.IsValid)
            {
                JobOffer offer = new JobOffer();
                offer.Title = model.Title;
                offer.Description = model.Description;
                offer.DateCreated = DateTime.Now;
                offer.TownId = model.TownId;
                //offer.Company = (JobFinder.Models.Company)this.currentUser;
                //offer.CompanyId = this.currentUser.Id;
                this.data.JobOffers.Add(offer);
                return RedirectToAction("CreateOffer");
            }

            return View(model);
        }
    }
}