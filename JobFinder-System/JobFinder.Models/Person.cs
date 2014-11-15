using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobFinder.Models
{
    [Table("People")]
    public class Person : User
    {
        public Person()
        {
            this.FollowedOffers = new HashSet<JobOffer>();
            this.Applications = new HashSet<Application>();
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public virtual ICollection<JobOffer> FollowedOffers { get; set; }

        public ICollection<Application> Applications { get; set; }
    }
}
