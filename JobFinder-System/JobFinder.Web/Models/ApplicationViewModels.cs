using JobFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace JobFinder.Web.Models
{
    public class ApplicationViewModel
    {
        public static Expression<Func<Application, ApplicationViewModel>> FromApplication
        {
            get
            {
                return a => new ApplicationViewModel
                {
                    Id = a.Id,
                    FileName = a.FileName,
                    DateUploaded = a.DateUploaded,
                    IsApproved = a.IsApproved,
                    JobOfferId = a.JobOfferId,
                    JobOfferTitle = a.JobOffer.Title
                };
            }
        }

        public int Id { get; set; }

        public string FileName { get; set; }

        public DateTime DateUploaded { get; set; }

        public bool? IsApproved { get; set; }

        public int JobOfferId { get; set; }

        public string JobOfferTitle { get; set; }
    }
}