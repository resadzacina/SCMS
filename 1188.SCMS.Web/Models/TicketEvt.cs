using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace _1188.SCMS.Web.Models
{
    [DataContract]
    [Serializable]
    public partial class TicketEvt
    {
        [DataMember]
        public Ticket Ticket { get; set; }
        [DataMember]
        public Event Event { get; set; }
    }
}