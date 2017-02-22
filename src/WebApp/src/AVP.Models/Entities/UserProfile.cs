using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.Models.Entities
{
    public class UserProfile
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public bool SmsOptIn { get; set; } 
        public bool EmailOptIn { get; set; }
        public bool PushOptIn { get; set; }
    }
}
