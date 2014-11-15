using JobFinder.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace JobFinder.Web.Areas.Admin.Models
{
    public class BusinessSectorViewModel
    {
        public static Expression<Func<BusinessSector, BusinessSectorViewModel>> FromBusinessSector
        {
            get
            {
                return t => new BusinessSectorViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                };
            }
        }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [MaxLength(25)]
        public string Name { get; set; }
    }
}