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
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
