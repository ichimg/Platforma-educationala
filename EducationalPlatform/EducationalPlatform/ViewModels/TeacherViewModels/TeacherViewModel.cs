using EducationalPlatform.Commands;
using EducationalPlatform.DataAccess.Models;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.Extensions;
using EducationalPlatform.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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
        private readonly IRepository<Grade> gradeRepository;
        private readonly IRepository<Absence> absenceRepository;

        private readonly WindowService windowService;
        private readonly IMessageBoxService messageBoxService;

        public TeacherViewModel(Person loggedUser, 
            IMessageBoxService messageBoxService, WindowService windowService,
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
            LoggedTeacher = teacherRepository.GetAll().Where(t => t.PersonId == loggedUser.Id).FirstOrDefault();
            this.windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            this.messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            this.personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
            this.studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            this.teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
            this.classroomRepository = classroomRepository ?? throw new ArgumentNullException(nameof(classroomRepository));
            this.specializationRepository = specializationRepository ?? throw new ArgumentNullException(nameof(specializationRepository));
            this.subjectRepository = subjectRepository ?? throw new ArgumentNullException(nameof(subjectRepository));
            this.teachingMaterialRepository = teachingMaterialRepository ?? throw new ArgumentNullException(nameof(teachingMaterialRepository));
            this.gradeRepository = gradeRepository ?? throw new ArgumentNullException(nameof(gradeRepository));
            this.absenceRepository = absenceRepository ?? throw new ArgumentNullException(nameof(absenceRepository));

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

        private Student? selectedStudent;
        public Student? SelectedStudent
        {
            get { return selectedStudent; }
            set
            {
                selectedStudent = value;
                NotifyPropertyChanged(nameof(SelectedStudent));
            }
        }

        private TeachingMaterial? selectedTeachingMaterial;
        public TeachingMaterial? SelectedTeachingMaterial
        {
            get { return selectedTeachingMaterial; }
            set
            {
                selectedTeachingMaterial = value;
                NotifyPropertyChanged(nameof(SelectedTeachingMaterial));
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
                    seeStudentDetailsCommand = new RelayCommand(() => OpenStudentDetails(), 
                    param => 
                    DisplayedList == EDisplayedList.Students 
                    && SelectedStudent != null);
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
                    downloadCommand = new RelayCommand(() => DownloadTeachingMaterial(), param => DisplayedList == EDisplayedList.TeachingMaterials && SelectedTeachingMaterial != null);
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
                    deleteCommand = new RelayCommand(() => DeleteTeachingMaterial(), param => DisplayedList == EDisplayedList.TeachingMaterials && SelectedTeachingMaterial != null);
                }
                return deleteCommand;
            }
        }

        private ICommand activateMasterModeCommand;
        public ICommand ActivateMasterModeCommand
        {
            get
            {
                if (activateMasterModeCommand is null)
                {
                    activateMasterModeCommand = new RelayCommand(ActivateMasterMode, param => IsMasterMode == false);
                }
                return activateMasterModeCommand;
            }
        }

        private void ActivateMasterMode()
        {
            if((bool)!LoggedTeacher.IsMaster)
            {
                messageBoxService.ShowError("Nu esti diriginte la o anumita clasa!");
                return;
            }

            var masteredClassroom = classroomRepository.GetAll().Where(c => c.TeacherId == LoggedTeacher.Id).FirstOrDefault();

            IsMasterMode = true;
            Students.Clear();
            TeachingMaterialsList.Clear();

            var list = new ObservableCollection<Student>(studentRepository.GetAll()
                        .OrderByDescending(s => s.ClassroomId == masteredClassroom.Id)
                        .ThenBy(s => s.Classroom.Year)
                        .ThenBy(s => s.Classroom.Letter)
                        .ThenBy(s => s.Person.FullName)
                        .Where(s => LoggedTeacher.Classrooms.Any(c => s.ClassroomId == c.Id) || s.ClassroomId == masteredClassroom.Id));

            Students.AddRange(list);
        }

        public bool IsMasterMode { get; set; }



        private void DeleteTeachingMaterial()
        {
            teachingMaterialRepository.Delete(SelectedTeachingMaterial.Id);

            TeachingMaterialsList.Clear();
            var list = teachingMaterialRepository.GetAll();
            TeachingMaterialsList.AddRange(list);

        }

        private void DownloadTeachingMaterial()
        {
            string savePath = ConfigurationManager.AppSettings["TeachingMaterialsPath"];

            var document = teachingMaterialRepository.GetAll().FirstOrDefault(d => d.Id == SelectedTeachingMaterial.Id);

            File.WriteAllBytes($"{savePath}{document.Id}-{document.Subject.Name}-{document.Name}.pdf", document.Bytes);
            messageBoxService.ShowInformation("Material didactic descarcat cu succes!");
        }

        private void OpenAddWindow()
        {
            windowService.ShowAddTeachingMaterialView(this, teachingMaterialRepository, messageBoxService);
        }

        private void OpenStudentDetails()
        {
            windowService.ShowStudentDetailsView(this, windowService, messageBoxService, gradeRepository,
                absenceRepository);
        }
    }
}
