using EducationalPlatform.DataAccess.Exceptions;
using EducationalPlatform.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationalPlatform.DataAccess.Repositories
{
    public class TeachingMaterialRepository : IRepository<TeachingMaterial>
    {
        private readonly EducationalPlatformDbContext dbContext;

        public TeachingMaterialRepository(EducationalPlatformDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public void Add(TeachingMaterial entity)
        {
            dbContext.Add(entity);
            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var teachingMaterialFromDb = GetById(id);

            if (teachingMaterialFromDb == null)
            {
                throw new EntityNotFoundException(id);
            }

            dbContext.TeachingMaterials.Remove(teachingMaterialFromDb);

            dbContext.SaveChanges();
        }

        public IEnumerable<TeachingMaterial> GetAll()
        {
            return dbContext.TeachingMaterials.Include(t => t.Subject);
        }

        public TeachingMaterial GetById(int id)
        {
            return dbContext.TeachingMaterials.Include(t => t.Subject).FirstOrDefault(t => t.Id == id);
        }

        public void Update(TeachingMaterial entity)
        {
            dbContext.TeachingMaterials.Update(entity);
            dbContext.SaveChanges();
        }
    }
}
