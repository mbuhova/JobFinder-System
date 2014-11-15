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
    public class TownViewModel
    {
        public static Expression<Func<Town, TownViewModel>> FromTown
        {
            get
            {
                return t => new TownViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                };
            }
        }

       [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }
    }
}