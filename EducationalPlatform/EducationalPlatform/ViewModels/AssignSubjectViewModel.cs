using EducationalPlatform.DataAccess.Models;
using System;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.Commands;
using System.Windows.Input;
using System.Linq;
using EducationalPlatform.Extensions;
using System.Collections.ObjectModel;

namespace EducationalPlatform.ViewModels
{
    public class AssignSubjectViewModel : ViewModelBase
    {
        private readonly AdministratorViewModel administratorViewModel;
        private readonly TeacherDetailsViewModel teacherDetailsViewModel;
        private readonly IRepository<Subject> subjectRepository;

        public AssignSubjectViewModel(AdministratorViewModel administratorViewModel,
            TeacherDetailsViewModel teacherDetailsViewModel,
            IRepository<Subject> subjectRepository)
        {
            this.administratorViewModel = administratorViewModel ?? throw new ArgumentNullException(nameof(administratorViewModel));
            this.teacherDetailsViewModel = teacherDetailsViewModel ?? throw new ArgumentNullException(nameof(teacherDetailsViewModel));
            this.subjectRepository = subjectRepository ?? throw new ArgumentNullException(nameof(subjectRepository));

            AllSubjects = new ObservableCollection<Subject>(administratorViewModel.Subjects
            .Where(c => c.Teachers.All(t => t.Id != administratorViewModel.SelectedTeacher.Id)));
        }

        private ObservableCollection<Subject> allSubjects;
        public ObservableCollection<Subject> AllSubjects
        {
            get { return allSubjects; }
            set
            {
                allSubjects = value;
                NotifyPropertyChanged(nameof(AllSubjects));
            }
        }

        private string chosenSubject;
        public string ChosenSubject
        {
            get { return chosenSubject; }
            set
            {
                chosenSubject = value;
                NotifyPropertyChanged(nameof(ChosenSubject));
            }
        }

        private ICommand assignCommand;
        public ICommand AssignCommand
        {
            get
            {
                if (assignCommand is null)
                {
                    assignCommand = new RelayCommand(() => AssignSubject());
                }
                return assignCommand;
            }
        }


        private void AssignSubject()
        {
            var assignedSubject = subjectRepository.GetAll().Where(c => c.Name == ChosenSubject).FirstOrDefault();
            assignedSubject.Teachers.Add(administratorViewModel.SelectedTeacher);

            subjectRepository.Update(assignedSubject);

            AllSubjects.Clear();
            var list = subjectRepository.GetAll()
            .Where(c => c.Teachers.All(t => t.Id != administratorViewModel.SelectedTeacher.Id));
            AllSubjects.AddRange(list);

            teacherDetailsViewModel.TeacherSubjects.Add(assignedSubject);

        }

    }
}
