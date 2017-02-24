using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.Models.Entities
{
    public class UserSmsLocation
    {
        public int UserSmsLocationID { get; set; }
        public int UserID { get; set; }
        public long PhoneNumber { get; set; }
        public int UserAddressID { get; set; }
    }
}
