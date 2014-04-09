using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using _1188.SCMS.ViewModels;

namespace _1188.SCMS.Views
{
    public partial class EditMemberTeamView : ChildWindow
    {
        readonly EditMemberTeamViewModel _viewModel;

        public EditMemberTeamView()
        {
            InitializeComponent();

            if (_viewModel == null)
            {
                _viewModel = new EditMemberTeamViewModel();
            }

            DataContext = _viewModel;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

