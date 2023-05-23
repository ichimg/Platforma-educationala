using EducationalPlatform.Services;
using EducationalPlatform.ViewModels.AdministratorViewModels;
using MahApps.Metro.Controls;
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
    /// Interaction logic for AdministratorView.xaml
    /// </summary>
    public partial class AdministratorView : MetroWindow
    {
        private readonly AdministratorViewModel viewModel;
        public AdministratorView(AdministratorViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            DataContext = this.viewModel;
            this.viewModel.RequestShowStudentsList += Handle_ShowStudentsList;
            this.viewModel.RequestShowTeachersList += Handle_ShowTeachersList;
            this.viewModel.RequestShowSpecializationsList += Handle_ShowSpecializationsList;
            this.viewModel.RequestShowSubjectsList += Handle_ShowSubjectsList;
        }


        private void Handle_ShowStudentsList()
        {
            viewModel.DisplayedList = EDisplayedList.Students;

            TeachersList.Visibility = Visibility.Hidden;
            SubjectsList.Visibility = Visibility.Hidden;
            SpecializationsList.Visibility = Visibility.Hidden;
            StudentsList.Visibility = Visibility.Visible;
        }

        private void Handle_ShowTeachersList()
        {
            viewModel.DisplayedList = EDisplayedList.Teachers;

            StudentsList.Visibility = Visibility.Hidden;
            SubjectsList.Visibility = Visibility.Hidden;
            SpecializationsList.Visibility = Visibility.Hidden;
            TeachersList.Visibility = Visibility.Visible;
        }

        private void Handle_ShowSpecializationsList()
        {
            viewModel.DisplayedList = EDisplayedList.Specializations;

            TeachersList.Visibility = Visibility.Hidden;
            StudentsList.Visibility = Visibility.Hidden;
            SubjectsList.Visibility = Visibility.Hidden;
            SpecializationsList.Visibility = Visibility.Visible;
        }

        private void Handle_ShowSubjectsList()
        {
            viewModel.DisplayedList = EDisplayedList.Subjects;

            TeachersList.Visibility = Visibility.Hidden;
            StudentsList.Visibility = Visibility.Hidden;
            SpecializationsList.Visibility = Visibility.Hidden;
            SubjectsList.Visibility = Visibility.Visible;
        }
    }
}
