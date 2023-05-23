using EducationalPlatform.Commands;
using EducationalPlatform.DataAccess.Models;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EducationalPlatform.ViewModels.TeacherViewModels
{
    public class TeacherViewModel : ViewModelBase
    {
        public Teacher LoggedTeacher { get; set; }

        public event Action RequestShowStudentsList;
        public event Action RequestShowTeachingMaterialsList;

        private readonly IRepository<Person> personRepository;
        private readonly IRepository<Student> studentRepository;
        private readonly IRepository<Teacher> teacherRepository;
        private readonly IRepository<Classroom> classroomRepository;
        private readonly IRepository<Specialization> specializationRepository;
        private readonly IRepository<Subject> subjectRepository;
        private readonly IRepository<TeachingMaterial> teachingMaterialRepository;

        private readonly WindowService windowService;

        public TeacherViewModel(Person loggedUser, WindowService windowService,
            IRepository<Person> personRepository,
            IRepository<Student> studentRepository,
            IRepository<Teacher> teacherRepository,
            IRepository<Classroom> classroomRepository,
            IRepository<Specialization> specializationRepository,
            IRepository<Subject> subjectRepository,
            IRepository<TeachingMaterial>  teachingMaterialRepository)
        {
            LoggedTeacher = teacherRepository.GetAll().Where(t => t.PersonId == loggedUser.Id).FirstOrDefault();
            this.windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            this.personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
            this.studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            this.teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
            this.classroomRepository = classroomRepository ?? throw new ArgumentNullException(nameof(classroomRepository));
            this.specializationRepository = specializationRepository ?? throw new ArgumentNullException(nameof(specializationRepository));
            this.subjectRepository = subjectRepository ?? throw new ArgumentNullException(nameof(subjectRepository));
            this.teachingMaterialRepository = teachingMaterialRepository ?? throw new ArgumentNullException(nameof(teachingMaterialRepository));

            Students = new ObservableCollection<Student>(studentRepository.GetAll()
                .Where(s => LoggedTeacher.Classrooms.Any(c => s.ClassroomId == c.Id))
                .OrderBy(s => s.Classroom.Year)
                .ThenBy(s => s.Classroom.Letter)
                .ThenBy(s => s.Person.FullName));

            TeachingMaterialsList = new ObservableCollection<TeachingMaterial>(teachingMaterialRepository.GetAll()
                .Where(tm => LoggedTeacher
                .Subjects.Any(s => tm.SubjectId == s.Id)));

            DisplayedList = EDisplayedList.None;
        }

        public EDisplayedList DisplayedList { get; set; }

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

        private ObservableCollection<TeachingMaterial> teachingMaterialsList;
        public ObservableCollection<TeachingMaterial> TeachingMaterialsList
        {
            get
            {
                return teachingMaterialsList;
            }

            set
            {
                teachingMaterialsList = value;
                NotifyPropertyChanged(nameof(teachingMaterialsList));
            }
        }

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

        private ICommand showTeachingMaterialsListCommand;
        public ICommand ShowTeachingMaterialsListCommand
        {
            get
            {
                if (showTeachingMaterialsListCommand is null)
                {
                    showTeachingMaterialsListCommand = new RelayCommand(() => RequestShowTeachingMaterialsList?.Invoke());
                }
                return showTeachingMaterialsListCommand;
            }
        }

        private ICommand seeStudentDetailsCommand;
        public ICommand SeeStudentDetailsCommand
        {
            get
            {
                if (seeStudentDetailsCommand is null)
                {
                    seeStudentDetailsCommand = new RelayCommand(() => OpenStudentDetails(), param => DisplayedList == EDisplayedList.Students);
                }
                return seeStudentDetailsCommand;
            }
        }

        private ICommand openAddViewCommand;
        public ICommand OpenAddViewCommand
        {
            get
            {
                if (openAddViewCommand is null)
                {
                    openAddViewCommand = new RelayCommand(() => OpenAddWindow(), param => DisplayedList == EDisplayedList.TeachingMaterials);

                }
                return openAddViewCommand;
            }
        }


        private ICommand downloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                if (downloadCommand is null)
                {
                    downloadCommand = new RelayCommand(() => DownloadTeachingMaterial(), param => DisplayedList == EDisplayedList.TeachingMaterials);
                }
                return downloadCommand;
            }
        }

        private ICommand deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand is null)
                {
                    deleteCommand = new RelayCommand(() => DeleteTeachingMaterial(), param => DisplayedList == EDisplayedList.TeachingMaterials);
                }
                return deleteCommand;
            }
        }

        private void DeleteTeachingMaterial()
        {
            throw new NotImplementedException();
        }

        private void DownloadTeachingMaterial()
        {
            throw new NotImplementedException();
        }

        private void OpenAddWindow()
        {
            throw new NotImplementedException();
        }

        private void OpenStudentDetails()
        {
            throw new NotImplementedException();
        }
    }
}
