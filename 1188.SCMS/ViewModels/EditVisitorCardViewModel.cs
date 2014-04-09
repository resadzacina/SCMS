using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Windows;
using System.Windows.Input;
using _1188.SCMS.CustomEvents;
using _1188.SCMS.Web;
using _1188.SCMS.Web.Services;

namespace _1188.SCMS.ViewModels
{
    public class EditVisitorCardViewModel : ViewModelBase
    {
        //commands
        private readonly RelayCommand _saveChangesCommand;
        public ICommand SaveChangesCommand { get { return _saveChangesCommand; } }

        //context
        private readonly VisitorCardContext _context;
        private readonly TeamContext _teamContext;

        public event EventHandler TeamValidationErrorsEvent;

        //properties
        private VisitorCard _selectedCard;
        public VisitorCard SelectedCard
        {
            get
            {
                return _selectedCard;
            }
            set
            {
                if (_selectedCard == value) return;

                _selectedCard = value;
                OnPropertyChanged("SelectedCard");
            }
        }

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

        private IEnumerable<Member> _memberList;
        public IEnumerable<Member> MemberList
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

        //ctor
        public EditVisitorCardViewModel()
        {
            _saveChangesCommand = new RelayCommand(OnSaveChanges);
            _context = ContextFactory.GetVisitorCardContext();
            _teamContext = ContextFactory.GetTeamContext();

            MemberList = _teamContext.Members;

            WebContext.Current.Authentication.LoggedIn += (s, e) => UpdateForUsersRole();
            WebContext.Current.Authentication.LoggedOut += (s, e) => UpdateForUsersRole();
            UpdateForUsersRole();

            LoadData();
        }

        /// <summary>
        /// Loads the data for the view.
        /// </summary>
        private void LoadData()
        {
            _teamContext.Load(_teamContext.GetMembersQuery()).Completed += MembersLoadCompleted;
        }

        public void LoadDataByCardId(int id)
        {
            try
            {
                _context.Load(_context.GetVisitorCardByIdQuery(id)).Completed += CardLoadCompleted;
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

        void CardLoadCompleted(object sender, EventArgs e)
        {
            var loadedCard = ((LoadOperation<VisitorCard>) sender).Entities.FirstOrDefault();
            SelectedCard = loadedCard ?? new VisitorCard { StartTime = DateTime.Today, EndTime = DateTime.Today };
        }

        void OnSaveChanges()
        {
            if (SelectedCard == null) return;

            try
            {
                //Validator.ValidateProperty( SelectedCard.Name,
                //    new ValidationContext( SelectedTeam, null, null ) { MemberName = "Name" } );

                if (!SelectedCard.HasValidationErrors)
                {
                    if (SelectedMember == Guid.Empty)
                        throw new ValidationException("User must be selected");

                    SelectedCard.UserID = SelectedMember;

                    if (SelectedCard.ID == 0)
                    {
                        _context.VisitorCards.Add( SelectedCard );
                    }

                    _context.SubmitChanges().Completed += SaveCompleted;
                    TeamValidationErrorsEvent( this, new CustomValidationErrorEventArgs( false, string.Empty ) );
                }
            }
            catch (ValidationException ex)
            {
                TeamValidationErrorsEvent( this, new CustomValidationErrorEventArgs( true, ex.ValidationResult.ErrorMessage ) );
            }
        }

        private static void SaveCompleted(object sender, EventArgs e)
        {
            AppMessages.CardAddedMessage.Send();
        }

        void MembersLoadCompleted(object sender, EventArgs e)
        {
            var loadedMember = ((LoadOperation<Member>)sender).Entities.FirstOrDefault();

            if (loadedMember != null && SelectedCard == null)
            {
                SelectedMember = loadedMember.UserID;
            }

            if (loadedMember != null && SelectedCard != null )
            {
                SelectedMember = SelectedCard.UserID;
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
