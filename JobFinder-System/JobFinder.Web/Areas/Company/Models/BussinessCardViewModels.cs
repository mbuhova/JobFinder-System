using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.WebPages.Html;

namespace JobFinder.Web.Areas.Company.Models
{
    public class BussinessCardViewModel
    {
        public static Expression<Func<JobFinder.Models.Company, BussinessCardViewModel>> FromCompany
        {
            get
            {
                return c => new BussinessCardViewModel
                {
                    Id = c.Id,
                    CompanyName = c.CompanyName,
                    Address = c.Address,
                    WebSite = c.WebSite,
                    BusinessSectors = c.BusinessSectors.Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() }),
                    AboutUs = c.AboutUs
                };
            }
        }

        public string Id { get; set; }

        public string CompanyName { get; set; }

        public IEnumerable<SelectListItem> BusinessSectors { get; set; }

        public string Address { get; set; }

        public string WebSite { get; set; }

        public string AboutUs { get; set; }
    }

    public class EditBussinessCardViewModel
    {
        public string Id { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public int[] BusinessSectors { get; set; }

        public string Address { get; set; }

        public string WebSite { get; set; }

        public string AboutUs { get; set; }
    }
}