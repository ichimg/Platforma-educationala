using EducationalPlatform.Commands;
using EducationalPlatform.DataAccess.Models;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.Services;
using System.Windows.Input;
using System;
using EducationalPlatform.Extensions;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Collections.Generic;

namespace EducationalPlatform.ViewModels
{
    public class AddOrEditSubjectViewModel : ViewModelBase
    {
        private readonly IRepository<Subject> subjectRepository;
        private readonly WindowService windowService;

        private readonly AdministratorViewModel administratorViewModel;

        private bool isEditing;

        public AddOrEditSubjectViewModel(IRepository<Subject> specializationRepository,
            WindowService windowService,
            AdministratorViewModel administratorViewModel,
            bool isEditing)
        {
            this.subjectRepository = specializationRepository ?? throw new ArgumentNullException(nameof(specializationRepository));
            this.windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            this.administratorViewModel = administratorViewModel ?? throw new ArgumentNullException(nameof(administratorViewModel));
            this.isEditing = isEditing;
            windowService.EditSubjectFormViewLaunched += Handle_EditSubjectFormViewLaunched;

            SelectedSpecializations = new List<Specialization>();
        }

        public ObservableCollection<Specialization> AllSpecializations => new ObservableCollection<Specialization>(administratorViewModel.Specializations);
        public List<Specialization> SelectedSpecializations { get; set; }

        private void Handle_EditSubjectFormViewLaunched()
        {
            Name = administratorViewModel.SelectedSubject?.Name;
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }

        private ICommand addOrEditSubjectCommand;
        public ICommand AddOrEditSubjectCommand
        {
            get
            {
                if (addOrEditSubjectCommand is null)
                {
                    if (isEditing)
                    {
                        addOrEditSubjectCommand = new RelayCommand(EditSubject);
                    }
                    else
                    {
                        addOrEditSubjectCommand = new RelayCommand(AddSubject);
                    }
                }
                return addOrEditSubjectCommand;
            }
        }

        private void AddSubject()
        {
            Subject subjectToAdd = new Subject
            {
                Name = this.Name,
                Specializations = SelectedSpecializations
            };

            subjectRepository.Add(subjectToAdd);

            administratorViewModel.Subjects.Clear();
            var list = subjectRepository.GetAll();
            administratorViewModel.Subjects.AddRange(list);

            SelectedSpecializations.Clear();
        }

        private void EditSubject()
        {
            administratorViewModel.SelectedSubject.Name = Name;
            administratorViewModel.SelectedSubject.Specializations = SelectedSpecializations;

            var subject = administratorViewModel.SelectedSubject;
            administratorViewModel.Subjects.Remove(subject);
            administratorViewModel.Subjects.Add(subject);
            administratorViewModel.SelectedSubject = subject;

            subjectRepository.Update(administratorViewModel.SelectedSubject);

            SelectedSpecializations.Clear();
        }
    }
}
