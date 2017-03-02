using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.WebApi.Config
{
    /// <summary>
    /// Options for sending Exchange email
    /// </summary>
    public class ExchangeOptions
    {
        /// <summary>
        /// Username for mailbox
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Password for mailbox
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Hostname for mailbox
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// Port for mailbox
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Enable SSL for mailbox
        /// </summary>
        public bool EnableSSL { get; set; }
        /// <summary>
        /// Email subject
        /// </summary>
        public string EmailSubject { get; set; }
    }
}
