using EducationalPlatform.Commands;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.Domain.Models;
using EducationalPlatform.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EducationalPlatform.ViewModels.AdministratorViewModels
{
    public class TeacherDetailsViewModel : ViewModelBase
    {
        private readonly AdministratorViewModel administratorViewModel;
        private readonly WindowService windowService;

        private readonly IRepository<Teacher> teacherRepository;
        private readonly IRepository<Subject> subjectRepository;
        private readonly IRepository<Classroom> classroomRepository;

        public TeacherDetailsViewModel(AdministratorViewModel administratorViewModel,
            WindowService windowService,
            IRepository<Teacher> teacherRepository,
            IRepository<Subject> subjectRepository,
            IRepository<Classroom> classroomRepository)
        {
            this.administratorViewModel = administratorViewModel ?? throw new ArgumentNullException(nameof(administratorViewModel));
            this.windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            this.teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
            this.subjectRepository = subjectRepository ?? throw new ArgumentNullException(nameof(subjectRepository));
            this.classroomRepository = classroomRepository ?? throw new ArgumentNullException(nameof(classroomRepository));
            TeacherClassrooms = new ObservableCollection<Classroom>(administratorViewModel.SelectedTeacher.Classrooms);
            TeacherSubjects = new ObservableCollection<Subject>(administratorViewModel.SelectedTeacher.Subjects);
        }

        private ObservableCollection<Classroom> teacherClassrooms;
        public ObservableCollection<Classroom> TeacherClassrooms
        {
            get
            {
                return teacherClassrooms;
            }

            set
            {
                teacherClassrooms = value;
                NotifyPropertyChanged(nameof(TeacherClassrooms));
            }
        }

        private ObservableCollection<Subject> teacherSubjects;
        public ObservableCollection<Subject> TeacherSubjects
        {
            get
            {
                return teacherSubjects;
            }

            set
            {
                teacherSubjects = value;
                NotifyPropertyChanged(nameof(TeacherSubjects));
            }
        }

        private Classroom? selectedClass;
        public Classroom? SelectedClass
        {
            get { return selectedClass; }
            set
            {
                selectedClass = value;
                NotifyPropertyChanged(nameof(SelectedClass));
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

        private ICommand openAssignClassCommand;
        public ICommand OpenAssignClassCommand
        {
            get
            {
                if (openAssignClassCommand is null)
                {
                    openAssignClassCommand = new RelayCommand(() => OpenAssignClassView());
                }
                return openAssignClassCommand;
            }
        }

        private ICommand openAssignSubjectCommand;
        public ICommand OpenAssignSubjectCommand
        {
            get
            {
                if (openAssignSubjectCommand is null)
                {
                    openAssignSubjectCommand = new RelayCommand(() => OpenAssignSubjectView());
                }
                return openAssignSubjectCommand;
            }
        }

        private ICommand deleteClassCommand;
        public ICommand DeleteClassCommand
        {
            get
            {
                if (deleteClassCommand is null)
                {
                    deleteClassCommand = new RelayCommand(() => DeleteClass(), param => SelectedClass != null);
                }
                return deleteClassCommand;
            }
        }

        private ICommand deleteSubjectCommand;
        public ICommand DeleteSubjectCommand
        {
            get
            {
                if (deleteSubjectCommand is null)
                {
                    deleteSubjectCommand = new RelayCommand(() => DeleteSubject(), param => SelectedClass != null);
                }
                return deleteSubjectCommand;
            }
        }

        private void DeleteClass()
        {
            administratorViewModel.SelectedTeacher.Classrooms.Remove(SelectedClass);
            teacherRepository.Update(administratorViewModel.SelectedTeacher);

            TeacherClassrooms.Remove(SelectedClass);
        }

        private void DeleteSubject()
        {
            administratorViewModel.SelectedTeacher.Subjects.Remove(SelectedSubject);
            teacherRepository.Update(administratorViewModel.SelectedTeacher);

            TeacherSubjects.Remove(SelectedSubject);
        }

        private void OpenAssignClassView()
        {
            windowService.ShowAssignClassView(this, administratorViewModel, classroomRepository);
        }

        private void OpenAssignSubjectView()
        {
            windowService.ShowAssignSubjectView(this, administratorViewModel, subjectRepository);
        }
    }
}

