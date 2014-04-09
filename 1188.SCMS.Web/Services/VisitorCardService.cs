
using System.Web.Security;

namespace _1188.SCMS.Web.Services
{
    using System;
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
    public class VisitorCardService : LinqToEntitiesDomainService<SportsTeamEntities>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'VisitorCards' query.
        public IQueryable<VisitorCard> GetVisitorCards()
        {
            return this.ObjectContext.VisitorCards.Include("User");
        }

        public IQueryable<VisitorCard> GetReportVisitorCards()
        {
            SportsTeamEntities ent = new SportsTeamEntities();

            return ent.VisitorCards.Include("User");
        }

        public void InsertVisitorCard(VisitorCard visitorCard)
        {
            if ((visitorCard.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(visitorCard, EntityState.Added);
            }
            else
            {
                this.ObjectContext.VisitorCards.AddObject(visitorCard);
            }
        }

        public void UpdateVisitorCard(VisitorCard currentVisitorCard)
        {
            this.ObjectContext.VisitorCards.AttachAsModified(currentVisitorCard, this.ChangeSet.GetOriginal(currentVisitorCard));
        }

        public void DeleteVisitorCard(VisitorCard visitorCard)
        {
            if ((visitorCard.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(visitorCard, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.VisitorCards.Attach(visitorCard);
                this.ObjectContext.VisitorCards.DeleteObject(visitorCard);
            }
        }

        [Query(IsComposable = false)]
        public VisitorCard GetVisitorCardById(int id)
        {
            var user = Membership.GetUser();
            if (user == null)
            {
                throw new InvalidOperationException("User not logged in");
            }

            return ObjectContext.VisitorCards.Where(t => t.ID == id).FirstOrDefault();
        }
    }
}


