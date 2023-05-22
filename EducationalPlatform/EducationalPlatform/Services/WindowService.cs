using EducationalPlatform.DataAccess.Models;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.ViewModels;
using EducationalPlatform.Views;
using System;

namespace EducationalPlatform.Services
{
    public class WindowService
    {
        public event Action EditStudentFormViewLaunched;
        public event Action EditTeacherFormViewLaunched;
        public event Action EditSpecializationFormViewLaunched;
        public event Action EditSubjectFormViewLaunched;
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
            IRepository<Classroom> classroomRepository,
            bool isEditing)
        {
            AddOrEditTeacherViewModel viewModel = new AddOrEditTeacherViewModel(administratorViewModel, windowService,
                personRepository, teacherRepository, classroomRepository, isEditing);
            AddOrEditTeacherView window = new AddOrEditTeacherView(viewModel);

            if (isEditing)
            { 
                EditTeacherFormViewLaunched?.Invoke(); 
            }
            else
            {
                administratorViewModel.ListenAddOrEditTeacherViewModel(viewModel);
            }

            window.ShowDialog();
        }

        public void ShowAddOrEditSpecializationView(AdministratorViewModel administratorViewModel,
            WindowService windowService,
            IRepository<Specialization> specializationRepository,
            bool isEditing)
        {
            AddOrEditSpecializationViewModel viewModel = new AddOrEditSpecializationViewModel(specializationRepository, windowService,
                administratorViewModel, isEditing);
            AddOrEditSpecializationView window = new AddOrEditSpecializationView(viewModel);

            if (isEditing)
                EditSpecializationFormViewLaunched?.Invoke();

            window.ShowDialog();
        }

        public void ShowAddOrEditSubjectView(AdministratorViewModel administratorViewModel,
          WindowService windowService,
          IRepository<Subject> subjectRepository,
          bool isEditing)
        {
            AddOrEditSubjectViewModel viewModel = new AddOrEditSubjectViewModel(subjectRepository, windowService,
                administratorViewModel, isEditing);

            AddOrEditSubjectView window = new AddOrEditSubjectView(viewModel);

            if (isEditing)
                EditSubjectFormViewLaunched?.Invoke();

            window.ShowDialog();
        }

        public void ShowTeacherDetailsView(AdministratorViewModel administratorViewModel,
            WindowService windowService,
            IRepository<Teacher> teacherRepository,
            IRepository<Subject> subjectRepository,
            IRepository<Classroom> classroomRepository)
        {
            TeacherDetailsViewModel viewModel = new TeacherDetailsViewModel(administratorViewModel, 
                windowService, teacherRepository, subjectRepository, classroomRepository);

            TeacherDetailsView window = new TeacherDetailsView(viewModel);

            window.ShowDialog();
        }

        public void ShowAssignClassView(TeacherDetailsViewModel teacherDetailsViewModel,
            AdministratorViewModel administratorViewModel, IRepository<Classroom> classroomRepository)
        {
            AssignClassViewModel viewModel = new AssignClassViewModel(administratorViewModel, teacherDetailsViewModel, classroomRepository);

            AssignClassView window = new AssignClassView(viewModel);

            window.ShowDialog();
        }

        public void ShowAssignSubjectView(TeacherDetailsViewModel teacherDetailsViewModel,
            AdministratorViewModel administratorViewModel, IRepository<Subject> classroomRepository)
        {
            AssignSubjectViewModel viewModel = new AssignSubjectViewModel(administratorViewModel, teacherDetailsViewModel, classroomRepository);

            AssignSubjectView window = new AssignSubjectView(viewModel);

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
