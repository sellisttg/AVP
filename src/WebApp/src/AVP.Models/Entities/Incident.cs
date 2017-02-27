using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.Models.Entities
{
    public class Incident
    {
        public int IncidentID { get; set; }
        public string IncidentName { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string IncidentType { get; set; }
        public int Radius { get; set; }
        public string Id { get; set; }
    }
}
