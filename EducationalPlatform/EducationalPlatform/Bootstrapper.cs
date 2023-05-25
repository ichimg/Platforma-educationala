using Autofac;
using EducationalPlatform.Commands;
using EducationalPlatform.Domain.Models;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.Services;
using EducationalPlatform.ViewModels;
using EducationalPlatform.Views;
using System.Reflection;
using EducationalPlatform.DataAccess;

namespace EducationalPlatform
{
    public class Bootstrapper
    {

        public AuthenticationViewModel Run()
        {
            IContainer container = ConfigureContainer();
            using (var scope = container.BeginLifetimeScope())
            {
                return scope.Resolve<AuthenticationViewModel>(); 
            }
        }

        public IContainer ConfigureContainer()
        {
            ContainerBuilder builder = new();

            builder.RegisterAssemblyTypes(Assembly.Load("EducationalPlatform"))
                .Where(a => a.Namespace.Contains("Views.AdministratorViews")).AsSelf();
            builder.RegisterAssemblyTypes(Assembly.Load("EducationalPlatform"))
                .Where(a => a.Namespace.Contains("Views.TeacherViews")).AsSelf();
            builder.RegisterAssemblyTypes(Assembly.Load("EducationalPlatform"))
                .Where(a => a.Namespace.Contains("Views.StudentViews")).AsSelf();
            builder.RegisterType<AuthenticationView>().AsSelf();

            builder.RegisterType<RelayCommand>().AsSelf();
            builder.RegisterType<MessageBoxService>().As<IMessageBoxService>();
            builder.RegisterType<WindowService>().AsSelf();

            builder.RegisterAssemblyTypes(Assembly.Load("EducationalPlatform"))
                .Where(a => a.Namespace.Contains("ViewModels.AdministratorViewModels")).AsSelf();
            builder.RegisterAssemblyTypes(Assembly.Load("EducationalPlatform"))
                .Where(a => a.Namespace.Contains("ViewModels.TeacherViewModels")).AsSelf();
            builder.RegisterAssemblyTypes(Assembly.Load("EducationalPlatform"))
                .Where(a => a.Namespace.Contains("ViewModels.StudentViewModels")).AsSelf();
            builder.RegisterType<AuthenticationViewModel>().AsSelf();
            builder.RegisterType<ViewModelBase>().AsSelf();

            builder.RegisterAssemblyTypes(Assembly.Load("EducationalPlatform.DataAccess"))
                .Where(a => a.Namespace.Contains("Models")).AsSelf();

            builder.RegisterType<EducationalPlatformDbContext>().AsSelf().SingleInstance();

            builder.RegisterType<AbsenceRepository>().As<IRepository<Absence>>();
            builder.RegisterType<ClassroomRepository>().As<IRepository<Classroom>>();
            builder.RegisterType<GradeRepository>().As<IRepository<Grade>>();
            builder.RegisterType<PersonRepository>().As<IRepository<Person>>();
            builder.RegisterType<SpecializationRepository>().As<IRepository<Specialization>>();
            builder.RegisterType<StudentRepository>().As<IRepository<Student>>();
            builder.RegisterType<SubjectRepository>().As<IRepository<Subject>>();
            builder.RegisterType<TeacherRepository>().As<IRepository<Teacher>>();
            builder.RegisterType<TeachingMaterialRepository>().As<IRepository<TeachingMaterial>>();

            return builder.Build(); 
        }
    }
}
