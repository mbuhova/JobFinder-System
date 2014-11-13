using JobFinder.Data;
using JobFinder.Web.Controllers;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobFinder.Web.Models;
using Kendo.Mvc.Extensions;
using JobFinder.Models;

namespace JobFinder.Web.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class OfferController : BaseController
    {
        public OfferController(IJobFinderData data) : base(data)
        {
        }

        // GET: Admin/Offer
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var model = this.data.JobOffers.All()
                .Select(AdminOfferViewModel.FromJobOffer).ToDataSourceResult(request, ModelState);
            return this.Json(model);
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, AdminOfferViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                JobOffer offer = this.data.JobOffers.Find(model.Id);
                if (offer != null)
                {
                    //update only status
                    offer.IsActive = model.IsActive;
                    this.data.JobOffers.Update(offer);
                }
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
    }
}