using LightNap.Core.Api;
using LightNap.Core.Configuration;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using LightNap.Core.Identity.Dto.Request;
using LightNap.Core.Identity.Dto.Response;
using LightNap.Core.Identity.Interfaces;
using LightNap.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Web;

namespace LightNap.Core.Identity.Services
{
    /// <summary>
    /// Service for managing identity.
    /// </summary>
    public class IdentityService(ILogger<IdentityService> logger,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        IEmailService emailService,
        IOptions<ApplicationSettings> applicationSettings,
        ApplicationDbContext db,
        ICookieManager cookieManager,
        IUserContext userContext) : IIdentityService
    {
        /// <summary>
        /// Handles user login asynchronously.
        /// </summary>
        /// <param name="user">The application user.</param>
        /// <param name="rememberMe">Indicates whether to remember the user.</param>
        /// <param name="deviceDetails">The device details.</param>
        /// <returns>The API response containing the login result.</returns>
        private async Task<ApiResponseDto<LoginResultDto>> HandleUserLoginAsync(ApplicationUser user, bool rememberMe, string deviceDetails)
        {
            if (user.TwoFactorEnabled)
            {
                string code = await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
                await emailService.SendTwoFactorEmailAsync(user, code);
                return new ApiResponseDto<LoginResultDto>(new LoginResultDto() { TwoFactorRequired = true });
            }

            await this.CreateRefreshTokenAsync(user, rememberMe, deviceDetails);
            return new ApiResponseDto<LoginResultDto>(
                new LoginResultDto()
                {
                    BearerToken = await tokenService.GenerateAccessTokenAsync(user)
                });
        }

        /// <summary>
        /// Validates the refresh token asynchronously.
        /// </summary>
        /// <returns>The application user if the refresh token is valid; otherwise, null.</returns>
        private async Task<ApplicationUser?> ValidateRefreshTokenAsync()
        {
            string? refreshTokenCookie = cookieManager.GetCookie(Constants.Cookies.RefreshToken);
            if (refreshTokenCookie is null) { return null; }

            var refreshToken = await db.RefreshTokens.Include(token => token.User).FirstOrDefaultAsync(token => token.Token == refreshTokenCookie);
            if (refreshToken is null || refreshToken.IsRevoked || refreshToken.Expires < DateTime.UtcNow) { return null; }

            refreshToken.LastSeen = DateTime.UtcNow;
            refreshToken.IpAddress = userContext.GetIpAddress() ?? Constants.RefreshTokens.NoIpProvided;
            refreshToken.Expires = DateTime.UtcNow.AddDays(applicationSettings.Value.LogOutInactiveDeviceDays);
            refreshToken.Token = tokenService.GenerateRefreshToken();

            await db.SaveChangesAsync();

            // If neither of these was set last time then the user doesn't want us to remember them across sessions.
            bool rememberMe = refreshTokenCookie.Contains(Constants.Cookies.Expires) || refreshTokenCookie.Contains(Constants.Cookies.MaxAge);

            cookieManager.SetCookie(Constants.Cookies.RefreshToken, refreshToken.Token, rememberMe, refreshToken.Expires);

            return refreshToken.User;
        }

        /// <summary>
        /// Creates a refresh token asynchronously.
        /// </summary>
        /// <param name="user">The application user.</param>
        /// <param name="rememberMe">Indicates whether to remember the user.</param>
        /// <param name="deviceDetails">The device details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task CreateRefreshTokenAsync(ApplicationUser user, bool rememberMe, string deviceDetails)
        {
            DateTime expires = DateTime.UtcNow.AddDays(applicationSettings.Value.LogOutInactiveDeviceDays);
            string refreshToken = tokenService.GenerateRefreshToken();

            db.RefreshTokens.Add(
                new RefreshToken()
                {
                    Id = Guid.NewGuid().ToString(),
                    Token = refreshToken,
                    Expires = expires,
                    LastSeen = DateTime.UtcNow,
                    IpAddress = userContext.GetIpAddress() ?? Constants.RefreshTokens.NoIpProvided,
                    Details = deviceDetails,
                    UserId = user.Id
                });
            await db.SaveChangesAsync();

            cookieManager.SetCookie(Constants.Cookies.RefreshToken, refreshToken, rememberMe, expires);
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="requestDto">The login request DTO.</param>
        /// <returns>The API response containing the login result.</returns>
        public async Task<ApiResponseDto<LoginResultDto>> LogInAsync(LoginRequestDto requestDto)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(requestDto.Email) ?? throw new UserFriendlyApiException("Invalid email/password combination.");

            var signInResult = await signInManager.CheckPasswordSignInAsync(user, requestDto.Password, false);
            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                {
                    throw new UserFriendlyApiException("This account is locked.");
                }
                if (signInResult.IsNotAllowed)
                {
                    throw new UserFriendlyApiException("This account is not allowed to log in.");
                }
                throw new UserFriendlyApiException("Invalid email/password combination.");
            }

            return await this.HandleUserLoginAsync(user, requestDto.RememberMe, requestDto.DeviceDetails);
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="requestDto">The registration request DTO.</param>
        /// <returns>The API response containing the login result.</returns>
        public async Task<ApiResponseDto<LoginResultDto>> RegisterAsync(RegisterRequestDto requestDto)
        {
            var existingUser = await userManager.FindByEmailAsync(requestDto.Email);
            if (existingUser is not null) { throw new UserFriendlyApiException("This email is already in use."); }

            ApplicationUser user = new(requestDto.UserName, requestDto.Email, applicationSettings.Value.RequireTwoFactorForNewUsers);

            var result = await userManager.CreateAsync(user, requestDto.Password);
            if (!result.Succeeded)
            {
                if (result.Errors.Any()) { throw new UserFriendlyApiException(result.Errors.Select(error => error.Description)); }
                throw new UserFriendlyApiException("Unable to create user.");
            }

            if (!user.TwoFactorEnabled)
            {
                await emailService.SendRegistrationEmailAsync(user);
            }

            logger.LogInformation("New user '{userName}' ('{email}') registered!", user.Email, user.UserName);

            return await this.HandleUserLoginAsync(user, requestDto.RememberMe, requestDto.DeviceDetails);
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>The API response indicating the success of the operation.</returns>
        public async Task<ApiResponseDto<bool>> LogOutAsync()
        {
            string? refreshTokenCookie = cookieManager.GetCookie(Constants.Cookies.RefreshToken);
            if (refreshTokenCookie is not null)
            {
                RefreshToken? refreshToken = await db.RefreshTokens.FirstOrDefaultAsync(token => token.Token == refreshTokenCookie);
                if (refreshToken is not null)
                {
                    db.RefreshTokens.Remove(refreshToken);
                    await db.SaveChangesAsync();
                }
                cookieManager.RemoveCookie(Constants.Cookies.RefreshToken);
            }
            return new ApiResponseDto<bool>(true);
        }

        /// <summary>
        /// Resets the password for a user.
        /// </summary>
        /// <param name="requestDto">The reset password request DTO.</param>
        /// <returns>The API response indicating the success of the operation.</returns>
        public async Task<ApiResponseDto<bool>> ResetPasswordAsync(ResetPasswordRequestDto requestDto)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(requestDto.Email) ?? throw new UserFriendlyApiException("An account with this email was not found.");


            string token = await userManager.GeneratePasswordResetTokenAsync(user);
            string url = $"{applicationSettings.Value.SiteUrlRootForEmails}#/identity/new-password/{HttpUtility.UrlEncode(user.Email)}/{HttpUtility.UrlEncode(token)}";

            try
            {
                await emailService.SendPasswordResetEmailAsync(user, url);
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while sending a password reset link to '{email}'", user.Email);
                throw new UserFriendlyApiException("An unexpected error occurred while sending the password reset link.");
            }

            return new ApiResponseDto<bool>(true);
        }

        /// <summary>
        /// Sets a new password for a user.
        /// </summary>
        /// <param name="requestDto">The new password request DTO.</param>
        /// <returns>The API response containing the new access token.</returns>
        public async Task<ApiResponseDto<string>> NewPasswordAsync(NewPasswordRequestDto requestDto)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(requestDto.Email) ?? throw new UserFriendlyApiException("An account with this email was not found.");

            IdentityResult result = await userManager.ResetPasswordAsync(user, requestDto.Token, requestDto.Password);
            if (!result.Succeeded)
            {
                if (result.Errors.Any()) { throw new UserFriendlyApiException(result.Errors.Select(error => error.Description)); }
                throw new UserFriendlyApiException("Unable to set new password.");
            }

            await this.CreateRefreshTokenAsync(user, requestDto.RememberMe, requestDto.DeviceDetails);

            return new ApiResponseDto<string>(await tokenService.GenerateAccessTokenAsync(user));
        }

        /// <summary>
        /// Verifies the two-factor authentication code.
        /// </summary>
        /// <param name="requestDto">The verify code request DTO.</param>
        /// <returns>The API response containing the new access token.</returns>
        public async Task<ApiResponseDto<string>> VerifyCodeAsync(VerifyCodeRequestDto requestDto)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(requestDto.Email) ?? throw new UserFriendlyApiException("An account with this email was not found.");
            if (!await userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, requestDto.Code))
            {
                throw new UserFriendlyApiException("Unable to verify code. Please try again or log in again to resend a new code.");
            }

            if (!user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);
                await emailService.SendRegistrationEmailAsync(user);
            }

            await this.CreateRefreshTokenAsync(user, requestDto.RememberMe, requestDto.DeviceDetails);

            return new ApiResponseDto<string>(await tokenService.GenerateAccessTokenAsync(user));
        }

        /// <summary>
        /// Gets a new access token using the refresh token.
        /// </summary>
        /// <returns>The API response containing the new access token.</returns>
        public async Task<ApiResponseDto<string>> GetAccessTokenAsync()
        {
            var user = await this.ValidateRefreshTokenAsync() ?? throw new UserFriendlyApiException("This account needs to sign in.");
            if (!await signInManager.CanSignInAsync(user)) { throw new UserFriendlyApiException("This account may not sign in."); }

            return new ApiResponseDto<string>(await tokenService.GenerateAccessTokenAsync(user));
        }
    }
}
