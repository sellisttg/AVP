using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.Models.Entities
{
    public class DashboardCriteria
    {
        public int UserID { get; set; }
        public string Role { get; set; }
    }
    public class DashboardNotification
    {
        public int NotificationID { get; set; }
        public int IncidentID { get; set; }
        public string IncidentType { get; set; }
        public string Name { get; set; }
        public DateTime MessageDateTime { get; set; }
        public string Message { get; set; }
        public string Address { get; set; }
    }
}
