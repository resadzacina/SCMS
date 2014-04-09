#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Input;
using _1188.SCMS.CustomEvents;
using _1188.SCMS.Web;
using _1188.SCMS.Web.Services;
using System.ServiceModel.DomainServices.Client.ApplicationServices;

#endregion

namespace _1188.SCMS.ViewModels
{
    public class EditTeamViewModel : ViewModelBase
    {
        //commands
        private readonly RelayCommand _saveChangesCommand;
        public ICommand SaveChangesCommand { get { return _saveChangesCommand; } }

        //context
        private readonly TeamContext _context;

        //properties
        private Team _selectedTeam;
        public Team SelectedTeam
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

        private EntitySet<SportsSociety> _societiesList;
        public EntitySet<SportsSociety> SocietiesList
        {
            get
            {
                return _societiesList;
            }
            set
            {
                if (_societiesList != value)
                {
                    _societiesList = value;
                    OnPropertyChanged("SocietiesList");
                }
            }
        }

        private int _selectedSociety;
        public int SelectedSociety
        {
            get { return _selectedSociety; }
            set
            {
                _selectedSociety = value;
                OnPropertyChanged("SelectedSociety");
            }
        }

        public event EventHandler TeamValidationErrorsEvent;


        //ctor
        public EditTeamViewModel()
        {
            LoadData();
            _saveChangesCommand = new RelayCommand(OnSaveChanges);
            _context = ContextFactory.GetTeamContext();
            SocietiesList = _context.SportsSocieties;

            WebContext.Current.Authentication.LoggedIn += (s, e) => UpdateForUsersRole();
            WebContext.Current.Authentication.LoggedOut += (s, e) => UpdateForUsersRole();
            UpdateForUsersRole();
        }

        /// <summary>
        /// Loads the data for the view.
        /// </summary>
        private static void LoadData()
        {
        }

        public void LoadDataByTeamId(int teamId)
        {
            try
            {
                _context.Load(_context.GetTeamByIdQuery(teamId)).Completed += TeamLoadCompleted;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadSportSocieties()
        {
            try
            {
                var query = _context.GetSportsSocietiesQuery();
                _context.Load(query).Completed += (OnSocietiesLoadCompleted);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateForUsersRole()
        {
            var isLoggedIn = WebContext.Current.User.IsAuthenticated;

            UpdateButtons(isLoggedIn);
        }

        private void UpdateButtons(bool isLoggedIn)
        {
            _saveChangesCommand.IsEnabled = isLoggedIn;
        }


        void TeamLoadCompleted(object sender, EventArgs e)
        {
            var loadedTeam = ((LoadOperation<Team>) sender).Entities.FirstOrDefault();
            SelectedTeam = loadedTeam ?? new Team();

            if (loadedTeam != null)
                SelectedSociety = loadedTeam.SportSocietyID;
        }

        void OnSaveChanges()
        {
            if (SelectedTeam == null) return;

            try
            {
                Validator.ValidateProperty( SelectedTeam.Name,
                    new ValidationContext( SelectedTeam, null, null ) { MemberName = "Name" } );

                if (!SelectedTeam.HasValidationErrors)
                {
                    SelectedTeam.SportSocietyID = SelectedSociety;

                    if (SelectedTeam.ID == 0)
                    {
                        SelectedTeam.IsActive = true;
                        _context.Teams.Add( SelectedTeam );
                    }

                    _context.SubmitChanges();
                    TeamValidationErrorsEvent( this, new CustomValidationErrorEventArgs( false, string.Empty ) );
                }
            }
            catch (ValidationException ex)
            {
                TeamValidationErrorsEvent( this, new CustomValidationErrorEventArgs( true, ex.ValidationResult.ErrorMessage ) );
            }
        }

        void OnSocietiesLoadCompleted( object sender, EventArgs a )
        {
            var loadedSociety = ((LoadOperation<SportsSociety>)sender).Entities.FirstOrDefault();
            
            if (loadedSociety!= null)
            {
                SelectedSociety = loadedSociety.ID;
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
