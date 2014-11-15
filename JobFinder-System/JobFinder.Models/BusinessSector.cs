using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Index(IsUnique=true)]
        [MaxLength(25)]
        public string Name { get; set; }

        public virtual ICollection<Company> Companies { get; set; }

        public bool IsDeleted { get; set; }
    }
}
