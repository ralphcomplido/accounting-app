using LightNap.Core.Configuration;
using LightNap.Core.Data.Entities;
using LightNap.Core.Email.Interfaces;
using LightNap.Core.Email.Templates;
using LightNap.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace LightNap.Core.Email.Services
{
    /// <summary>
    /// Service for sending emails using SMTP.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DefaultEmailService"/> class.
    /// </remarks>
    /// <param name="configuration">The configuration to use for setting up the default email service.</param>
    /// <param name="emailSender">The email sending service.</param>
    /// <param name="applicationSettings">The application settings to use for the email service.</param>
    public class DefaultEmailService(IConfiguration configuration, IEmailSender emailSender, IOptions<ApplicationSettings> applicationSettings) : IEmailService
    {
        private readonly string _fromEmail = configuration.GetRequiredSetting("Email:FromEmail");
        private readonly string _fromDisplayName = configuration.GetRequiredSetting("Email:FromDisplayName");
        private readonly string _siteUrlRoot = applicationSettings.Value.SiteUrlRootForEmails;


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
        /// <param name="token">The token required to reset the password.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendPasswordResetAsync(ApplicationUser user, string token)
        {
            await this.SendMailAsync(user, "Reset your password",
                new ResetPasswordTemplate()
                {
                    FromDisplayName = this._fromDisplayName,
                    SiteUrlRoot = this._siteUrlRoot,
                    Token = token,
                    User = user
                }.TransformText());
        }

        /// <summary>
        /// Sends an email change email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="token">The token for verifying the email change.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendChangeEmailAsync(ApplicationUser user, string newEmail, string token)
        {
            await emailSender.SendMailAsync(
                new MailMessage(this._fromEmail, newEmail, "Confirm your email change",
                    new ChangeEmailTemplate()
                    {
                        FromDisplayName = this._fromDisplayName,
                        NewEmail = newEmail,
                        SiteUrlRoot = this._siteUrlRoot,
                        Token = token,
                        User = user
                    }.TransformText()));
        }

        /// <summary>
        /// Sends an email verification email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="token">The token for verifying the email.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendEmailVerificationAsync(ApplicationUser user, string token)
        {
            await this.SendMailAsync(user, "Confirm your email",
                new ConfirmEmailTemplate()
                {
                    FromDisplayName = this._fromDisplayName,
                    SiteUrlRoot = this._siteUrlRoot,
                    Token = token,
                    User = user
                }.TransformText());
        }

        /// <summary>
        /// Sends a registration email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendRegistrationWelcomeAsync(ApplicationUser user)
        {
            await this.SendMailAsync(user, "Welcome to our site",
                new RegistrationWelcomeTemplate()
                {
                    FromDisplayName = this._fromDisplayName,
                    SiteUrlRoot = this._siteUrlRoot,
                    User = user
                }.TransformText());
        }

        /// <summary>
        /// Sends a two-factor authentication email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="code">The two-factor authentication code.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendTwoFactorAsync(ApplicationUser user, string code)
        {
            await this.SendMailAsync(user, "Your login security code",
                new TwoFactorTemplate()
                {
                    Code = code,
                    FromDisplayName = this._fromDisplayName,
                    SiteUrlRoot = this._siteUrlRoot,
                    User = user
                }.TransformText());
        }

        /// <summary>
        /// Sends a magic link email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="token">The magic link token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendMagicLinkAsync(ApplicationUser user, string token)
        {
            await this.SendMailAsync(user, "Your login link",
                new MagicLinkTemplate()
                {
                    FromDisplayName = this._fromDisplayName,
                    SiteUrlRoot = this._siteUrlRoot,
                    Token = token,
                    User = user
                }.TransformText());
        }
    }
}
