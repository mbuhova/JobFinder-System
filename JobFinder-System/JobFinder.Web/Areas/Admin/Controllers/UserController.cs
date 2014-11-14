using JobFinder.Data;
using JobFinder.Web.Areas.Admin.Models;
using JobFinder.Web.Controllers;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using JobFinder.Models;

namespace JobFinder.Web.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class UserController : BaseController
    {
        public UserController(IJobFinderData data) : base(data)
        {
        }

        // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var model = this.data.Companies.All()
                .Select(CompanyViewModel.FromCompany).ToDataSourceResult(request, ModelState);
            return this.Json(model);
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, CompanyViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                JobFinder.Models.Company company = this.data.Companies.Find(model.Id);
                if (company != null)
                {
                    company.IsApproved = model.IsApproved;
                    this.data.Companies.Update(company);
                }
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, CompanyViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                JobFinder.Models.Company toDelete = this.data.Companies.Find(model.Id);
                this.data.Companies.Delete(toDelete);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
    }
}