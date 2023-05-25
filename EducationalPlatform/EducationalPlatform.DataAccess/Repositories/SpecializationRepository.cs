using EducationalPlatform.DataAccess.Exceptions;
using EducationalPlatform.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalPlatform.DataAccess.Repositories
{
    public class SpecializationRepository : IRepository<Specialization>
    {
        private readonly EducationalPlatformDbContext dbContext;

        public SpecializationRepository(EducationalPlatformDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void Add(Specialization entity)
        {
            dbContext.Specializations.Add(entity);  
            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var specializationFromDb = GetById(id);

            if (specializationFromDb == null)
            {
                throw new EntityNotFoundException(id);
            }

            dbContext.Specializations.Remove(specializationFromDb);

            dbContext.SaveChanges();
        }

        public IEnumerable<Specialization> GetAll()
        {
            return dbContext.Specializations.Include(s => s.Subjects);
        }

        public Specialization GetById(int id)
        {
            return dbContext.Specializations.Include(s => s.Subjects).FirstOrDefault(s => s.Id == id);
        }

        public void Update(Specialization entity)
        {
            dbContext.Specializations.Update(entity);
            dbContext.SaveChanges();
        }
    }
}
