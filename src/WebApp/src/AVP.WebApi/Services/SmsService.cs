using AVP.DataAccess;
using AVP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace AVP.WebApi.Services
{


    public interface ISmsService
    {
        Task<int> SendSmsForNotification(Notification notification, List<UserSmsLocation> locations);
    }
    public class SmsService : ISmsService
    {
        private string _accountSid { get; set; }
        private string _authToken { get; set; }
        private string _msgServiceSid { get; set; }

        public SmsService()
        {
            //setup the config items to send messages
            _accountSid = "ACfa841159cde856eedf95b63b3bd85bcb";
            _authToken = "ff93ad066621e2fec4a3412086ef2e5e";
            _msgServiceSid = "MG6cc1ba72f34643860053b0ea63ac76a1";
        }

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
                }                
            }
            return sent;
        }

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
