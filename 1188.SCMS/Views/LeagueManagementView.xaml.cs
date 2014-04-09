using System.Windows;
using _1188.SCMS.ViewModels;

namespace _1188.SCMS.Views
{
    public partial class LeagueManagementView
    {
        readonly LeagueManagementViewModel _viewModel;

        public LeagueManagementView()
        {
            InitializeComponent();

            if ( _viewModel == null )
            {
                _viewModel = new LeagueManagementViewModel();
            }

            DataContext = _viewModel;
        }

        private void NewLeagueButtonClick( object sender, RoutedEventArgs e )
        {
            var dialog = new EditLeagueView( 0 );
            dialog.Show();
        }
    }
}
