using EducationalPlatform.DataAccess.Models;
using EducationalPlatform.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EducationalPlatform.Views
{
    /// <summary>
    /// Interaction logic for AddOrEditSubjectView.xaml
    /// </summary>
    public partial class AddOrEditSubjectView : Window
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
