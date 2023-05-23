using EducationalPlatform.ViewModels.AdministratorViewModels;
using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace EducationalPlatform.Views
{
    /// <summary>
    /// Interaction logic for AssignClassView.xaml
    /// </summary>
    public partial class AssignClassView : MetroWindow
    {
        public AssignClassView(AssignClassViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }
    }
}
