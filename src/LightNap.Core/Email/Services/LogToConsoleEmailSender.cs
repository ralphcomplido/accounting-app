using LightNap.Core.Email.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace LightNap.Core.Email.Services
{
    /// <summary>
    /// Service for logging email details to the console instead of sending them.
    /// </summary>
    public class LogToConsoleEmailSender(ILogger<LogToConsoleEmailSender> logger) : IEmailSender
    {
        /// <summary>
        /// Logs the email details to the console asynchronously.
        /// </summary>
        /// <param name="message">The email message to log.</param>
        /// <returns>A completed task.</returns>
        public Task SendMailAsync(MailMessage message)
        {
            logger.LogInformation(
@$"Logging email to console
To: {message.To}
From: {message.From}
Subject: {message.Subject}
--- Body Start ---
{message.Body}
--- Body End ---");
            return Task.CompletedTask;
        }
    }
}
