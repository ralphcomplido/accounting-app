using System.Collections.Generic;
using System.Threading.Tasks;
using LightNap.Core.Accounts.Dto.Request;
using LightNap.Core.Accounts.Dto.Response;
using LightNap.Core.Accounts.Services;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using LightNap.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LightNap.Core.Tests.Accounts.Services
{
    public class AccountServiceTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AccountServiceTestDb")
                .Options;
            return new ApplicationDbContext(options);
        }

        private IUserContext GetUserContext()
        {
            var mock = new Mock<IUserContext>();
            mock.Setup(x => x.GetUserId()).Returns("test-user");
            mock.Setup(x => x.IsAdministrator).Returns(true);
            return mock.Object;
        }

        [Fact]
        public async Task CreateAsync_ShouldAddAccount()
        {
            // Arrange
            var dbContext = GetDbContext();
            var service = new AccountService(dbContext, GetUserContext());
            var dto = new CreateAccountDto
            {
                Name = "Test Account",
                Type = "Asset",
                Description = "Test Desc",
                Balance = 100
            };

            // Act
            var result = await service.CreateAsync(dto);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal("Test Account", result.Name);
            Xunit.Assert.Equal("Asset", result.Type);
            Xunit.Assert.Equal("Test Desc", result.Description);
            Xunit.Assert.Equal(100, result.Balance);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllAccounts()
        {
            // Arrange
            var dbContext = GetDbContext();
            dbContext.Accounts.Add(new Account { Name = "A", Type = "Asset", Balance = 10 });
            dbContext.Accounts.Add(new Account { Name = "B", Type = "Liability", Balance = 20 });
            dbContext.SaveChanges();
            var service = new AccountService(dbContext, GetUserContext());

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Xunit.Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnAccount_WhenExists()
        {
            // Arrange
            var dbContext = GetDbContext();
            var account = new Account { Name = "A", Type = "Asset", Balance = 10 };
            dbContext.Accounts.Add(account);
            dbContext.SaveChanges();
            var service = new AccountService(dbContext, GetUserContext());

            // Act
            var result = await service.GetAsync(account.Id);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal("A", result.Name);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            var dbContext = GetDbContext();
            var service = new AccountService(dbContext, GetUserContext());

            // Act
            var result = await service.GetAsync(999);

            // Assert
            Xunit.Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateAccount()
        {
            // Arrange
            var dbContext = GetDbContext();
            var account = new Account { Name = "A", Type = "Asset", Balance = 10 };
            dbContext.Accounts.Add(account);
            dbContext.SaveChanges();
            var service = new AccountService(dbContext, GetUserContext());
            var dto = new UpdateAccountDto
            {
                Id = account.Id,
                Name = "Updated",
                Type = "Liability",
                Description = "Updated Desc",
                Balance = 50
            };

            // Act
            var result = await service.UpdateAsync(dto);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal("Updated", result.Name);
            Xunit.Assert.Equal("Liability", result.Type);
            Xunit.Assert.Equal("Updated Desc", result.Description);
            Xunit.Assert.Equal(50, result.Balance);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            var dbContext = GetDbContext();
            var service = new AccountService(dbContext, GetUserContext());
            var dto = new UpdateAccountDto { Id = 999, Name = "X" };

            // Act
            var result = await service.UpdateAsync(dto);

            // Assert
            Xunit.Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveAccount()
        {
            // Arrange
            var dbContext = GetDbContext();
            var account = new Account { Name = "A", Type = "Asset", Balance = 10 };
            dbContext.Accounts.Add(account);
            dbContext.SaveChanges();
            var service = new AccountService(dbContext, GetUserContext());
            var dto = new DeleteAccountDto { Id = account.Id };

            // Act
            var result = await service.DeleteAsync(dto);

            // Assert
            Xunit.Assert.True(result);
            Xunit.Assert.Empty(dbContext.Accounts);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenNotExists()
        {
            // Arrange
            var dbContext = GetDbContext();
            var service = new AccountService(dbContext, GetUserContext());
            var dto = new DeleteAccountDto { Id = 999 };

            // Act
            var result = await service.DeleteAsync(dto);

            // Assert
            Xunit.Assert.False(result);
        }
    }
}