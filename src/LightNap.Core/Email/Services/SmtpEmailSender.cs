using LightNap.Core.Email.Interfaces;
using LightNap.Core.Extensions;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace LightNap.Core.Email.Services
{
    /// <summary>
    /// Service for sending emails using SMTP.
    /// </summary>
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpEmailSender"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use for setting up the SMTP client.</param>
        public SmtpEmailSender(IConfiguration configuration)
        {
            this._smtpClient = new SmtpClient(configuration.GetRequiredSetting("Email:Smtp:Host"), int.Parse(configuration.GetRequiredSetting("Email:Smtp:Port")))
            {
                Credentials = new NetworkCredential(configuration.GetRequiredSetting("Email:Smtp:User"), configuration.GetRequiredSetting("Email:Smtp:Password")),
                EnableSsl = bool.Parse(configuration.GetRequiredSetting("Email:Smtp:EnableSsl"))
            };
        }

        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="message">The email message to send.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendMailAsync(MailMessage message)
        {
            await this._smtpClient.SendMailAsync(message);
        }
    }
}
