using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.Models.Entities
{
    public class UserEmailLocation
    {
        public int UserEmailLocationID { get; set; }
        public int UserID { get; set; }
        public string EmailAddress { get; set; }
        public int UserAddressID { get; set; }
    }
}

