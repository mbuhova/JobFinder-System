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
    [Authorize(Roles = "Admin")]
    public class BusinessSectorController : BaseController
    {
        public BusinessSectorController(IJobFinderData data) : base(data)
        {
        }

        // GET: Admin/BusinessSector
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var model = this.data.BusinessSectors.All().Where(s => !s.IsDeleted).OrderBy(s => s.Name)
                .Select(s => new BusinessSectorViewModel { Id = s.Id, Name = s.Name }).ToDataSourceResult(request, ModelState);
            return this.Json(model);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, BusinessSectorViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                BusinessSector town = this.data.BusinessSectors.All().Where(s => s.Name == model.Name).FirstOrDefault();
                if (town == null)
                {
                    BusinessSector toAdd = new BusinessSector { Name = model.Name };
                    this.data.BusinessSectors.Add(toAdd);
                    model.Id = toAdd.Id;
                }
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, BusinessSectorViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                BusinessSector sector = this.data.BusinessSectors.All().Where(s => s.Name == model.Name).FirstOrDefault();
                if (sector == null)
                {
                    BusinessSector toUpdate = this.data.BusinessSectors.Find(model.Id);
                    toUpdate.Name = model.Name;
                    this.data.BusinessSectors.Update(toUpdate);
                }
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, BusinessSectorViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                BusinessSector toDelete = this.data.BusinessSectors.Find(model.Id);
                toDelete.IsDeleted = true;
                this.data.BusinessSectors.Update(toDelete);
                //this.data.Towns.Delete(model.Id);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
    }
}