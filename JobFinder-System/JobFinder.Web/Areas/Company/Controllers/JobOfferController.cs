﻿using JobFinder.Models;
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
using PagedList;

namespace JobFinder.Web.Areas.Company.Controllers
{
    [Authorize(Roles="Company")]
    public class JobOfferController : BaseController
    {
        private const int OffersPerPage = 5;

        public JobOfferController(IJobFinderData data) : base(data)
        {            
        }

        public ActionResult GetOffers(int? page, bool? onlyActive)
        {
            string companyId = User.Identity.GetUserId();
            int pageNumber = page ?? 1;

            IEnumerable<ListOfferViewModel> model = this.data.JobOffers.All().Where(o => o.CompanyId == companyId)
                .OrderByDescending(o => o.DateCreated).Select(ListOfferViewModel.FromJobOffer);

            if (onlyActive != null)
            {
                model = model.Where(m => m.IsActive == onlyActive);
                TempData["onlyActive"] = onlyActive;
            }

            model = model.ToPagedList(pageNumber, OffersPerPage);
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

            if (model == null)
            {
                MessageViewModel message = new MessageViewModel { Text = "Job offer not found.", Type = MessageType.Error };
                TempData["Message"] = message;
                return RedirectToAction("GetOffers", "JobOffer");
                //TempData["NotFound"] = "Job offer not found.";
            }

            return View(model);
        }

        public ActionResult CreateOffer()
        {
            TempData["Towns"] = this.data.Towns.All().Where(t => !t.IsDeleted).Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() });
            TempData["BusinessSectors"] = this.data.BusinessSectors.All().Where(s => !s.IsDeleted).Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() });
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
                offer.IsActive = true;
                offer.CompanyId = companyId;
                offer.BusinessSectorId = model.BusinessSectorId;
                this.data.JobOffers.Add(offer);
                MessageViewModel message = new MessageViewModel 
                    { Type = MessageType.Success, Text = "You have successfully created your job offer." };
                TempData["Message"] = message;
                //TempData["Success"] = "You have successfully created your job offer.";
                return RedirectToAction("CreateOffer");
            }

            return View(model);
        }

        public ActionResult MarkAsExpired(int? id)
        {
            if (id != null)
            {
                JobOffer offer = this.data.JobOffers.Find((int)id);
                if (offer != null)
                {
                    offer.IsActive = false;
                    this.data.JobOffers.Update(offer);
                }               
            }

            return new EmptyResult();
        }
    }
}