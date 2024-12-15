using LightNap.Core.Data.Entities;
using LightNap.Core.Extensions;
using LightNap.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace LightNap.Core.Services
{
    /// <summary>
    /// Service for sending emails using SMTP.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DefaultEmailService"/> class.
    /// </remarks>
    /// <param name="configuration">The configuration to use for setting up the default email service.</param>
    /// <param name="emailSender">The email sending service.</param>
    public class DefaultEmailService(IConfiguration configuration, IEmailSender emailSender) : IEmailService
    {
        private readonly string _fromEmail = configuration.GetRequiredSetting("Email:FromEmail");
        private readonly string _fromDisplayName = configuration.GetRequiredSetting("Email:FromDisplayName");

        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="message">The email message to send.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendMailAsync(MailMessage message)
        {
            await emailSender.SendMailAsync(message);
        }

        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="message">The email message to send.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendMailAsync(ApplicationUser user, string subject, string body)
        {
            await emailSender.SendMailAsync(
                new MailMessage(new MailAddress(this._fromEmail, this._fromDisplayName), new MailAddress(user.Email!, user.UserName))
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                });
        }

        /// <summary>
        /// Sends a password reset email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="passwordResetUrl">The URL for resetting the password.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendPasswordResetAsync(ApplicationUser user, string passwordResetUrl)
        {
            await this.SendMailAsync(user, "Reset your password", $"You may reset your password <a href='{passwordResetUrl}'>here</a>.");
        }

        /// <summary>
        /// Sends an email change email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="emailChangeUrl">The URL for verifying the email change.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendChangeEmailAsync(ApplicationUser user, string newEmail, string emailChangeUrl)
        {
            await emailSender.SendMailAsync(new MailMessage(this._fromEmail, newEmail, "Confirm your email change", $"You may confirm your email change <a href='{emailChangeUrl}'>here</a>."));
        }

        /// <summary>
        /// Sends an email verification email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="emailVerificationUrl">The URL for verifying the email.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendEmailVerificationAsync(ApplicationUser user, string emailVerificationUrl)
        {
            await this.SendMailAsync(user, "Confirm your email", $"You may confirm your email <a href='{emailVerificationUrl}'>here</a>.");
        }

        /// <summary>
        /// Sends a registration email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendRegistrationWelcomeAsync(ApplicationUser user)
        {
            await this.SendMailAsync(user, "Welcome to our site", $"Thank you for registering.");
        }

        /// <summary>
        /// Sends a two-factor authentication email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="code">The two-factor authentication code.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendTwoFactorAsync(ApplicationUser user, string code)
        {
            await this.SendMailAsync(user, "Your login security code", $"Your login code is: {code}");
        }

        /// <summary>
        /// Sends a magic link email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="magicLinkUrl">The magic link URL.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendMagicLinkAsync(ApplicationUser user, string magicLinkUrl)
        {
            await this.SendMailAsync(user, "Your login link", $"You may log in <a href='{magicLinkUrl}'>here</a>.");
        }
    }
}
