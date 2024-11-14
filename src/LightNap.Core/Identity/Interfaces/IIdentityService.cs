using LightNap.Core.Api;
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
        /// <returns>A task that represents the asynchronous operation. The task result contains the login result data transfer object.</returns>
        Task<LoginResultDto> LogInAsync(LoginRequestDto requestDto);

        /// <summary>
        /// Registers a new user asynchronously.
        /// </summary>
        /// <param name="requestDto">The register request data transfer object.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the login result data transfer object.</returns>
        Task<LoginResultDto> RegisterAsync(RegisterRequestDto requestDto);

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
        /// <returns>A task that represents the asynchronous operation. The task result contains the new password as a string.</returns>
        Task<string> NewPasswordAsync(NewPasswordRequestDto requestDto);

        /// <summary>
        /// Verifies a 2FA code asynchronously.
        /// </summary>
        /// <param name="requestDto">The verify code request data transfer object.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the verification result as a string.</returns>
        Task<string> VerifyCodeAsync(VerifyCodeRequestDto requestDto);

        /// <summary>
        /// Gets the access token of the current user asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the access token as a string.</returns>
        Task<string> GetAccessTokenAsync();
    }
}