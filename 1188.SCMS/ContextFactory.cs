using _1188.SCMS.Web;
using _1188.SCMS.Web.Services;

namespace _1188.SCMS
{
    
    public class ContextFactory
    {
        static TeamContext _teamContext;
        public static TeamContext GetTeamContext()
        {
            return _teamContext ?? (_teamContext = new TeamContext());
        }

        static UserRegistrationContext _userRegistrationContext;
        public static UserRegistrationContext GetUserRegistrationContext()
        {
            return _userRegistrationContext ?? (_userRegistrationContext = new UserRegistrationContext());
        }

        static CountryContext _countryContext;
        public static CountryContext GetCountryContext()
        {
            return _countryContext ?? (_countryContext = new CountryContext());
        }

        static MembershipContext _membershipContext;
        public static MembershipContext GetMembershipContext()
        {
            return _membershipContext ?? (_membershipContext = new MembershipContext());
        }

        static EventContext _eventContext;
        public static EventContext GetEventContext()
        {
            return _eventContext ?? (_eventContext = new EventContext());
        }

        static MessageContext _messageContext;
        public static MessageContext GetMessageContext()
        {
            return _messageContext ?? (_messageContext = new MessageContext());
        }

        static UsersContext _userContext;
        public static UsersContext GetUserContext()
        {
            return _userContext ?? (_userContext = new UsersContext());
        }

        static EventTeamContext _eventTeamContext;
        public static EventTeamContext GetEventTeamContext()
        {
            return _eventTeamContext ?? (_eventTeamContext = new EventTeamContext());
        }

        static VisitorCardContext _visitorContext;
        public static VisitorCardContext GetVisitorCardContext()
        {
            return _visitorContext ?? (_visitorContext = new VisitorCardContext());
        }

        static TicketContext _ticketContext;
        public static TicketContext GetTicketContext()
        {
            return _ticketContext ?? (_ticketContext = new TicketContext());
        }

        static EventTicketContext _evtticketContext;
        public static EventTicketContext GetEventTicketContext()
        {
            return _evtticketContext ?? (_evtticketContext = new EventTicketContext());
        }
    }
}
