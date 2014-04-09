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
    public enum TicketMode
    {
        Add,
        Edit
    }
    public class EditTicketReservationViewModel : ViewModelBase
    {
        //commands
        private readonly RelayCommand _saveChangesCommand;
        public ICommand SaveChangesCommand { get { return _saveChangesCommand; } }

        private readonly RelayCommand _generateSeatCommand;
        public ICommand GenerateSeatCommand { get { return _generateSeatCommand; } }

        //context
        private readonly TicketContext _context;
        private readonly EventTicketContext _eventTicketContext;
        private readonly TeamContext _teamContext;

        public event EventHandler TicketValidationErrorsEvent;

        #region Properties
        //properties
        private Ticket _selectedTicket;
        public Ticket SelectedTicket
        {
            get
            {
                return _selectedTicket;
            }
            set
            {
                if (_selectedTicket == value) return;

                _selectedTicket = value;
                OnPropertyChanged("SelectedTicket");
            }
        }

        private Event _selectedEvent;
        public Event SelectedEvent
        {
            get
            {
                return _selectedEvent;
            }
            set
            {
                if (_selectedEvent == value) return;

                _selectedEvent = value;
                OnPropertyChanged("SelectedEvent");
            }
        }

        private int _selectedTickeType;
        public int SelectedTicketType
        {
            get
            {
                return _selectedTickeType;
            }
            set
            {
                if (_selectedTickeType == value) return;

                _selectedTickeType = value;
                OnPropertyChanged("SelectedTicketType");
            }
        }

        private int _selectedPm;
        public int SelectedPaymentMethod
        {
            get
            {
                return _selectedPm;
            }
            set
            {
                if (_selectedPm == value) return;

                _selectedPm = value;
                OnPropertyChanged("SelectedPaymentMethod");
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

        private IEnumerable<TicketType> _ticketTypes;
        public IEnumerable<TicketType> TicketTypes
        {
            get
            {
                return _ticketTypes;
            }
            set
            {
                if (_ticketTypes != value)
                {
                    _ticketTypes = value;
                    OnPropertyChanged("TicketTypes");
                }
            }
        }

        private IEnumerable<PaymentMethod> _paymenthMetodsList;
        public IEnumerable<PaymentMethod> PaymentMethodsList
        {
            get
            {
                return _paymenthMetodsList;
            }
            set
            {
                if (_paymenthMetodsList != value)
                {
                    _paymenthMetodsList = value;
                    OnPropertyChanged("PaymentMethodsList");
                }
            }
        }

        private TicketMode _viewMode;
        public TicketMode TicketMode
        {
            get
            {
                return _viewMode;
            }
            set
            {
                if (_viewMode != value)
                {
                    _viewMode = value;
                    OnPropertyChanged("TicketMode");
                }
            }
        }
        #endregion

        public EditTicketReservationViewModel(Event selectedEvent)
        {
            _saveChangesCommand = new RelayCommand(OnSaveChanges);
            _generateSeatCommand = new RelayCommand(OnGenerateSeatNumber);
            _context = ContextFactory.GetTicketContext();
            _eventTicketContext = ContextFactory.GetEventTicketContext();
            _teamContext = ContextFactory.GetTeamContext();
            SelectedEvent = selectedEvent;

            MemberList = _teamContext.Members;
            TicketTypes = _context.TicketTypes;
            PaymentMethodsList = _context.PaymentMethods;

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
            _context.Load(_context.GetTicketTypesQuery()).Completed += TicketTypesLoadCompleted;
            _context.Load(_context.GetPaymentMethodsQuery()).Completed += PaymentMethodsLoadCompleted;
        }

        public void LoadDataByTicketId(int id)
        {
            try
            {
                _context.Load(_context.GetTicketByIdQuery(id)).Completed += TicketLoadCompleted;
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
            _generateSeatCommand.IsEnabled = isLoggedIn;
        }

        void TicketLoadCompleted(object sender, EventArgs e)
        {
            var loadedTck = ((LoadOperation<Ticket>)sender).Entities.FirstOrDefault();
            SelectedTicket = loadedTck ?? new Ticket();
        }

        void TicketTypesLoadCompleted(object sender, EventArgs e)
        {
            var loadedTckType = ((LoadOperation<TicketType>)sender).Entities.FirstOrDefault();

            SelectedTicketType = loadedTckType != null ? loadedTckType.ID : 0;
        }

        void PaymentMethodsLoadCompleted(object sender, EventArgs e)
        {
            var loadedpmet = ((LoadOperation<PaymentMethod>)sender).Entities.FirstOrDefault();
            SelectedPaymentMethod = loadedpmet != null ? loadedpmet.ID : 0;
        }

        void OnGenerateSeatNumber()
        {
            var ticketExists = true;
            var seat = 0;
            var r = new Random(1);

            while (ticketExists || (Convert.ToInt16(seat) == SelectedTicket.SeatNumber))
            {
                seat = r.Next(SelectedEvent.AvailableTickets);

                ticketExists = _context.Tickets.Where(t => t.SeatNumber == seat).Count() > 0;
            }

            SelectedTicket.SeatNumber = Convert.ToInt16(seat);

        }

        void OnSaveChanges()
        {
            if (SelectedTicket == null) return;

            try
            {
                if (!SelectedTicket.HasValidationErrors)
                {
                    if (SelectedMember == Guid.Empty)
                        throw new ValidationException("User must be selected");

                    if (SelectedTicket.SeatNumber == null)
                        throw new ValidationException("Seat number is not selected");

                    var ticketExists = _context.Tickets.Where( t => t.SeatNumber == SelectedTicket.SeatNumber && t.ID != SelectedTicket.ID ).Count() > 0;

                    if (ticketExists)
                        throw new ValidationException("The selected seat number is already taken");

                    SelectedTicket.UserID = SelectedMember;
                    SelectedTicket.TicketTypeID = SelectedTicketType;
                    SelectedTicket.PaymentMethodID = SelectedPaymentMethod;


                    if (SelectedTicket.ID == 0)
                    {
                        TicketMode = TicketMode.Add;
                        _context.Tickets.Add(SelectedTicket);
                    }
                    else
                    {
                        TicketMode = TicketMode.Edit;
                    }

                    _context.SubmitChanges().Completed += SaveCompleted;
                    TicketValidationErrorsEvent(this, new CustomValidationErrorEventArgs(false, string.Empty));
                }
            }
            catch (ValidationException ex)
            {
                TicketValidationErrorsEvent(this, new CustomValidationErrorEventArgs(true, ex.ValidationResult.ErrorMessage));
            }
        }

        private void SaveCompleted(object sender, EventArgs e)
        {
            if (TicketMode == TicketMode.Add)
            {
                //SelectedEvent.AvailableTickets -= 1;

                _eventTicketContext.EventTickets.Add(new EventTicket() { EventID = SelectedEvent.ID, TicketID = _context.Tickets.Last().ID });
                _eventTicketContext.SubmitChanges();
            }

            AppMessages.TicketAddedMessage.Send();
        }

        void MembersLoadCompleted(object sender, EventArgs e)
        {
            var loadedMember = ((LoadOperation<Member>)sender).Entities.FirstOrDefault();

            if (loadedMember != null && SelectedTicket == null)
            {
                SelectedMember = loadedMember.UserID;
            }

            if (loadedMember != null && SelectedTicket != null)
            {
                SelectedMember = SelectedTicket.UserID;
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
