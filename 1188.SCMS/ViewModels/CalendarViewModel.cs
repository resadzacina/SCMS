using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Input;
using _1188.SCMS.Web;
using _1188.SCMS.Web.Services;

namespace _1188.SCMS.ViewModels
{
    public class CalendarViewModel : ViewModelBase
    {
        private readonly EventContext _eventContext;
        private readonly TeamContext _teamContext;
       
        private readonly RelayCommand _deleteCommand;
        public ICommand DeleteEventCommand { get { return _deleteCommand; } }

        public CalendarViewModel()
        {
            _eventContext = ContextFactory.GetEventContext();
            _teamContext = ContextFactory.GetTeamContext();
            _currentDate = DateTime.Now;
            _deleteCommand = new RelayCommand(OnDelete);
            AppMessages.EventAddedMessage.Register(this, OnEventAdded);
            IsLoggedIn = true;
            LoadData();
        }

        private void OnEventAdded(string obj)
        {
            LoadData();
            IsEditVisible = Visibility.Visible;
        }

        private void OnDelete()
        {
            if (SelectedEvent == null)
                ShowDialog("You must select an event first");
            else
                _eventContext.DeleteEvent(SelectedEvent).Completed += OnDeleteCompleted;
        }

        private void OnDeleteCompleted(object sender, EventArgs e)
        {
            LoadData();
        }

        #region Properties
        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set
            {
                _currentDate = value;
                LoadData();
                //EventList = _eventContext.Events;
                OnPropertyChanged("CurrentDate");
            }
        }

        private DateTime _selectedCalDate;
        public DateTime SelectedCalDate
        {
            get { return _selectedCalDate; }
            set
            {
                _selectedCalDate = value;
                OnPropertyChanged( "SelectedCalDate" );
            }
        }

        private Event _selectedEvent;
        public Event SelectedEvent
        {
            get { return _selectedEvent; }
            set
            {
                if (_selectedEvent == value) return;

                _selectedEvent = value;

                if (_selectedEvent != null)
                    LoadTeams(_selectedEvent.ID);

                OnPropertyChanged("SelectedEvent");
            }
        }

        private Team _selectedTeam;
        public Team SelectedTeam
        {
            get { return _selectedTeam; }
            set
            {
                if (_selectedTeam == value) return;

                _selectedTeam = value;
                OnPropertyChanged("SelectedTeam");
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

        private IEnumerable<Team> _teamList;
        public IEnumerable<Team> TeamList
        {
            get { return _teamList; }
            set
            {
                if (_teamList != value)
                {
                    _teamList = value;
                    OnPropertyChanged("TeamList");
                }
            }
        }

        private Visibility _isEditVisible;
        public Visibility IsEditVisible
        {
            get { return _isEditVisible; }
            set
            {
                if ( _isEditVisible != value )
                {
                    _isEditVisible = value;
                    OnPropertyChanged( "IsEditVisible" );
                }
            }
        }

        #endregion

        void LoadData()
        {
            try
            {
                _eventContext.Load(_eventContext.GetEventsQuery()).Completed += EventsLoadCompleted;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        void LoadTeams(int id)
        {
            _teamContext.Load(_teamContext.GetTeamsByEventQuery(id)).Completed += TeamByEventLoadCompleted;
        }

        private void TeamByEventLoadCompleted(object sender, EventArgs e)
        {
            var loadedTeams = ((LoadOperation<Team>)sender).Entities;

            if (loadedTeams != null)
                TeamList = loadedTeams;
        }

        void EventsLoadCompleted(object sender, EventArgs e)
        {
            var loadedEvents = ((LoadOperation<Event>)sender).Entities;

            IsEditVisible = Visibility.Collapsed;

            if (loadedEvents != null)
            {
                EventList = loadedEvents;
                _deleteCommand.IsEnabled = true;

                //if (SelectedCalDate == DateTime.MinValue)
                //{
                //    SelectedCalDate = CurrentDate;
                //}

                if (SelectedCalDate != DateTime.MinValue && loadedEvents.Where( ev => ev.DateStart.ToShortDateString() == CurrentDate.ToShortDateString() ).Count() > 0 )
                {
                    IsEditVisible = Visibility.Visible;
                }
            }
        }

        public void CheckSelection(DateTime? selected)
        {
            if ( EventList.Where( ev => ev.DateStart.ToShortDateString() == selected.Value.ToShortDateString() ).Count() > 0 )
            {
                IsEditVisible = Visibility.Visible;
            }
            else
            {
                IsEditVisible = Visibility.Collapsed;
            }
        }

        public override void AuthenticationLoggedIn(object sender, System.ServiceModel.DomainServices.Client.ApplicationServices.AuthenticationEventArgs e)
        {
            IsLoggedIn = true;
        }

        public override void AuthenticationLoggedOut(object sender, System.ServiceModel.DomainServices.Client.ApplicationServices.AuthenticationEventArgs e)
        {
            IsLoggedIn = false;
        }
    }
}