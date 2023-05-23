using EducationalPlatform.DataAccess.Exceptions;
using EducationalPlatform.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EducationalPlatform.DataAccess.Repositories
{
    public class StudentRepository : IRepository<Student>
    {
        private readonly EducationalPlatformDbContext dbContext;

        public StudentRepository(EducationalPlatformDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void Add(Student entity)
        {
            dbContext.Students.Add(entity);
            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var studentFromDb = GetById(id);

            if (studentFromDb == null)
            {
                throw new EntityNotFoundException(id);
            }

            dbContext.Students.Remove(studentFromDb);

            dbContext.SaveChanges();
        }

        public IEnumerable<Student> GetAll()
        {
            return dbContext.Students.Include(s => s.Classroom).ThenInclude(c => c.Specialization).Include(s => s.Person);
        }

        public Student GetById(int id)
        {
            return dbContext.Students.Include(s => s.Classroom).ThenInclude(c => c.Specialization).Include(s => s.Person).FirstOrDefault(s => s.Id == id);
        }

        public void Update(Student entity)
        {
            dbContext.Students.Update(entity);

            dbContext.SaveChanges();
        }
    }
}
