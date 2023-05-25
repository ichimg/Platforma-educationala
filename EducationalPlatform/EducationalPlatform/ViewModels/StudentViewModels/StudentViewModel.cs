using EducationalPlatform.Commands;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.Domain.Models;
using EducationalPlatform.Services;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace EducationalPlatform.ViewModels.StudentViewModels
{
    public class StudentViewModel : ViewModelBase
    {
        public Student LoggedStudent { get; set; }

        public event Action RequestShowSubjectsList;
        public event Action RequestShowTeachingMaterialsList;

        private readonly Person loggedUser;
        private readonly IMessageBoxService messageBoxService;
        private readonly WindowService windowService;
        private readonly IRepository<Subject> subjectRepository;
        private readonly IRepository<TeachingMaterial> teachingMaterialRepository;
        private readonly IRepository<Grade> gradeRepository;
        private readonly IRepository<Absence> absenceRepository;
        private readonly IRepository<Student> studentRepository;

        public StudentViewModel(Person loggedUser, IMessageBoxService messageBoxService,
            WindowService windowService,
            IRepository<Student> studentRepository,
            IRepository<Subject> subjectRepository,
            IRepository<TeachingMaterial> teachingMaterialRepository,
            IRepository<Grade> gradeRepository,
            IRepository<Absence> absenceRepository)
        {
            LoggedStudent = studentRepository.GetAll().Where(s => s.PersonId == loggedUser.Id).FirstOrDefault();
            this.messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            this.windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            this.subjectRepository = subjectRepository ?? throw new ArgumentNullException(nameof(subjectRepository));
            this.teachingMaterialRepository = teachingMaterialRepository ?? throw new ArgumentNullException(nameof(teachingMaterialRepository));
            this.gradeRepository = gradeRepository ?? throw new ArgumentNullException(nameof(gradeRepository));
            this.absenceRepository = absenceRepository ?? throw new ArgumentNullException(nameof(absenceRepository));

            DisplayedList = EDisplayedList.None;

            Subjects = new ObservableCollection<Subject>(subjectRepository.GetAll().Where(s => s.Specializations.Any(sp => sp.Id == LoggedStudent.Classroom.SpecializationId)));
            TeachingMaterialsList = new ObservableCollection<TeachingMaterial>(teachingMaterialRepository.GetAll().Where(tm => Subjects.Any(s => s.Id == tm.SubjectId)));
        }

        public EDisplayedList DisplayedList { get; set; }

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
                NotifyPropertyChanged(nameof(Subjects));
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

        private Subject? selectedSubject;
        public Subject? SelectedSubject
        {
            get { return selectedSubject; }
            set
            {
                selectedSubject = value;
                NotifyPropertyChanged(nameof(SelectedSubject));
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

        private ICommand seeSubjectDetailsCommand;
        public ICommand SeeSubjectDetailsCommand
        {
            get
            {
                if (seeSubjectDetailsCommand is null)
                {
                    seeSubjectDetailsCommand = new RelayCommand(() => OpenSubjectDetails(),
                    param =>
                    DisplayedList == EDisplayedList.Students
                    && SelectedSubject != null);
                }
                return seeSubjectDetailsCommand;
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

        private void DownloadTeachingMaterial()
        {
            string savePath = ConfigurationManager.AppSettings["TeachingMaterialsPath"];

            var document = teachingMaterialRepository.GetAll().FirstOrDefault(d => d.Id == SelectedTeachingMaterial.Id);

            File.WriteAllBytes($"{savePath}{document.Id}-{document.Subject.Name}-{document.Name}.pdf", document.Bytes);
            messageBoxService.ShowInformation("Material didactic descarcat cu succes!");
        }

        private void OpenSubjectDetails()
        {
            windowService.ShowSubjectDetailsView(SelectedSubject, LoggedStudent, gradeRepository, absenceRepository, messageBoxService);
        }
    }
}
