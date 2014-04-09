using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Windows;
using System.Windows.Input;
using _1188.SCMS.CustomEvents;
using _1188.SCMS.Models;
using _1188.SCMS.Web;
using _1188.SCMS.Web.Services;

namespace _1188.SCMS.ViewModels
{
    public class EventViewModel : ViewModelBase
    {
        //commands
        private readonly RelayCommand _saveChangesCommand;
        public ICommand SaveChangesCommand { get { return _saveChangesCommand; } }

        //context
        private readonly EventContext _context;
        private readonly TeamContext _teamContext;
        private readonly EventTeamContext _eventTeamContext;

        public event EventHandler EventValidationErrorsEvent;

        //properties
        private Event _selectedEvent;
        public Event SelectedEvent
        {
            get
            {
                return _selectedEvent;
            }
            set
            {
                if ( _selectedEvent == value ) return;

                _selectedEvent = value;
                OnPropertyChanged( "SelectedEvent" );
            }
        }

        private List<TeamModel> _teamList;
        public List<TeamModel> TeamList
        {
            get { return _teamList; }
            set
            {
                if ( _teamList != value )
                {
                    _teamList = value;
                    OnPropertyChanged( "TeamList" );
                }
            }
        }

        //ctor
        public EventViewModel()
        {
            _saveChangesCommand = new RelayCommand( OnSaveChanges );

            _context = ContextFactory.GetEventContext();
            _teamContext = ContextFactory.GetTeamContext();
            _eventTeamContext = ContextFactory.GetEventTeamContext();

            WebContext.Current.Authentication.LoggedIn += ( s, e ) => UpdateForUsersRole();
            WebContext.Current.Authentication.LoggedOut += ( s, e ) => UpdateForUsersRole();
            UpdateForUsersRole();
            LoadTeams();
        }



        void LoadTeams()
        {
            _teamContext.Load( _teamContext.GetTeamsQuery() ).Completed += TeamByEventLoadCompleted;
        }

        private void TeamByEventLoadCompleted( object sender, EventArgs e )
        {
            var loadedTeams = ( (LoadOperation<Team>)sender ).Entities;

            if ( loadedTeams != null )
            {
                TeamList = new List<TeamModel>();

                foreach ( var t in loadedTeams )
                {
                    TeamList.Add( new TeamModel() { Checked = false, ID = t.ID, Name = t.Name } );
                }
            }
        }

        public void LoadDataByEventId( int eventId )
        {
            try
            {
                _context.Load( _context.GetEventByIdQuery( eventId ) ).Completed += EventLoadCompleted;
            }
            catch ( InvalidOperationException ex )
            {
                MessageBox.Show( ex.Message );
            }
        }

        void EventLoadCompleted( object sender, EventArgs e )
        {
            var loadedEvent = ( (LoadOperation<Event>)sender ).Entities.FirstOrDefault();

            SelectedEvent = loadedEvent ?? new Event() { DateStart = DateTime.Today, DateEnd = DateTime.Today };

            if ( loadedEvent != null )
            {
                if ( loadedEvent.EventTeams.Count > 0 )
                {
                    TeamList = new List<TeamModel>();

                    foreach ( var t in loadedEvent.EventTeams )
                    {
                        //var item = TeamList.IndexOf(new TeamModel(){Checked = false, ID = t.TeamID, Name = t.Team.Name});

                        TeamList.Add( new TeamModel() { Checked = true, ID = t.TeamID, Name = t.Team.Name } );
                    }
                }
            }
        }

        private void UpdateForUsersRole()
        {
            var isLoggedIn = WebContext.Current.User.IsAuthenticated;

            _saveChangesCommand.IsEnabled = isLoggedIn;
        }

        void OnSaveChanges()
        {
            if ( SelectedEvent == null ) return;

            try
            {
                if ( !SelectedEvent.HasValidationErrors )
                {
                    if ( SelectedEvent.ID == 0 )
                    {
                        if ( string.IsNullOrEmpty( SelectedEvent.Name ) )
                            throw new ValidationException( "Name must be entered" );

                        var checkedTeamsCount = TeamList.Where( s => s.Checked == true ).Count();

                        if ( checkedTeamsCount == 0 )
                            throw new ValidationException( "At least one team must be selected" );

                        //foreach ( var teamModel in TeamList.Where( s => s.Checked == true ) )
                        //{
                        //    SelectedEvent.EventTeams.Add( new EventTeam() { EventID = SelectedEvent.ID, TeamID = teamModel.ID } );
                        //}

                        _context.Events.Add( SelectedEvent );
                        //SelectedEvent.EventTeams.Add();
                        _context.SubmitChanges().Completed += SyncEventTeams;
                        //SyncEventTeams();
                        // _context.InsertTeamEvents( TeamList.Where( s => s.Checked == true ).ToList() );

                    }
                    else
                        _context.SubmitChanges();
                }
            }
            catch ( ValidationException ex )
            {
                EventValidationErrorsEvent( this, new CustomValidationErrorEventArgs( true, ex.ValidationResult.ErrorMessage ) );
            }
        }

        //private void SubmitNewEventAndFillTeams( object sender, EventArgs e )
        //{
        //    _context.SubmitChanges().Completed += EventAddingCompleted;
        //}

        private void SyncEventTeams( object sender, EventArgs e )
        {
            var inserted = _context.Events.Last();

            var eventId = inserted.ID;

            if ( eventId == 0 )
            {
                ShowDialog( "Event was not added successfully" );
                return;
            }

            var checkedTeamsCount = TeamList.Where( s => s.Checked == true ).Count();

            if ( checkedTeamsCount > 0 )
            {
                foreach ( var t in TeamList.Where( s => s.Checked == true ) )
                {
                    _eventTeamContext.EventTeams.Add( new EventTeam() { EventID = eventId, TeamID = t.ID } );
                }

                _eventTeamContext.SubmitChanges();
            }

            AppMessages.EventAddedMessage.Send();
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
