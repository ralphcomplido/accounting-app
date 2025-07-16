using LightNap.Core.Accounts.Dto.Request;
using LightNap.Core.Accounts.Dto.Response;

namespace LightNap.Core.Accounts.Interfaces
{
    /// <summary>
    /// Interface for managing account-related operations.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Retrieves all accounts.
        /// </summary>
        /// <returns>A list of account data transfer objects.</returns>
        Task<IList<AccountResponseDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a single account by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the account.</param>
        /// <returns>The account data transfer object, or null if not found.</returns>
        Task<AccountResponseDto?> GetAsync(int id);

                        
        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="dto">The account data to create.</param>
        /// <returns>The newly created account.</returns>
        Task<AccountResponseDto> CreateAsync(CreateAccountDto dto);

        /// <summary>
        /// Updates an existing account.
        /// </summary>
        /// <param name="dto">The account data to update.</param>
        /// <returns>The updated account.</returns>
        Task<AccountResponseDto> UpdateAsync(UpdateAccountDto dto);

        /// <summary>
        /// Deletes an account.
        /// </summary>
        /// <param name="dto">The account data to delete.</param>
        /// <returns>True if the account was deleted; otherwise, false.</returns>
        Task<bool> DeleteAsync(DeleteAccountDto dto);
    }
}
