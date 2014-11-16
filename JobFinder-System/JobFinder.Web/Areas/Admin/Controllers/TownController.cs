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
    public class TownController : BaseController
    {
        public TownController(IJobFinderData data) : base(data)
        {
        }


        // GET: Admin/Town
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var model = this.data.Towns.All().Where(t => !t.IsDeleted).OrderBy(t => t.Name)
                .Select(t => new TownViewModel { Id = t.Id, Name = t.Name }).ToDataSourceResult(request, ModelState);
            return this.Json(model);
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, TownViewModel model)
        {
           if (model != null && ModelState.IsValid)
           {
               Town town = this.data.Towns.All().Where(t => t.Name == model.Name).FirstOrDefault();
               if (town == null)
               {
                   Town toAdd = new Town { Name = model.Name };
                   this.data.Towns.Add(toAdd);
                   model.Id = toAdd.Id;
               } 
           }
          
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, TownViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                Town town = this.data.Towns.All().Where(t => t.Name == model.Name).FirstOrDefault();
                if (town == null)
                {
                    Town toUpdate = this.data.Towns.Find(model.Id);
                    toUpdate.Name = model.Name;
                    this.data.Towns.Update(toUpdate);
                }
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, TownViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                Town toDelete = this.data.Towns.Find(model.Id);
                toDelete.IsDeleted = true;
                this.data.Towns.Update(toDelete);
                //this.data.Towns.Delete(model.Id);
            }
            
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
    }
}