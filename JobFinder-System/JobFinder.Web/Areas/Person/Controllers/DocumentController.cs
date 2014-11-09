using JobFinder.Data;
using JobFinder.Web.Areas.Person.Models;
using JobFinder.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using JobFinder.Models;
using System.IO;

namespace JobFinder.Web.Areas.Person.Controllers
{
    [Authorize(Roles = "Person")]

    public class DocumentController : BaseController
    {
        private const int MaxFileSize = 4 * 1024 * 1024;

        public DocumentController(IJobFinderData data) : base(data)
        {
        }

        // GET: Person/Document
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDocuments()
        {
            string personId = User.Identity.GetUserId();

            IEnumerable<ListDocumentViewModel> model = this.data.Documents.All().Where(d => d.PersonId == personId)
                .Select(d => new ListDocumentViewModel { Id = d.Id, FileName = d.FileName, DateUploaded = d.DateUploaded })
                .OrderByDescending(d => d.DateUploaded);
            return View(model);
        }

        public FileContentResult DownloadFile(int id)
        {
            string personId = User.Identity.GetUserId();

            Document cv = this.data.Documents.All().FirstOrDefault(d => d.Id == id && d.PersonId == personId);
            //string mimeType = "application/pdf";
            //Response.AppendHeader("Content-Disposition", "inline; filename=" + cv.FileName);
            //return File(cv.FileData, mimeType);
            return File(cv.FileData, cv.ContentType, cv.FileName);
        }

        
        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };

            Document cv = new Document();

            foreach (string upload in Request.Files)
            {
                var extension = Path.GetExtension(Request.Files[upload].FileName);
                if (!allowedExtensions.Contains(extension))
                {
                    TempData["NotAllowedFile"] = "Please upload file in .doc, .docx or .pdf format.";
                    return RedirectToAction("UploadFile");
                }

                if (Request.Files[upload].ContentLength > MaxFileSize)
	            {
                    TempData["NotAllowedFile"] = "Please upload file with size less than 4 MB.";
                    return RedirectToAction("UploadFile");
	            }

                byte[] fileData = new byte[Request.Files[upload].InputStream.Length];
                Request.Files[upload].InputStream.Read(fileData, 0, Convert.ToInt32(Request.Files[upload].InputStream.Length));
                cv.FileData = fileData;
                cv.FileName = Request.Files[upload].FileName;
                cv.ContentType = Request.Files[upload].ContentType;
                cv.FileSize = Request.Files[upload].ContentLength;
                cv.DateUploaded = DateTime.Now;
                cv.PersonId = User.Identity.GetUserId();
                this.data.Documents.Add(cv);
            }

            TempData["Success"] = "File upload was successfull.";
            return RedirectToAction("UploadFile");
        }
    }
}