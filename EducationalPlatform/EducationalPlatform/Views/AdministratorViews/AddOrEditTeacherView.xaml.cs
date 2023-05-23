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
    /// Interaction logic for AddOrEditTeacherView.xaml
    /// </summary>
    public partial class AddOrEditTeacherView : MetroWindow
    {
        public AddOrEditTeacherView(AddOrEditTeacherViewModel viewModel)
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
