using LightNap.Core.Api;
using LightNap.Core.Profile.Dto.Request;
using LightNap.Core.Profile.Dto.Response;
using LightNap.Core.Profile.Interfaces;
using LightNap.WebApi.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LightNap.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProfileController(IProfileService profileService) : ControllerBase
    {
        /// <summary>
        /// Retrieves the profile of the current user.
        /// </summary>
        /// <returns>
        /// An <see cref="ApiResponseDto{T}"/> containing the profile of the current user.
        /// </returns>
        /// <response code="200">Returns the profile of the current user.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseDto<ProfileDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<ApiResponseDto<ProfileDto>> GetProfile()
        {
            return new ApiResponseDto<ProfileDto>(await profileService.GetProfileAsync());
        }

        /// <summary>
        /// Updates the profile of the current user.
        /// </summary>
        /// <param name="requestDto">The updated profile information.</param>
        /// <returns>
        /// An <see cref="ApiResponseDto{T}"/> containing the updated profile of the current user.
        /// </returns>
        /// <response code="200">Returns the updated profile of the current user.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponseDto<ProfileDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        public async Task<ApiResponseDto<ProfileDto>> UpdateProfile(UpdateProfileDto requestDto)
        {
            return new ApiResponseDto<ProfileDto>(await profileService.UpdateProfileAsync(requestDto));
        }

        /// <summary>
        /// Changes the password of the current user.
        /// </summary>
        /// <param name="requestDto">The password change request.</param>
        /// <returns>
        /// An <see cref="ApiResponseDto{T}"/> indicating whether the password was changed successfully.
        /// </returns>
        /// <response code="200">If the password was changed successfully.</response>
        /// <response code="400">If the request is invalid or the current password is incorrect.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpPost("change-password")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ApiResponseDto<bool>> ChangePassword(ChangePasswordRequestDto requestDto)
        {
            await profileService.ChangePasswordAsync(requestDto);
            return new ApiResponseDto<bool>(true);
        }

        /// <summary>
        /// Retrieves the settings of the current user.
        /// </summary>
        /// <returns>
        /// An <see cref="ApiResponseDto{T}"/> containing the settings of the current user.
        /// </returns>
        /// <response code="200">Returns the settings of the current user.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet("settings")]
        [ProducesResponseType(typeof(ApiResponseDto<BrowserSettingsDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<ApiResponseDto<BrowserSettingsDto>> GetSettings()
        {
            return new ApiResponseDto<BrowserSettingsDto>(await profileService.GetSettingsAsync());
        }

        /// <summary>
        /// Updates the settings of the current user.
        /// </summary>
        /// <param name="requestDto">The updated settings information.</param>
        /// <returns>
        /// A <see cref="ApiResponseDto{T}"/> containing true if the update succeeded.
        /// </returns>
        /// <response code="200">Returns the updated settings of the current user.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpPut("settings")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        public async Task<ApiResponseDto<bool>> UpdateSettings(BrowserSettingsDto requestDto)
        {
            await profileService.UpdateSettingsAsync(requestDto);
            return new ApiResponseDto<bool>(true);
        }

        /// <summary>
        /// Retrieves the list of devices.
        /// </summary>
        /// <returns>The list of devices.</returns>
        /// <response code="200">Returns the list of devices.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpGet("devices")]
        [ProducesResponseType(typeof(ApiResponseDto<IList<DeviceDto>>), 200)]
        [ProducesResponseType(401)]
        public async Task<ApiResponseDto<IList<DeviceDto>>> GetDevices()
        {
            return new ApiResponseDto<IList<DeviceDto>>(await profileService.GetDevicesAsync());
        }

        /// <summary>
        /// Revokes a device.
        /// </summary>
        /// <param name="deviceId">The ID of the device to revoke.</param>
        /// <returns>A response indicating whether the device was successfully revoked.</returns>
        /// <response code="200">Device successfully revoked.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Device not found.</response>
        [HttpDelete("devices/{deviceId}")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ApiResponseDto<bool>> RevokeDevice(string deviceId)
        {
            await profileService.RevokeDeviceAsync(deviceId);
            return new ApiResponseDto<bool>(true);
        }
    }
}