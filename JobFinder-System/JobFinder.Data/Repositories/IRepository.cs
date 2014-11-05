using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Data.Repositories
{
    public interface IRepository<T> where T: class
    {
        IQueryable<T> All();

        T Find(int id);

        void Add(T item);

        void Update(T item); 

        T Delete(int id); 

        T Delete(T item);

        void SaveChanges();
    }
}
