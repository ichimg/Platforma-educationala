using EducationalPlatform.Commands;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.Domain.Models;
using EducationalPlatform.Extensions;
using EducationalPlatform.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EducationalPlatform.ViewModels.AdministratorViewModels
{
    public class AddOrEditTeacherViewModel : ViewModelBase
    {


        private readonly IRepository<Person> personRepository;
        private readonly IRepository<Teacher> teacherRepository;
        private readonly IRepository<Classroom> classroomRepository;
        private readonly WindowService windowService;

        private readonly AdministratorViewModel administratorViewModel;

        private bool isEditing;

        public AddOrEditTeacherViewModel(AdministratorViewModel administratorViewModel,
            WindowService windowService,
            IRepository<Person> personRepository,
            IRepository<Teacher> teacherRepository,
            IRepository<Classroom> classroomRepository,
            bool isEditing)
        {
            this.administratorViewModel = administratorViewModel ?? throw new ArgumentNullException(nameof(administratorViewModel));
            this.personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
            this.teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
            this.classroomRepository = classroomRepository ?? throw new ArgumentNullException(nameof(classroomRepository));
            this.isEditing = isEditing;
            this.windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            windowService.EditTeacherFormViewLaunched += Handle_EdiTeacherFormViewLaunched;
        }

        public ObservableCollection<Classroom> NoMasterClassrooms => new ObservableCollection<Classroom>(administratorViewModel.Classrooms.Where(c => c.Teacher == null));

        private void Handle_EdiTeacherFormViewLaunched()
        {
            FullName = administratorViewModel.SelectedTeacher?.Person.FullName;
            Cnp = administratorViewModel.SelectedTeacher?.Person.Cnp;
            Username = administratorViewModel.SelectedTeacher?.Person.Username;
            Password = administratorViewModel.SelectedTeacher?.Person.Password;
        }

        private string fullName;
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                NotifyPropertyChanged(nameof(FullName));
            }
        }

        private string cnp;
        public string Cnp
        {
            get { return cnp; }
            set
            {
                cnp = value;
                NotifyPropertyChanged(nameof(Cnp));
            }
        }

        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                NotifyPropertyChanged(nameof(Username));
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                NotifyPropertyChanged(nameof(Password));
            }
        }

        private string masterChosenClass;
        public string MasterChosenClass
        {
            get { return masterChosenClass; }
            set
            {
                masterChosenClass = value;
                NotifyPropertyChanged(nameof(MasterChosenClass));
            }
        }


        private bool isMaster;
        public bool IsMaster
        {
            get { return isMaster; }
            set
            {
                isMaster = value;
                NotifyPropertyChanged(nameof(IsMaster));
            }
        }

        private ICommand addOrEditTeacherCommand;
        public ICommand AddOrEditTeacherCommand
        {
            get
            {
                if (addOrEditTeacherCommand is null)
                {
                    if (isEditing)
                    {
                        addOrEditTeacherCommand = new RelayCommand(EditTeacher);
                    }
                    else
                    {
                        addOrEditTeacherCommand = new RelayCommand(AddTeacher);
                    }
                }
                return addOrEditTeacherCommand;
            }
        }

        private void AddTeacher()
        {
            Person personToAdd = new Person
            {
                FullName = FullName,
                Cnp = Cnp,
                Username = Username,
                Password = Password,
                Role = ERole.Teacher
            };


            Teacher teacherToAdd = new Teacher
            {
                Person = personToAdd,
                IsMaster = IsMaster
            };


            personRepository.Add(personToAdd);
            teacherRepository.Add(teacherToAdd);

            administratorViewModel.SelectedTeacher = teacherToAdd;

            administratorViewModel.Teachers.Clear();
            var list = teacherRepository.GetAll();
            administratorViewModel.Teachers.AddRange(list);
        }

        private void EditTeacher()
        {
            administratorViewModel.SelectedTeacher.Person.FullName = FullName;
            administratorViewModel.SelectedTeacher.Person.Cnp = Cnp;
            administratorViewModel.SelectedTeacher.Person.Username = Username;
            administratorViewModel.SelectedTeacher.Person.Password = Password;
            administratorViewModel.SelectedTeacher.IsMaster = IsMaster;

            Classroom previousClassroom = classroomRepository.GetAll().Where(c => c.TeacherId == administratorViewModel.SelectedTeacher.Id).FirstOrDefault();

            if (previousClassroom != null)
            {
                previousClassroom.TeacherId = null;
                previousClassroom.Teacher = null;
                classroomRepository.Update(previousClassroom);
            }

            Classroom classroomToMaster = classroomRepository.GetAll().Where(c => c.FullName == MasterChosenClass).FirstOrDefault();

            classroomToMaster.TeacherId = administratorViewModel.SelectedTeacher.Id;

            var teacher = administratorViewModel.SelectedTeacher;
            administratorViewModel.Teachers.Remove(teacher);
            administratorViewModel.Teachers.Add(teacher);
            administratorViewModel.SelectedTeacher = teacher;

            teacherRepository.Update(administratorViewModel.SelectedTeacher);
            classroomRepository.Update(classroomToMaster);
        }

    }
}
