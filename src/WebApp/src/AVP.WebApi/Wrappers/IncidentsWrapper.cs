using AVP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.WebApi.Wrappers
{
    /// <summary>
    /// Wrapper class for Incident objects to ease GIS component integration
    /// </summary>
    public class IncidentsWrapper
    {
        /// <summary>
        /// List of Incident objects
        /// </summary>
        public List<Incident> incidents { get; set; }
    }
}
