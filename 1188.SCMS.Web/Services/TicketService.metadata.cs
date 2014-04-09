
namespace _1188.SCMS.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies PaymentMethodMetadata as the class
    // that carries additional metadata for the PaymentMethod class.
    [MetadataTypeAttribute(typeof(PaymentMethod.PaymentMethodMetadata))]
    public partial class PaymentMethod
    {

        // This class allows you to attach custom attributes to properties
        // of the PaymentMethod class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class PaymentMethodMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private PaymentMethodMetadata()
            {
            }

            public int ID { get; set; }

            public string Method { get; set; }

            public EntityCollection<Ticket> Tickets { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies TicketMetadata as the class
    // that carries additional metadata for the Ticket class.
    [MetadataTypeAttribute(typeof(Ticket.TicketMetadata))]
    public partial class Ticket
    {

        // This class allows you to attach custom attributes to properties
        // of the Ticket class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class TicketMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private TicketMetadata()
            {
            }

            [Include]
            public EntityCollection<EventTicket> EventTickets { get; set; }

            public int ID { get; set; }

            public bool IsAlreadyChecked { get; set; }

            [Include]
            public PaymentMethod PaymentMethod { get; set; }
            [Required]
            public Nullable<int> PaymentMethodID { get; set; }

            [Required]
            public Nullable<short> SeatNumber { get; set; }

          
            [Include]
            public TicketType TicketType { get; set; }

            public Nullable<int> TicketTypeID { get; set; }


            [Include]
            public aspnet_User User { get; set; }
            [Required]
            public Guid UserID { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies TicketTypeMetadata as the class
    // that carries additional metadata for the TicketType class.
    [MetadataTypeAttribute(typeof(TicketType.TicketTypeMetadata))]
    public partial class TicketType
    {

        // This class allows you to attach custom attributes to properties
        // of the TicketType class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class TicketTypeMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private TicketTypeMetadata()
            {
            }

            public int ID { get; set; }

            public decimal Price { get; set; }

            public EntityCollection<Ticket> Tickets { get; set; }

            public string TypeName { get; set; }
        }
    }
}
