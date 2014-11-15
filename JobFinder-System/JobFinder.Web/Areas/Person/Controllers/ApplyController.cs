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
using PagedList;

namespace JobFinder.Web.Areas.Person.Controllers
{
    [Authorize(Roles="Person")]
    public class ApplyController : BaseController
    {
        private const int MaxFileSize = 1024 * 1024;

        private const int PageSize = 5;

        public ApplyController(IJobFinderData data) : base(data)
        {
        }


        // GET: Person/Apply
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ApplyForOffer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyForOffer(HttpPostedFileBase file)
        {
            bool isValid = IsValidRequest(file);

            if (!IsValidRequest(file))
            {
                return RedirectToAction("ApplyForOffer");
            }

            int offerId = int.Parse(RouteData.Values["id"].ToString());
            Application cv = new Application();

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

            MessageViewModel message = new MessageViewModel 
            { Text = "Your application was successfull. Good luck!", Type = MessageType.Success };
            TempData["Message"] = message;
            return RedirectToAction("ApplyForOffer");
        }

        public ActionResult MyApplications(int? page)
        {
            string personId = User.Identity.GetUserId();

            IEnumerable<ApplicationViewModel> model = this.data.Applications.All().Where(d => d.PersonId == personId)
                .AsQueryable().Include("Company").Select(ApplicationViewModel.FromApplication)
                .OrderByDescending(a => a.DateUploaded);
            int pageNumber = page ?? 1;
            model = model.ToPagedList(pageNumber, PageSize);
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

        private bool IsValidRequest(HttpPostedFileBase file)
        {
            string personId = User.Identity.GetUserId();
            var routeId = RouteData.Values["id"];
            MessageViewModel message = null;

            if (routeId == null)
            {
                message = new MessageViewModel { Type = MessageType.Error, Text = "Select a offer" };
                TempData["Message"] = message;
                return false;
            }

            string id = routeId.ToString();

            if (String.IsNullOrEmpty(id))
            {
                message = new MessageViewModel { Type = MessageType.Error, Text = "Select a offer" };
                TempData["Message"] = message;
                return false;
            }

            int offerId = int.Parse(id);
            Application doc = this.data.Applications.All().Where(d => d.JobOfferId == offerId && d.PersonId == personId).FirstOrDefault();

            if (doc != null)
            {
                message = new MessageViewModel { Type = MessageType.Error, Text = "You have already applied for this offer." };
                TempData["Message"] = message;
                return false;
            }

            JobOffer offer = this.data.JobOffers.Find(offerId);
            if (offer == null)
            {
                message = new MessageViewModel { Type = MessageType.Error, Text = "Job offer you are appling for doesnt exist." };
                TempData["Message"] = message;
                return false;
            }

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };

            if (file == null)
            {
                message = new MessageViewModel { Type = MessageType.Error, Text = "Please select a file to upload (.doc, .docx or .pdf format)." };
                TempData["Message"] = message;
                return false;
            }

            var extension = Path.GetExtension(file.FileName);
            if (!allowedExtensions.Contains(extension))
            {
                message = new MessageViewModel { Type = MessageType.Error, Text = "Please upload file in .doc, .docx or .pdf format." };
                TempData["Message"] = message;
                return false;
            }

            if (file.ContentLength > MaxFileSize)
            {
                message = new MessageViewModel { Type = MessageType.Error, Text = "Please upload file with size less than 1 MB." };
                TempData["Message"] = message;
                return false;
            }

            return true;
        }
    }
}