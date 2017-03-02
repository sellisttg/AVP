using AVP.DataAccess;
using AVP.Models.Entities;
using AVP.WebApi.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

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
        private ExchangeOptions _exchangeOptions { get; set; }
        private readonly ILogger _logger;

        /// <summary>
        /// Email Service contstructor configures the options necessary to send email with this implementation
        /// </summary>
        public EmailService(ILoggerFactory loggerFactory, IOptions<SendGridOptions> sendGridConfig, IOptions<ExchangeOptions> exchangeOptions)
        {
            _exchangeOptions = exchangeOptions.Value;
            _logger = loggerFactory.CreateLogger<DAO>();
            //setup the config items to send messages
            _sendGridApiKey = sendGridConfig.Value.SendGridApiKey;
            _fromEmail = sendGridConfig.Value.FromEmail;
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
            foreach (UserEmailLocation location in locations)
            {
                try
                {
                    await SendEmail(notification.Message, location.EmailAddress);
                    //await SendEmailAsync(location.EmailAddress, _exchangeOptions.EmailSubject, notification.Message);
                    sent++;
                }
                catch (Exception e)
                {
                    _logger.LogInformation($"Unable send notification to {location.EmailAddress}. Exception message is: {e.Message}");
                }
            }
            return sent;
        }

        private async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("AVP Thunderstruck", _exchangeOptions.UserName));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                client.LocalDomain = _exchangeOptions.HostName;
                await client.ConnectAsync(_exchangeOptions.HostName, _exchangeOptions.Port, SecureSocketOptions.StartTls).ConfigureAwait(false);
                await client.AuthenticateAsync(_exchangeOptions.UserName, _exchangeOptions.Password);
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
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
