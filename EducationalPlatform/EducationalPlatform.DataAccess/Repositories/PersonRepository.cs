using EducationalPlatform.DataAccess.Exceptions;
using EducationalPlatform.DataAccess.Models;

namespace EducationalPlatform.DataAccess.Repositories
{
    public class PersonRepository : IRepository<Person>
    {
        private readonly EducationalPlatformDbContext dbContext;

        public PersonRepository(EducationalPlatformDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void Add(Person person)
        {
            dbContext.Persons.Add(person);
            dbContext.SaveChanges();
        }

        public IEnumerable<Person> GetAll()
        {
            return dbContext.Persons;
        }

        public Person GetById(int id)
        {
            return dbContext.Persons.FirstOrDefault(p => p.Id == id);
        }

        public void Update(Person person)
        {
            dbContext.Persons.Update(person);
            dbContext.SaveChanges();

        }

        public void Delete(int id)
        {
            var personFromDb = GetById(id);

            if (personFromDb == null)
            {
                throw new EntityNotFoundException(id);
            }

            dbContext.Persons.Remove(personFromDb);

            dbContext.SaveChanges();
        }
    }
}
