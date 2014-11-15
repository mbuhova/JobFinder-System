using JobFinder.Data.Migrations;
using JobFinder.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Data
{
    public class JobFinderDbContext : IdentityDbContext<User>
    {
        public JobFinderDbContext()
            : base("JobFinderConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<JobFinderDbContext, Configuration>());
        }

        public static JobFinderDbContext Create()
        {
            return new JobFinderDbContext();
        }

        public IDbSet<BusinessSector> BusinessSectors { get; set; }

        public IDbSet<Person> People { get; set; }

        public IDbSet<Company> Companies { get; set; }

        public IDbSet<Application> Applications { get; set; }

        public IDbSet<JobOffer> JobOffers { get; set; }

        public IDbSet<Town> Towns { get; set; }
    }
}
