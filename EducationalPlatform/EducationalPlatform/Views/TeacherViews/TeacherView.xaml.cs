using EducationalPlatform.ViewModels.TeacherViewModels;
using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace EducationalPlatform.Views.TeacherViews
{
    /// <summary>
    /// Interaction logic for TeacherView.xaml
    /// </summary>
    public partial class TeacherView : MetroWindow
    {
        private readonly TeacherViewModel viewModel;
        public TeacherView(TeacherViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            DataContext = this.viewModel;
            this.viewModel.RequestShowStudentsList += Handle_ShowStudentsList;
            this.viewModel.RequestShowTeachingMaterialsList += Handle_ShowTeachingMaterialsList;
        }

        private void Handle_ShowTeachingMaterialsList()
        {
            viewModel.DisplayedList = Services.EDisplayedList.TeachingMaterials;

            StudentsList.Visibility = Visibility.Hidden;
            TeachingMaterialsList.Visibility = Visibility.Visible;
        }

        private void Handle_ShowStudentsList()
        {
            viewModel.DisplayedList = Services.EDisplayedList.Students;

            StudentsList.Visibility = Visibility.Visible;
            TeachingMaterialsList.Visibility = Visibility.Hidden;
        }
    }
}
