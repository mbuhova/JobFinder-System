using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JobFinder.Models
{
    public class Application
    {
        [Key]
        public int Id { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public byte[] FileData { get; set; }

        public long FileSize { get; set; }

        public DateTime DateUploaded { get; set; }

        public string PersonId { get; set; }
        public virtual Person Person { get; set; }

        public int JobOfferId { get; set; }
        public virtual JobOffer JobOffer { get; set; }
    }
}
