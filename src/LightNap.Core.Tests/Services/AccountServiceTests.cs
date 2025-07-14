using LightNap.Core.Accounts.Dto.Request;
using LightNap.Core.Accounts.Dto.Response;
using LightNap.Core.Accounts.Services;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using LightNap.Core.Extensions;
using LightNap.Core.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace LightNap.Core.Tests.Services
{
    [TestClass]
    public class AccountServiceTests
    {
#pragma warning disable CS8618
        private ApplicationDbContext _dbContext;
        private AccountService _accountService;
        private TestUserContext _userContext;
#pragma warning restore CS8618

        [TestInitialize]
        public void TestInitialize()
        {
            var services = new ServiceCollection();

            services.AddLogging()
                    .AddLightNapInMemoryDatabase();

            var serviceProvider = services.BuildServiceProvider();
            this._dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            this._userContext = new TestUserContext();
            this._accountService = new AccountService(this._dbContext, this._userContext);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this._dbContext.Database.EnsureDeleted();
            this._dbContext.Dispose();
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsAccounts()
        {
            // Arrange
            this._dbContext.Accounts.Add(new Account { Name = "Cash", Type = "Asset", Description = "Cash in hand" });
            this._dbContext.Accounts.Add(new Account { Name = "Revenue", Type = "Income", Description = "Sales revenue" });
            await this._dbContext.SaveChangesAsync();

            // Act
            var result = await this._accountService.GetAllAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(a => a.Name == "Cash"));
            Assert.IsTrue(result.Any(a => a.Type == "Income"));
        }

        [TestMethod]
        public async Task CreateAsync_ValidRequest_ReturnsCreatedAccount()
        {
            // Arrange
            var createDto = new CreateAccountDto
            {
                Name = "Cash",
                Type = "Asset",
                Description = "Cash on hand"
            };

            // Act
            var result = await this._accountService.CreateAsync(createDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Cash", result.Name);
            Assert.AreEqual("Asset", result.Type);
            Assert.AreEqual("Cash on hand", result.Description);

            var accountInDb = await this._dbContext.Accounts.FindAsync(result.Id);
            Assert.IsNotNull(accountInDb);
        }

    }
}
