using AVP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.WebApi.Wrappers
{
    public class SubscribersWrapper
    {
        public List<Subscriber> Subscribers { get; set; }
    }

    public class SubscriberUnderNotificationWrapper
    {
        public Incident incident { get; set; }
        public List<Subscriber> SubscriberUnderNotification { get; set; }
    }
}
