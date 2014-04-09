using System.Windows;
using _1188.SCMS.ViewModels;

namespace _1188.SCMS.Views
{
    public partial class LeagueAssignmentView
    {
        readonly LeagueAssignmentViewModel _viewModel;

        public LeagueAssignmentView()
        {
            InitializeComponent();

            if (_viewModel == null)
            {
                _viewModel = new LeagueAssignmentViewModel();
            }

            DataContext = _viewModel;
        }

        private void NewTeamLeagueButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new EditTeamLeagueView();
            dialog.Show();
        }
    }
}
