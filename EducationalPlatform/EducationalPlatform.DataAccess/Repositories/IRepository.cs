using System.Collections.Generic;

namespace EducationalPlatform.DataAccess.Repositories
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Delete(int id);
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Update(T entity);
    }
}