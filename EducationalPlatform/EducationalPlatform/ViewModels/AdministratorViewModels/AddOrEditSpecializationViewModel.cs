using EducationalPlatform.Commands;
using EducationalPlatform.DataAccess.Models;
using EducationalPlatform.DataAccess.Repositories;
using EducationalPlatform.Extensions;
using EducationalPlatform.Services;
using System;
using System.Windows.Input;

namespace EducationalPlatform.ViewModels.AdministratorViewModels
{
    public class AddOrEditSpecializationViewModel : ViewModelBase
    {
        private readonly IRepository<Specialization> specializationRepository;
        private readonly WindowService windowService;

        private readonly AdministratorViewModel administratorViewModel;

        private bool isEditing;

        public AddOrEditSpecializationViewModel(IRepository<Specialization> specializationRepository,
            WindowService windowService,
            AdministratorViewModel administratorViewModel,
            bool isEditing)
        {
            this.specializationRepository = specializationRepository ?? throw new ArgumentNullException(nameof(specializationRepository));
            this.windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
            this.administratorViewModel = administratorViewModel ?? throw new ArgumentNullException(nameof(administratorViewModel));
            this.isEditing = isEditing;
            windowService.EditSpecializationFormViewLaunched += Handle_EditSpecializationFormViewLaunched;
        }

        private void Handle_EditSpecializationFormViewLaunched()
        {
            Name = administratorViewModel.SelectedSpecialization?.Name;
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

        private ICommand addOrEditSpecializationCommand;
        public ICommand AddOrEditSpecializationCommand
        {
            get
            {
                if (addOrEditSpecializationCommand is null)
                {
                    if (isEditing)
                    {
                        addOrEditSpecializationCommand = new RelayCommand(EditSpecialization);
                    }
                    else
                    {
                        addOrEditSpecializationCommand = new RelayCommand(AddSpecialization);
                    }
                }
                return addOrEditSpecializationCommand;
            }
        }

        private void AddSpecialization()
        {
            Specialization specializationToAdd = new Specialization
            {
                Name = Name
            };

            specializationRepository.Add(specializationToAdd);

            administratorViewModel.Specializations.Clear();
            var list = specializationRepository.GetAll();
            administratorViewModel.Specializations.AddRange(list);
        }

        private void EditSpecialization()
        {
            administratorViewModel.SelectedSpecialization.Name = Name;

            var specialization = administratorViewModel.SelectedSpecialization;
            administratorViewModel.Specializations.Remove(specialization);
            administratorViewModel.Specializations.Add(specialization);
            administratorViewModel.SelectedSpecialization = specialization;

            specializationRepository.Update(administratorViewModel.SelectedSpecialization);
        }
    }
}
