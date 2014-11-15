using JobFinder.Data;
using JobFinder.Models;
using JobFinder.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using JobFinder.Web.Areas.Company.Models;

namespace JobFinder.Web.Controllers
{
    public class PublicOfferController : BaseController
    {
        public PublicOfferController(IJobFinderData data) : base(data)
        {
        }

        // GET: PublicOffer
        public ActionResult OfferDetails(int? id)
        {
            JobOffer offer = this.data.JobOffers.Find((int)id);

            if (id == null || offer == null)
            {
                return RedirectToAction("SearchOffers", "SearchOffer");
            }

            offer.Views += 1;
            this.data.JobOffers.Update(offer);
            
            DetailsOfferViewModel model = this.data.JobOffers.All().Where(o => o.Id == id)
                .Select(DetailsOfferViewModel.FromJobOffer).FirstOrDefault();

            if (Request.IsAuthenticated && User.IsInRole("Person"))
            {
                string personId = this.User.Identity.GetUserId();
                Application app = this.data.Applications.All().Where(a => a.PersonId == personId && a.JobOfferId == model.Id)
                    .Include("Person").FirstOrDefault();
                TempData["HasApplied"] = (app == null) ? false : true;
                Person current = this.data.People.Find(personId);

                if (offer.PeopleFollowing.Contains(current))
                {
                    TempData["FollowButtonText"] = "Unfollow";
                }
                else
                {
                    TempData["FollowButtonText"] = "Follow";
                }
            }

            return View(model);
        }

        public ActionResult GetCompanyBusinessCard(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return RedirectToAction("SearchOffers", "SearchOffer");
            }

            BussinessCardViewModel model = this.data.Companies.All().Where(c => c.Id == id)
                .Select(BussinessCardViewModel.FromCompany).FirstOrDefault();

            TempData["Sectors"] = this.data.BusinessSectors.All()
                .Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() });

            if (model == null)
            {
                TempData["NotFound"] = "Company not found.";
            }

            return View("BussinessCard", model);
        }
    }
}