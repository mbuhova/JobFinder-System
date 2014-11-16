using JobFinder.Data;
using JobFinder.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Microsoft.AspNet.Identity;
using JobFinder.Web.Models;
using System.Data.Entity;
using JobFinder.Models;

namespace JobFinder.Web.Areas.Company.Controllers
{
    [Authorize(Roles="Company")]
    public class ApplicationController : BaseController
    {
        private const int ApplicationsPerPage = 5;

        public ApplicationController(IJobFinderData data) : base(data)
        {
        }

        public ActionResult GetApplications(int? id, int? page, string approved, string rejected, string notSeen)
        {
            int pageNumber = page ?? 1;

            IEnumerable<ApplicationViewModel> model = this.data.Applications.All()
                .Where(a => a.JobOfferId == (int)id).OrderByDescending(a => a.DateUploaded).Include("JobOffer")
                .Select(ApplicationViewModel.FromApplication);

            model = Filter(model, approved, rejected, notSeen);

            model = model.ToPagedList(pageNumber, ApplicationsPerPage);

            TempData["approved"] = approved == "on" ? true : false; 
            TempData["rejected"] = rejected == "on" ? true : false; 
            TempData["notSeen"] = notSeen == "on" ? true : false; 

            return View(model);
        }

        public ActionResult ChangeStatus(int id, bool isApproved)
        {
            Application app = this.data.Applications.Find(id);
            if (app != null)
            {
                app.IsApproved = isApproved;
                this.data.Applications.Update(app);
            }

            return new EmptyResult();
        }

        public FileContentResult DownloadFile(int id)
        {
            Application cv = this.data.Applications.All().FirstOrDefault(a => a.Id == id);
            return File(cv.FileData, cv.ContentType, cv.FileName);
        }

        private IEnumerable<ApplicationViewModel> Filter(IEnumerable<ApplicationViewModel> model, string appr, string rej, string notseen)
        {
            if (appr == null && rej == null && notseen == null)
            {
                return model;
            }

            bool approved = appr == "on" ? true : false;
            bool rejected = rej == "on" ? true : false;
            bool notSeen = notseen == "on" ? true : false;

            if (approved && rejected && notSeen)
            {
                return model;
            }
            else if (approved && rejected)
            {
                return model.Where(m => m.IsApproved != null);
            }
            else if (approved && notSeen)
            {
                return model.Where(m => m.IsApproved != false);
            }
            else if (rejected && notSeen)
            {
                return model.Where(m => m.IsApproved != true);
            }
            else if (approved)
            {
                return model.Where(m => m.IsApproved == true);
            }
            else if (rejected)
            {
                return model.Where(m => m.IsApproved == false);
            }
            else if (notSeen)
            {
                return model.Where(m => m.IsApproved == null);
            }

            return model;
        }
    }
}