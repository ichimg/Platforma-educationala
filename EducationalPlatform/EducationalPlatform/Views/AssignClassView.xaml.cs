using EducationalPlatform.ViewModels;
using System;
using System.Windows;

namespace EducationalPlatform.Views
{
    /// <summary>
    /// Interaction logic for AssignClassView.xaml
    /// </summary>
    public partial class AssignClassView : Window
    {
        public AssignClassView(AssignClassViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }
    }
}
