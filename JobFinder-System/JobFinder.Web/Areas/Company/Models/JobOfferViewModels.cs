using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using JobFinder.Models;

namespace JobFinder.Web.Areas.Company.Models
{
    public class CreateOfferViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int TownId { get; set; }

        [Required]
        public int BusinessSectorId { get; set; }
    }

    public class ListOfferViewModel
    {
        public static Expression<Func<JobOffer, ListOfferViewModel>> FromJobOffer
        {
            get
            {
                return o => new ListOfferViewModel
                {
                    Id = o.Id,
                    Title = o.Title,
                    DateCreated = o.DateCreated,
                    IsActive = o.IsActive
                };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsActive { get; set; }
    }
}