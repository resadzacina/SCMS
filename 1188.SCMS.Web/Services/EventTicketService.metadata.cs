
namespace _1188.SCMS.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies EventTicketMetadata as the class
    // that carries additional metadata for the EventTicket class.
    [MetadataTypeAttribute(typeof(EventTicket.EventTicketMetadata))]
    public partial class EventTicket
    {

        // This class allows you to attach custom attributes to properties
        // of the EventTicket class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class EventTicketMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private EventTicketMetadata()
            {
            }

            public string Alias { get; set; }

            public Event Event { get; set; }

            public int EventID { get; set; }

            public Ticket Ticket { get; set; }

            public int TicketID { get; set; }
        }
    }
}
