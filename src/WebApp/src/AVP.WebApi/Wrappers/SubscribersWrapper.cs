using AVP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.WebApi.Wrappers
{
    /// <summary>
    /// Wrapper for subscriber objects to facilitate GIS integration
    /// </summary>
    public class SubscribersWrapper
    {
        /// <summary>
        /// List of Subscribers
        /// </summary>
        public List<Subscriber> Subscribers { get; set; }
    }

    /// <summary>
    /// Wrapper for subscriber objects to facilitate GIS integration
    /// </summary>
    public class SubscriberUnderNotificationWrapper
    {
        /// <summary>
        /// List of subscribers
        /// </summary>
        public List<Subscriber> SubscriberUnderNotification { get; set; }
    }
}
