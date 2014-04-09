
namespace _1188.SCMS.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies MessageMetadata as the class
    // that carries additional metadata for the Message class.
    [MetadataTypeAttribute(typeof(Message.MessageMetadata))]
    public partial class Message
    {

        // This class allows you to attach custom attributes to properties
        // of the Message class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class MessageMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private MessageMetadata()
            {
            }

            //public aspnet_Users aspnet_Users { get; set; }

            //public aspnet_Users aspnet_Users1 { get; set; }

            public string Body { get; set; }

            [Required]
            public string From { get; set; }

            public int ID { get; set; }

            [Required]
            public string Subject { get; set; }

            public Nullable<DateTime> TimeStamp { get; set; }

            [Required]
            public string To { get; set; }
        }
    }
}
