using EducationalPlatform.Commands;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.Domain.Models;
using EducationalPlatform.Extensions;
using EducationalPlatform.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EducationalPlatform.ViewModels.TeacherViewModels
{
    public class StudentDetailsViewModel : ViewModelBase
    {
        private TeacherViewModel teacherViewModel;
        private readonly WindowService windowService;
        private readonly IRepository<Grade> gradeRepository;
        private readonly IRepository<Absence> absenceRepository;
        private readonly IRepository<Classroom> classroomRepository;
        private readonly IMessageBoxService messageBoxService;

        public StudentDetailsViewModel(TeacherViewModel teacherViewModel,
            WindowService windowService,
            IMessageBoxService messageBoxService,
            IRepository<Grade> gradeRepository,
            IRepository<Classroom> classroomRepository,
            IRepository<Absence> absenceRepository)
        {
            this.teacherViewModel = teacherViewModel ?? throw new ArgumentNullException(nameof(teacherViewModel));  
            this.windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            this.gradeRepository = gradeRepository ?? throw new ArgumentNullException(nameof(gradeRepository));
            this.absenceRepository = absenceRepository ?? throw new ArgumentNullException(nameof(absenceRepository));
            this.classroomRepository = classroomRepository ?? throw new ArgumentNullException(nameof(classroomRepository));
            this.messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));

            Grades = new ObservableCollection<Grade>(RefreshGradesList());
            Absences = new ObservableCollection<Absence>(RefreshAbsencesList());
        }

        public ObservableCollection<Subject> TeacherSubjects => new ObservableCollection<Subject>(teacherViewModel.LoggedTeacher.Subjects);

        public IEnumerable<Grade> RefreshGradesList()
        {
            var masteredClassroom = classroomRepository.GetAll().Where(c => c.TeacherId == teacherViewModel.LoggedTeacher.Id).FirstOrDefault();

            if (teacherViewModel.IsMasterMode && teacherViewModel.SelectedStudent.Classroom.TeacherId == teacherViewModel.LoggedTeacher.Id)
            {
                return gradeRepository.GetAll()
                .Where(g => g.StudentId == teacherViewModel.SelectedStudent.Id
                    && teacherViewModel.LoggedTeacher.Subjects.Any(s => g.SubjectId == s.Id)
                    || g.StudentId == teacherViewModel.SelectedStudent.Id && teacherViewModel.SelectedStudent.ClassroomId == masteredClassroom.Id)
                .OrderByDescending(g => g.Semester)
                .ThenBy(g => g.Subject.Name)
                .ThenByDescending(g => g.Value);
            }

            else
            {
                return gradeRepository.GetAll()
               .Where(g => g.StudentId == teacherViewModel.SelectedStudent.Id 
                    && teacherViewModel.LoggedTeacher.Subjects.Any(s => g.SubjectId == s.Id))
               .OrderByDescending(g => g.Semester)
               .ThenBy(g => g.Subject.Name)
               .ThenByDescending(g => g.Value);
            }
        }

        public IEnumerable<Absence> RefreshAbsencesList()
        {
            var masteredClassroom = classroomRepository.GetAll().Where(c => c.TeacherId == teacherViewModel.LoggedTeacher.Id).FirstOrDefault();

            if (teacherViewModel.IsMasterMode && teacherViewModel.SelectedStudent.Classroom.TeacherId == teacherViewModel.LoggedTeacher.Id)
            {
                return absenceRepository.GetAll()
               .Where(a => a.StudentId == teacherViewModel.SelectedStudent.Id 
                    && teacherViewModel.LoggedTeacher.Subjects.Any(s => a.SubjectId == s.Id)
                    || a.StudentId == teacherViewModel.SelectedStudent.Id && teacherViewModel.SelectedStudent.ClassroomId == masteredClassroom.Id)
               .OrderByDescending(a => a.Semester)
               .ThenBy(a => a.Subject.Name)
               .ThenByDescending(a => a.Date);
            }
            else
            {
                return absenceRepository.GetAll()
               .Where(a => a.StudentId == teacherViewModel.SelectedStudent.Id 
               && teacherViewModel.LoggedTeacher.Subjects.Any(s => a.SubjectId == s.Id))
               .OrderByDescending(a => a.Semester)
               .ThenBy(a => a.Subject.Name)
               .ThenByDescending(a => a.Date);
            }
        }

        private ObservableCollection<Grade> grades;
        public ObservableCollection<Grade> Grades
        {
            get
            {
                return grades;
            }

            set
            {
                grades = value;
                NotifyPropertyChanged(nameof(Grades));
            }
        }

        private Grade? selectedGrade;
        public Grade? SelectedGrade
        {
            get { return selectedGrade; }
            set
            {
                selectedGrade = value;
                NotifyPropertyChanged(nameof(SelectedGrade));
            }
        }

        private ObservableCollection<Absence> absences;
        public ObservableCollection<Absence> Absences
        {
            get
            {
                return absences;
            }

            set
            {
                absences = value;
                NotifyPropertyChanged(nameof(Absences));
            }
        }

        private Absence? selectedAbsence;
        public Absence? SelectedAbsence
        {
            get { return selectedAbsence; }
            set
            {
                selectedAbsence = value;
                NotifyPropertyChanged(nameof(SelectedAbsence));
            }
        }

        private string selectedSubject;
        public string SelectedSubject
        {
            get { return selectedSubject; }
            set
            {
                selectedSubject = value;
                NotifyPropertyChanged(nameof(SelectedSubject));
            }
        }

        private string semester;
        public string Semester
        {
            get
            {
                return semester;
            }

            set
            {
                semester = value;
                NotifyPropertyChanged(nameof(Semester));
            }
        }

        private ICommand openAddGradeCommand;
        public ICommand OpenAddGradeCommand
        {
            get
            {
                if (openAddGradeCommand is null)
                {
                    openAddGradeCommand = new RelayCommand(() => OpenAddGradeView());
                }
                return openAddGradeCommand;
            }
        }

        private ICommand cancelGradeCommand;
        public ICommand CancelGradeCommand
        {
            get
            {
                if (cancelGradeCommand is null)
                {
                    cancelGradeCommand = new RelayCommand(CancelGrade,
                        param => SelectedGrade != null 
                    && SelectedGrade.IsCanceled == false);
                }
                return cancelGradeCommand;
            }
        }

        private ICommand openAddAbsenceCommand;
        public ICommand OpenAddAbsenceCommand
        {
            get
            {
                if (openAddAbsenceCommand is null)
                {
                    openAddAbsenceCommand = new RelayCommand(() => OpenAddAbsenceView());
                }
                return openAddAbsenceCommand;
            }
        }

        private ICommand motivateAbsenceCommand;
        public ICommand MotivateAbsenceCommand
        {
            get
            {
                if (motivateAbsenceCommand is null)
                {
                    motivateAbsenceCommand = new RelayCommand(MotivateAbsence,
                        param => SelectedAbsence != null
                    && SelectedAbsence.IsMotivated == false);
                }
                return motivateAbsenceCommand;
            }
        }

        private ICommand calculateAverageCommand;
        public ICommand CalculateAverageCommand
        {
            get
            {
                if (calculateAverageCommand is null)
                {
                    calculateAverageCommand = new RelayCommand(CalculateAverage, param => SelectedSubject != null && Semester != null);
                }
                return calculateAverageCommand;
            }
        }

        private void CalculateAverage()
        {
            var chosenSubject = TeacherSubjects.Where(s => s.Name == SelectedSubject).FirstOrDefault();
            var grades = gradeRepository.GetAll()
                .Where(g => g.StudentId == teacherViewModel.SelectedStudent.Id
                && g.SubjectId == chosenSubject.Id 
                && g.Semester == (ESemester)Int32.Parse(Semester)
                && !g.IsCanceled);

            int gradesCount = grades.Count();

            if(chosenSubject.HasThesis)
            {
                CalculateAverageWithThesis(grades, chosenSubject);
                return;
            }

            if (grades.Count() < 3)
            {
                messageBoxService.ShowError("Elevul nu are minim 3 note!");
                return;
            }

            int gradeSum = 0;
            grades.ToList().ForEach(g => gradeSum += g.Value);
            decimal average = (decimal)gradeSum / grades.Count();
            messageBoxService.ShowInformation($"Media calculata pentru {SelectedSubject}, semestrul {Semester} este: {average.ToString("0.00")}");
        }

        private void CalculateAverageWithThesis(IEnumerable<Grade> grades, Subject chosenSubject)
        {
            if (grades.Count() < 4)
            {
                messageBoxService.ShowError("Elevul nu are minim 3 note si o nota de teza!");
                return;
            }

            if(Grades
                .Where(g => g.StudentId == teacherViewModel.SelectedStudent.Id && g.SubjectId == chosenSubject.Id).All(g => !g.IsThesis))
            {
                messageBoxService.ShowError("Elevul nu are nota pentru teza!");
                return;
            }

            int gradeSum = 0;
            grades.Where(g => !g.IsThesis).ToList().ForEach(g =>  gradeSum += g.Value);
            int thesisValue = grades.Where(g => g.IsThesis).FirstOrDefault().Value;

            decimal average = ((decimal)gradeSum / (grades.Count() - 1) * 3 + thesisValue) / 4;
            messageBoxService.ShowInformation($"Media calculata pentru {SelectedSubject}, semestrul {Semester} este: {average.ToString("0.00")}");
        }

        private void OpenAddAbsenceView()
        {
            windowService.ShowAddAbsenceView(this, teacherViewModel.LoggedTeacher, messageBoxService, teacherViewModel.SelectedStudent, absenceRepository);
        }

        private void MotivateAbsence()
        {
            if (!messageBoxService.AskConfirmation("Esti sigur ca vrei sa motivezi absenta selectata?"))
            {
                return;
            }

            SelectedAbsence.IsMotivated = true;
            absenceRepository.Update(SelectedAbsence);

            Absences.Clear();
            var list = RefreshAbsencesList();
            Absences.AddRange(list);
        }

        private void CancelGrade()
        {
            if (!messageBoxService.AskConfirmation("Esti sigur ca vrei sa anulezi nota selectata?"))
            {
                return;
            }

            SelectedGrade.IsCanceled = true;
            gradeRepository.Update(SelectedGrade);

            Grades.Clear();
            var list = RefreshGradesList();
            Grades.AddRange(list);
        }

        private void OpenAddGradeView()
        {
            windowService.ShowAddGradeView(this, teacherViewModel.LoggedTeacher, messageBoxService, teacherViewModel.SelectedStudent, gradeRepository);
        }
    }
}
