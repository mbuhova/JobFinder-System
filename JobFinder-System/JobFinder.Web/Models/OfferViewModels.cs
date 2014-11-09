using JobFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace JobFinder.Web.Models
{
    public class SearchOfferViewModel
    {
        public int? Town { get; set; }

        public int[] Sectors { get; set; }

        public string Word { get; set; }
    }

    public class SearchResultOfferViewModel
    {       
        public static Expression<Func<JobOffer, SearchResultOfferViewModel>> FromJobOffer
        {
            get
            {
                return o => new SearchResultOfferViewModel
                {
                    Id = o.Id,
                    Title = o.Title,
                    DateCreated = o.DateCreated,
                    Town = o.Town.Name,
                    CompanyId = o.CompanyId,
                    CompanyName = o.Company.CompanyName
                };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Town { get; set; }

        public DateTime DateCreated { get; set; }

        public string CompanyId { get; set; }

        public string CompanyName { get; set; }
    }

    public class DetailsOfferViewModel
    {
        public static Expression<Func<JobOffer, DetailsOfferViewModel>> FromJobOffer
        {
            get
            {
                return o => new DetailsOfferViewModel
                {
                    Id = o.Id,
                    Title = o.Title,
                    Description = o.Description,
                    DateCreated = o.DateCreated,
                    Views = o.Views,
                    Town = o.Town.Name,
                    ApplicationsCount = o.ApplicationsCount,
                    BusinessSector = o.BusinessSector.Name
                };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime DateCreated { get; set; }

        public string Description { get; set; }

        public int Views { get; set; }

        public int ApplicationsCount { get; set; }

        public string Town { get; set; }

        public string BusinessSector { get; set; }
    }
}