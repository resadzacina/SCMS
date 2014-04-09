using System;
using System.Net;
using System.ServiceModel.DomainServices.Client;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using _1188.SCMS.Web;
using _1188.SCMS.Web.Services;

namespace _1188.SCMS.ViewModels
{
    public class MemberToTeamAssignViewModel : ViewModelBase
    {
        private readonly RelayCommand _deleteMemberTeamCommand;
        public ICommand DeleteMemberTeamCommand { get { return _deleteMemberTeamCommand; } }

        private readonly TeamContext _context;

        //properties
        private MemberTeam _selectedMemberTeam;
        public MemberTeam SelectedMemberTeam
        {
            get
            {
                return _selectedMemberTeam;
            }
            set
            {
                if ( _selectedMemberTeam == value ) return;

                _selectedMemberTeam = value;
                OnPropertyChanged( "SelectedMemberTeam" );
            }
        }

        private EntitySet<MemberTeam> _memberTeamList;
        public EntitySet<MemberTeam> MemberTeamList
        {
            get
            {
                return _memberTeamList;
            }
            set
            {
                if ( _memberTeamList == value ) return;
                _memberTeamList = value;
                OnPropertyChanged( "MemberTeamList" );
            }
        }

        public bool IsBusy { get; private set; }

        public MemberToTeamAssignViewModel()
        {
            _context = ContextFactory.GetTeamContext();
            _deleteMemberTeamCommand = new RelayCommand( OnDeleteMemberTeam );
            MemberTeamList = _context.MemberTeams;
            IsLoggedIn = true;
            LoadData();
        }

        /// <summary>
        /// Loads the data for the view.
        /// </summary>
        private void LoadData()
        {
            SetBusy( true );
            _context.Load( _context.GetMemberTeamsQuery() ).Completed += OnTeamLeaguesLoadCompleted;
        }

        private void OnTeamLeaguesLoadCompleted( object sender, EventArgs e )
        {
            _deleteMemberTeamCommand.IsEnabled = MemberTeamList.Count > 0;
            SetBusy( false );
        }

        private void OnDeleteMemberTeam()
        {
            if ( SelectedMemberTeam == null )
            {
                ShowDialog( "Assignment not selected" );
                return;
            }

            _context.MemberTeams.Remove( SelectedMemberTeam );

            _context.SubmitChanges();
        }

        void SetBusy( bool isBusy )
        {
            this.IsBusy = isBusy;
            OnPropertyChanged( "IsBusy" );
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
