using EducationalPlatform.Commands;
using EducationalPlatform.DataAccess.Models;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.Extensions;
using EducationalPlatform.Services;
using EducationalPlatform.ViewModels.AdministratorViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EducationalPlatform.ViewModels.TeacherViewModels
{
    public class AddAbsenceViewModel : ViewModelBase
    {
        private readonly StudentDetailsViewModel studentDetailsViewModel;
        private Teacher loggedTeacher;
        private IMessageBoxService messageBoxService;
        private Student? selectedStudent;
        private readonly IRepository<Absence> absenceRepository;

        public AddAbsenceViewModel(StudentDetailsViewModel studentDetailsViewModel,
            Teacher loggedTeacher,
            IMessageBoxService messageBoxService,
            Student? selectedStudent,
            IRepository<Absence> gradeRepository)
        {
            this.loggedTeacher = loggedTeacher ?? throw new ArgumentNullException(nameof(loggedTeacher));
            this.selectedStudent = selectedStudent ?? throw new ArgumentNullException(nameof(selectedStudent));
            this.absenceRepository = gradeRepository ?? throw new ArgumentNullException(nameof(gradeRepository));
            this.messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            this.studentDetailsViewModel = studentDetailsViewModel ?? throw new ArgumentNullException(nameof(studentDetailsViewModel));
        }

        public ObservableCollection<Subject> TeacherSubjects => new ObservableCollection<Subject>(loggedTeacher.Subjects);

        private DateTime date = DateTime.Now;
        public DateTime Date
        {
            get
            {
                return date;
            }

            set
            {
                if (date != value)
                {
                    date = value;
                    NotifyPropertyChanged(nameof(Date));
                }
            }
        }

        private string subjectName;
        public string SubjectName
        {
            get
            {
                return subjectName;
            }

            set
            {
                subjectName = value;
                NotifyPropertyChanged(nameof(subjectName));
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

        private ICommand addAbsenceCommand;
        public ICommand AddAbsenceCommand
        {
            get
            {
                if (addAbsenceCommand is null)
                {
                    addAbsenceCommand = new RelayCommand(() => AddAbsence());
                }

                return addAbsenceCommand;
            }
        }

        private void AddAbsence()
        {

            if (Date == default(DateTime))
            {
                messageBoxService.ShowError("Data este invalida!");
                return;
            }

            Subject chosenSubject = loggedTeacher.Subjects.Where(s => s.Name == SubjectName).FirstOrDefault();

            if (chosenSubject is null)
            {
                messageBoxService.ShowError("Materia la care sa fie adaugata nota nu a fost selectata!");
                return;
            }

            if (string.IsNullOrEmpty(Semester))
            {
                messageBoxService.ShowError("Semestrul nu a fost selectat!");
                return;
            }

            Absence absenceToAdd = new Absence
            { 
                StudentId = selectedStudent.Id,
                TeacherId = loggedTeacher.Id,
                Date = this.Date,
                SubjectId = chosenSubject.Id,
                Semester = (ESemester)Int32.Parse(this.Semester),
                IsMotivated = false,
            };

            absenceRepository.Add(absenceToAdd);

            studentDetailsViewModel.Absences.Clear();
            var list = studentDetailsViewModel.RefreshAbsencesList();
            studentDetailsViewModel.Absences.AddRange(list);
        }
    }
}
