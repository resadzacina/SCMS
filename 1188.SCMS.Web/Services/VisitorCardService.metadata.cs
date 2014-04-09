using System.ServiceModel.DomainServices.Server;

namespace _1188.SCMS.Web
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Objects.DataClasses;

    [MetadataTypeAttribute(typeof(VisitorCard.VisitorCardMetadata))]
    public partial class VisitorCard
    {
        internal sealed class VisitorCardMetadata
        {
            private VisitorCardMetadata()
            {
            }

            public int ID { get; set; }
            public Guid UserID { get; set; }
            //public string UserName { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public bool IsAllowedAccess { get; set; }

            [Include]
            public aspnet_Users User { get; set; }
        }
    }
}