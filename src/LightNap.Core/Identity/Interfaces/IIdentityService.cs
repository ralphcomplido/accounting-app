using LightNap.Core.Identity.Dto.Request;
using LightNap.Core.Identity.Dto.Response;

namespace LightNap.Core.Identity.Interfaces
{
    /// <summary>  
    /// Provides methods to manage identity.  
    /// </summary>  
    public interface IIdentityService
    {
        /// <summary>
        /// Logs in a user asynchronously.
        /// </summary>
        /// <param name="requestDto">The login request data transfer object.</param>
        /// <returns>The result of the login operation which may be a token or indicate additional required steps.</returns>
        Task<LoginSuccessDto> LogInAsync(LoginRequestDto requestDto);

        /// <summary>
        /// Registers a new user asynchronously.
        /// </summary>
        /// <param name="requestDto">The register request data transfer object.</param>
        /// <returns>The result of the login operation which may be a token or indicate additional required steps.</returns>
        Task<LoginSuccessDto> RegisterAsync(RegisterRequestDto requestDto);

        /// <summary>
        /// Logs out the current user asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task LogOutAsync();

        /// <summary>
        /// Resets the password of a user asynchronously.
        /// </summary>
        /// <param name="requestDto">The reset password request data transfer object.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ResetPasswordAsync(ResetPasswordRequestDto requestDto);

        /// <summary>
        /// Sets a new password for a user asynchronously.
        /// </summary>
        /// <param name="requestDto">The new password request data transfer object.</param>
        /// <returns>The result of the login operation which may be a token or indicate additional required steps.</returns>
        Task<LoginSuccessDto> NewPasswordAsync(NewPasswordRequestDto requestDto);

        /// <summary>
        /// Verifies a 2FA code asynchronously.
        /// </summary>
        /// <param name="requestDto">The verify code request data transfer object.</param>
        /// <returns>The access token.</returns>
        Task<string> VerifyCodeAsync(VerifyCodeRequestDto requestDto);

        /// <summary>
        /// Gets the access token of the current user asynchronously.
        /// </summary>
        /// <returns>A new access token.</returns>
        Task<string> GetAccessTokenAsync();

        /// <summary>
        /// Requests email verification for a user asynchronously.
        /// </summary>
        /// <param name="requestDto">Contains the email address of the user.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RequestVerificationEmailAsync(SendVerificationEmailRequestDto requestDto);

        /// <summary>
        /// Verifies an email asynchronously.
        /// </summary>
        /// <param name="requestDto">The verify email request data transfer object.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task VerifyEmailAsync(VerifyEmailRequestDto requestDto);
    }
}