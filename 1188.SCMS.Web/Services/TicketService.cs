
using System.Web.Security;

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
    using _1188.SCMS.Web.Models;


    // Implements application logic using the SportsTeamEntities context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class TicketService : LinqToEntitiesDomainService<SportsTeamEntities>
    {
        #region PaymentMethod

        public IQueryable<PaymentMethod> GetPaymentMethods()
        {
            return this.ObjectContext.PaymentMethods;
        }

        public void InsertPaymentMethod(PaymentMethod paymentMethod)
        {
            if ((paymentMethod.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(paymentMethod, EntityState.Added);
            }
            else
            {
                this.ObjectContext.PaymentMethods.AddObject(paymentMethod);
            }
        }

        public void UpdatePaymentMethod(PaymentMethod currentPaymentMethod)
        {
            this.ObjectContext.PaymentMethods.AttachAsModified(currentPaymentMethod, this.ChangeSet.GetOriginal(currentPaymentMethod));
        }

        public void DeletePaymentMethod(PaymentMethod paymentMethod)
        {
            if ((paymentMethod.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(paymentMethod, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.PaymentMethods.Attach(paymentMethod);
                this.ObjectContext.PaymentMethods.DeleteObject(paymentMethod);
            }
        }
        #endregion

        public IQueryable<Ticket> GetTickets()
        {
            return this.ObjectContext.Tickets.Include("PaymentMethod").Include("TicketType").Include("User");
        }

        public IQueryable<Ticket> GetTicketsByEvent(int eventId)
        {
            // var events = ObjectContext.EventTickets.Where(ev => ev.EventID == eventId);
            return this.ObjectContext.Tickets.Include("PaymentMethod").Include("TicketType").Include("User").Where(e => e.EventTickets.Any(ev => ev.EventID == eventId));
        }

        [Query(IsComposable = false)]
        public Ticket GetTicketById(int id)
        {
            var user = Membership.GetUser();
            if (user == null)
            {
                throw new InvalidOperationException("User not logged in");
            }

            return ObjectContext.Tickets.Where(t => t.ID == id).FirstOrDefault();
        }

        public void InsertTicket(Ticket ticket)
        {
            if ((ticket.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(ticket, EntityState.Added);
            }
            else
            {
                this.ObjectContext.Tickets.AddObject(ticket);
            }
        }

        public void UpdateTicket(Ticket currentTicket)
        {
            this.ObjectContext.Tickets.AttachAsModified(currentTicket, this.ChangeSet.GetOriginal(currentTicket));
        }

        [Invoke]
        public void DeleteTicket(Ticket ticket)
        {
            var original = ObjectContext.EventTickets.Where(e => e.TicketID == ticket.ID).First();

            if ((ticket.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(ticket, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.Tickets.Attach(ticket);
                this.ObjectContext.EventTickets.DeleteObject(original);
                this.ObjectContext.Tickets.DeleteObject(ticket);
            }

            ObjectContext.SaveChanges();
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'TicketTypes' query.
        public IQueryable<TicketType> GetTicketTypes()
        {
            return this.ObjectContext.TicketTypes;
        }

        public void InsertTicketType(TicketType ticketType)
        {
            if ((ticketType.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(ticketType, EntityState.Added);
            }
            else
            {
                this.ObjectContext.TicketTypes.AddObject(ticketType);
            }
        }

        public void UpdateTicketType(TicketType currentTicketType)
        {
            this.ObjectContext.TicketTypes.AttachAsModified(currentTicketType, this.ChangeSet.GetOriginal(currentTicketType));
        }

        public void DeleteTicketType(TicketType ticketType)
        {
            if ((ticketType.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(ticketType, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.TicketTypes.Attach(ticketType);
                this.ObjectContext.TicketTypes.DeleteObject(ticketType);
            }
        }
    }
}


