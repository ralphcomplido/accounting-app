using LightNap.Core.Administrator.Dto.Request;
using LightNap.Core.Administrator.Dto.Response;
using LightNap.Core.Administrator.Interfaces;
using LightNap.Core.Administrator.Models;
using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using LightNap.Core.Extensions;
using LightNap.Core.Identity.Dto.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LightNap.Core.Administrator.Services
{
    /// <summary>
    /// Service for managing administrator-related operations.
    /// </summary>
    public class AdministratorService(UserManager<ApplicationUser> userManager, ApplicationDbContext db) : IAdministratorService
    {
        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>The user details.</returns>
        public async Task<ApiResponseDto<AdminUserDto?>> GetUserAsync(string userId)
        {
            var user = await db.Users.FindAsync(userId);
            // NOTE: The requested user not being found is a successful request if nothing fails. As a result, we return null as
            // confirmation that the request succeeded but there is no user matching that ID. It makes client development easier.
            return new ApiResponseDto<AdminUserDto?>(user?.ToAdminUserDto());
        }

        /// <summary>
        /// Searches for users based on the specified criteria.
        /// </summary>
        /// <param name="requestDto">The search criteria.</param>
        /// <returns>The list of users matching the criteria.</returns>
        public async Task<ApiResponseDto<PagedResponse<AdminUserDto>>> SearchUsersAsync(SearchAdminUsersRequestDto requestDto)
        {
            IQueryable<ApplicationUser> query = db.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(requestDto.Email))
            {
                query = query.Where(user => EF.Functions.Like(user.Email!.ToLower(), $"%{requestDto.Email.ToLower()}%"));
            }

            if (!string.IsNullOrWhiteSpace(requestDto.UserName))
            {
                query = query.Where(user => EF.Functions.Like(user.UserName!.ToLower(), $"%{requestDto.UserName.ToLower()}%"));
            }

            query = requestDto.SortBy switch
            {
                ApplicationUserSortBy.Email => requestDto.ReverseSort ? query.OrderByDescending(user => user.Email) : query.OrderBy(user => user.Email),
                ApplicationUserSortBy.UserName => requestDto.ReverseSort ? query.OrderByDescending(user => user.UserName) : query.OrderBy(user => user.UserName),
                ApplicationUserSortBy.CreatedDate => requestDto.ReverseSort ? query.OrderByDescending(user => user.CreatedDate) : query.OrderBy(user => user.CreatedDate),
                ApplicationUserSortBy.LastModifiedDate => requestDto.ReverseSort ? query.OrderByDescending(user => user.LastModifiedDate) : query.OrderBy(user => user.LastModifiedDate),
                _ => throw new ArgumentException("Invalid sort field: '{sortBy}'", requestDto.SortBy.ToString()),
            };
            int totalCount = await query.CountAsync();

            if (requestDto.PageNumber > 1)
            {
                query = query.Skip((requestDto.PageNumber - 1) * requestDto.PageSize);
            }

            var users = await query.Take(requestDto.PageSize).Select(user => user.ToAdminUserDto()).ToListAsync();

            return new ApiResponseDto<PagedResponse<AdminUserDto>>(
                new PagedResponse<AdminUserDto>(users, requestDto.PageNumber, requestDto.PageSize, totalCount));
        }

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="requestDto">The updated user information.</param>
        /// <returns>The updated user details.</returns>
        public async Task<ApiResponseDto<AdminUserDto>> UpdateUserAsync(string userId, UpdateAdminUserDto requestDto)
        {
            var user = await db.Users.FindAsync(userId);
            if (user is null) { throw new UserFriendlyApiException("The specified user was not found."); }

            user.UpdateAdminUserDto(requestDto);

            await db.SaveChangesAsync();

            return new ApiResponseDto<AdminUserDto>(user.ToAdminUserDto());
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>True if the user was successfully deleted.</returns>
        public async Task<ApiResponseDto<bool>> DeleteUserAsync(string userId)
        {
            var user = await db.Users.FindAsync(userId);
            if (user is null) { throw new UserFriendlyApiException("The specified user was not found."); }

            if (await userManager.IsInRoleAsync(user, ApplicationRoles.Administrator.Name!)) { throw new UserFriendlyApiException("You may not delete an Administrator."); }

            db.Users.Remove(user);

            await db.SaveChangesAsync();

            return new ApiResponseDto<bool>(true);
        }

        /// <summary>
        /// Retrieves all available roles.
        /// </summary>
        /// <returns>The list of roles.</returns>
        public ApiResponseDto<IList<RoleDto>> GetRoles()
        {
            return new ApiResponseDto<IList<RoleDto>>(ApplicationRoles.All.ToDtoList());
        }

        /// <summary>
        /// Retrieves the roles for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The list of roles for the user.</returns>
        public async Task<ApiResponseDto<IList<string>>> GetRolesForUserAsync(string userId)
        {
            var user = await db.Users.FindAsync(userId);
            if (user is null) { throw new UserFriendlyApiException("The specified user was not found."); }

            var roles = await userManager.GetRolesAsync(user);

            return new ApiResponseDto<IList<string>>(roles);
        }

        /// <summary>
        /// Retrieves the users in a specific role.
        /// </summary>
        /// <param name="role">The role to search for.</param>
        /// <returns>The list of users in the specified role.</returns>
        public async Task<ApiResponseDto<IList<AdminUserDto>>> GetUsersInRoleAsync(string role)
        {
            var users = await userManager.GetUsersInRoleAsync(role);
            return new ApiResponseDto<IList<AdminUserDto>>(users.ToAdminUserDtoList());
        }

        /// <summary>
        /// Adds a user to a role.
        /// </summary>
        /// <param name="role">The role to add the user to.</param>
        /// <param name="userId">The ID of the user to add to the role.</param>
        /// <returns>True if the user was successfully added to the role.</returns>
        public async Task<ApiResponseDto<bool>> AddUserToRoleAsync(string role, string userId)
        {
            var user = await db.Users.FindAsync(userId);
            if (user is null) { throw new UserFriendlyApiException("The specified user was not found."); }

            var result = await userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                throw new UserFriendlyApiException(result.Errors.Select(error => error.Description));
            }

            return new ApiResponseDto<bool>(true);
        }

        /// <summary>
        /// Removes a user from a role.
        /// </summary>
        /// <param name="role">The role to remove the user from.</param>
        /// <param name="userId">The ID of the user to remove from the role.</param>
        /// <returns>True if the user was successfully removed from the role.</returns>
        public async Task<ApiResponseDto<bool>> RemoveUserFromRoleAsync(string role, string userId)
        {
            var user = await db.Users.FindAsync(userId);
            if (user is null) { throw new UserFriendlyApiException("The specified user was not found."); }

            var result = await userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded)
            {
                throw new UserFriendlyApiException(result.Errors.Select(error => error.Description));
            }

            return new ApiResponseDto<bool>(true);
        }

        /// <summary>
        /// Locks a user account.
        /// </summary>
        /// <param name="userId">The ID of the user to lock.</param>
        /// <returns>True if the user was successfully locked.</returns>
        public async Task<ApiResponseDto<bool>> LockUserAccountAsync(string userId)
        {
            var user = await db.Users.FindAsync(userId);
            if (user is null) { throw new UserFriendlyApiException("The specified user was not found."); }

            if (await userManager.IsInRoleAsync(user, ApplicationRoles.Administrator.Name!)) { throw new UserFriendlyApiException("You may not lock an Administrator account."); }

            user.LockoutEnd = DateTimeOffset.MaxValue;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new UserFriendlyApiException(result.Errors.Select(error => error.Description));
            }

            return new ApiResponseDto<bool>(true);
        }

        /// <summary>
        /// Unlocks a user account.
        /// </summary>
        /// <param name="userId">The ID of the user to unlock.</param>
        /// <returns>True if the user was successfully unlocked.</returns>
        public async Task<ApiResponseDto<bool>> UnlockUserAccountAsync(string userId)
        {
            var user = await db.Users.FindAsync(userId);
            if (user is null) { throw new UserFriendlyApiException("The specified user was not found."); }

            user.LockoutEnd = null;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new UserFriendlyApiException(result.Errors.Select(error => error.Description));
            }

            return new ApiResponseDto<bool>(true);
        }
    }
}
