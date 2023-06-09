﻿using EducationalPlatform.DataAccess.Exceptions;
using EducationalPlatform.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationalPlatform.DataAccess.Repositories
{
    public class TeacherRepository : IRepository<Teacher>
    {
        private readonly EducationalPlatformDbContext dbContext;

        public TeacherRepository(EducationalPlatformDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public void Add(Teacher entity)
        {
            dbContext.Add(entity);
            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var teacherFromDb = GetById(id);

            if (teacherFromDb == null)
            {
                throw new EntityNotFoundException(id);
            }

            dbContext.Teachers.Remove(teacherFromDb);

            dbContext.SaveChanges();
        }

        public IEnumerable<Teacher> GetAll()
        {
            return dbContext.Teachers.Include(t => t.Person).Include(t => t.Subjects).ThenInclude(s => s.Specializations).Include(t => t.Classrooms);
        }

        public Teacher GetById(int id)
        {
            return dbContext.Teachers.Include(t => t.Person).Include(t => t.Subjects).ThenInclude(s => s.Specializations).Include(t => t.Classrooms).FirstOrDefault(t => t.Id == id);
        }

        public void Update(Teacher entity)
        {
            dbContext.Teachers.Update(entity);
            dbContext.SaveChanges();
        }
    }
}
