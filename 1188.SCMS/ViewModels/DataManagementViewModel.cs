#region

using System;
using System.Windows.Input;
using System.ServiceModel.DomainServices.Client.ApplicationServices;

#endregion

namespace _1188.SCMS.ViewModels
{
    public class DataManagementViewModel : ViewModelBase
    {
        //ICommand TeamManagementClickCommand;

        private readonly AuthenticationService _authService = WebContext.Current.Authentication;
        private readonly RelayCommand _teamManagementClickCommand;
        private readonly RelayCommand _leagueManagementClickCommand;
        private readonly RelayCommand _leagueAssignmentClickCommand;
        private readonly RelayCommand _memberManagementClickCommand;
        private readonly RelayCommand _memberTeamClickCommand;

        public ICommand TeamManagementClickCommand { get { return _teamManagementClickCommand; } }
        public ICommand LeagueManagementClickCommand { get { return _leagueManagementClickCommand; } }
        public ICommand LeagueAssignmentClickCommand { get { return _leagueAssignmentClickCommand; } }
        public ICommand MemberManagementClickCommand { get { return _memberManagementClickCommand; } }
        public ICommand MemberTeamClickCommand { get { return _memberTeamClickCommand; } }

        public DataManagementViewModel()
        {
            _teamManagementClickCommand = new RelayCommand(OnTeamManagementClick);
            _leagueManagementClickCommand = new RelayCommand(OnLeagueClick);
            _leagueAssignmentClickCommand = new RelayCommand(OnLeagueAssignmentClick);
            _memberManagementClickCommand = new RelayCommand(OnMemberManagementClick);
            _memberTeamClickCommand = new RelayCommand(OnMemberTeamClick);
            IsLoggedIn = true;
            LoadData();
        }

        /// <summary>
        /// Loads the data for the view.
        /// </summary>
        private void LoadData()
        {
            UpdateForUsersRole();
        }

        private void OnLeagueAssignmentClick()
        {
        }

        private static void OnTeamManagementClick()
        {
        }

        private static void OnMemberManagementClick()
        {
        }

        private static void OnLeagueClick()
        {
        }

        private static void OnMemberTeamClick()
        {
        }

        private void UpdateForUsersRole()
        {
            UpdateButtons(WebContext.Current.User.IsAuthenticated);
        }

        private void UpdateButtons(bool isLoggedIn)
        {
            _teamManagementClickCommand.IsEnabled = isLoggedIn;
            _leagueManagementClickCommand.IsEnabled = isLoggedIn;
            _leagueAssignmentClickCommand.IsEnabled = isLoggedIn;
            _memberManagementClickCommand.IsEnabled = isLoggedIn;
            _memberTeamClickCommand.IsEnabled = isLoggedIn;
        }

        public override void AuthenticationLoggedIn(object sender, AuthenticationEventArgs e)
        {
            UpdateForUsersRole();
        }

        public override void AuthenticationLoggedOut(object sender, AuthenticationEventArgs e)
        {
            UpdateForUsersRole();
        }
    }
}
