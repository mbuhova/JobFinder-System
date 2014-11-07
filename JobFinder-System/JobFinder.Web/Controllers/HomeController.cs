using JobFinder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobFinder.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController() : base()
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult File()
        {
            return View();
        }

        [HttpPost]
        public ActionResult File(HttpPostedFileBase file)
        {
            var allowedExtensions = new[] { ".doc", ".docx", ".txt", ".jpeg" };
            

            CV cv = new CV();

            foreach (string upload in Request.Files)
            {
                var extension = Path.GetExtension(Request.Files[upload].FileName);
                if (!allowedExtensions.Contains(extension))
                {
                    // Not allowed
                }


                byte[] fileData = new byte[Request.Files[upload].InputStream.Length];
                Request.Files[upload].InputStream.Read(fileData, 0, Convert.ToInt32(Request.Files[upload].InputStream.Length));
                cv.FileData = fileData;
                cv.FileName = Request.Files[upload].FileName;
                cv.ContentType = Request.Files[upload].ContentType;
                cv.FileSize = Request.Files[upload].ContentLength;
                cv.DateUploaded = DateTime.Now;
                this.data.CVs.Add(cv);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Download()
        {
            CV cv = this.data.CVs.Find(2);
            return File(cv.FileData, cv.ContentType, "aaaa");
        }
    }
}