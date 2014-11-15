namespace JobFinder.Data.Migrations
{
    using JobFinder.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<JobFinder.Data.JobFinderDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(JobFinder.Data.JobFinderDbContext context)
        {
            //Add roles
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);

                if (!context.Roles.Any(r => r.Name == "Company"))
                {
                    var companyRole = new IdentityRole { Name = "Company" };
                    manager.Create(companyRole);
                }

                if (!context.Roles.Any(r => r.Name == "Person"))
                {
                    var personRole = new IdentityRole { Name = "Person" };
                    manager.Create(personRole);
                }
            }

            //Add towns
            if (!context.Towns.Any())
            {
                context.Towns.AddOrUpdate(t => t.Name,
                    new Town { Name = "Sofia" },
                    new Town { Name = "Plovdiv" },
                    new Town { Name = "Varna" },
                    new Town { Name = "Burgas" },
                    new Town { Name = "Ruse" },
                    new Town { Name = "Vraca" },
                    new Town { Name = "Pavlikeni" },
                    new Town { Name = "Veliko Turnovo" },
                    new Town { Name = "Svishtov" },
                    new Town { Name = "Razgrad" },
                    new Town { Name = "Shumen" },
                    new Town { Name = "Haskovo" },
                    new Town { Name = "Pernik" },
                    new Town { Name = "Blagoevgrad" },
                    new Town { Name = "Pazardjik" }
                );
            }


            BusinessSector sports = new BusinessSector { Name = "Sports" };
            BusinessSector security = new BusinessSector { Name = "Scurity" };
            BusinessSector travel = new BusinessSector { Name = "Travel" };

            //Add business sectors
            if (!context.BusinessSectors.Any())
            {
                context.BusinessSectors.AddOrUpdate(s => s.Name,
                    new BusinessSector { Name = "Accounting" },
                    new BusinessSector { Name = "Finance" },
                    new BusinessSector { Name = "Advertising/PR" },
                    new BusinessSector { Name = "Auto Service" },
                    new BusinessSector { Name = "Aviation" },
                    new BusinessSector { Name = "Banking" },
                    new BusinessSector { Name = "Contact centers" },
                    new BusinessSector { Name = "Design" },
                    new BusinessSector { Name = "Drivers" },
                    new BusinessSector { Name = "Education" },
                    new BusinessSector { Name = "Engineers" },
                    new BusinessSector { Name = "Healthcare" },
                    new BusinessSector { Name = "Human Resources" },
                    new BusinessSector { Name = "Software development" },
                    new BusinessSector { Name = "IT - Hardware" },
                    new BusinessSector { Name = "Insurance" },
                    new BusinessSector { Name = "Logistics" },
                    new BusinessSector { Name = "Management" },
                    new BusinessSector { Name = "Marketing" },
                    new BusinessSector { Name = "Production" },
                    new BusinessSector { Name = "Food & Beverage" },
                    new BusinessSector { Name = "Telecommunications" },
                    sports, security, travel
                    );
            }           

            //Create admin, company and person
            if (!context.Users.Any(u => u.UserName == "admin@admin.bg"))
            {
                var store = new UserStore<User>(context);
                var manager = new UserManager<User>(store);

                var user = new Person { UserName = "admin@admin.bg", Email = "admin@admin.bg", FirstName = "Admin", LastName = "Admin" };              
                manager.Create(user, "adminn");
                manager.AddToRole(user.Id, "Admin");

                if (!context.Companies.Any(c => c.Bulstat == "1234567891234"))
                {
                    Company company = new Company
                    {
                        Bulstat = "1234567891234",
                        Email = "aaaaaa@abv.bg",
                        BusinessSectors = new HashSet<BusinessSector> { sports, travel },
                        UserName = "aaaaaa@abv.bg",
                        CompanyName = "Abc",
                        IsApproved = true,
                        PhoneNumber = "0888888888"
                    };

                    manager.Create(company, "aaaaaa");
                    manager.AddToRole(company.Id, "Company");
                }

                if (!context.Companies.Any(c => c.Bulstat == "2234567891234"))
                {
                    Company company = new Company
                    {
                        Bulstat = "2234567891234",
                        Email = "company@abv.bg",
                        BusinessSectors = new HashSet<BusinessSector> { security },
                        UserName = "company@abv.bg",
                        CompanyName = "B&B",
                        IsApproved = true,
                        PhoneNumber = "0888888666"
                    };

                    manager.Create(company, "company");
                    manager.AddToRole(company.Id, "Company");
                }

                if (!context.People.Any(p => p.Email == "person@abv.bg"))
                {
                    Person person = new Person
                    {
                        Email = "person@abv.bg",
                        FirstName = "Petar",
                        LastName = "Petrov",
                        UserName = "person@abv.bg"
                    };

                    manager.Create(person, "person");
                    manager.AddToRole(person.Id, "Person");
                }
            }

        }
    }
}
