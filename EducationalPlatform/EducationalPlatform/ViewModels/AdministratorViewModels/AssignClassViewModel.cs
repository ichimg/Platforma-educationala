using EducationalPlatform.DataAccess.Models;
using System;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.Commands;
using System.Windows.Input;
using System.Linq;
using EducationalPlatform.Extensions;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Collections.Generic;

namespace EducationalPlatform.ViewModels.AdministratorViewModels
{
    public class AssignClassViewModel : ViewModelBase
    {
        private readonly AdministratorViewModel administratorViewModel;
        private readonly TeacherDetailsViewModel teacherDetailsViewModel;
        private readonly IRepository<Classroom> classroomRepository;

        public AssignClassViewModel(AdministratorViewModel administratorViewModel,
            TeacherDetailsViewModel teacherDetailsViewModel,
            IRepository<Classroom> classroomRepository)
        {
            this.administratorViewModel = administratorViewModel ?? throw new ArgumentNullException(nameof(administratorViewModel));
            this.teacherDetailsViewModel = teacherDetailsViewModel ?? throw new ArgumentNullException(nameof(teacherDetailsViewModel));
            this.classroomRepository = classroomRepository ?? throw new ArgumentNullException(nameof(classroomRepository));

            AllClassrooms = new ObservableCollection<Classroom>(administratorViewModel.Classrooms
            .Where(c => c.Teachers.All(t => t.Id != administratorViewModel.SelectedTeacher.Id)));
        }

        private ObservableCollection<Classroom> allClassrooms;
        public ObservableCollection<Classroom> AllClassrooms
        {
            get { return allClassrooms; }
            set
            {
                allClassrooms = value;
                NotifyPropertyChanged(nameof(AllClassrooms));
            }
        }

        private string chosenClassroom;
        public string ChosenClassroom
        {
            get { return chosenClassroom; }
            set
            {
                chosenClassroom = value;
                NotifyPropertyChanged(nameof(ChosenClassroom));
            }
        }

        private ICommand assignCommand;
        public ICommand AssignCommand
        {
            get
            {
                if (assignCommand is null)
                {
                    assignCommand = new RelayCommand(() => AssignClass());
                }
                return assignCommand;
            }
        }


        private void AssignClass()
        {
            var assignedClassroom = classroomRepository.GetAll().Where(c => c.FullName == ChosenClassroom).FirstOrDefault();
            assignedClassroom.Teachers.Add(administratorViewModel.SelectedTeacher);

            classroomRepository.Update(assignedClassroom);

            AllClassrooms.Clear();
            var list = classroomRepository.GetAll()
            .Where(c => c.Teachers.All(t => t.Id != administratorViewModel.SelectedTeacher.Id));
            AllClassrooms.AddRange(list);

            teacherDetailsViewModel.TeacherClassrooms.Add(assignedClassroom);

        }

    }
}
