using EducationalPlatform.Domain.Models;
using EducationalPlatform.ViewModels.AdministratorViewModels;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;


namespace EducationalPlatform.Views
{
    /// <summary>
    /// Interaction logic for AddOrEditSubjectView.xaml
    /// </summary>
    public partial class AddOrEditSubjectView : MetroWindow
    {
        private readonly AddOrEditSubjectViewModel viewModel;
        public AddOrEditSubjectView(AddOrEditSubjectViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            DataContext = this.viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            var selectedSpecialization = (Specialization)checkbox.DataContext;

            viewModel.SelectedSpecializations.Add(selectedSpecialization);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            var selectedSpecialization = (Specialization)checkbox.DataContext;

            viewModel.SelectedSpecializations.Remove(selectedSpecialization);
        }
    }
}
