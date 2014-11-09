using JobFinder.Data;
using JobFinder.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

            DetailsOfferViewModel model = this.data.JobOffers.All().Where(o => o.Id == id)
                .Select(DetailsOfferViewModel.FromJobOffer).FirstOrDefault();

            return View(model);
        }
    }
}