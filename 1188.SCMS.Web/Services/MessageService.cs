
using System.Data;
using System.ServiceModel.DomainServices.Server;

namespace _1188.SCMS.Web.Services
{
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using Web;

    [EnableClientAccess]
    public class MessageService : LinqToEntitiesDomainService<SportsTeamEntities>
    {
        [Update]
        public void UpdateMessage(Message message)
        {
            ObjectContext.Messages.AttachAsModified(message, ChangeSet.GetOriginal(message));
        }

        [Invoke(HasSideEffects = true)]
        public void InsertMessage(Message message)
        {
            if ((message.EntityState != EntityState.Detached))
            {
                ObjectContext.ObjectStateManager.ChangeObjectState(message, EntityState.Added);
            }
            else
            {
                ObjectContext.Messages.AddObject(message);

                ObjectContext.SaveChanges();
            }
        }

        [Invoke(HasSideEffects = true)]
        public void DeleteMessage(Message message)
        {
            var original = ObjectContext.Messages.Where(m => m.ID == message.ID).First();

            if (original != null)
            {
                original.IsDeleted = true;
                ObjectContext.Messages.ApplyCurrentValues(original);
                ObjectContext.SaveChanges();
            }
        }

        public IQueryable<Message> GetMessages()
        {
            return ObjectContext.Messages;
        }

        public IQueryable<Message> GetMessagesSentToUserName(string name)
        {
            if (!string.IsNullOrEmpty(name))
                return ObjectContext.Messages.Where(c => c.To == name && c.IsDeleted == false);

            return new Message[0].AsQueryable();
        }

        public IQueryable<Message> GetMessagesSentFromUserName(string name)
        {
            if (!string.IsNullOrEmpty(name))
                return ObjectContext.Messages.Where(c => c.From == name && c.IsDeleted == false);

            return new Message[0].AsQueryable();
        }

        //public IQueryable<Message> GetDeletedMessagesUserName(string name)
        //{
        //    if (!string.IsNullOrEmpty(name))
        //        return ObjectContext.Messages.Where(c => c.From == name && c.IsDeleted == false);

        //    return new Message[0].AsQueryable();
        //}
    }
}


