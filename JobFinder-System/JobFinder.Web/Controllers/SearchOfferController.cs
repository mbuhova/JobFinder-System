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
using PagedList;

namespace JobFinder.Web.Controllers
{
    public class SearchOfferController : BaseController
    {
        private const int OffersPerPage = 10;

        public SearchOfferController(IJobFinderData data) : base(data)
        {
        }

        public ActionResult SearchOffers()
        {
           //IEnumerable<SearchResultOfferViewModel> offers = this.data.JobOffers.All().Select(SearchResultOfferViewModel.FromJobOffer).ToList();
           //SearchOfferViewModel lastSearch = HttpContext.Session["LastSearch"] as SearchOfferViewModel;
           //if (HttpContext.Session["LastSearch"] == null)
           //{
           //    HttpContext.Session["LastSearch"] = new SearchOfferViewModel();
           //    lastSearch = new SearchOfferViewModel();
           //}

            //IEnumerable<SearchResultOfferViewModel> offers = GetResults(lastSearch);


            IEnumerable<SelectListItem> towns = this.data.Towns.All()
                .Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() })
                .OrderBy(t => t.Text);
            IEnumerable<SelectListItem> businessSectors = this.data.BusinessSectors.All()
                .Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() })
                .OrderBy(b => b.Text);
            TempData["Towns"] = towns;
            TempData["BusinessSectors"] = businessSectors;
            return View(); //offers.ToPagedList(1, 2)
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OfferSearchResults(SearchOfferViewModel model)
        {
           SearchOfferViewModel lastSearch = HttpContext.Session["LastSearch"] as SearchOfferViewModel;
           if (model.Page == null)
           {
               HttpContext.Session["LastSearch"] = model;
               lastSearch = model;
           }

            IEnumerable<SearchResultOfferViewModel> offers = GetResults(lastSearch);
            int page = model.Page ?? 1;

            return View(offers.ToPagedList(page, OffersPerPage));
            //return this.PartialView("_OfferSearchResultsPartial", offers.ToPagedList(2, 2));
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
                string word = model.Word.ToLower();
                offers = offers.Where(o => o.Title.ToLower().Contains(word) || o.Description.ToLower().Contains(word) 
                    || o.BusinessSector.Name.ToLower().Contains(word) || o.Company.CompanyName.ToLower().Contains(word));
            }

            return offers;
        }

        private IEnumerable<JobOffer> FilterBySector(int[] sectorsIds)
        {
            if (sectorsIds == null || sectorsIds.Length == 0)
            {
                return this.data.JobOffers.All().Where(o => o.IsActive);
            }

            int id = sectorsIds[0];
            IQueryable<JobOffer> result = this.data.JobOffers.All().Where(o => o.BusinessSectorId == id && o.IsActive);

            IQueryable<JobOffer> singleSector = null;

            for (int i = 1; i < sectorsIds.Length; i++)
			{
                id = sectorsIds[i];
                singleSector = this.data.JobOffers.All().Where(o => o.BusinessSectorId == id && o.IsActive);
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

        private IEnumerable<SearchResultOfferViewModel> GetResults(SearchOfferViewModel lastSearch)
        {
            //int skipPages = (lastSearch.Page == null || lastSearch.Page <= 0) ? 0 : (int)lastSearch.Page - 1;
            int offersCount = 0;

            IEnumerable<SearchResultOfferViewModel> offers = FilterOffers(lastSearch).AsQueryable().Include("Company").Include("Town")
                .Select(SearchResultOfferViewModel.FromJobOffer).OrderByDescending(o => o.DateCreated);

            offersCount = offers.Count();

            //offers = offers.Skip(skipPages * OffersPerPage).Take(OffersPerPage).ToList();
            TempData["OffersCount"] = offersCount;
            return offers;
        }
    }
}