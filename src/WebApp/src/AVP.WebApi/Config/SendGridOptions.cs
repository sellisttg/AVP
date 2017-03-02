using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.WebApi.Config
{
    /// <summary>
    /// Configuration class for SendGrid options
    /// </summary>
    public class SendGridOptions
    {
        /// <summary>
        /// API Key for Send Grid
        /// </summary>
        public string SendGridApiKey { get; set; }
        /// <summary>
        /// Email to send from SendGrid
        /// </summary>
        public string FromEmail { get; set; }
    }
}
