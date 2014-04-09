#region

using System.ServiceModel.DomainServices.Client;
using System.Windows.Input;
using _1188.SCMS.Views;
using _1188.SCMS.Web;
using _1188.SCMS.Web.Services;
using System;
using System.ServiceModel.DomainServices.Client.ApplicationServices;

#endregion

namespace _1188.SCMS.ViewModels
{
    public class TeamManagementViewModel : ViewModelBase
    {
        private readonly TeamContext _teamContext;

        private readonly RelayCommand _deleteTeamCommand;
        public ICommand DeleteTeamCommand { get { return _deleteTeamCommand; } }

        private readonly RelayCommand _editCommand;
        public ICommand EditCommand { get { return _editCommand; } }

        private EntitySet<Team> _teamsList;
        public EntitySet<Team> TeamsList
        {
            get
            {
                return _teamsList;
            }
            set
            {
                if ( _teamsList == value ) return;
                _teamsList = value;
                OnPropertyChanged( "TeamsList" );
            }
        }

        private Team _selectedTeam;
        public Team SelectedTeam
        {
            get
            {
                return _selectedTeam;
            }
            set
            {
                if ( _selectedTeam == value ) return;

                _selectedTeam = value;
                OnPropertyChanged( "SelectedTeam" );
            }
        }


        public TeamManagementViewModel()
        {
            _teamContext = ContextFactory.GetTeamContext();
            _deleteTeamCommand = new RelayCommand( OnDeleteTeam );
            _editCommand = new RelayCommand( OnEdit );
            TeamsList = _teamContext.Teams;
            LoadData();
            IsLoggedIn = true;
        }

        private void OnDeleteTeam()
        {
            if ( SelectedTeam == null )
            {
                ShowDialog( "Team not selected" );
                return;
            }

            SelectedTeam.IsActive = false;

            _teamContext.SubmitChanges().Completed += OnSubmitChangesComplete;
        }

        private void OnEdit()
        {
            if ( SelectedTeam != null )
            {
                var teamId = SelectedTeam.ID;
                var dialog = new EditTeamView( teamId );
                dialog.Show();
            }
            else
                ShowDialog( "Team not selected" );
        }

        /// <summary>
        /// Loads the data for the view.
        /// </summary>
        private void LoadData()
        {
            TeamsList.Clear();
            _teamContext.Load( _teamContext.GetTeamsQuery().Where( t => t.IsActive == true ) ).Completed += OnTeamsLoadCompleted;
        }

        private void OnTeamsLoadCompleted( object sender, EventArgs e )
        {
            var isEnabled = TeamsList.Count > 0;

            _deleteTeamCommand.IsEnabled = isEnabled;
            _editCommand.IsEnabled = isEnabled;
        }

        private void OnSubmitChangesComplete( object sender, EventArgs e )
        {
            LoadData();
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
