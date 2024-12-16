using System.Net.Mail;

namespace LightNap.Core.Email.Interfaces
{
    /// <summary>
    /// Interface for sending email.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="message">The email message to send.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SendMailAsync(MailMessage message);
    }
}