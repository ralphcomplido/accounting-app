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
        /// Creates a new account.
        /// </summary>
        /// <param name="dto">The account data to create.</param>
        /// <returns>The newly created account.</returns>
        Task<AccountResponseDto> CreateAsync(CreateAccountDto dto);
    }
}
