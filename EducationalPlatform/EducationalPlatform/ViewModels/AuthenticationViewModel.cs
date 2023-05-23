﻿using EducationalPlatform.Commands;
using EducationalPlatform.DataAccess.Models;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.Services;
using System;
using System.Linq;
using System.Windows.Input;

namespace EducationalPlatform.ViewModels
{
    public class AuthenticationViewModel : ViewModelBase
    {
        public event Action RequestClose;

        private readonly WindowService windowService;
        private readonly IMessageBoxService messageBoxService;
        private readonly IRepository<Person> personRepository;
        private readonly IRepository<Student> studentRepository;
        private readonly IRepository<Teacher> teacherRepository;
        private readonly IRepository<Classroom> classroomRepository;
        private readonly IRepository<Specialization> specializationRepository;
        private readonly IRepository<Subject> subjectRepository;
        private readonly IRepository<TeachingMaterial> teachingMaterialRepository;


        public AuthenticationViewModel(IMessageBoxService messageBoxService,
            WindowService windowService,
            IRepository<Person> personRepository,
            IRepository<Student> studentRepository,
            IRepository<Teacher> teacherRepository,
            IRepository<Classroom> classroomRepository,
            IRepository<Specialization> specializationRepository,
            IRepository<Subject> subjectRepository,
            IRepository<TeachingMaterial> teachingMaterialRepository
            )
        {
            this.windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            this.personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
            this.studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            this.messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            this.teacherRepository = teacherRepository ?? throw new ArgumentNullException(nameof(teacherRepository));
            this.classroomRepository = classroomRepository ?? throw new ArgumentNullException(nameof(classroomRepository));
            this.specializationRepository = specializationRepository ?? throw new ArgumentNullException(nameof(specializationRepository));
            this.subjectRepository = subjectRepository ?? throw new ArgumentNullException(nameof(subjectRepository));
            this.teachingMaterialRepository = teachingMaterialRepository ?? throw new ArgumentNullException(nameof(teachingMaterialRepository));
        }

        private string username;
        public string Username
        {
            get { return username; }
            set { username = value; NotifyPropertyChanged(nameof(Username)); }
        }

        public string Password { private get; set; }

        private ICommand openAdministratorViewCommand;
        public ICommand OpenAdministratorViewCommand
        {
            get
            {
                if (openAdministratorViewCommand is null)
                {
                    openAdministratorViewCommand = new RelayCommand(() => Login());
                }
                return openAdministratorViewCommand;
            }
        }

        public void Login()
        {
            var loggedUser = personRepository.GetAll().Where(p => p.Username == Username && p.Password == Password).FirstOrDefault();

            if (loggedUser != null && loggedUser.Role == ERole.Administrator)
            {
                windowService.ShowAdminView(windowService, 
                    personRepository,
                    studentRepository,
                    teacherRepository,
                    classroomRepository,
                    specializationRepository,
                    subjectRepository);
                RequestClose?.Invoke();
                return;
            }

            if (loggedUser != null && loggedUser.Role == ERole.Teacher)
            {
                windowService.ShowTeacherView(loggedUser,
                    windowService,
                    personRepository,
                    studentRepository,
                    teacherRepository,
                    classroomRepository,
                    specializationRepository,
                    subjectRepository,
                    teachingMaterialRepository);
                RequestClose?.Invoke();
                return;
            }

             messageBoxService.ShowError("Login failed");
           
        }
    }
}
