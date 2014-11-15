using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobFinder.Models
{
    public class Town
    {
        public Town()
        {
            this.JobOffers = new HashSet<JobOffer>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Index(IsUnique=true)]
        [MaxLength(20)]
        public string Name { get; set; }

        public virtual ICollection<JobOffer> JobOffers { get; set; }

        public bool IsDeleted { get; set; }
    }
}
