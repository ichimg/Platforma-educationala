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

            builder.RegisterAssemblyTypes(Assembly.Load("EducationalPlatform.DataAccess"))
                .Where(a => a.Namespace.Contains("Models")).AsSelf();

            builder.RegisterType<EducationalPlatformDbContext>().AsSelf().SingleInstance();

            builder.RegisterType<AbsenceRepository>().As<IRepository<Absence>>().SingleInstance();
            builder.RegisterType<ClassroomRepository>().As<IRepository<Classroom>>().SingleInstance();
            builder.RegisterType<GradeRepository>().As<IRepository<Grade>>().SingleInstance();
            builder.RegisterType<PersonRepository>().As<IRepository<Person>>().SingleInstance();
            builder.RegisterType<SpecializationRepository>().As<IRepository<Specialization>>().SingleInstance();
            builder.RegisterType<StudentRepository>().As<IRepository<Student>>().SingleInstance();
            builder.RegisterType<SubjectRepository>().As<IRepository<Subject>>().SingleInstance();
            builder.RegisterType<TeacherRepository>().As<IRepository<Teacher>>().SingleInstance();
            builder.RegisterType<TeachingMaterialRepository>().As<IRepository<TeachingMaterial>>().SingleInstance();

            return builder.Build(); 
        }
    }
}
