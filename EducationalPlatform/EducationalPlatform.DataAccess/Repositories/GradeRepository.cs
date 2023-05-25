using EducationalPlatform.DataAccess.Exceptions;
using EducationalPlatform.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationalPlatform.DataAccess.Repositories
{
    public class GradeRepository : IRepository<Grade>
    {
        private readonly EducationalPlatformDbContext dbContext;

        public GradeRepository(EducationalPlatformDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void Add(Grade entity)
        {
            dbContext.Grades.Add(entity);
            dbContext.SaveChanges();
        }

        public IEnumerable<Grade> GetAll()
        {
            return dbContext.Grades.Include(g => g.Student).Include(g => g.Subject);
        }

        public Grade GetById(int id)
        {
            return dbContext.Grades.Include(g => g.Student).Include(g => g.Subject).FirstOrDefault(g => g.Id == id);
        }

        public void Update(Grade grade)
        {
            dbContext.Grades.Update(grade);
            dbContext.SaveChanges();

        }

        public void Delete(int id)
        {
            var gradeFromDb = GetById(id);

            if (gradeFromDb == null)
            {
                throw new EntityNotFoundException(id);
            }

            dbContext.Grades.Remove(gradeFromDb);

            dbContext.SaveChanges();
        }
    }
}
