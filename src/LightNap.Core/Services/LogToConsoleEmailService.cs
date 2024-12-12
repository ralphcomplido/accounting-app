using LightNap.Core.Data.Entities;
using LightNap.Core.Interfaces;
using System.Diagnostics;
using System.Net.Mail;

namespace LightNap.Core.Services
{
    /// <summary>
    /// Service for logging email details to the console instead of sending them.
    /// </summary>
    public class LogToConsoleEmailService : IEmailService
    {
        /// <summary>
        /// Logs the email details to the console asynchronously.
        /// </summary>
        /// <param name="message">The email message to log.</param>
        /// <returns>A completed task.</returns>
        public Task SendEmailAsync(MailMessage message)
        {
            Trace.TraceInformation($"Not sending email to '{message.To}' with subject '{message.Subject}' and body '{message.Body}'");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Logs the password reset email details to the console asynchronously.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="passwordResetUrl">The URL for resetting the password.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendPasswordResetAsync(ApplicationUser user, string passwordResetUrl)
        {
            await this.SendEmailAsync(new MailMessage("noreply@sharplogic.com", user.Email!, "Reset your password", $"You may reset your password at: {passwordResetUrl}"));
        }

        /// <summary>
        /// Sends an email change email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="emailChangeUrl">The URL for verifying the email change.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendChangeEmailAsync(ApplicationUser user, string newEmail, string emailChangeUrl)
        {
            await this.SendEmailAsync(new MailMessage("noreply@sharplogic.com", newEmail, "Confirm your email change", $"You may confirm your email change at: {emailChangeUrl}"));
        }

        /// <summary>
        /// Logs the email verification email details to the console asynchronously.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="emailVerificationUrl">The URL for verifying the email.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendEmailVerificationAsync(ApplicationUser user, string emailVerificationUrl)
        {
            await this.SendEmailAsync(new MailMessage("noreply@sharplogic.com", user.Email!, "Confirm your email", $"You may confirm your email at: {emailVerificationUrl}"));
        }

        /// <summary>
        /// Logs the registration email details to the console asynchronously.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendRegistrationWelcomeAsync(ApplicationUser user)
        {
            await this.SendEmailAsync(new MailMessage("noreply@sharplogic.com", user.Email!, "Welcome to our site", $"Thank you for registering."));
        }

        /// <summary>
        /// Logs the two-factor authentication email details to the console asynchronously.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="code">The two-factor authentication code.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendTwoFactorAsync(ApplicationUser user, string code)
        {
            await this.SendEmailAsync(new MailMessage("noreply@sharplogic.com", user.Email!, "Your login security code", $"Your login code is: {code}"));
        }

        /// <summary>
        /// Sends a magic link email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="magicLinkUrl">The magic link URL.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendMagicLinkAsync(ApplicationUser user, string magicLinkUrl)
        {
            await this.SendEmailAsync(new MailMessage("noreply@sharplogic.com", user.Email!, "Your login link", $"You may log in at: {magicLinkUrl}"));
        }

    }
}
