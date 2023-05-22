using EducationalPlatform.Commands;
using EducationalPlatform.DataAccess.Exceptions;
using EducationalPlatform.DataAccess.Models;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.Events;
using EducationalPlatform.Extensions;
using EducationalPlatform.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EducationalPlatform.ViewModels
{
    public class AdministratorViewModel : ViewModelBase
    {
        public event Action RequestShowStudentsList;
        public event Action RequestShowTeachersList;
        public event Action RequestShowSpecializationsList;
        public event Action RequestShowSubjectsList;


        private readonly IRepository<Person> personRepository;
        private readonly IRepository<Student> studentRepository;
        private readonly IRepository<Teacher> teacherRepository;
        private readonly IRepository<Classroom> classroomRepository;
        private readonly IRepository<Specialization> specializationRepository;
        private readonly IRepository<Subject> subjectRepository;

        private readonly WindowService windowService;

        public AdministratorViewModel(WindowService windowService,
            IRepository<Person> personRepository,
            IRepository<Student> studentRepository,
            IRepository<Teacher> teacherRepository,
            IRepository<Classroom> classroomRepository,
            IRepository<Specialization> specializationRepository,
            IRepository<Subject> subjectRepository)
        {
            this.windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            this.personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
            this.studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            this.teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
            this.classroomRepository = classroomRepository ?? throw new ArgumentNullException(nameof(classroomRepository));
            this.specializationRepository = specializationRepository ?? throw new ArgumentNullException(nameof(specializationRepository));
            this.subjectRepository = subjectRepository ?? throw new ArgumentNullException(nameof(subjectRepository));

            DisplayedList = EDisplayedList.None;
            Students = new ObservableCollection<Student>(studentRepository.GetAll());
            Teachers = new ObservableCollection<Teacher>(teacherRepository.GetAll());
            Specializations = new ObservableCollection<Specialization>(specializationRepository.GetAll());
            Subjects = new ObservableCollection<Subject>(subjectRepository.GetAll());
        }

        private ObservableCollection<Student> students;
        public ObservableCollection<Student> Students
        {
            get
            {
                return students;
            }

            set
            {
                students = value;
                NotifyPropertyChanged(nameof(Students));
            }
        }

        private ObservableCollection<Teacher> teachers;
        public ObservableCollection<Teacher> Teachers
        {
            get
            {
                return teachers;
            }

            set
            {
                teachers = value;
                NotifyPropertyChanged(nameof(Teachers));
            }
        }

        private ObservableCollection<Specialization> specializations;
        public ObservableCollection<Specialization> Specializations
        {
            get
            {
                return specializations;
            }

            set
            {
                specializations = value;
                NotifyPropertyChanged(nameof(specializations));
            }
        }

        private ObservableCollection<Subject> subjects;
        public ObservableCollection<Subject> Subjects
        {
            get
            {
                return subjects;
            }

            set
            {
                subjects = value;
                NotifyPropertyChanged(nameof(subjects));
            }
        }
        public ObservableCollection<Classroom> Classrooms => new ObservableCollection<Classroom>(classroomRepository.GetAll());

        private Student? selectedStudent;
        public Student? SelectedStudent
        {
            get { return selectedStudent; }
            set
            {
                selectedTeacher = null;
                selectedStudent = value;
                NotifyPropertyChanged(nameof(SelectedStudent));
            }
        }

        private Teacher? selectedTeacher;
        public Teacher? SelectedTeacher
        {
            get { return selectedTeacher; }
            set
            {
                selectedStudent = null;
                selectedTeacher = value;
                NotifyPropertyChanged(nameof(SelectedTeacher));
            }
        }

        private Specialization? selectedSpecialization;
        public Specialization? SelectedSpecialization
        {
            get { return selectedSpecialization; }
            set
            {
                selectedStudent = null;
                selectedTeacher = null;
                selectedSubject = null;
                selectedSpecialization = value;
                NotifyPropertyChanged(nameof(SelectedSpecialization));
            }
        }

        private Subject? selectedSubject;
        public Subject? SelectedSubject
        {
            get { return selectedSubject; }
            set
            {
                selectedStudent = null;
                selectedTeacher = null;
                selectedSpecialization = null;
                selectedSubject = value;
                NotifyPropertyChanged(nameof(SelectedSubject));
            }
        }

        public EDisplayedList DisplayedList { get; set; }

        private ICommand showStudentsListCommand;
        public ICommand ShowStudentsListCommand
        {
            get
            {
                if (showStudentsListCommand is null)
                {
                    showStudentsListCommand = new RelayCommand(() => RequestShowStudentsList?.Invoke());
                }
                return showStudentsListCommand;
            }
        }

        private ICommand showSpecializationsListCommand;
        public ICommand ShowSpecializationsListCommand
        {
            get
            {
                if (showSpecializationsListCommand is null)
                {
                    showSpecializationsListCommand = new RelayCommand(() => RequestShowSpecializationsList?.Invoke());
                }
                return showSpecializationsListCommand;
            }
        }

        private ICommand showTeachersListCommand;
        public ICommand ShowTeachersListCommand
        {
            get
            {
                if (showTeachersListCommand is null)
                {
                    showTeachersListCommand = new RelayCommand(() => RequestShowTeachersList?.Invoke());
                }
                return showTeachersListCommand;
            }
        }

        private ICommand showSubjectsListCommand;
        public ICommand ShowSubjectsListCommand
        {
            get
            {
                if (showSubjectsListCommand is null)
                {
                    showSubjectsListCommand = new RelayCommand(() => RequestShowSubjectsList?.Invoke());
                }
                return showSubjectsListCommand;
            }
        }


        private ICommand openAddViewCommand;
        public ICommand OpenAddViewCommand
        {
            get
            {
                if (openAddViewCommand is null)
                {
                    openAddViewCommand = new RelayCommand(() => OpenAddWindow());

                }
                return openAddViewCommand;
            }
        }

        private ICommand openEditViewCommand;
        /// <summary>
        ///		Button command to open the edit student form.
        /// </summary>
        public ICommand OpenEditViewCommand
        {
            get
            {
                if (openEditViewCommand is null)
                {
                    openEditViewCommand = new RelayCommand(() => OpenEditWindow());
                }
                return openEditViewCommand;
            }
        }

        private ICommand deleteCommand;
        /// <summary>
        ///		Button command to open the edit student form.
        /// </summary>
        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand is null)
                {
                    deleteCommand = new RelayCommand(() => DeleteEntity());
                }
                return deleteCommand;
            }
        }

        private ICommand seeDetailsCommand;
        public ICommand SeeDetailsCommand
        {
            get
            {
                if (seeDetailsCommand is null)
                {
                    seeDetailsCommand = new RelayCommand(() => OpenDetailsWindow(), param => SelectedTeacher != null);
                }
                return seeDetailsCommand;
            }
        }

        private void OpenDetailsWindow()
        {
            if (DisplayedList == EDisplayedList.Teachers)
            {
                windowService.ShowTeacherDetailsView(this, windowService, teacherRepository,subjectRepository, classroomRepository);
            }
        }

        private void OpenAddWindow()
        {
            if (DisplayedList == EDisplayedList.Students)
            {
                windowService.ShowAddOrEditStudentView(this, windowService, personRepository, studentRepository, classroomRepository, false);
            }

            if (DisplayedList == EDisplayedList.Teachers)
            {
                windowService.ShowAddOrEditTeacherView(this, windowService, personRepository, teacherRepository, classroomRepository, false);
                windowService.ShowTeacherDetailsView(this, windowService, teacherRepository, subjectRepository, classroomRepository);
            }

            if (DisplayedList == EDisplayedList.Specializations)
            {
                windowService.ShowAddOrEditSpecializationView(this, windowService, specializationRepository, false);
            }

            if (DisplayedList == EDisplayedList.Subjects)
            {
                windowService.ShowAddOrEditSubjectView(this, windowService, subjectRepository, false);
            }
        }

        private void OpenEditWindow()
        {
            if (DisplayedList == EDisplayedList.Students)
            {
                windowService.ShowAddOrEditStudentView(this, windowService, personRepository, studentRepository, classroomRepository, true);
            }

            if (DisplayedList == EDisplayedList.Teachers)
            {
                windowService.ShowAddOrEditTeacherView(this, windowService, personRepository, teacherRepository, classroomRepository, true);
            }

            if (DisplayedList == EDisplayedList.Specializations)
            {
                windowService.ShowAddOrEditSpecializationView(this, windowService, specializationRepository, true);
            }

            if (DisplayedList == EDisplayedList.Subjects)
            {
                windowService.ShowAddOrEditSubjectView(this, windowService, subjectRepository, true);
            }
        }

        private void DeleteEntity()
        {
            if (DisplayedList == EDisplayedList.Students && SelectedStudent != null)
            {
                var student = SelectedStudent;

                personRepository.Delete(student.PersonId);

                Students.Clear();
                Students.AddRange(studentRepository.GetAll());
            }

            if (DisplayedList == EDisplayedList.Teachers && SelectedTeacher != null)
            {
                var teacher = SelectedTeacher;

                personRepository.Delete(teacher.PersonId);

                Teachers.Clear();
                Teachers.AddRange(teacherRepository.GetAll());
            }

            if (DisplayedList == EDisplayedList.Specializations && SelectedSpecialization != null)
            {
                var specialization = SelectedSpecialization;

                specializationRepository.Delete(specialization.Id);

                Specializations.Clear();
                Specializations.AddRange(specializationRepository.GetAll());
            }
        }

        public void ListenAddOrEditTeacherViewModel(AddOrEditTeacherViewModel viewModel)
        {
            viewModel.NewTeacherCreated += Handle_NewTeacherCreated;
        }

        private void Handle_NewTeacherCreated(object sender, NewTeacherEventArgs e)
        {
            SelectedTeacher = e.Data;
        }
    }
}
