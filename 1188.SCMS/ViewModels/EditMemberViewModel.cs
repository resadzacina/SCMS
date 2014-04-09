using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using _1188.SCMS.Helpers;
using _1188.SCMS.Models;
using _1188.SCMS.Web;
using _1188.SCMS.Web.Services;

namespace _1188.SCMS.ViewModels
{
    public class EditMemberViewModel : ViewModelBase
    {
        //commands
        private readonly RelayCommand _saveChangesCommand;
        private readonly RelayCommand _changePasswordCommand;

        public ICommand SaveChangesCommand { get { return _saveChangesCommand; } }
        public ICommand ChangePasswordCommand { get { return _changePasswordCommand; } }

        private DispatcherTimer statusTimer;

        //context
        private readonly TeamContext _memberContext;
        private readonly UserRegistrationContext _userContext;
        private readonly CountryContext _countryContext;
        private readonly MembershipContext _membershipContext;

        //properties
        #region BindableProperties



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

        private Country _selectedCountry;
        public Country SelectedCountry
        {
            get
            {
                return _selectedCountry;
            }
            set
            {
                if ( _selectedCountry == value ) return;

                _selectedCountry = value;
                OnPropertyChanged( "SelectedCountry" );
            }
        }

        private ObservableCollection<RoleContainer> _assignedRoles;
        public ObservableCollection<RoleContainer> AssignedRoles
        {
            get
            {
                return _assignedRoles;
            }
            set
            {
                if ( _assignedRoles == value ) return;
                _assignedRoles = value;
                OnPropertyChanged( "AssignedRoles" );
            }
        }

        private ObservableCollection<RoleContainer> _UnassignedRoles;
        public ObservableCollection<RoleContainer> UnassignedRoles
        {
            get
            {
                return _UnassignedRoles;
            }
            set
            {
                if ( _UnassignedRoles == value ) return;
                _UnassignedRoles = value;
                OnPropertyChanged( "UnassignedRoles" );
            }
        }

        private ObservableCollection<Country> _CountriesList;
        public ObservableCollection<Country> CountriesList
        {
            get
            {
                return _CountriesList;
            }
            set
            {
                if ( _CountriesList == value ) return;
                _CountriesList = value;
                OnPropertyChanged( "CountriesList" );
            }
        }



        private BitmapImage _memberImage;
        public BitmapImage MemberImage
        {
            get
            {
                return _memberImage;
            }
            set
            {
                if ( _memberImage == value ) return;
                _memberImage = value;
                OnPropertyChanged( "MemberImage" );
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged( "Status" );
            }
        }

        private Visibility _isStatusVisible;
        public Visibility IsStatusVisible
        {
            get
            {
                return _isStatusVisible;
            }
            set
            {
                _isStatusVisible = value;
                OnPropertyChanged( "IsStatusVisible" );
            }
        }

        private byte[] _ImageBytes;
        public byte[] ImageBytes
        {
            get
            {
                return _ImageBytes;
            }
            set
            {
                _ImageBytes = value;
                OnPropertyChanged( "ImageBytes" );
            }
        }

        public bool IsDefaultImage { get; set; }
        public bool IsBusy { get; private set; }
        public DataFormMode Mode { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        #endregion


        public EditMemberViewModel()
        {
            _memberContext = ContextFactory.GetTeamContext();
            _userContext = ContextFactory.GetUserRegistrationContext();
            _countryContext = ContextFactory.GetCountryContext();
            _membershipContext = ContextFactory.GetMembershipContext();

            _saveChangesCommand = new RelayCommand( OnSaveChanges );
            _changePasswordCommand = new RelayCommand( OnChangePassword );
            statusTimer = new DispatcherTimer();
            statusTimer.Interval = new TimeSpan( 0, 0, 0, 3 );
            statusTimer.Tick += new EventHandler( OnTimerTick );
            IsStatusVisible = Visibility.Collapsed;
            IsLoggedIn = true;
            //LoadData(id);
            UpdateForUsersRole();
        }

        /// <summary>
        /// Loads the data for the view.
        /// </summary>
        public void LoadData( int id )
        {
            try
            {
                SetBusy( true );
                _memberContext.Load( _memberContext.GetMemberByIdQuery( id ), delegate( LoadOperation<Member> operation )
                {
                    if ( operation.HasError )
                    {
                        Status = "There was an error loading the member. " + operation.Error.Message;
                        operation.MarkErrorAsHandled();
                    }
                    else
                    {
                        var loadedMember = operation.Entities.FirstOrDefault();

                        if ( loadedMember != null )
                        {
                            SelectedMember = loadedMember;

                            LoadCountries();
                            LoadRoles();

                            if ( SelectedMember.Photo == null )
                            {
                                LoadDefaultPhoto();

                            }
                            else
                            {
                                LoadMemberPhoto( SelectedMember.Photo );
                                IsDefaultImage = false;
                            }
                            Mode = DataFormMode.Edit;
                        }
                        else if ( id == 0 )
                        {
                            LoadCountries();
                            LoadRoles();
                            LoadDefaultPhoto();

                            SelectedMember = new Member();
                            Mode = DataFormMode.AddNew;
                        }
                    }


                }, null );
            }
            catch ( InvalidOperationException ex )
            {
                MessageBox.Show( ex.Message );
            }
        }

        void SetBusy( bool isBusy )
        {
            this.IsBusy = isBusy;
            OnPropertyChanged( "IsBusy" );
        }

        private void UpdateForUsersRole()
        {
            var isLoggedIn = WebContext.Current.User.IsAuthenticated;
            // bool IsAdmin = IsLoggedIn && WebContext.Current.User.IsInRole("Administrators");

            //AdminButtonsVisibility = IsAdmin ? Visibility.Visible : Visibility.Collapsed;

            UpdateButtons( isLoggedIn );

        }

        private void UpdateButtons( bool isLoggedIn )
        {
            _saveChangesCommand.IsEnabled = isLoggedIn;
            _changePasswordCommand.IsEnabled = isLoggedIn;
            // _NewTeamCommand.IsEnabled = isLoggedIn;
        }

        private void LoadDefaultPhoto()
        {
            MemberImage = ResourceHelper.GetBitmap( "Images/DefaultPhoto.png" );
            IsDefaultImage = true;
        }

        private void LoadMemberPhoto( Byte[] photoBytes )
        {
            MemberImage = ResourceHelper.GetImageFromByteArray( photoBytes );
        }

        private void LoadRoles()
        {
            //Load the roles
            if ( SelectedMember != null )
            {
                var io = _userContext.GetAssignedRoles( SelectedMember.Name );
                io.Completed += new EventHandler( RolesLoadCompleted );

                var ioUnassigned = _userContext.GetUnassignedRoles( SelectedMember.Name );
                ioUnassigned.Completed += new EventHandler( UnassignedRolesLoadCompleted );
            }
            else
            {
                var ioAllRoles = _userContext.GetAllRoles();
                ioAllRoles.Completed += new EventHandler( UnassignedRolesLoadCompleted );
                // UnassignedRoles = new ObservableCollection<RoleContainer>();
                AssignedRoles = new ObservableCollection<RoleContainer>();
            }

        }

        private void LoadCountries()
        {
            _countryContext.Load( _countryContext.GetCountriesQuery(), delegate( LoadOperation<Country> operation )
            {
                if ( operation.HasError )
                {
                    Status = "There was an error loading countries. " + operation.Error.Message;
                    operation.MarkErrorAsHandled();
                }
                else
                {
                    var loadedCountries = operation.Entities;
                    CountriesList = new ObservableCollection<Country>( loadedCountries );

                    if ( SelectedMember != null && SelectedMember.UserID != Guid.Empty )
                        if ( SelectedMember.CountryID != 0 && SelectedMember.CountryID != null )
                            SelectedCountry = loadedCountries.Where( c => c.ID == SelectedMember.CountryID ).First();
                }
            }, null );
        }

        void RolesLoadCompleted( object sender, EventArgs e )
        {
            var rolesArray = ( (InvokeOperation<string[]>)sender ).Value;
            var rolesList = rolesArray.Select( role => new RoleContainer { Name = role } ).AsEnumerable();

            AssignedRoles = new ObservableCollection<RoleContainer>( rolesList );
        }

        void UnassignedRolesLoadCompleted( object sender, EventArgs e )
        {
            var rolesArray = ( (InvokeOperation<string[]>)sender ).Value;
            var rolesList = rolesArray.Select( role => new RoleContainer { Name = role } ).AsEnumerable();

            UnassignedRoles = new ObservableCollection<RoleContainer>( rolesList );

            SetBusy( false );
        }

        void OnChangePassword()
        {
            if ( string.IsNullOrEmpty( OldPassword ) )
            {
                ShowDialog( "Old password must be entered!" );
                return;
            }

            if ( string.IsNullOrEmpty( NewPassword ) )
            {
                ShowDialog( "New password must be entered!" );
                return;
            }

            _userContext.ChangePassword( SelectedMember.Name, OldPassword, NewPassword );

            ShowDialog( "Password successfully changed!" );
        }

        void OnSaveChanges()
        {

            if ( SelectedMember == null ) return;

            try
            {
                Validator.ValidateProperty( SelectedMember.Name,
                    new ValidationContext( SelectedMember, null, null ) { MemberName = "Name" } );

                if ( !SelectedMember.HasValidationErrors )
                {
                    if (string.IsNullOrEmpty(SelectedMember.Surname))
                    {
                        throw new ValidationException("Surname must be entered");
                    }

                    // Populate member picture with currently selected picture

                    if ( IsDefaultImage && ImageBytes == null )
                        SelectedMember.Photo = ResourceHelper.GetBytesFromResource( "Images/DefaultPhoto.png" );
                    else if ( IsDefaultImage == false && ImageBytes != null )
                        SelectedMember.Photo = ImageBytes;

                    // Populate with selected country
                    if ( SelectedCountry != null )
                        SelectedMember.CountryID = SelectedCountry.ID;

                    // create two string arrays for assigned and unassigned roles
                    var assignedArray = new string[ AssignedRoles.Count ];
                    for ( int i = 0; i < AssignedRoles.Count; i++ )
                    {
                        assignedArray[ i ] = AssignedRoles.ElementAt( i ).Name;
                    }

                    var unassignedArray = new string[ UnassignedRoles.Count ];
                    for ( int i = 0; i < UnassignedRoles.Count; i++ )
                    {
                        unassignedArray[ i ] = UnassignedRoles.ElementAt( i ).Name;
                    }

                    _userContext.SyncronyzeRoles( assignedArray, unassignedArray, SelectedMember.Name );
                    _memberContext.SubmitChanges().Completed += OnSubmitChangesCompleted;
                }
            }
            catch ( ValidationException ex )
            {
                ShowDialog(ex.Message);
            }
        }

        private void OnSubmitChangesCompleted( object sender, EventArgs e )
        {
            Status = "Successfully saved changes";
            IsStatusVisible = Visibility.Visible;
            statusTimer.Start();
        }

        private void OnTimerTick( object sender, EventArgs e )
        {
            IsStatusVisible = Visibility.Collapsed;
            if ( statusTimer.IsEnabled )
                statusTimer.Stop();
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
