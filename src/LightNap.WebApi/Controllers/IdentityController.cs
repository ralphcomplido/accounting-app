using LightNap.Core.Api;
using LightNap.Core.Identity.Dto.Request;
using LightNap.Core.Identity.Dto.Response;
using LightNap.Core.Identity.Interfaces;
using LightNap.WebApi.Api;
using Microsoft.AspNetCore.Mvc;

namespace LightNap.WebApi.Controllers
{
    /// <summary>
    /// Controller for handling identity-related actions such as login, registration, and password reset.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController(IIdentityService identityService) : ControllerBase
    {
        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="requestDto">The login request DTO.</param>
        /// <returns>The API response containing the login result.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponseDto<LoginResultDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<ApiResponseDto<LoginResultDto>> LogIn(LoginRequestDto requestDto)
        {
            return new ApiResponseDto<LoginResultDto>(await identityService.LogInAsync(requestDto));
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="requestDto">The registration request DTO.</param>
        /// <returns>The API response containing the login result.</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponseDto<LoginResultDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<ApiResponseDto<LoginResultDto>> Register(RegisterRequestDto requestDto)
        {
            return new ApiResponseDto<LoginResultDto>(await identityService.RegisterAsync(requestDto));
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>The API response indicating the success of the operation.</returns>
        [HttpGet("logout")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 200)]
        public async Task<ApiResponseDto<bool>> LogOut()
        {
            await identityService.LogOutAsync();
            return new ApiResponseDto<bool>(true);
        }

        /// <summary>
        /// Resets the password for a user.
        /// </summary>
        /// <param name="requestDto">The reset password request DTO.</param>
        /// <returns>The API response indicating the success of the operation.</returns>
        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 200)]
        [ProducesResponseType(400)]
        public async Task<ApiResponseDto<bool>> ResetPassword(ResetPasswordRequestDto requestDto)
        {
            await identityService.ResetPasswordAsync(requestDto);
            return new ApiResponseDto<bool>(true);
        }

        /// <summary>
        /// Sets a new password for a user.
        /// </summary>
        /// <param name="requestDto">The new password request DTO.</param>
        /// <returns>The API response containing the new access token.</returns>
        [HttpPost("new-password")]
        [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
        [ProducesResponseType(400)]
        public async Task<ApiResponseDto<string>> NewPassword(NewPasswordRequestDto requestDto)
        {
            return new ApiResponseDto<string>(await identityService.NewPasswordAsync(requestDto));
        }

        /// <summary>
        /// Verifies the two-factor authentication code.
        /// </summary>
        /// <param name="requestDto">The verify code request DTO.</param>
        /// <returns>The API response containing the new access token.</returns>
        [HttpPost("verify-code")]
        [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
        [ProducesResponseType(400)]
        public async Task<ApiResponseDto<string>> VerifyCode(VerifyCodeRequestDto requestDto)
        {
            return new ApiResponseDto<string>(await identityService.VerifyCodeAsync(requestDto));
        }

        /// <summary>
        /// Refreshes the access token using the refresh token.
        /// </summary>
        /// <returns>The API response containing the new access token.</returns>
        [HttpGet("access-token")]
        [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
        public async Task<ApiResponseDto<string>> RefreshToken()
        {
            return new ApiResponseDto<string>(await identityService.GetAccessTokenAsync());
        }
    }
}
