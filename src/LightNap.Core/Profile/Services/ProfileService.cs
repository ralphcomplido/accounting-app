using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using LightNap.Core.Extensions;
using LightNap.Core.Interfaces;
using LightNap.Core.Profile.Dto.Request;
using LightNap.Core.Profile.Dto.Response;
using LightNap.Core.Profile.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LightNap.Core.Profile.Services
{
    /// <summary>
    /// Service for managing user profiles.
    /// </summary>
    public class ProfileService(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IUserContext userContext) : IProfileService
    {
        /// <summary>
        /// Changes the password for the specified user.
        /// </summary>
        /// <param name="requestDto">The data transfer object containing the current and new passwords.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> indicating the success or failure of the operation.</returns>
        public async Task<ApiResponseDto<bool>> ChangePasswordAsync(ChangePasswordRequestDto requestDto)
        {
            if (requestDto.NewPassword != requestDto.ConfirmNewPassword) { throw new UserFriendlyApiException("New password does not match confirmation password."); }

            ApplicationUser user = await userManager.FindByIdAsync(userContext.GetUserId()) ?? throw new UserFriendlyApiException("Unable to change password.");

            var result = await userManager.ChangePasswordAsync(user, requestDto.CurrentPassword, requestDto.NewPassword);
            if (!result.Succeeded)
            {
                if (result.Errors.Any()) { throw new UserFriendlyApiException(result.Errors.Select(error => error.Description)); }
                throw new UserFriendlyApiException("Unable to change password.");
            }

            return new ApiResponseDto<bool>(true);
        }

        /// <summary>
        /// Retrieves the profile of the specified user.
        /// </summary>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the user's profile.</returns>
        public async Task<ApiResponseDto<ProfileDto>> GetProfileAsync()
        {
            var user = await db.Users.FindAsync(userContext.GetUserId());
            return new ApiResponseDto<ProfileDto>(user?.ToLoggedInUserDto());
        }

        /// <summary>
        /// Updates the profile of the specified user.
        /// </summary>
        /// <param name="requestDto">The data transfer object containing the updated profile information.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> indicating the success or failure of the operation.</returns>
        public async Task<ApiResponseDto<ProfileDto>> UpdateProfileAsync(UpdateProfileDto requestDto)
        {
            var user = await db.Users.FindAsync(userContext.GetUserId()) ?? throw new UserFriendlyApiException("Unable to update profile.");

            user.UpdateLoggedInUser(requestDto);

            await db.SaveChangesAsync();

            return new ApiResponseDto<ProfileDto>(user.ToLoggedInUserDto());
        }

        /// <summary>
        /// Retrieves the settings of the specified user.
        /// </summary>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the user's settings.</returns>
        public async Task<ApiResponseDto<BrowserSettingsDto>> GetSettingsAsync()
        {
            var user = await db.Users.FindAsync(userContext.GetUserId()) ?? throw new UserFriendlyApiException("Unable to load settings");
            return new ApiResponseDto<BrowserSettingsDto>(user.BrowserSettings);
        }

        /// <summary>
        /// Updates the settings of the specified user.
        /// </summary>
        /// <param name="requestDto">The data transfer object containing the updated settings information.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> indicating the success or failure of the operation.</returns>
        public async Task<ApiResponseDto<bool>> UpdateSettingsAsync(BrowserSettingsDto requestDto)
        {
            var user = await db.Users.FindAsync(userContext.GetUserId()) ?? throw new UserFriendlyApiException("Unable to update settings");
            user.BrowserSettings = requestDto;
            await db.SaveChangesAsync();
            return new ApiResponseDto<bool>(true);
        }

        /// <summary>
        /// Retrieves the list of devices for the specified user.
        /// </summary>
        /// <param name="requestingUserId">The ID of the user requesting the devices.</param>
        /// <returns>A list of devices associated with the user.</returns>
        public async Task<ApiResponseDto<IList<DeviceDto>>> GetDevicesAsync()
        {
            var tokens = await db.RefreshTokens
                            .Where(token => token.UserId == userContext.GetUserId() && !token.IsRevoked && token.Expires > DateTime.UtcNow)
                            .OrderByDescending(device => device.Expires)
                            .ToListAsync();

            return new ApiResponseDto<IList<DeviceDto>>(tokens.ToDtoList());
        }

        /// <summary>
        /// Revokes a device for the specified user.
        /// </summary>
        /// <param name="requestingUserId">The ID of the user requesting the revocation.</param>
        /// <param name="deviceId">The ID of the device to be revoked.</param>
        /// <returns>A boolean indicating whether the revocation was successful.</returns>
        public async Task<ApiResponseDto<bool>> RevokeDeviceAsync(string deviceId)
        {
            var token = await db.RefreshTokens.FindAsync(deviceId);

            if (token?.UserId == userContext.GetUserId())
            {
                token.IsRevoked = true;
                await db.SaveChangesAsync();
                return new ApiResponseDto<bool>(true);
            }

            throw new UserFriendlyApiException("Device not found.");
        }
    }
}
