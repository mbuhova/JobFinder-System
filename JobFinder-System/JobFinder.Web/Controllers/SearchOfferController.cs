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
        private const int OffersPerPage = 2;

        public SearchOfferController(IJobFinderData data) : base(data)
        {
        }

        public ActionResult SearchOffers()
        {
            IEnumerable<SearchResultOfferViewModel> lastTen = this.data.JobOffers.All()
                .Select(SearchResultOfferViewModel.FromJobOffer).OrderByDescending(o => o.DateCreated).Take(10);
            lastTen = lastTen.ToPagedList(1, OffersPerPage);

            IEnumerable<SelectListItem> towns = this.data.Towns.All().Where(t => !t.IsDeleted)
                .Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() })
                .OrderBy(t => t.Text);
            IEnumerable<SelectListItem> businessSectors = this.data.BusinessSectors.All().Where(s => !s.IsDeleted)
                .Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() })
                .OrderBy(b => b.Text);
            TempData["Towns"] = towns;
            TempData["BusinessSectors"] = businessSectors;
            return View(lastTen);
        }
        
        public ActionResult OfferSearchResults(int? page, int[] sectors, int? town, string word)
        {
            SearchOfferViewModel search = new SearchOfferViewModel();
            search.Page = page ?? 1;
            search.Sectors = sectors;
            search.Town = town;
            search.Word = word;
            IEnumerable<SearchResultOfferViewModel> offers = GetResults(search);

            TempData["Town"] = search.Town;
            TempData["Word"] = search.Word;


            return View(offers.ToPagedList((int)search.Page, OffersPerPage));
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