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
    }
}
