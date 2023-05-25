using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.Domain.Models;
using EducationalPlatform.ViewModels.AdministratorViewModels;
using EducationalPlatform.ViewModels.StudentViewModels;
using EducationalPlatform.ViewModels.TeacherViewModels;
using EducationalPlatform.Views;
using EducationalPlatform.Views.StudentViews;
using EducationalPlatform.Views.TeacherViews;
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

        // Teacher windows
        public void ShowTeacherView(Person loggedUser,
          IMessageBoxService messageBoxService,
          WindowService windowService,
          IRepository<Person> personRepository,
          IRepository<Student> studentRepository,
          IRepository<Teacher> teacherRepository,
          IRepository<Classroom> classroomRepository,
          IRepository<Specialization> specializationRepository,
          IRepository<Subject> subjectRepository,
          IRepository<TeachingMaterial>  teachingMaterialRepository,
          IRepository<Grade> gradeRepository,
          IRepository<Absence> absenceRepository)
        {
            TeacherViewModel viewModel = new TeacherViewModel(loggedUser, 
                messageBoxService,
                windowService,
                personRepository,
                studentRepository,
                teacherRepository,
                classroomRepository,
                specializationRepository,
                subjectRepository,
                teachingMaterialRepository,
                gradeRepository,
                absenceRepository
                );
            TeacherView window = new TeacherView(viewModel);

            window.Show();
        }

        public void ShowStudentDetailsView(TeacherViewModel teacherViewModel,
            WindowService windowService,
            IMessageBoxService messageBoxService,
            IRepository<Grade> gradeRepository,
            IRepository<Absence> absenceRepository,
            IRepository<Classroom> classroomRepository)
        {
            StudentDetailsViewModel viewModel = new StudentDetailsViewModel(teacherViewModel, windowService, messageBoxService, gradeRepository,
                classroomRepository, absenceRepository);

            StudentDetailsView window = new StudentDetailsView(viewModel);

            window.ShowDialog();
        }

        public void ShowAddGradeView(StudentDetailsViewModel studentDetailsViewModel, Teacher loggedTeacher, IMessageBoxService messageBoxService, Student? selectedStudent, IRepository<Grade> gradeRepository)
        {
            AddGradeViewModel viewModel = new AddGradeViewModel(studentDetailsViewModel, loggedTeacher, messageBoxService, selectedStudent, gradeRepository);

            AddGradeView window = new AddGradeView(viewModel);

            window.ShowDialog();
        }

        public void ShowAddAbsenceView(StudentDetailsViewModel studentDetailsViewModel, Teacher loggedTeacher, IMessageBoxService messageBoxService, Student? selectedStudent, IRepository<Absence> absenceRepository)
        {
            AddAbsenceViewModel viewModel = new AddAbsenceViewModel(studentDetailsViewModel, loggedTeacher, messageBoxService, selectedStudent, absenceRepository);

            AddAbsenceView window = new AddAbsenceView(viewModel);

            window.ShowDialog();
        }

        public void ShowAddTeachingMaterialView(TeacherViewModel teacherViewModel, IRepository<TeachingMaterial> teachingMaterialRepository, IMessageBoxService messageBoxService)
        {
            AddTeachingMaterialViewModel viewModel = new AddTeachingMaterialViewModel(teacherViewModel, teachingMaterialRepository, messageBoxService);

            AddTeachingMaterialView window = new AddTeachingMaterialView(viewModel);

            window.ShowDialog();
        }

        public void ShowStudentView(Person loggedUser, IMessageBoxService messageBoxService, IRepository<Student> studentRepository, WindowService windowService,
            IRepository<Subject> subjectRepository,
            IRepository<TeachingMaterial> teachingMaterialRepository,
            IRepository<Grade> gradeRepository,
            IRepository<Absence> absenceRepository)
        {
            StudentViewModel viewModel = new StudentViewModel(loggedUser, messageBoxService, windowService, studentRepository,
                subjectRepository, teachingMaterialRepository, gradeRepository, absenceRepository);

            StudentView window = new StudentView(viewModel);

            window.Show();
        }

        public void ShowSubjectDetailsView(Subject selectedSubject, Student loggedStudent, IRepository<Grade> gradeRepository, IRepository<Absence> absenceRepository, IMessageBoxService messageBoxService)
        {
            SubjectDetailsViewModel viewModel = new SubjectDetailsViewModel(selectedSubject, loggedStudent, gradeRepository, absenceRepository, messageBoxService);

            SubjectDetailsView window = new SubjectDetailsView(viewModel);

            window.ShowDialog();
        }
    }

   public enum EDisplayedList
    {
        Students,
        Teachers,
        Specializations,
        Subjects,
        TeachingMaterials,
        None
    }
}
