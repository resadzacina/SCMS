using System;
using System.ServiceModel.DomainServices.Client;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using _1188.SCMS.Web;
using _1188.SCMS.Web.Services;

namespace _1188.SCMS.ViewModels
{
    public class MemberManagementViewModel : ViewModelBase
    {
        private readonly TeamContext _memberContext;

        private readonly RelayCommand _deleteMemberCommand;
        public ICommand DeleteMemberCommand { get { return _deleteMemberCommand; } }

        private readonly RelayCommand _editCommand;
        public ICommand EditCommand { get { return _editCommand; } }

        private EntitySet<Member> _memberList;
        public EntitySet<Member> MemberList
        {
            get
            {
                return _memberList;
            }
            set
            {
                if ( _memberList == value ) return;
                _memberList = value;
                OnPropertyChanged( "MemberList" );
            }
        }

        private Member _selectedMember;
        public Member SelectedMember
        {
            get
            {
                return _selectedMember;
            }
            set
            {
                if ( _selectedMember == value ) return;

                _selectedMember = value;
                OnPropertyChanged( "SelectedMember" );
            }
        }

        public MemberManagementViewModel()
        {
            _memberContext = ContextFactory.GetTeamContext();
            _deleteMemberCommand = new RelayCommand( OnDeleteMember );
            _editCommand = new RelayCommand( OnEdit );
            MemberList = _memberContext.Members;
            LoadData();
            IsLoggedIn = true;
        }

        private void OnDeleteMember()
        {
            if ( SelectedMember == null )
            {
                ShowDialog( "Memnber is not selected" );
                return;
            }

            _memberContext.Members.Remove( SelectedMember );

            _memberContext.SubmitChanges();
        }

        private void OnEdit()
        {
            if ( SelectedMember != null )
            {
                var memberId = SelectedMember.ID;

                AppMessages.NavigateToEditMemberMessage.Send( memberId.ToString() );
            }
            else
            {
                ShowDialog( "Memnber is not selected" );
            }
        }

        /// <summary>
        /// Loads the data for the view.
        /// </summary>
        private void LoadData()
        {
            try
            {
                _memberContext.Load( _memberContext.GetMembersQuery() ).Completed += OnMembersLoadCompleted;
            }
            catch ( Exception ex ) { MessageBox.Show( ex.Message ); }
        }

        private void OnMembersLoadCompleted( object sender, EventArgs e )
        {
            var isEnabled = MemberList.Count > 0;

            _deleteMemberCommand.IsEnabled = isEnabled;
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
