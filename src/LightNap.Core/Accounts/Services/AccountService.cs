using LightNap.Core.Accounts.Dto.Request;
using LightNap.Core.Accounts.Dto.Response;
using LightNap.Core.Accounts.Interfaces;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using LightNap.Core.Extensions;
using LightNap.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LightNap.Core.Accounts.Services;

/// <summary>
/// Service for managing account-related operations.
/// </summary>
public class AccountService : IAccountService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IUserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountService"/> class.
    /// </summary>
    /// <param name="dbContext">The application's database context.</param>
    /// <param name="userContext">The user context providing information about the current user.</param>
    public AccountService(ApplicationDbContext dbContext, IUserContext userContext)
    {
        _dbContext = dbContext;
        _userContext = userContext;
    }

    /// <summary>
    /// Retrieves all accounts asynchronously.
    /// </summary>
    /// <returns>A list of account data transfer objects.</returns>
    public async Task<IList<AccountResponseDto>> GetAllAsync()
    {
        var accounts = await _dbContext.Accounts.ToListAsync();

        return accounts.Select(a => new AccountResponseDto
        {
            Id = a.Id,
            Name = a.Name,
            Type = a.Type,
            Description = a.Description,
            Balance = a.Balance
        }).ToList();
    }

    /// <summary>
    /// Retrieves a single account by its ID asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the account.</param>
    /// <returns>The account data transfer object, or null if not found.</returns>
    public async Task<AccountResponseDto?> GetAsync(int id)
    {
        var account = await _dbContext.Accounts.FindAsync(id);
        if (account == null)
            return null;

        return new AccountResponseDto
        {
            Id = account.Id,
            Name = account.Name,
            Type = account.Type,
            Description = account.Description,
            Balance = account.Balance
        };
    }

    /// <summary>
    /// Creates a new account asynchronously.
    /// </summary>
    /// <param name="dto">The account data to create.</param>
    /// <returns>The newly created account.</returns>
    public async Task<AccountResponseDto> CreateAsync(CreateAccountDto dto)
    {
        var entity = new Account
        {
            Name = dto.Name,
            Type = dto.Type,
            Description = dto.Description,
            Balance = dto.Balance
        };

        _dbContext.Accounts.Add(entity);
        await _dbContext.SaveChangesAsync();

        return new AccountResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Type = entity.Type,
            Description = entity.Description,
            Balance = entity.Balance
        };
    }

    /// <summary>
    /// Updates an existing account asynchronously.
    /// </summary>
    /// <param name="dto">The account data to update.</param>
    /// <returns>The updated account, or null if not found.</returns>
    public async Task<AccountResponseDto?> UpdateAsync(UpdateAccountDto dto)
    {
        var account = await _dbContext.Accounts.FindAsync(dto.Id);
        if (account == null)
            return null;

        if (dto.Name is not null)
            account.Name = dto.Name;
        if (dto.Type is not null)
            account.Type = dto.Type;
        if (dto.Description is not null)
            account.Description = dto.Description;
        if (dto.Balance.HasValue)
            account.Balance = dto.Balance.Value;

        await _dbContext.SaveChangesAsync();

        return new AccountResponseDto
        {
            Id = account.Id,
            Name = account.Name,
            Type = account.Type,
            Description = account.Description,
            Balance = account.Balance
        };
    }

    /// <summary>
    /// Deletes an account asynchronously.
    /// </summary>
    /// <param name="dto">The account data to delete.</param>
    /// <returns>True if the account was deleted; otherwise, false.</returns>
    public async Task<bool> DeleteAsync(DeleteAccountDto dto)
    {
        var account = await _dbContext.Accounts.FindAsync(dto.Id);
        if (account == null)
            return false;

        _dbContext.Accounts.Remove(account);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
