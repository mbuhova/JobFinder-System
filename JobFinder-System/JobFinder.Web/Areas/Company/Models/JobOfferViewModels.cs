using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

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
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime DateCreated { get; set; }

    }

    public class DetailsOfferViewModel
    {
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