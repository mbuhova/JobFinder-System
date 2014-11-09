using JobFinder.Data;
using JobFinder.Models;
using JobFinder.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace JobFinder.Web.Controllers
{
    public class SearchOfferController : BaseController
    {
        public SearchOfferController(IJobFinderData data) : base(data)
        {
        }

        public ActionResult SearchOffers()
        {
            IEnumerable<SelectListItem> towns = this.data.Towns.All()
                .Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() })
                .OrderBy(t => t.Text);
            IEnumerable<SelectListItem> businessSectors = this.data.BusinessSectors.All()
                .Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() })
                .OrderBy(b => b.Text);
            TempData["Towns"] = towns;
            TempData["BusinessSectors"] = businessSectors;
            return View();
        }

        public ActionResult OfferSearchResults(SearchOfferViewModel model)
        {
            IEnumerable<SearchResultOfferViewModel> offers = FilterOffers(model).AsQueryable().Include("Company").Include("Town")
                .Select(SearchResultOfferViewModel.FromJobOffer).ToList();
            return View(offers);
        }

        private IEnumerable<JobOffer> FilterOffers(SearchOfferViewModel model)
        {
            IEnumerable<JobOffer> offers = FilterBySector(model.Sectors);
            if (offers != null && model.Town != null)
            {
                offers = offers.Where(o => o.TownId == model.Town);                
            }

            if (offers != null && model.Word != null)
            {
                offers = offers.Where(o => o.Title.Contains(model.Word) || o.Description.Contains(model.Word));
            }

            return offers;
        }

        private IEnumerable<JobOffer> FilterBySector(int[] sectorsIds)
        {
            if (sectorsIds == null || sectorsIds.Length == 0)
            {
                return this.data.JobOffers.All();
            }

            int id = sectorsIds[0];
            IQueryable<JobOffer> result = this.data.JobOffers.All().Where(o => o.BusinessSectorId == id);

            IQueryable<JobOffer> singleSector = null;

            for (int i = 1; i < sectorsIds.Length; i++)
			{
                id = sectorsIds[i];
                singleSector = this.data.JobOffers.All().Where(o => o.BusinessSectorId == id);
                if (result != null)
                {
                    result.Concat(singleSector);
                }
                else
                {
                    result = singleSector;
                }
			}

            return result;
        }
    }
}