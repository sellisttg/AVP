using AVP.DataAccess;
using AVP.Models.Entities;
using SendGrid;
using SendGrid.Helpers.Mail;
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
    /// IEmailService defines the interface necessary for email services
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Method to send emails to a list of locations with a specific notification.
        /// </summary>
        /// <param name="notification">Notification</param>
        /// <param name="locations">List of UserEmailLocation</param>
        /// <returns>int count of messages sent</returns>
        Task<int> SendEmailForNotification(Notification notification, List<UserEmailLocation> locations);
    }
    /// <summary>
    /// Implementation of IEmailService
    /// </summary>
    public class EmailService : IEmailService
    {
        private string _sendGridApiKey { get; set; }
        private string _fromEmail { get; set; }

        /// <summary>
        /// Email Service contstructor configures the options necessary to send email with this implementation
        /// </summary>
        public EmailService()
        {
            //setup the config items to send messages
            _sendGridApiKey = "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";
            _fromEmail = "devtest@trinitytg.com";
        }

        /// <summary>
        /// Send Emails to all locations supplied for a given notification
        /// </summary>
        /// <param name="notification">Notification</param>
        /// <param name="locations">List of UserEmailLocations</param>
        /// <returns>int count of sent messages</returns>
        public async Task<int> SendEmailForNotification(Notification notification, List<UserEmailLocation> locations)
        {
            int sent = 0;
            foreach(UserEmailLocation location in locations)
            {
                try
                {
                    await SendEmail(notification.Message, location.EmailAddress);
                    sent++;
                } catch (Exception e)
                {
                }                
            }
            return sent;
        }

        private async Task SendEmail(string messageBody, string toEmail)
        {
            var apiKey = _sendGridApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_fromEmail, "TTG Thunderstruck");
            var subject = "Alert from Thunderstruck Notification System";
            var to = new EmailAddress(toEmail);
            var plainTextContent = messageBody;
            var htmlContent = $"<strong>{messageBody}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
