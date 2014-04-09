using System;

namespace _1188.SCMS.Models
{
    public class VisitorCardExtension
    {
        public int ID { get; set; }
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsAllowedAccess { get; set; }
    }
}
