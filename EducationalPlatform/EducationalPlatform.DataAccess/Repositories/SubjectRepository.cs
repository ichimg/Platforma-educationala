using EducationalPlatform.DataAccess.Exceptions;
using EducationalPlatform.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationalPlatform.DataAccess.Repositories
{
    public class SubjectRepository : IRepository<Subject>
    {
        private readonly EducationalPlatformDbContext dbContext;

        public SubjectRepository(EducationalPlatformDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void Add(Subject entity)
        {
            dbContext.Subjects.Add(entity);
            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var subjectFromDb = GetById(id);

            if (subjectFromDb == null)
            {
                throw new EntityNotFoundException(id);
            }

            dbContext.Subjects.Remove(subjectFromDb);

            dbContext.SaveChanges();
        }

        public IEnumerable<Subject> GetAll()
        {
            return dbContext.Subjects.Include(s => s.Specializations);
        }

        public Subject GetById(int id)
        {
            return dbContext.Subjects.Include(s => s.Specializations).FirstOrDefault(s => s.Id == id);
        }

        public void Update(Subject entity)
        {
            dbContext.Subjects.Update(entity);
            dbContext.SaveChanges();
        }
    }
}
