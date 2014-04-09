
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


    // The MetadataTypeAttribute identifies aspnet_UsersMetadata as the class
    // that carries additional metadata for the aspnet_Users class.
    [MetadataTypeAttribute(typeof(aspnet_Users.aspnet_UsersMetadata))]
    public partial class aspnet_Users
    {

        // This class allows you to attach custom attributes to properties
        // of the aspnet_Users class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class aspnet_UsersMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private aspnet_UsersMetadata()
            {
            }

            public Guid ApplicationId { get; set; }

            public EntityCollection<aspnet_Roles> aspnet_Roles { get; set; }

            public EntityCollection<Contract> Contracts { get; set; }

            public bool IsAnonymous { get; set; }

            public DateTime LastActivityDate { get; set; }

            public string LoweredUserName { get; set; }

            public Member Member { get; set; }

            public EntityCollection<Message> Messages { get; set; }

            public EntityCollection<Message> Messages1 { get; set; }

            public string MobileAlias { get; set; }

            public EntityCollection<Ticket> Tickets { get; set; }

            public Guid UserId { get; set; }

            public string UserName { get; set; }

            public VisitorCard VisitorCard { get; set; }
        }
    }
}
