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

        //IRepository<Game> Games { get; }

       // IRepository<Guess> Guesses { get; }

        //IRepository<Notification> Notifications { get; }

        void SaveChanges();
    }
}
