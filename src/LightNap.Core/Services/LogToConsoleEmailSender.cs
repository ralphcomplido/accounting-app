using LightNap.Core.Interfaces;
using System.Diagnostics;
using System.Net.Mail;

namespace LightNap.Core.Services
{
    /// <summary>
    /// Service for logging email details to the console instead of sending them.
    /// </summary>
    public class LogToConsoleEmailSender : IEmailSender
    {
        /// <summary>
        /// Logs the email details to the console asynchronously.
        /// </summary>
        /// <param name="message">The email message to log.</param>
        /// <returns>A completed task.</returns>
        public Task SendMailAsync(MailMessage message)
        {
            Trace.TraceInformation($"Not sending email to '{message.To}' with subject '{message.Subject}' and body '{message.Body}'");
            return Task.CompletedTask;
        }
    }
}
