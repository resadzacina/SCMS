using System.ServiceModel.DomainServices.Server;

namespace _1188.SCMS.Web
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Objects.DataClasses;

    [MetadataTypeAttribute( typeof( Event.EventMetadata ) )]
    public partial class Event
    {
        internal sealed class EventMetadata
        {
            private EventMetadata()
            {
            }

            public DateTime DateEnd { get; set; }

            public DateTime DateStart { get; set; }

            public string Description { get; set; }

            public int ID { get; set; }

            [Required( AllowEmptyStrings = false )]
            public string Name { get; set; }

            //public bool Checked { get; set; }

            public Nullable<bool> IsCancelled { get; set; }

            public Nullable<bool> IsPostponed { get; set; }

            [Include]
            public EntityCollection<EventTeam> EventTeams { get; set; }

            public EntityCollection<EventTicket> EventTickets { get; set; }

            public int AvailableTickets { get; set; }
        }
    }
}
