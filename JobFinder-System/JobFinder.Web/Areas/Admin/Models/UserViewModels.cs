using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace JobFinder.Web.Areas.Admin.Models
{
    public class CompanyViewModel
    {
        public static Expression<Func<JobFinder.Models.Company, CompanyViewModel>> FromCompany
        {
            get
            {
                return c => new CompanyViewModel
                {
                    Id = c.Id,
                    Name = c.CompanyName,
                    Bulstat = c.Bulstat,
                    Email = c.Email,
                    IsApproved = c.IsApproved
                };
            }
        }

        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public string Bulstat { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public string Name { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public string Email { get; set; }

        public bool IsApproved { get; set; }
    }

    public class PersonViewModel
    {
        public static Expression<Func<JobFinder.Models.Person, PersonViewModel>> FromPerson
        {
            get
            {
                return c => new PersonViewModel
                {
                    Id = c.Id,
                    Email = c.Email,
                    FirstName = c.FirstName,
                    LastName = c.LastName
                };
            }
        }

        [HiddenInput(DisplayValue=false)]
        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}