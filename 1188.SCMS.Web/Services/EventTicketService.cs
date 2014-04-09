
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
    public class EventTicketService : LinqToEntitiesDomainService<SportsTeamEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'EventTickets' query.
        public IQueryable<EventTicket> GetEventTickets()
        {
            return this.ObjectContext.EventTickets;
        }

        public void InsertEventTicket(EventTicket eventTicket)
        {
            if ((eventTicket.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(eventTicket, EntityState.Added);
            }
            else
            {
                this.ObjectContext.EventTickets.AddObject(eventTicket);
            }
        }

        public void UpdateEventTicket(EventTicket currentEventTicket)
        {
            this.ObjectContext.EventTickets.AttachAsModified(currentEventTicket, this.ChangeSet.GetOriginal(currentEventTicket));
        }

        public void DeleteEventTicket(EventTicket eventTicket)
        {
            if ((eventTicket.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(eventTicket, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.EventTickets.Attach(eventTicket);
                this.ObjectContext.EventTickets.DeleteObject(eventTicket);
            }
        }
    }
}


