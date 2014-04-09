#region

using System.Windows;
using System.Windows.Controls;
using _1188.SCMS.ViewModels;

#endregion

namespace _1188.SCMS.Views
{
    public partial class EditTeamLeagueView
    {
        readonly EditTeamLeagueViewModel _viewModel;

        public EditTeamLeagueView()
        {
            InitializeComponent();

            if (_viewModel == null)
            {
                _viewModel = new EditTeamLeagueViewModel();
            }

            DataContext = _viewModel;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

