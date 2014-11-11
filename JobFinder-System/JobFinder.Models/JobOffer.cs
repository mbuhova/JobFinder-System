using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JobFinder.Models
{
    public class JobOffer
    {
        public JobOffer()
        {
            this.Applications = new HashSet<Application>();
            this.PeopleFollowing = new HashSet<Person>();
        }

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsActive { get; set; }

        public int Views { get; set; }

        public int ApplicationsCount { get; set; }

        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public int TownId { get; set; }
        public virtual Town Town { get; set; }

        public int BusinessSectorId { get; set; }
        public virtual BusinessSector BusinessSector { get; set; }

        public virtual ICollection<Application> Applications { get; set; }

        public virtual ICollection<Person> PeopleFollowing { get; set; }
    }
}
