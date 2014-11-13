using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JobFinder.Models
{
    public class BusinessSector
    {
        public BusinessSector()
        {
            this.Companies = new HashSet<Company>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Company> Companies { get; set; }

        public bool IsDeleted { get; set; }
    }
}
