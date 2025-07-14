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
            Description = a.Description
        }).ToList();
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
            Description = dto.Description
        };

        _dbContext.Accounts.Add(entity);
        await _dbContext.SaveChangesAsync();

        return new AccountResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Type = entity.Type,
            Description = entity.Description
        };
    }
}
