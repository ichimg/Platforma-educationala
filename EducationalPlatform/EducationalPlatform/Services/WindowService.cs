using EducationalPlatform.DataAccess.Models;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.ViewModels;
using EducationalPlatform.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalPlatform.Services
{
    public class WindowService
    {
        public event Action EditStudentFormViewLaunched;
        public event Action EditTeacherFormViewLaunched;
        public void ShowAdminView(WindowService windowService,
            IRepository<Person> personRepository,
            IRepository<Student> studentRepository,
            IRepository<Teacher> teacherRepository,
            IRepository<Classroom> classroomRepository,
            IRepository<Specialization> specializationRepository,
            IRepository<Subject> subjectRepository)
        {
            AdministratorViewModel viewModel = new AdministratorViewModel(windowService,
                personRepository,
                studentRepository,
                teacherRepository,
                classroomRepository,
                specializationRepository,
                subjectRepository
                );
            AdministratorView window = new AdministratorView(viewModel);

            window.Show();
        }

        public void ShowAddOrEditStudentView(AdministratorViewModel administratorViewModel,
            WindowService windowService,
            IRepository<Person> personRepository,
            IRepository<Student> studentRepository,
            IRepository<Classroom> classroomRepository,
            bool isEditing)
        {
            AddOrEditStudentViewModel viewModel = new AddOrEditStudentViewModel(administratorViewModel, windowService, personRepository, studentRepository, classroomRepository, isEditing);
            AddOrEditStudentView window = new AddOrEditStudentView(viewModel);

            if (isEditing)
                EditStudentFormViewLaunched?.Invoke();

            window.ShowDialog();
        }

        public void ShowAddOrEditTeacherView(AdministratorViewModel administratorViewModel,
            WindowService windowService,
            IRepository<Person> personRepository,
            IRepository<Teacher> teacherRepository,
            bool isEditing)
        {
            AddOrEditTeacherViewModel viewModel = new AddOrEditTeacherViewModel(administratorViewModel, windowService,
                personRepository, teacherRepository, isEditing);
            AddOrEditTeacherView window = new AddOrEditTeacherView(viewModel);

            if (isEditing)
                EditTeacherFormViewLaunched?.Invoke();

            window.ShowDialog();
        }
    }

   public enum EDisplayedList
    {
        Students,
        Teachers,
        Specializations,
        Subjects,
        None
    }
}
