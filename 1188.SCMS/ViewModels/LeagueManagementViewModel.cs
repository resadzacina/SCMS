using System;
using System.ServiceModel.DomainServices.Client;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Windows.Input;
using _1188.SCMS.Views;
using _1188.SCMS.Web;
using _1188.SCMS.Web.Services;

namespace _1188.SCMS.ViewModels
{
    public class LeagueManagementViewModel : ViewModelBase
    {
        private readonly RelayCommand _deleteLeagueCommand;
        public ICommand DeleteLeagueCommand { get { return _deleteLeagueCommand; } }

        private readonly RelayCommand _editCommand;
        public ICommand EditCommand { get { return _editCommand; } }

        private readonly TeamContext _teamContext;

        //properties
        private League _selectedLeague;
        public League SelectedLeague
        {
            get
            {
                return _selectedLeague;
            }
            set
            {
                if ( _selectedLeague == value ) return;

                _selectedLeague = value;
                OnPropertyChanged( "SelectedLeague" );
            }
        }

        private EntitySet<League> _leagueList;
        public EntitySet<League> LeagueList
        {
            get
            {
                return _leagueList;
            }
            set
            {
                if ( _leagueList == value ) return;
                _leagueList = value;
                OnPropertyChanged( "LeagueList" );
            }
        }

        public LeagueManagementViewModel()
        {
            _teamContext = ContextFactory.GetTeamContext();
            LeagueList = _teamContext.Leagues;
            _deleteLeagueCommand = new RelayCommand( OnDeleteLeague );
            _editCommand = new RelayCommand( OnEdit );
            IsLoggedIn = true;
            LoadData();
        }

        /// <summary>
        /// Loads the data for the view.
        /// </summary>
        private void LoadData()
        {
            _teamContext.Load( _teamContext.GetLeaguesQuery() ).Completed += OnLeaguesLoadCompleted;
        }

        private void OnEdit()
        {
            if ( SelectedLeague == null )
            {
                ShowDialog( "League not selected" );
                return;
            }

            var id = SelectedLeague.ID;
            var dialog = new EditLeagueView( id );
            dialog.Show();
        }

        private void OnDeleteLeague()
        {
            if ( SelectedLeague == null )
            {
                ShowDialog( "League not selected" );
                return;
            }

            _teamContext.Leagues.Remove( SelectedLeague );

            _teamContext.SubmitChanges();
        }

        private void OnLeaguesLoadCompleted( object sender, EventArgs e )
        {
            var isEnabled = LeagueList.Count > 0;

            _deleteLeagueCommand.IsEnabled = isEnabled;
            _editCommand.IsEnabled = isEnabled;
        }

        public override void AuthenticationLoggedIn( object sender, AuthenticationEventArgs e )
        {
            IsLoggedIn = true;
        }

        public override void AuthenticationLoggedOut( object sender, AuthenticationEventArgs e )
        {
            IsLoggedIn = false;
        }
    }
}
