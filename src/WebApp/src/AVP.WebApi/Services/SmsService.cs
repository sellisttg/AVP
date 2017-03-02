using AVP.DataAccess;
using AVP.Models.Entities;
using AVP.WebApi.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace AVP.WebApi.Services
{

    /// <summary>
    /// Interface for AVP SMS Service
    /// </summary>
    public interface ISmsService
    {
        /// <summary>
        /// Send SMS messages based on list of locations and notification.
        /// </summary>
        /// <param name="notification">Notification</param>
        /// <param name="locations">List of UserSmsLocation</param>
        /// <returns></returns>
        Task<int> SendSmsForNotification(Notification notification, List<UserSmsLocation> locations);
    }
    /// <summary>
    /// Implementation of ISmsService
    /// </summary>
    public class SmsService : ISmsService
    {

        private JwtIssuerOptions _jwt;
        private TwilioOptions _twilioConfig;
        private string _accountSid { get; set; }
        private string _authToken { get; set; }
        private string _msgServiceSid { get; set; }
        private readonly ILogger _logger;

        /// <summary>
        /// Service to deliver SMS messages
        /// </summary>
        public SmsService(ILoggerFactory loggerFactory, IOptions<TwilioOptions> twilioConfig, IOptions<JwtIssuerOptions> jwt)
        {
            _jwt = jwt.Value;
            _twilioConfig = twilioConfig.Value;
            _logger = loggerFactory.CreateLogger<DAO>();
            //setup the config items to send messages
            _accountSid = twilioConfig.Value.AccountSid;
            _authToken = twilioConfig.Value.AuthToken;
            _msgServiceSid = twilioConfig.Value.MsgServiceSid;
        }
        /// <summary>
        /// Send SMS messages for a given notification
        /// </summary>
        /// <param name="notification">Notification</param>
        /// <param name="locations">List of UserSmsLocation</param>
        /// <returns></returns>
        public async Task<int> SendSmsForNotification(Notification notification, List<UserSmsLocation> locations)
        {
            int sent = 0;
            foreach(UserSmsLocation location in locations)
            {
                try
                {
                    await SendSms(notification.Message, location.PhoneNumber.ToString());
                    sent++;
                } catch (Exception e)
                {
                    _logger.LogInformation($"Unable send notification to {location.PhoneNumber}. Exception message is: {e.Message}");
                }                
            }
            return sent;
        }

        /// <summary>
        /// Send SMS Message
        /// </summary>
        /// <param name="messageBody">string</param>
        /// <param name="toPhone">string</param>
        /// <returns></returns>
        private async Task SendSms(string messageBody, string toPhone)
        {
            TwilioClient.Init(_accountSid, _authToken);

            var message = await MessageResource.CreateAsync(
                messagingServiceSid: _msgServiceSid,
                to: new PhoneNumber(toPhone),
                body: messageBody);
        }
    }
}
