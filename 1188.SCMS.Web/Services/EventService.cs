
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel.DomainServices.Server;
using System.Web.Security;

namespace _1188.SCMS.Web.Services
{
    using System;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using Web;

    [EnableClientAccess]
    public class EventService : LinqToEntitiesDomainService<SportsTeamEntities>
    {
        public IQueryable<Event> GetEvents()
        {
            IQueryable<Event> events = ObjectContext.Events.Where( e => e.IsDeleted == false );
            return events;
        }

        public IQueryable<Event> SearchEvents( string searchTerm )
        {
            IQueryable<Event> events;

            if ( string.IsNullOrEmpty( searchTerm ) )
            {
                events = ObjectContext.Events.Where( e => e.IsDeleted == false );
            }
            else
                events = ObjectContext.Events.Where( e => e.IsDeleted == false && e.Name.ToLower().Contains( searchTerm.ToLower() ) );

            return events;
        }

        public IQueryable<Event> GetEventsByStartDay( DateTime dt )
        {
            var events = ObjectContext.Events.Include( "Team" ).Where( c => c.DateStart.Day == dt.Day && c.DateStart.Month == dt.Month && c.DateStart.Year == dt.Year );
            return events;
        }

        [Query( IsComposable = false )]
        public Event GetEventById( int evtId )
        {
            var user = Membership.GetUser();

            if ( user == null )
            {
                throw new InvalidOperationException( "User not logged in" );
            }

            return Queryable.FirstOrDefault( Queryable.Where( ObjectContext.Events.Include( "EventTeams.Event" ).Include( "EventTeams.Team" ), t => t.ID == evtId ) );
        }
        
        public void InsertEvent( Event entity )
        {
            if ( ( entity.EntityState != EntityState.Detached ) )
            {
                ObjectContext.ObjectStateManager.ChangeObjectState( entity, EntityState.Added );
            }
            else
            {
                ObjectContext.Events.AddObject( entity );
            }

            //ObjectContext.SaveChanges();
        }

        //[Invoke]
        //public bool InsertTeamEvents( List<Team> teamList )
        //{
        //    var inserted = ObjectContext.Events.Last();

        //    var eventId = inserted.ID;

        //    if ( eventId == 0 )
        //    {
        //        return false;
        //    }

        //    //var checkedTeamsCount = teamList.Where( s => s.Checked == true ).Count();

        //    //if ( checkedTeamsCount > 0 )
        //    //{
        //    foreach ( var t in teamList )
        //    {
        //        ObjectContext.EventTeams.AddObject( new EventTeam() { EventID = eventId, TeamID = t.ID } );
        //    }

        //    ObjectContext.SaveChanges();
        //    //}

        //    return true;
        //}

        [Invoke]
        public Event GetLast()
        {
            return ObjectContext.Events.Last();
        }


        [Update]
        public void UpdateEvent( Event evt )
        {
            try
            {
                var original = this.ChangeSet.GetOriginal( evt );

                this.ObjectContext.Events.AttachAsModified( evt, original );
            }
            catch ( Exception )
            {
            }
        }

        [Invoke]
        public void DeleteEvent( Event evt )
        {
            var original = ObjectContext.Events.Where( e => e.ID == evt.ID ).First();

            original.IsDeleted = true;

            ObjectContext.Events.ApplyCurrentValues( original );

            ObjectContext.SaveChanges();
        }
    }
}


