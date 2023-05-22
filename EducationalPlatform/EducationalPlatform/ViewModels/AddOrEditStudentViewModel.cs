using ControlzEx.Standard;
using EducationalPlatform.Commands;
using EducationalPlatform.DataAccess.Models;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.Extensions;
using EducationalPlatform.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace EducationalPlatform.ViewModels
{
    public class AddOrEditStudentViewModel : ViewModelBase
    {
        private readonly IRepository<Person> personRepository;
        private readonly IRepository<Student> studentRepository;
        private readonly IRepository<Classroom> classroomRepository;
        private readonly WindowService windowService;

        private readonly AdministratorViewModel administratorViewModel;

        private bool isEditing;

        public AddOrEditStudentViewModel(AdministratorViewModel administratorViewModel,
            WindowService windowService,
            IRepository<Person> personRepository,
            IRepository<Student> studentRepository,
            IRepository<Classroom> classroomRepository,
            bool isEditing)
        {
            this.administratorViewModel = administratorViewModel ?? throw new ArgumentNullException(nameof(administratorViewModel));
            this.personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
            this.studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            this.classroomRepository = classroomRepository ?? throw new ArgumentNullException(nameof(classroomRepository));
            this.isEditing = isEditing;
            this.windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            windowService.EditStudentFormViewLaunched += Handle_EditStudentFormViewLaunched;
        }

        private void Handle_EditStudentFormViewLaunched()
        {
            FullName = administratorViewModel.SelectedStudent?.Person.FullName;
            Cnp = administratorViewModel.SelectedStudent?.Person.Cnp;
            Username = administratorViewModel.SelectedStudent?.Person.Username;
            Password = administratorViewModel.SelectedStudent?.Person.Password;
            FullClassName = administratorViewModel.SelectedStudent?.Classroom?.FullName;
        }

        public ObservableCollection<Classroom> AllClassrooms => administratorViewModel.Classrooms;

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

        private string fullClassName;
        public string FullClassName
        {
            get { return fullClassName; }
            set
            {
                fullClassName = value;
                NotifyPropertyChanged(nameof(FullClassName));
            }
        }

        private ICommand addOrEditStudentCommand;
        public ICommand AddOrEditStudentCommand
        {
            get
            {
                if (addOrEditStudentCommand is null)
                {
                    if (isEditing)
                    {
                        addOrEditStudentCommand = new RelayCommand(EditStudent);
                    }
                    else
                    {
                        addOrEditStudentCommand = new RelayCommand(AddStudent);
                    }
                }
                return addOrEditStudentCommand;
            }
        }

        private void EditStudent()
        {
            administratorViewModel.SelectedStudent.Person.FullName = this.FullName;
            administratorViewModel.SelectedStudent.Person.Cnp = this.Cnp;
            administratorViewModel.SelectedStudent.Person.Username = this.Username;
            administratorViewModel.SelectedStudent.Person.Password = this.Password;

            Classroom chosenClassroom = classroomRepository.GetAll().Where(c => c.FullName == FullClassName).FirstOrDefault();
            administratorViewModel.SelectedStudent.Classroom = chosenClassroom;

            var student = administratorViewModel.SelectedStudent;
            administratorViewModel.Students.Remove(student);
            administratorViewModel.Students.Add(student);
            administratorViewModel.SelectedStudent = student;

            studentRepository.Update(administratorViewModel.SelectedStudent);
        }

        private void AddStudent()
        {
            Person personToAdd = new Person
            {
                FullName = this.FullName,
                Cnp = this.Cnp,
                Username = this.Username,
                Password = this.Password,
                Role = ERole.Student
            };

            Classroom chosenClassroom = classroomRepository.GetAll().Where(c => c.FullName == FullClassName).FirstOrDefault();

            Student studentToAdd = new Student
            {
                Person = personToAdd,
                Classroom = chosenClassroom,
            };

            personRepository.Add(personToAdd);
            studentRepository.Add(studentToAdd);

            administratorViewModel.Students.Clear();
            var list = studentRepository.GetAll();
            administratorViewModel.Students.AddRange(list);
        }
    }
}
