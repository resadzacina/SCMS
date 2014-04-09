using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Input;
using _1188.SCMS.CustomEvents;
using _1188.SCMS.Web;
using _1188.SCMS.Web.Services;

namespace _1188.SCMS.ViewModels
{
    public class EditLeagueViewModel : ViewModelBase
    {
        //commands
        private readonly RelayCommand _saveChangesCommand;
        public ICommand SaveChangesCommand { get { return _saveChangesCommand; } }

        //context
        private readonly TeamContext _context;

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
                if (_selectedLeague == value) return;

                _selectedLeague = value;
                OnPropertyChanged("SelectedLeague");
            }
        }

        public event EventHandler ValidationErrorsEvent;

        public bool CanCloseWindowAfterSave { get; set; }

        public EditLeagueViewModel()
        {
            _context = ContextFactory.GetTeamContext();
            _saveChangesCommand = new RelayCommand(OnSaveChanges);
            UpdateForUsersRole();
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

        void OnSaveChanges()
        {

            if (SelectedLeague == null) return;

            try
            {
                Validator.ValidateProperty(SelectedLeague.Name,
                    new ValidationContext(SelectedLeague, null, null) { MemberName = "Name" });

                if (!SelectedLeague.HasValidationErrors)
                {
                    if (SelectedLeague.ID == 0)
                    {
                        _context.Leagues.Add(SelectedLeague);
                    }

                    _context.SubmitChanges();
                    ValidationErrorsEvent(this, new CustomValidationErrorEventArgs(false, string.Empty));
                }
            }
            catch (ValidationException ex)
            {
               // CanCloseWindowAfterSave = false;
                ValidationErrorsEvent(this, new CustomValidationErrorEventArgs(true, ex.ValidationResult.ErrorMessage));
                //Validator.RaiseDataMemberChanged("Name");
            }
        }

        public void LoadDataByLeagueId(int leagueId)
        {
            try
            {
                _context.Load(_context.GetLeagueByIdQuery(leagueId)).Completed += LeagueLoadCompleted; ;
            }
            catch (InvalidOperationException ex)
            {
                var messageBoxResult = MessageBox.Show(ex.Message);
            }
        }

        void LeagueLoadCompleted(object sender, EventArgs e)
        {
            var loadedLeague = ((LoadOperation<League>)sender).Entities.FirstOrDefault();
            SelectedLeague = loadedLeague ?? new League();
        }

        public override void AuthenticationLoggedIn(object sender, System.ServiceModel.DomainServices.Client.ApplicationServices.AuthenticationEventArgs e)
        {
        }

        public override void AuthenticationLoggedOut(object sender, System.ServiceModel.DomainServices.Client.ApplicationServices.AuthenticationEventArgs e)
        {
        }
    }
}
