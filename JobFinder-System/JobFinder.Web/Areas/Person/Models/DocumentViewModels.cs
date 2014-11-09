using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobFinder.Web.Areas.Person.Models
{
    public class ListDocumentViewModel
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public DateTime DateUploaded { get; set; }
    }
}