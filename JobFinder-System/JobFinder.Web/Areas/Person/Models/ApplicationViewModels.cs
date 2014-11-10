using JobFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace JobFinder.Web.Areas.Person.Models
{
    public class ApplicationViewModel
    {
        public static Expression<Func<Document, ApplicationViewModel>> FromApplication
        {
            get
            {
                return a => new ApplicationViewModel
                {
                    FileId = a.Id,
                    FileName = a.FileName,
                    DateUploaded = a.DateUploaded,
                    JobOfferId = a.JobOfferId,
                    JobOfferTitle = a.JobOffer.Title
                };
            }
        }

        public int FileId { get; set; }

        public string FileName { get; set; }

        public DateTime DateUploaded { get; set; }

        public int JobOfferId { get; set; }

        public string JobOfferTitle { get; set; }
    }
}