using EducationalPlatform.ViewModels.TeacherViewModels;
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

namespace EducationalPlatform.Views.TeacherViews
{
    /// <summary>
    /// Interaction logic for AddTeachingMaterialView.xaml
    /// </summary>
    public partial class AddTeachingMaterialView : MetroWindow
    {
        public AddTeachingMaterialView(AddTeachingMaterialViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));  
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
