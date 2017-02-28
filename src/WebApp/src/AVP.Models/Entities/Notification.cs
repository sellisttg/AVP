using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.Models.Entities
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string Message { get; set; }
        public DateTime MessageDateTime { get; set; }
        public int SendingUserID { get; set; }
        public int IncidentID { get; set; }
    }
}
