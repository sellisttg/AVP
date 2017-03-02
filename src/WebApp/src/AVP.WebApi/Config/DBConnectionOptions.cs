using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.WebApi.Config
{
    /// <summary>
    /// Defines db connection settings
    /// </summary>
    public class DBConnectionOptions
    {
        /// <summary>
        /// Connection string for data layer
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// Schema to use in DB Connection
        /// </summary>
        public string Schema { get; set; }
    }
}
