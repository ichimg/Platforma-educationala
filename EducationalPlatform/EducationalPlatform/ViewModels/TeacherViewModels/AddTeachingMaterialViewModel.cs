using EducationalPlatform.Commands;
using EducationalPlatform.DataAccess.Models;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.Extensions;
using EducationalPlatform.Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace EducationalPlatform.ViewModels.TeacherViewModels
{
    public class AddTeachingMaterialViewModel : ViewModelBase
    {
        private TeacherViewModel teacherViewModel;
        private readonly IMessageBoxService messageBoxService;
        private readonly IRepository<TeachingMaterial> teachingMaterialRepository;

        public AddTeachingMaterialViewModel(TeacherViewModel teacherViewModel, IRepository<TeachingMaterial> teachingMaterialRepository, IMessageBoxService messageBoxService)
        {
            this.teacherViewModel = teacherViewModel ?? throw new ArgumentNullException(nameof(teacherViewModel));
            this.teachingMaterialRepository = teachingMaterialRepository ?? throw new ArgumentNullException(nameof(teachingMaterialRepository));
            this.messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
        }

        public ObservableCollection<Subject> TeacherSubjects => new ObservableCollection<Subject>(teacherViewModel.LoggedTeacher.Subjects);

        private string name;
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                NotifyPropertyChanged(nameof(Name));
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

        private string filePath;
        public string FilePath
        {
            get
            {
                return filePath;
            }

            set
            {
                filePath = value;
                NotifyPropertyChanged(nameof(FilePath));
            }
        }

        private ICommand addCommand;
        public ICommand AddCommand
        {
            get
            {
                if (addCommand is null)
                {
                    addCommand = new RelayCommand(AddTeachingMaterial);
                }

                return addCommand;
            }
        }

        private void AddTeachingMaterial()
        {
            VerifyInput();

            string extension = Path.GetExtension(FilePath);

            if (extension == null || !extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                messageBoxService.ShowError($"Fisier invalid! Se accepta doar fisiere cu extensie {".PDF"}");
                return;
            }

            var chosenSubject = TeacherSubjects.Where(s => s.Name == SubjectName).FirstOrDefault();

            TeachingMaterial teachingMaterial = new TeachingMaterial
            {
                Name = this.Name,
                SubjectId = chosenSubject.Id,
                Bytes = File.ReadAllBytes(FilePath),
                Semester = (ESemester)Int32.Parse(Semester)
            };

            teachingMaterialRepository.Add(teachingMaterial);

            teacherViewModel.TeachingMaterialsList.Clear();
            var list = teachingMaterialRepository.GetAll();
            teacherViewModel.TeachingMaterialsList.AddRange(list);
        }

        private void VerifyInput()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new ArgumentNullException(nameof(Name));
            }

            if (string.IsNullOrEmpty(Semester))
            {
                throw new ArgumentNullException(nameof(Semester));
            }

            if (string.IsNullOrEmpty(SubjectName))
            {
                throw new ArgumentNullException(nameof(SubjectName));
            }

            if (string.IsNullOrEmpty(FilePath))
            {
                throw new ArgumentNullException(nameof(FilePath));
            }
        }
    }
}
