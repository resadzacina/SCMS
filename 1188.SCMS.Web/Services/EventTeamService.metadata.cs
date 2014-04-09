
namespace _1188.SCMS.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies EventTeamMetadata as the class
    // that carries additional metadata for the EventTeam class.
    [MetadataTypeAttribute(typeof(EventTeam.EventTeamMetadata))]
    public partial class EventTeam
    {

        // This class allows you to attach custom attributes to properties
        // of the EventTeam class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class EventTeamMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private EventTeamMetadata()
            {
            }

            public string Description { get; set; }

            [Include]
            public Event Event { get; set; }

            public int EventID { get; set; }

            [Include]
            public Team Team { get; set; }

            public int TeamID { get; set; }
        }
    }
}
