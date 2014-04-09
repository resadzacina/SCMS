
namespace _1188.SCMS.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using _1188.SCMS.Web;


    // Implements application logic using the SportsTeamEntities context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class EventTeamService : LinqToEntitiesDomainService<SportsTeamEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'EventTeams' query.
        public IQueryable<EventTeam> GetEventTeams()
        {
            return this.ObjectContext.EventTeams.Include("Event").Include("Team");
        }

        public void InsertEventTeam(EventTeam eventTeam)
        {
            if ((eventTeam.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(eventTeam, EntityState.Added);
            }
            else
            {
                this.ObjectContext.EventTeams.AddObject(eventTeam);
            }
        }

        public void UpdateEventTeam(EventTeam currentEventTeam)
        {
            this.ObjectContext.EventTeams.AttachAsModified(currentEventTeam, this.ChangeSet.GetOriginal(currentEventTeam));
        }

        public void DeleteEventTeam(EventTeam eventTeam)
        {
            if ((eventTeam.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(eventTeam, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.EventTeams.Attach(eventTeam);
                this.ObjectContext.EventTeams.DeleteObject(eventTeam);
            }
        }
    }
}


