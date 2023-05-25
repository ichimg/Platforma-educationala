﻿using EducationalPlatform.ViewModels.StudentViewModels;
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
    /// Interaction logic for SubjectDetailsView.xaml
    /// </summary>
    public partial class SubjectDetailsView : MetroWindow
    {
        public SubjectDetailsView(SubjectDetailsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }
    }
}
