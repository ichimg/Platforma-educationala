using EducationalPlatform.Domain.Models;
using EducationalPlatform.Services;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using EducationalPlatform.Commands;
using System.Windows.Input;
using EducationalPlatform.DataAccess.Repositories;

namespace EducationalPlatform.ViewModels.StudentViewModels
{
    public class SubjectDetailsViewModel : ViewModelBase
    {
        private Student loggedStudent;
        private Subject selectedSubject;

        private readonly IMessageBoxService messageBoxService;

        private readonly IRepository<Grade> gradeRepository;
        private readonly IRepository<Absence> absenceRepository;

        public SubjectDetailsViewModel(Subject selectedSubject,
            Student loggedStudent,
            IRepository<Grade> gradeRepository,
            IRepository<Absence> absenceRepository,
            IMessageBoxService messageBoxService)
        {
            this.selectedSubject = selectedSubject ?? throw new ArgumentNullException(nameof(selectedSubject));
            this.loggedStudent = loggedStudent ?? throw new ArgumentNullException(nameof(loggedStudent));
            this.gradeRepository = gradeRepository ?? throw new ArgumentNullException(nameof(gradeRepository));
            this.absenceRepository = absenceRepository ?? throw new ArgumentNullException(nameof(absenceRepository));
            this.messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));

            Grades = new ObservableCollection<Grade>(RefreshGradesList());
            Absences = new ObservableCollection<Absence>(RefreshAbsencesList());
        }

        public IEnumerable<Grade> RefreshGradesList()
        {
            return gradeRepository.GetAll()
                .Where(g => g.StudentId == loggedStudent.Id && g.SubjectId == selectedSubject.Id)
                .OrderBy(g => g.Semester)
                .ThenBy(g => g.Subject.Name)
                .ThenByDescending(g => g.Value);
        }

        public IEnumerable<Absence> RefreshAbsencesList()
        {
            return absenceRepository.GetAll()
                .Where(a => a.StudentId == loggedStudent.Id && a.SubjectId == selectedSubject.Id)
                .OrderBy(a => a.Semester)
                .ThenBy(a => a.Subject.Name)
                .ThenByDescending(a => a.Date);
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

        private ICommand calculateAverageCommand;
        public ICommand CalculateAverageCommand
        {
            get
            {
                if (calculateAverageCommand is null)
                {
                    calculateAverageCommand = new RelayCommand(CalculateAverage, param =>  Semester != null);
                }
                return calculateAverageCommand;
            }
        }

        private void CalculateAverage()
        {
            var grades = Grades.Where(g => g.Semester == (ESemester)Int32.Parse(Semester));

            int gradesCount = grades.Count();

            if (selectedSubject.HasThesis)
            {
                CalculateAverageWithThesis(grades, selectedSubject);
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
            messageBoxService.ShowInformation($"Media calculata pentru {selectedSubject}, semestrul {Semester} este: {average.ToString("0.00")}");
        }

        private void CalculateAverageWithThesis(IEnumerable<Grade> grades, Subject chosenSubject)
        {
            if (grades.Count() < 4)
            {
                messageBoxService.ShowError("Elevul nu are minim 3 note si o nota de teza!");
                return;
            }

            if (Grades.All(g => !g.IsThesis))
            {
                messageBoxService.ShowError("Elevul nu are nota pentru teza!");
                return;
            }

            int gradeSum = 0;
            grades.Where(g => !g.IsThesis).ToList().ForEach(g => gradeSum += g.Value);
            int thesisValue = grades.Where(g => g.IsThesis).FirstOrDefault().Value;

            decimal average = ((decimal)gradeSum / (grades.Count() - 1) * 3 + thesisValue) / 4;
            messageBoxService.ShowInformation($"Media calculata pentru {selectedSubject}, semestrul {Semester} este: {average.ToString("0.00")}");
        }
    }
}
