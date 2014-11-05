﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobFinder.Models
{
    [Table("Companies")]
    public class Company : User
    {
        public Company()
        {
            this.BusinessSectors = new HashSet<BusinessSector>();
        }

        [Required]
        public string Bulstat { get; set; }

        [Required]
        public string CompanyName { get; set; }

        //[CollectionMaxLength(3)]
        public virtual ICollection<BusinessSector> BusinessSectors { get; set; }

        public string Address { get; set; }

        public string WebSite { get; set; }
    }
}
