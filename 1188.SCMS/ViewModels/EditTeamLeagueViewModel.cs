#region

using System;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Windows;
using System.Windows.Input;
using _1188.SCMS.Web;
using _1188.SCMS.Web.Services;

#endregion

namespace _1188.SCMS.ViewModels
{
    public class EditTeamLeagueViewModel : ViewModelBase
    {
        //commands
        private readonly RelayCommand _saveChangesCommand;
        public ICommand SaveChangesCommand { get { return _saveChangesCommand; } }

        //context
        private readonly TeamContext _context;

        //properties
        private int _selectedTeam;
        public int SelectedTeam
        {
            get
            {
                return _selectedTeam;
            }
            set
            {
                if (_selectedTeam == value) return;

                _selectedTeam = value;
                OnPropertyChanged("SelectedTeam");
            }
        }

        private int _selectedLeague;
        public int SelectedLeague
        {
            get
            {
                return _selectedLeague;
            }
            set
            {
                if (_selectedLeague == value) return;

                _selectedLeague = value;
                OnPropertyChanged("SelectedLeague");
            }
        }

        private string _selectedYear;
        public string SelectedYear
        {
            get
            {
                return _selectedYear;
            }
            set
            {
                if (_selectedYear == value) return;

                _selectedYear = value;
                OnPropertyChanged("SelectedYear");
            }
        }

        private EntitySet<Team> _teamList;
        public EntitySet<Team> TeamList
        {
            get
            {
                return _teamList;
            }
            set
            {
                if (_teamList != value)
                {
                    _teamList = value;
                    OnPropertyChanged("TeamList");
                }
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
                if (_leagueList != value)
                {
                    _leagueList = value;
                    OnPropertyChanged("LeagueList");
                }
            }
        }

        public EditTeamLeagueViewModel()
        {
            _context = ContextFactory.GetTeamContext();
            _saveChangesCommand = new RelayCommand(OnSaveChanges);
            LeagueList = _context.Leagues;
            TeamList = _context.Teams;
            SelectedYear = DateTime.Now.Year.ToString();

            LoadData();
            UpdateForUsersRole();
        }

        /// <summary>
        /// Loads the data for the view.
        /// </summary>
        private void LoadData()
        {
            try
            {
                _context.Load(_context.GetTeamsQuery()).Completed += TeamsLoadCompleted;
                _context.Load(_context.GetLeaguesQuery()).Completed += LeaguesLoadCompleted;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void TeamsLoadCompleted(object sender, EventArgs e)
        {
            var loadedTeam = ((LoadOperation<Team>)sender).Entities.FirstOrDefault();
            SelectedTeam = loadedTeam != null ? loadedTeam.ID : new Team().ID;
        }

        void LeaguesLoadCompleted(object sender, EventArgs e)
        {
            var loadedLeague = ((LoadOperation<League>)sender).Entities.FirstOrDefault();
            SelectedLeague = loadedLeague != null ? loadedLeague.ID : new League().ID;
        }

        private void UpdateForUsersRole()
        {
            var isLoggedIn = WebContext.Current.User.IsAuthenticated;
            // bool IsAdmin = IsLoggedIn && WebContext.Current.User.IsInRole("Administrators");

            //AdminButtonsVisibility = IsAdmin ? Visibility.Visible : Visibility.Collapsed;

            UpdateButtons(isLoggedIn);

        }

        private void UpdateButtons(bool isLoggedIn)
        {
            _saveChangesCommand.IsEnabled = isLoggedIn;
            // _NewTeamCommand.IsEnabled = isLoggedIn;
        }

        void OnSaveChanges()
        {
            if (SelectedTeam == 0 || SelectedLeague == 0)
            {
                ShowDialog("Team and/or league are not selected");
                return;
            }

            if (string.IsNullOrEmpty(SelectedYear))
            {
                ShowDialog("The Year is not selected");
                return;
            }

            var newTeamLeague = new TeamLeague
                                    {
                                        Year = Convert.ToInt16(SelectedYear),
                                        LeagueID = SelectedLeague,
                                        TeamID = SelectedTeam 
                                    };

            var existingContextTeam =
                _context.TeamLeagues.Where(
                    t =>
                    (t.Year == newTeamLeague.Year)
                    && (t.League.ID == newTeamLeague.LeagueID)
                    && (t.Team.ID == newTeamLeague.TeamID)).
                    Select(te => te).FirstOrDefault();

            if (existingContextTeam != null)
            {
                return;
            }

            try
            {
                _context.TeamLeagues.Add(newTeamLeague);
                _context.SubmitChanges();

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error {0}", ex.Message));
            }
         
        }

        public override void AuthenticationLoggedIn(object sender, AuthenticationEventArgs e)
        {
        }

        public override void AuthenticationLoggedOut(object sender, AuthenticationEventArgs e)
        {
        }
    }
}
