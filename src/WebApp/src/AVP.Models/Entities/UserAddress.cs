using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.Models.Entities
{
    public class UserAddress
    {
        public int UserAddressID { get; set; }
        public int UserID { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
