#region Usings
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
    public class EditMemberTeamViewModel : ViewModelBase
    {
        //commands
        private readonly RelayCommand _saveChangesCommand;
        public ICommand SaveChangesCommand { get { return _saveChangesCommand; } }

        //context
        private readonly TeamContext _context;

        //properties
        #region Properties
        private Guid _selectedMember;
        public Guid SelectedMember
        {
            get
            {
                return _selectedMember;
            }
            set
            {
                if (_selectedMember == value) return;

                _selectedMember = value;
                OnPropertyChanged("SelectedMember");
            }
        }

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

        private EntitySet<Member> _memberList;
        public EntitySet<Member> MemberList
        {
            get
            {
                return _memberList;
            }
            set
            {
                if (_memberList != value)
                {
                    _memberList = value;
                    OnPropertyChanged("MemberList");
                }
            }
        } 
        #endregion

        public EditMemberTeamViewModel()
        {
            _context = ContextFactory.GetTeamContext();
            _saveChangesCommand = new RelayCommand(OnSaveChanges);
            MemberList = _context.Members;
            TeamList = _context.Teams;
            SelectedYear = DateTime.Now.Year.ToString();

            LoadData();
            UpdateForUsersRole();
        }

        // Loads the data for the view.
        private void LoadData()
        {
            try
            {
                _context.Load(_context.GetTeamsQuery()).Completed += TeamsLoadCompleted;
                _context.Load(_context.GetMembersQuery()).Completed += MembersLoadCompleted;
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

        void MembersLoadCompleted(object sender, EventArgs e)
        {
            var loadedMember = ((LoadOperation<Member>)sender).Entities.FirstOrDefault();

            if (loadedMember != null)
            {
                SelectedMember = loadedMember.UserID;
            }
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
            if (SelectedTeam == 0 || SelectedMember.ToString() == string.Empty)
            {
                ShowDialog("Team and/or member are not selected");
                return;
            }

            if (string.IsNullOrEmpty(SelectedYear))
            {
                ShowDialog("The Year is not selected");
                return;
            }

            var newMemberTeam = new MemberTeam()
                                    {
                                        Year = Convert.ToInt16(SelectedYear),
                                        MemberID = SelectedMember,
                                        TeamID = SelectedTeam 
                                    };

            var existingContextTeam =
                _context.MemberTeams.Where(
                    t =>
                    (t.Year == newMemberTeam.Year)
                    && (t.Member.UserID == newMemberTeam.MemberID)
                    && (t.Team.ID == newMemberTeam.TeamID)).
                    Select(te => te).FirstOrDefault();

            if (existingContextTeam != null)
            {
                return;
            }

            try
            {
                _context.MemberTeams.Add(newMemberTeam);
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
