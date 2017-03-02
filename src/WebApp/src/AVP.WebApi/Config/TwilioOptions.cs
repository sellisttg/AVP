using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AVP.WebApi.Config
{
    /// <summary>
    /// Configuration class for Twilio
    /// </summary>
    public class TwilioOptions
    {
        /// <summary>
        /// Account SID from Twilio
        /// </summary>
        public string AccountSid { get; set; }
        /// <summary>
        /// Auth Token from Twilio
        /// </summary>
        public string AuthToken { get; set; }
        /// <summary>
        /// Message Service SID from Twilio
        /// </summary>
        public string MsgServiceSid { get; set; }
    }
}
