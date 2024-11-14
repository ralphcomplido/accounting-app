using LightNap.Core.Administrator.Dto.Request;
using LightNap.Core.Administrator.Dto.Response;
using LightNap.Core.Api;
using LightNap.Core.Identity.Dto.Response;

namespace LightNap.Core.Administrator.Interfaces
{
    /// <summary>  
    /// Interface for administrator services.  
    /// </summary>  
    public interface IAdministratorService
    {
        /// <summary>  
        /// Gets a user asynchronously by user ID.  
        /// </summary>  
        /// <param name="userId">The user ID.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains the user data.</returns>  
        Task<AdminUserDto?> GetUserAsync(string userId);

        /// <summary>  
        /// Searches users asynchronously based on the specified request DTO.  
        /// </summary>  
        /// <param name="requestDto">The request DTO containing search parameters.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains the paged user data.</returns>  
        Task<PagedResponse<AdminUserDto>> SearchUsersAsync(SearchAdminUsersRequestDto requestDto);

        /// <summary>  
        /// Updates a user asynchronously by user ID.  
        /// </summary>  
        /// <param name="userId">The user ID.</param>  
        /// <param name="requestDto">The request DTO containing update information.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated user data.</returns>  
        Task<AdminUserDto> UpdateUserAsync(string userId, UpdateAdminUserDto requestDto);

        /// <summary>  
        /// Deletes a user asynchronously by user ID.  
        /// </summary>  
        /// <param name="userId">The user ID.</param>  
        /// <returns>A task that represents the asynchronous operation.</returns>  
        Task DeleteUserAsync(string userId);

        /// <summary>  
        /// Gets all roles.  
        /// </summary>  
        /// <returns>The list of roles.</returns>  
        IList<RoleDto> GetRoles();

        /// <summary>  
        /// Gets roles for a user asynchronously by user ID.  
        /// </summary>  
        /// <param name="userId">The user ID.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of roles for the user.</returns>  
        Task<IList<string>> GetRolesForUserAsync(string userId);

        /// <summary>  
        /// Gets users in a role asynchronously by role name.  
        /// </summary>  
        /// <param name="role">The role name.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of users in the role.</returns>  
        Task<IList<AdminUserDto>> GetUsersInRoleAsync(string role);

        /// <summary>  
        /// Adds a user to a role asynchronously.  
        /// </summary>  
        /// <param name="role">The role name.</param>  
        /// <param name="userId">The user ID.</param>  
        /// <returns>A task that represents the asynchronous operation.</returns>  
        Task AddUserToRoleAsync(string role, string userId);

        /// <summary>  
        /// Removes a user from a role asynchronously.  
        /// </summary>  
        /// <param name="role">The role name.</param>  
        /// <param name="userId">The user ID.</param>  
        /// <returns>A task that represents the asynchronous operation.</returns>  
        Task RemoveUserFromRoleAsync(string role, string userId);

        /// <summary>  
        /// Locks a user account asynchronously by user ID.  
        /// </summary>  
        /// <param name="userId">The user ID.</param>  
        /// <returns>A task that represents the asynchronous operation.</returns>  
        Task LockUserAccountAsync(string userId);

        /// <summary>  
        /// Unlocks a user account asynchronously by user ID.  
        /// </summary>  
        /// <param name="userId">The user ID.</param>  
        /// <returns>A task that represents the asynchronous operation.</returns>  
        Task UnlockUserAccountAsync(string userId);
    }
}