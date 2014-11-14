using JobFinder.Data;
using JobFinder.Models;
using JobFinder.Web.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using JobFinder.Web.Models;

namespace JobFinder.Web.Areas.Person.Controllers
{
    [Authorize(Roles="Person")]
    public class ApplyController : BaseController
    {
        private const int MaxFileSize = 4 * 1024 * 1024;

        public ApplyController(IJobFinderData data) : base(data)
        {
        }


        // GET: Person/Apply
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ApplyForOffer() //int id
        {
            //return View();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyForOffer(HttpPostedFileBase file)
        {
            string personId = User.Identity.GetUserId();
            
            
            var routeId = RouteData.Values["id"];

            if (routeId == null)
            {
                TempData["NotAllowedFile"] = "Select a offer";
                return RedirectToAction("ApplyForOffer");
            }

            string id = routeId.ToString();

            if (String.IsNullOrEmpty(id))
            {
                //TODO change this
                TempData["NotAllowedFile"] = "Select a offer";
                return RedirectToAction("ApplyForOffer");
            }

            int offerId = int.Parse(id);

            Application doc = this.data.Applications.All().Where(d => d.JobOfferId == offerId && d.PersonId == personId).FirstOrDefault();

            if (doc != null)
            {
                TempData["AlreadyApplied"] = "You have already applied for this offer.";
                return RedirectToAction("ApplyForOffer");
            }

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };

            Application cv = new Application();

            if (file == null)
            {
                TempData["NotAllowedFile"] = "Please select a file to upload (.doc, .docx or .pdf format).";
                return RedirectToAction("ApplyForOffer");
            }

            var extension = Path.GetExtension(file.FileName);
            if (!allowedExtensions.Contains(extension))
            {
                TempData["NotAllowedFile"] = "Please upload file in .doc, .docx or .pdf format.";
                return RedirectToAction("ApplyForOffer");
            }

            if (file.ContentLength > MaxFileSize)
            {
                TempData["NotAllowedFile"] = "Please upload file with size less than 4 MB.";
                return RedirectToAction("ApplyForOffer");
            }

            byte[] fileData = new byte[file.InputStream.Length];
            file.InputStream.Read(fileData, 0, Convert.ToInt32(file.InputStream.Length));
            cv.FileData = fileData;
            cv.FileName = file.FileName;
            cv.ContentType = file.ContentType;
            cv.FileSize = file.ContentLength;
            cv.DateUploaded = DateTime.Now;
            cv.PersonId = User.Identity.GetUserId();
            cv.JobOfferId = offerId;
            this.data.Applications.Add(cv);

            var offer = this.data.JobOffers.Find(offerId);
            offer.ApplicationsCount += 1;
            this.data.JobOffers.Update(offer);

            TempData["Success"] = "Your application was successfull. Good luck!";
            return RedirectToAction("ApplyForOffer");
        }

        public ActionResult MyApplications()
        {
            string personId = User.Identity.GetUserId();

            IEnumerable<ApplicationViewModel> model = this.data.Applications.All().Where(d => d.PersonId == personId)
                .AsQueryable().Include("Company").Select(ApplicationViewModel.FromApplication)
                .OrderByDescending(a => a.DateUploaded);
            return View(model);
        }

        public FileContentResult DownloadFile(int id)
        {
            string personId = User.Identity.GetUserId();

            Application cv = this.data.Applications.All().FirstOrDefault(a => a.Id == id && a.PersonId == personId);
            //string mimeType = "application/pdf";
            //Response.AppendHeader("Content-Disposition", "inline; filename=" + cv.FileName);
            //return File(cv.FileData, mimeType);
            return File(cv.FileData, cv.ContentType, cv.FileName);
        }
    }
}