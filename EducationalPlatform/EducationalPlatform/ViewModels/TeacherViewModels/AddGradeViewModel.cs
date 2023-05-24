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
    public class AddGradeViewModel : ViewModelBase
    {
        private readonly StudentDetailsViewModel studentDetailsViewModel;
        private Teacher loggedTeacher;
        private IMessageBoxService messageBoxService;
        private Student? selectedStudent;
        private readonly IRepository<Grade> gradeRepository;

        public AddGradeViewModel(StudentDetailsViewModel studentDetailsViewModel, Teacher loggedTeacher, IMessageBoxService messageBoxService, Student? selectedStudent, IRepository<Grade> gradeRepository)
        {
            this.loggedTeacher = loggedTeacher ?? throw new ArgumentNullException(nameof(loggedTeacher));
            this.selectedStudent = selectedStudent ?? throw new ArgumentNullException(nameof(selectedStudent));
            this.gradeRepository = gradeRepository ?? throw new ArgumentNullException(nameof(gradeRepository));
            this.messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            this.studentDetailsViewModel = studentDetailsViewModel ?? throw new ArgumentNullException(nameof(studentDetailsViewModel));
        }

        public ObservableCollection<Subject> TeacherSubjects => new ObservableCollection<Subject>(loggedTeacher.Subjects);

        private string grade;
        public string Grade
        {
            get
            {
                return grade;
            }

            set
            {
                grade = value;
                NotifyPropertyChanged(nameof(Grade));
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

        private bool isThesis;
        public bool IsThesis
        {
            get
            {
                return isThesis;
            }

            set
            {
                isThesis = value;
                NotifyPropertyChanged(nameof(IsThesis));
            }
        }

        private ICommand addGradeCommand;
        public ICommand AddGradeCommand
        {
            get
            {
                if(addGradeCommand is null)
                {
                    addGradeCommand = new RelayCommand(() => AddGrade()); 
                }

                return addGradeCommand;
            }
        }

        private void AddGrade()
        {
            if(!Single.TryParse(Grade, out float resultedGrade))
            {
                messageBoxService.ShowError("Formatul notei este invalid!");
                return;
            }

            Subject chosenSubject = loggedTeacher.Subjects.Where(s => s.Name == SubjectName).FirstOrDefault();

            if(chosenSubject is null)
            {
                messageBoxService.ShowError("Materia la care sa fie adaugata nota nu a fost selectata!");
                return;
            }

            if(!chosenSubject.HasThesis && IsThesis)
            {
                messageBoxService.ShowError("Materia aleasa nu poate avea nota de teza!");
                return;
            }

            if(string.IsNullOrEmpty(Semester))
            {
                messageBoxService.ShowError("Semestrul nu a fost selectat!");
                return;
            }

            Grade gradeToAdd = new Grade
            {
                Value = (int)Math.Ceiling(resultedGrade), // rounding the float
                StudentId = selectedStudent.Id,
                Date = DateTime.Now,
                SubjectId = chosenSubject.Id,
                Semester = (ESemester)Int32.Parse(this.Semester),
                IsCanceled = false,
                IsThesis = this.IsThesis
            };

            gradeRepository.Add(gradeToAdd);

            studentDetailsViewModel.Grades.Clear();
            var list = studentDetailsViewModel.RefreshGradesList();
            studentDetailsViewModel.Grades.AddRange(list);
        }
    }
}
