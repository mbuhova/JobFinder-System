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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Web.Security;

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
            var model = this.data.Companies.All().OrderBy(c => c.CompanyName)
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
                   // if (model.IsApproved)
                   // {
                   //     Roles.AddUserToRole(company.UserName, "Company");
                   // }
                   // else
                   // {
                   //     Roles.RemoveUserFromRole(company.UserName, "Company");
                   // }
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