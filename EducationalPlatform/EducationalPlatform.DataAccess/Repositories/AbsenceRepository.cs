using EducationalPlatform.DataAccess.Exceptions;
using EducationalPlatform.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalPlatform.DataAccess.Repositories
{
    public class AbsenceRepository : IRepository<Absence>
    {
        private readonly EducationalPlatformDbContext dbContext;

        public AbsenceRepository(EducationalPlatformDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void Add(Absence entity)
        {
            dbContext.Absences.Add(entity);
            dbContext.SaveChanges();
        }

        public IEnumerable<Absence> GetAll()
        {
            return dbContext.Absences.Include(a => a.Student).Include(a => a.Subject).Include(a => a.Teacher);
        }

        public Absence GetById(int id)
        {
            return dbContext.Absences.Include(a => a.Student).Include(a => a.Subject).Include(a => a.Teacher).FirstOrDefault(a => a.Id == id);
        }

        public void Update(Absence grade)
        {
            dbContext.Absences.Update(grade);
            dbContext.SaveChanges();

        }

        public void Delete(int id)
        {
            var absenceFromDb = GetById(id);

            if (absenceFromDb == null)
            {
                throw new EntityNotFoundException(id);
            }

            dbContext.Absences.Remove(absenceFromDb);

            dbContext.SaveChanges();
        }
    }
}
