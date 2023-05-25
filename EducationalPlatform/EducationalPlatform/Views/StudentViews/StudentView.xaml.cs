using EducationalPlatform.ViewModels.StudentViewModels;
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

namespace EducationalPlatform.Views.StudentViews
{
    /// <summary>
    /// Interaction logic for StudentView.xaml
    /// </summary>
    public partial class StudentView : MetroWindow
    {
        private readonly StudentViewModel viewModel;
        public StudentView(StudentViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            DataContext = this.viewModel;
            this.viewModel.RequestShowSubjectsList += Handle_ShowSubjectsList;
            this.viewModel.RequestShowTeachingMaterialsList += Handle_ShowTeachingMaterialsList;
        }

        private void Handle_ShowTeachingMaterialsList()
        {
            viewModel.DisplayedList = Services.EDisplayedList.TeachingMaterials;

            SubjectsList.Visibility = Visibility.Hidden;
            TeachingMaterialsList.Visibility = Visibility.Visible;
        }

        private void Handle_ShowSubjectsList()
        {
            viewModel.DisplayedList = Services.EDisplayedList.Students;

            SubjectsList.Visibility = Visibility.Visible;
            TeachingMaterialsList.Visibility = Visibility.Hidden;
        }
    }
}
