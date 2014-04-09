#region

using System.Windows;
using _1188.SCMS.ViewModels;


#endregion

namespace _1188.SCMS.Views
{
    public partial class TeamManagementView
    {
        readonly TeamManagementViewModel _viewModel;

        public TeamManagementView()
        {
            InitializeComponent();

            if (_viewModel == null)
            {
                _viewModel = new TeamManagementViewModel();
            }

            DataContext = _viewModel;
        }

        private void BtnNewEventClick(object sender, RoutedEventArgs e)
        {
            var dialog = new EditTeamView(0);
            dialog.Show();
        }
    }
}
