using LightNap.Core.Data.Entities;
using LightNap.Core.Extensions;
using LightNap.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace LightNap.Core.Services
{
    /// <summary>
    /// Service for sending emails using SMTP.
    /// </summary>
    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;
        private readonly string _fromDisplayName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpEmailService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use for setting up the SMTP client.</param>
        public SmtpEmailService(IConfiguration configuration)
        {
            this._smtpClient = new SmtpClient(configuration.GetRequiredSetting("Smtp:Host"), int.Parse(configuration.GetRequiredSetting("Smtp:Port")))
            {
                Credentials = new NetworkCredential(configuration.GetRequiredSetting("Smtp:User"), configuration.GetRequiredSetting("Smtp:Password")),
                EnableSsl = bool.Parse(configuration.GetRequiredSetting("Smtp:EnableSsl"))
            };

            this._fromEmail = configuration.GetRequiredSetting("Smtp:FromEmail");
            this._fromDisplayName = configuration.GetRequiredSetting("Smtp:FromDisplayName");
        }

        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="message">The email message to send.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendEmailAsync(MailMessage message)
        {
            message.From = new MailAddress(this._fromEmail, this._fromDisplayName);
            await this._smtpClient.SendMailAsync(message);
        }

        /// <summary>
        /// Sends a password reset email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="passwordResetUrl">The URL for resetting the password.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendPasswordResetEmailAsync(ApplicationUser user, string passwordResetUrl)
        {
            await this.SendEmailAsync(new MailMessage(this._fromEmail, user.Email!, "Reset your password", $"You may reset your password at: {passwordResetUrl}"));
        }

        /// <summary>
        /// Sends an email change email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="emailChangeUrl">The URL for verifying the email change.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendChangeEmailAsync(ApplicationUser user, string newEmail, string emailChangeUrl)
        {
            await this.SendEmailAsync(new MailMessage(this._fromEmail, newEmail, "Confirm your email change", $"You may confirm your email change at: {emailChangeUrl}"));
        }

        /// <summary>
        /// Sends an email verification email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="emailVerificationUrl">The URL for verifying the email.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendVerificationEmailAsync(ApplicationUser user, string emailVerificationUrl)
        {
            await this.SendEmailAsync(new MailMessage(this._fromEmail, user.Email!, "Confirm your email", $"You may confirm your email at: {emailVerificationUrl}"));
        }

        /// <summary>
        /// Sends a registration email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendRegistrationEmailAsync(ApplicationUser user)
        {
            await this.SendEmailAsync(new MailMessage(this._fromEmail, user.Email!, "Welcome to our site", $"Thank you for registering."));
        }

        /// <summary>
        /// Sends a two-factor authentication email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="code">The two-factor authentication code.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendTwoFactorEmailAsync(ApplicationUser user, string code)
        {
            await this.SendEmailAsync(new MailMessage(this._fromEmail, user.Email!, "Your login security code", $"Your login code is: {code}"));
        }
    }
}
