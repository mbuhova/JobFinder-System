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
            if (id == null)
            {
                return RedirectToAction("SearchOffers", "SearchOffer");
            }

            JobOffer offer = this.data.JobOffers.Find((int)id);
            offer.Views += 1;
            this.data.JobOffers.Update(offer);
            
            DetailsOfferViewModel model = this.data.JobOffers.All().Where(o => o.Id == id)
                .Select(DetailsOfferViewModel.FromJobOffer).FirstOrDefault();

            if (Request.IsAuthenticated && User.IsInRole("Person"))
            {
               //Application app = this.data.Applications.All().Where(a => a.JobOfferId == model.Id)
               //   .Include("JobOffer").Include("Person").FirstOrDefault();
               //
               //if (app.JobOffer.PeopleFollowing.Contains(app.Person))
               //{
               //    TempData["FollowButtonText"] = "Unfollow";
               //}
               //else
               //{
               //    TempData["FollowButtonText"] = "Follow";
               //}

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
    }
}