using System;
using System.ServiceModel.DomainServices.Client;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Windows.Input;
using _1188.SCMS.Web;
using _1188.SCMS.Web.Services;

namespace _1188.SCMS.ViewModels
{
    public class LeagueAssignmentViewModel : ViewModelBase
    {
        private readonly RelayCommand _deleteTeamLeagueCommand;
        public ICommand DeleteTeamLeagueCommand { get { return _deleteTeamLeagueCommand; } }

        private readonly TeamContext _teamContext;

        //properties
        private TeamLeague _selectedTeamLeague;
        public TeamLeague SelectedTeamLeague
        {
            get
            {
                return _selectedTeamLeague;
            }
            set
            {
                if ( _selectedTeamLeague == value ) return;

                _selectedTeamLeague = value;
                OnPropertyChanged( "SelectedTeamLeague" );
            }
        }

        private EntitySet<TeamLeague> _teamLeagueList;
        public EntitySet<TeamLeague> TeamLeagueList
        {
            get
            {
                return _teamLeagueList;
            }
            set
            {
                if ( _teamLeagueList == value ) return;
                _teamLeagueList = value;
                OnPropertyChanged( "TeamLeagueList" );
            }
        }

        public LeagueAssignmentViewModel()
        {
            _teamContext = ContextFactory.GetTeamContext();
            _deleteTeamLeagueCommand = new RelayCommand( OnDeleteTeamLeague );
            TeamLeagueList = _teamContext.TeamLeagues;
            LoadData();
            IsLoggedIn = true;
        }

        /// <summary>
        /// Loads the data for the view.
        /// </summary>
        private void LoadData()
        {
            _teamContext.Load( _teamContext.GetTeamLeaguesQuery() ).Completed += OnTeamLeaguesLoadCompleted;
        }

        private void OnTeamLeaguesLoadCompleted( object sender, EventArgs e )
        {
            _deleteTeamLeagueCommand.IsEnabled = TeamLeagueList.Count > 0;
        }

        private void OnDeleteTeamLeague()
        {
            if ( SelectedTeamLeague == null )
            {
                ShowDialog( "Assignment not selected" );
                return;
            }

            _teamContext.TeamLeagues.Remove( SelectedTeamLeague );

            _teamContext.SubmitChanges();
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
