using EducationalPlatform.DataAccess.Exceptions;
using EducationalPlatform.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationalPlatform.DataAccess.Repositories
{
    public class ClassroomRepository : IRepository<Classroom>
    {
        private readonly EducationalPlatformDbContext dbContext;

        public ClassroomRepository(EducationalPlatformDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public void Add(Classroom entity)
        {
            dbContext.Classrooms.Add(entity);
            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var classromFromDb = GetById(id);

            if (classromFromDb == null)
            {
                throw new EntityNotFoundException(id);
            }

            dbContext.Classrooms.Remove(classromFromDb);

            dbContext.SaveChanges();
        }

        public IEnumerable<Classroom> GetAll()
        {
            return dbContext.Classrooms.Include(c => c.Specialization).Include(c => c.Teacher);
        }

        public Classroom GetById(int id)
        {
            return dbContext.Classrooms.Include(c => c.Specialization).Include(c => c.Teacher).FirstOrDefault(c => c.Id == id);
        }

        public void Update(Classroom entity)
        {
            dbContext.Classrooms.Update(entity);
            dbContext.SaveChanges();
        }
    }
}
