
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


    // The MetadataTypeAttribute identifies CountryMetadata as the class
    // that carries additional metadata for the Country class.
    [MetadataTypeAttribute( typeof( Country.CountryMetadata ) )]
    public partial class Country
    {

        // This class allows you to attach custom attributes to properties
        // of the Country class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class CountryMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private CountryMetadata()
            {
            }

            public string CountryName { get; set; }

            public int ID { get; set; }

            public EntityCollection<Member> Members { get; set; }
        }
    }
}
