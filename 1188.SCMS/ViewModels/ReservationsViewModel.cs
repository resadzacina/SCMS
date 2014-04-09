using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using _1188.SCMS.Views;
using _1188.SCMS.Web;
using _1188.SCMS.Web.Services;

namespace _1188.SCMS.ViewModels
{
    public class ReservationsViewModel : ViewModelBase
    {
        private readonly EventContext _eventContext;
        private readonly TicketContext _ticketContext;

        public bool IsBusy { get; private set; }

        private readonly RelayCommand _deleteCommand;
        public ICommand DeleteCommand { get { return _deleteCommand; } }

        private readonly RelayCommand _searchEventsCommand;
        public ICommand SearchEventsCommand { get { return _searchEventsCommand; } }

        private readonly RelayCommand _editCommand;
        public ICommand EditCommand { get { return _editCommand; } }

        private DispatcherTimer timer;

        #region Properties

        private Event _selectedEvent;
        public Event SelectedEvent
        {
            get { return _selectedEvent; }
            set
            {
                if (_selectedEvent == value) return;

                _selectedEvent = value;
                LoadDataByEventId();
                OnPropertyChanged("SelectedEvent");
            }
        }

        private string _eventSearchTerm;
        public string EventSearchTerm
        {
            get { return _eventSearchTerm; }
            set
            {
                if (_eventSearchTerm == value) return;

                _eventSearchTerm = value;
                OnPropertyChanged("EventSearchTerm");
            }
        }


        private Ticket _selectedTicket;
        public Ticket SelectedTicket
        {
            get { return _selectedTicket; }
            set
            {
                if (_selectedTicket == value) return;

                _selectedTicket = value;

                OnPropertyChanged("SelectedTicket");
            }
        }

        private IEnumerable<Event> _eventList;
        public IEnumerable<Event> EventList
        {
            get { return _eventList; }
            set
            {
                if (_eventList != value)
                {
                    _eventList = value;
                    OnPropertyChanged("EventList");
                }
            }
        }

        private IEnumerable<Ticket> _ticketList;
        public IEnumerable<Ticket> TicketList
        {
            get { return _ticketList; }
            set
            {
                if (_ticketList != value)
                {
                    _ticketList = value;
                    OnPropertyChanged("TicketList");
                }
            }
        }

        #endregion

        public ReservationsViewModel()
        {
            _eventContext = ContextFactory.GetEventContext();
            _ticketContext = ContextFactory.GetTicketContext();
            IsLoggedIn = true;
            AppMessages.TicketAddedMessage.Register(this, OnTicketAdded);

            LoadData();

            _deleteCommand = new RelayCommand(OnDelete);
            _searchEventsCommand = new RelayCommand(OnSearchEvents);
            _editCommand = new RelayCommand(OnEdit);
        }

        private void OnDelete()
        {
            if (SelectedTicket == null)
                ShowDialog("You must select a ticket first");
            else
            {
                _eventContext.Events.Where(e => e.ID == SelectedEvent.ID).FirstOrDefault().AvailableTickets += 1;
                _ticketContext.DeleteTicket(SelectedTicket).Completed += OnDeleteCompleted;
            }
        }

        private void OnEdit()
        {
            var selected = SelectedTicket;

            if ( selected == null )
            {
                ShowDialog( "You must select a ticket in order to edit a ticket" );
                return;
            }

            var id = selected.ID;
            var dialog = new EditTicketReservation( id, SelectedEvent );
            dialog.Show();
        }

        void SetBusy(bool isBusy)
        {
            this.IsBusy = isBusy;
            OnPropertyChanged("IsBusy");
        }

        private void OnSearchEvents()
        {
            SetBusy(true);
            _eventContext.Load(_eventContext.SearchEventsQuery(EventSearchTerm)).Completed += EventsLoadCompleted;
        }


        private void OnDeleteCompleted(object sender, EventArgs e)
        {
            _eventContext.SubmitChanges();
            LoadDataByEventId();
        }

        void LoadData()
        {
            try
            {
                SetBusy(true);
                _eventContext.Load(_eventContext.GetEventsQuery()).Completed += EventsLoadCompleted;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void OnTicketAdded(string obj)
        {
            //Remove tickets from sum of available ones
            _eventContext.Events.Where(e => e.ID == SelectedEvent.ID).FirstOrDefault().AvailableTickets -= 1;
            _eventContext.SubmitChanges();

            //fire a timer to delay the UI display 2 secs
            timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 2000) };
            timer.Tick += EachTick;
            timer.Start();
        }

        private void EachTick(object o, EventArgs sender)
        {
            timer.Stop();
            LoadDataByEventId();
        }

        void LoadDataByEventId()
        {
            if ( SelectedEvent != null)
            {
                try
                {
                    _ticketContext.Load( _ticketContext.GetTicketsByEventQuery( SelectedEvent.ID ) ).Completed += TicketsLoadCompleted;
                }
                catch ( Exception ex ) { ShowDialog( ex.Message ); }
            }
        }

        void EventsLoadCompleted(object sender, EventArgs e)
        {
            var loadedEvents = ((LoadOperation<Event>)sender).Entities;

            if (loadedEvents != null)
            {
                EventList = loadedEvents;
                _searchEventsCommand.IsEnabled = true;
                SetBusy(false);
            }
            else
                ShowDialog("No Events loaded");
        }

        void TicketsLoadCompleted(object sender, EventArgs e)
        {
            var loadedTickets = ((LoadOperation<Ticket>)sender).Entities;

            if (loadedTickets != null)
            {
                TicketList = loadedTickets;

                if (loadedTickets.Count() > 0)
                {
                    _deleteCommand.IsEnabled = true;
                    _editCommand.IsEnabled = true;
                }
                else
                {
                    _deleteCommand.IsEnabled = false;
                    _editCommand.IsEnabled = false;
                }
            }
        }

        public override void AuthenticationLoggedIn(object sender, AuthenticationEventArgs e)
        {
            IsLoggedIn = true;
        }

        public override void AuthenticationLoggedOut(object sender, AuthenticationEventArgs e)
        {
            IsLoggedIn = false;
        }
    }
}
