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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EducationalPlatform.Views
{
    /// <summary>
    /// Interaction logic for AuthenticationView.xaml
    /// </summary>
    public partial class AuthenticationView : UserControl
    {
        public AuthenticationView()
        {
            InitializeComponent();
            var bootstrapper = new Bootstrapper();
            var viewModel = bootstrapper.Run();
            DataContext = viewModel;
            viewModel.RequestClose += Handle_CloseWindow;
        }

        private void Handle_CloseWindow()
        {
            Window.GetWindow(this).Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            { 
                ((AuthenticationViewModel)DataContext).Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
