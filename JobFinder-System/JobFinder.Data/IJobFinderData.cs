using JobFinder.Data.Repositories;
using JobFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Data
{
    public interface IJobFinderData
    {
        IRepository<User> Users { get; }

        IRepository<Person> People { get; }

        IRepository<Company> Companies { get; }

        IRepository<BusinessSector> BusinessSectors { get; }

        IRepository<Document> Documents { get; }

        IRepository<JobOffer> JobOffers { get; }

        IRepository<Town> Towns { get; }

        void SaveChanges();
    }
}
