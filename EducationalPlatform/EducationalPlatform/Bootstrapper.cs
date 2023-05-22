using EducationalPlatform.DataAccess;
using EducationalPlatform.DataAccess.Models;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.Services;
using EducationalPlatform.ViewModels;

namespace EducationalPlatform
{
    public class Bootstrapper
    {

        public AuthenticationViewModel Run()
        {
            WindowService windowService = new WindowService();
            IMessageBoxService messageBoxService = new MessageBoxService();
            EducationalPlatformDbContext dbContext = new EducationalPlatformDbContext();
            IRepository<Person> personRepository = new PersonRepository(dbContext);
            IRepository<Student> studentRepository = new StudentRepository(dbContext);
            IRepository<Teacher> teacherRepository = new TeacherRepository(dbContext);
            IRepository<Classroom> classroomRepository = new ClassroomRepository(dbContext);
            IRepository<Specialization> specializationRepository = new SpecializationRepository(dbContext);
            IRepository<Subject> subjectRepository = new SubjectRepository(dbContext);
            return new AuthenticationViewModel(messageBoxService,
                windowService,
                personRepository,
                studentRepository,
                teacherRepository,
                classroomRepository,
                specializationRepository,
                subjectRepository);
        }
    }
}
